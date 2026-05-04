namespace Orc.Snapshots;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catel;
using Catel.Logging;
using Microsoft.Extensions.Logging;

public class InMemorySnapshotStorageService : SnapshotStorageServiceBase
{
    private readonly ILogger<InMemorySnapshotStorageService> _logger;

    private readonly Dictionary<string, byte[]> _snapshots = new Dictionary<string, byte[]>();

    public InMemorySnapshotStorageService(ILogger<InMemorySnapshotStorageService> logger)
        : base(logger)
    {
        _logger = logger;
    }

    public override async Task<IEnumerable<ISnapshot>> LoadSnapshotsAsync()
    {
        _logger.LogDebug("Loading snapshots");

        var snapshots = new List<ISnapshot>();

        foreach (var snapshotData in _snapshots)
        {
            var snapshot = await LoadSnapshotAsync(snapshotData.Key);
            if (snapshot is not null)
            {
                snapshots.Add(snapshot);
            }
        }

        _logger.LogDebug("Loaded '{SnapshotCount}' snapshots", snapshots.Count);

        return snapshots;
    }

    protected virtual async Task<ISnapshot?> LoadSnapshotAsync(string source)
    {
        Argument.IsNotNullOrEmpty(() => source);

        ISnapshot? result = null;

        try
        {
            _logger.LogDebug("Loading snapshot from '{Source}'", source);

            if (_snapshots.TryGetValue(source, out var bytes))
            {
                result = await ConvertBytesToSnapshotAsync(bytes);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load snapshot from '{Source}'", source);
        }

        return result;
    }

    public override async Task SaveSnapshotsAsync(IEnumerable<ISnapshot> snapshots)
    {
        ArgumentNullException.ThrowIfNull(snapshots);

        _logger.LogDebug("Deleting previous snapshot files");

        _snapshots.Clear();

        foreach (var snapshot in snapshots)
        {
            await SaveSnapshotAsync(snapshot.Title, snapshot);
        }
    }

    protected virtual async Task SaveSnapshotAsync(string source, ISnapshot snapshot)
    {
        Argument.IsNotNullOrEmpty(() => source);
        ArgumentNullException.ThrowIfNull(snapshot);

        _logger.LogDebug("Saving snapshot '{Snapshot}' to '{Source}'", snapshot, source);

        var bytes = await ConvertSnapshotToBytesAsync(snapshot);
        if (bytes is not null)
        {
            _snapshots[source] = bytes;
        }
    }
}
