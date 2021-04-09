// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotManager.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Snapshots
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;

    public class SnapshotManager : ISnapshotManager
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IServiceLocator _serviceLocator;

        private readonly List<ISnapshotProvider> _providers = new List<ISnapshotProvider>();
        private readonly List<ISnapshot> _snapshots = new List<ISnapshot>();

        private ISnapshotStorageService _snapshotStorageService;
        private object _scope;

        #region Constructors
        public SnapshotManager(ISnapshotStorageService snapshotStorageService, IServiceLocator serviceLocator)
        {
            Argument.IsNotNull(() => snapshotStorageService);
            Argument.IsNotNull(() => serviceLocator);

            _snapshotStorageService = snapshotStorageService;
            _serviceLocator = serviceLocator;

            UniqueIdentifier = UniqueIdentifierHelper.GetUniqueIdentifier<SnapshotManager>();
        }
        #endregion

        #region Properties
        public int UniqueIdentifier { get; private set; }

        public IEnumerable<ISnapshotProvider> Providers
        {
            get { return _providers.ToArray(); }
        }

        public object Scope
        {
            get { return _scope; }
            set
            {
                _scope = value;
                _snapshotStorageService = _serviceLocator.ResolveType<ISnapshotStorageService>(_scope);
            }
        }

        public IEnumerable<ISnapshot> Snapshots
        {
            get { return _snapshots.ToArray(); }
        }
        #endregion

        #region Events
        public event AsyncEventHandler<CancelEventArgs> LoadingAsync;
        public event EventHandler<EventArgs> Loaded;

        public event AsyncEventHandler<CancelEventArgs> SavingAsync;
        public event EventHandler<EventArgs> Saved;

        public event AsyncEventHandler<SnapshotEventArgs> SnapshotCreatingAsync;
        public event EventHandler<SnapshotEventArgs> SnapshotCreated;

        public event AsyncEventHandler<SnapshotEventArgs> SnapshotRestoringAsync;
        public event EventHandler<SnapshotEventArgs> SnapshotRestored;

        public event EventHandler<EventArgs> SnapshotsChanged;
        public event EventHandler<SnapshotEventArgs> SnapshotAdded;
        public event EventHandler<SnapshotEventArgs> SnapshotRemoved;

        public event EventHandler<SnapshotProviderEventArgs> SnapshotProviderAdded;
        public event EventHandler<SnapshotProviderEventArgs> SnapshotProviderRemoved;
        #endregion

        #region ISnapshotManager Members
        public async Task<bool> LoadAsync()
        {
            Log.Debug($"[{Scope}] Loading snapshots");

            var loadingAsync = LoadingAsync;
            if (loadingAsync is not null)
            {
                var cancelEventArgs = new CancelEventArgs();
                await loadingAsync(this, cancelEventArgs);
                if (cancelEventArgs.Cancel)
                {
                    Log.Info("Loading canceled by LoadingAsync event");
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

            Log.Info($"[{Scope}] Loaded '{snapshots.Count()}' snapshots");

            return true;
        }

        public async Task<bool> SaveAsync()
        {
            Log.Debug($"[{Scope}] Saving snapshots");

            var savingAsync = SavingAsync;
            if (savingAsync is not null)
            {
                var cancelEventArgs = new CancelEventArgs();
                await savingAsync(this, cancelEventArgs);
                if (cancelEventArgs.Cancel)
                {
                    Log.Info("Saving canceled by SavingAsync event");
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

            Log.Info($"[{Scope}] Saved '{snapshots.Count}' snapshots");

            return true;
        }

        public void AddProvider(ISnapshotProvider snapshotProvider)
        {
            Argument.IsNotNull(() => snapshotProvider);

#if DEBUG
            Log.Debug($"[{Scope}] Adding provider {snapshotProvider.GetType()} to the SnapshotManager (Scope = '{Scope ?? "null"}')");
#endif

            lock (_providers)
            {
                _providers.Add(snapshotProvider);
            }

            SnapshotProviderAdded?.Invoke(this, new SnapshotProviderEventArgs(snapshotProvider));
        }

        public bool RemoveProvider(ISnapshotProvider snapshotProvider)
        {
            Argument.IsNotNull(() => snapshotProvider);

#if DEBUG
            Log.Debug($"[{Scope}] Removing provider {snapshotProvider.GetType()} from the SnapshotManager (Tag == \"{Scope ?? "null"}\")");
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
            Argument.IsNotNull(() => snapshot);

            Log.Info($"Creating snapshot '{snapshot}'");

            await SnapshotCreatingAsync.SafeInvokeAsync(this, new SnapshotEventArgs(snapshot));

            var providers = GetProviders();

            foreach (var provider in providers)
            {
                await provider.CreatingSnapshotAsync(snapshot);
            }

            foreach (var provider in providers)
            {
                Log.Debug($"Creating data for snapshot '{snapshot}' using provider '{provider}'");

                var names = provider.GetNames();

                foreach (var name in names)
                {
                    Log.Debug($"Creating data for snapshot '{snapshot}' using provider '{provider}::{name}'");

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

            Log.Info($"Created snapshot '{snapshot}'");

            return snapshot;
        }

        public virtual async Task RestoreSnapshotAsync(ISnapshot snapshot)
        {
            Argument.IsNotNull(() => snapshot);

            Log.Info($"Restoring snapshot '{snapshot}'");

            await SnapshotRestoringAsync.SafeInvokeAsync(this, new SnapshotEventArgs(snapshot));

            var providers = GetProviders();

            foreach (var provider in providers)
            {
                await provider.RestoringSnapshotAsync(snapshot);
            }

            foreach (var provider in providers)
            {
                Log.Debug($"Restoring data for snapshot '{snapshot}' using provider '{provider}'");

                var names = provider.GetNames();

                foreach (var name in names)
                {
                    Log.Debug($"Restoring data for snapshot '{snapshot}' using provider '{provider}::{name}'");

                    var providerData = snapshot.GetData(name);
                    if (providerData is null)
                    {
                        providerData = new byte[] {};
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

            Log.Info($"Restored snapshot '{snapshot}'");
        }

        public void Add(ISnapshot snapshot)
        {
            Argument.IsNotNull(() => snapshot);

            if (!_snapshots.Contains(snapshot))
            {
                Log.Debug($"[{Scope}] Adding snapshot '{snapshot}'");

                _snapshots.Add(snapshot);

                SnapshotAdded?.Invoke(this, new SnapshotEventArgs(snapshot));
                SnapshotsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool Remove(ISnapshot snapshot)
        {
            Argument.IsNotNull(() => snapshot);

            Log.Debug($"[{Scope}] Deleting snapshot '{snapshot}'");

            if (!_snapshots.Contains(snapshot))
            {
                Log.Debug($"[{Scope}] Can't delete snapshot '{snapshot}', snapshot is not managed by the manager");
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
        #endregion
    }
}
