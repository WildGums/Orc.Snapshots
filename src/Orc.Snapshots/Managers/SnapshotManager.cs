namespace Orc.Snapshots;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Catel;
using Catel.IoC;
using Catel.Logging;
using Microsoft.Extensions.Logging;

public class SnapshotManager : ISnapshotManager
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(SnapshotManager));

    private readonly List<ISnapshotProvider> _providers = new List<ISnapshotProvider>();
    private readonly List<ISnapshot> _snapshots = new List<ISnapshot>();

    private readonly ISnapshotStorageService _snapshotStorageService;

    public SnapshotManager(ISnapshotStorageService snapshotStorageService)
    {
        _snapshotStorageService = snapshotStorageService;

        UniqueIdentifier = UniqueIdentifierHelper.GetUniqueIdentifier<SnapshotManager>();
    }

    public int UniqueIdentifier { get; private set; }

    public IEnumerable<ISnapshotProvider> Providers
    {
        get { return _providers.ToArray(); }
    }

    public IEnumerable<ISnapshot> Snapshots
    {
        get { return _snapshots.ToArray(); }
    }

    public event AsyncEventHandler<CancelEventArgs>? LoadingAsync;
    public event EventHandler<EventArgs>? Loaded;

    public event AsyncEventHandler<CancelEventArgs>? SavingAsync;
    public event EventHandler<EventArgs>? Saved;

    public event AsyncEventHandler<SnapshotEventArgs>? SnapshotCreatingAsync;
    public event EventHandler<SnapshotEventArgs>? SnapshotCreated;

    public event AsyncEventHandler<SnapshotEventArgs>? SnapshotRestoringAsync;
    public event EventHandler<SnapshotEventArgs>? SnapshotRestored;

    public event EventHandler<EventArgs>? SnapshotsChanged;
    public event EventHandler<SnapshotEventArgs>? SnapshotAdded;
    public event EventHandler<SnapshotEventArgs>? SnapshotRemoved;

    public event EventHandler<SnapshotProviderEventArgs>? SnapshotProviderAdded;
    public event EventHandler<SnapshotProviderEventArgs>? SnapshotProviderRemoved;

    public async Task<bool> LoadAsync()
    {
        Logger.LogDebug($"Loading snapshots");

        var loadingAsync = LoadingAsync;
        if (loadingAsync is not null)
        {
            var cancelEventArgs = new CancelEventArgs();
            await loadingAsync(this, cancelEventArgs);
            if (cancelEventArgs.Cancel)
            {
                Logger.LogInformation("Loading canceled by LoadingAsync event");
                return false;
            }
        }

        var snapshots = await _snapshotStorageService.LoadSnapshotsAsync();

        lock (_snapshots)
        {
            _snapshots.Clear();
            _snapshots.AddRange(snapshots);
        }

        Loaded?.Invoke(this, EventArgs.Empty);

        Logger.LogInformation($"Loaded '{snapshots.Count()}' snapshots");

        return true;
    }

    public async Task<bool> SaveAsync()
    {
        Logger.LogDebug($"Saving snapshots");

        var savingAsync = SavingAsync;
        if (savingAsync is not null)
        {
            var cancelEventArgs = new CancelEventArgs();
            await savingAsync(this, cancelEventArgs);
            if (cancelEventArgs.Cancel)
            {
                Logger.LogInformation("Saving canceled by SavingAsync event");
                return false;
            }
        }

        var snapshots = new List<ISnapshot>();

        lock (_snapshots)
        {
            snapshots.AddRange(_snapshots);
        }

        await _snapshotStorageService.SaveSnapshotsAsync(snapshots);

        Saved?.Invoke(this, EventArgs.Empty);

        Logger.LogInformation($"Saved '{snapshots.Count}' snapshots");

        return true;
    }

    public void AddProvider(ISnapshotProvider snapshotProvider)
    {
        ArgumentNullException.ThrowIfNull(snapshotProvider);

#if DEBUG
        Logger.LogDebug($"Adding provider {snapshotProvider.GetType()} to the SnapshotManager");
#endif

        lock (_providers)
        {
            _providers.Add(snapshotProvider);
        }

        SnapshotProviderAdded?.Invoke(this, new SnapshotProviderEventArgs(snapshotProvider));
    }

    public bool RemoveProvider(ISnapshotProvider snapshotProvider)
    {
        ArgumentNullException.ThrowIfNull(snapshotProvider);

#if DEBUG
        Logger.LogDebug($"Removing provider {snapshotProvider.GetType()} from the SnapshotManager");
#endif

        var removed = false;

        lock (_providers)
        {
            removed = _providers.Remove(snapshotProvider);
        }

        if (removed)
        {
            SnapshotProviderRemoved?.Invoke(this, new SnapshotProviderEventArgs(snapshotProvider));
            return true;
        }

        return false;
    }

    public virtual async Task<ISnapshot> CreateSnapshotAsync(ISnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        Logger.LogInformation($"Creating snapshot '{snapshot}'");

        await SnapshotCreatingAsync.SafeInvokeAsync(this, new SnapshotEventArgs(snapshot));

        var providers = GetProviders();

        foreach (var provider in providers)
        {
            await provider.CreatingSnapshotAsync(snapshot);
        }

        foreach (var provider in providers)
        {
            Logger.LogDebug($"Creating data for snapshot '{snapshot}' using provider '{provider}'");

            var names = provider.GetNames();

            foreach (var name in names)
            {
                Logger.LogDebug($"Creating data for snapshot '{snapshot}' using provider '{provider}::{name}'");

                byte[] providerData;

                using (var memoryStream = new MemoryStream())
                {
                    await provider.StoreDataToSnapshotAsync(name, memoryStream);

                    providerData = memoryStream.ToArray();
                }

                snapshot.SetData(name, providerData);
            }
        }

        foreach (var provider in providers)
        {
            await provider.CreatedSnapshotAsync(snapshot);
        }

        SnapshotCreated?.Invoke(this, new SnapshotEventArgs(snapshot));

        Logger.LogInformation($"Created snapshot '{snapshot}'");

        return snapshot;
    }

    public virtual async Task RestoreSnapshotAsync(ISnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        Logger.LogInformation($"Restoring snapshot '{snapshot}'");

        await SnapshotRestoringAsync.SafeInvokeAsync(this, new SnapshotEventArgs(snapshot));

        var providers = GetProviders();

        foreach (var provider in providers)
        {
            await provider.RestoringSnapshotAsync(snapshot);
        }

        foreach (var provider in providers)
        {
            Logger.LogDebug($"Restoring data for snapshot '{snapshot}' using provider '{provider}'");

            var names = provider.GetNames();

            foreach (var name in names)
            {
                Logger.LogDebug($"Restoring data for snapshot '{snapshot}' using provider '{provider}::{name}'");

                var providerData = snapshot.GetData(name);
                if (providerData is null)
                {
                    providerData = new byte[] { };
                }

                using (var memoryStream = new MemoryStream(providerData))
                {
                    await provider.RestoreDataFromSnapshotAsync(name, memoryStream);
                }
            }
        }

        foreach (var provider in providers)
        {
            await provider.RestoredSnapshotAsync(snapshot);
        }

        SnapshotRestored?.Invoke(this, new SnapshotEventArgs(snapshot));

        Logger.LogInformation($"Restored snapshot '{snapshot}'");
    }

    public void Add(ISnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        if (!_snapshots.Contains(snapshot))
        {
            Logger.LogDebug($"Adding snapshot '{snapshot}'");

            _snapshots.Add(snapshot);

            SnapshotAdded?.Invoke(this, new SnapshotEventArgs(snapshot));
            SnapshotsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool Remove(ISnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        Logger.LogDebug($"Deleting snapshot '{snapshot}'");

        if (!_snapshots.Contains(snapshot))
        {
            Logger.LogDebug($"Can't delete snapshot '{snapshot}', snapshot is not managed by the manager");
            return false;
        }

        var removed = _snapshots.Remove(snapshot);
        if (removed)
        {
            SnapshotRemoved?.Invoke(this, new SnapshotEventArgs(snapshot));
            SnapshotsChanged?.Invoke(this, EventArgs.Empty);
        }

        return removed;
    }

    private List<ISnapshotProvider> GetProviders()
    {
        var providers = new List<ISnapshotProvider>();

        lock (_providers)
        {
            providers.AddRange(_providers);
        }

        return providers;
    }
}
