namespace Orc.Snapshots;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Catel;

public interface ISnapshotManager
{
    IEnumerable<ISnapshot> Snapshots { get; }
    IEnumerable<ISnapshotProvider> Providers { get; }

    object? Scope { get; set; }

    event AsyncEventHandler<CancelEventArgs>? LoadingAsync;
    event EventHandler<EventArgs>? Loaded;

    event AsyncEventHandler<CancelEventArgs>? SavingAsync;
    event EventHandler<EventArgs>? Saved;

    event AsyncEventHandler<SnapshotEventArgs>? SnapshotCreatingAsync;
    event EventHandler<SnapshotEventArgs>? SnapshotCreated;

    event AsyncEventHandler<SnapshotEventArgs>? SnapshotRestoringAsync;
    event EventHandler<SnapshotEventArgs>? SnapshotRestored;

    event EventHandler<EventArgs>? SnapshotsChanged;
    event EventHandler<SnapshotEventArgs>? SnapshotAdded;
    event EventHandler<SnapshotEventArgs>? SnapshotRemoved;

    event EventHandler<SnapshotProviderEventArgs>? SnapshotProviderAdded;
    event EventHandler<SnapshotProviderEventArgs>? SnapshotProviderRemoved;

    Task<bool> LoadAsync();
    Task<bool> SaveAsync();

    void AddProvider(ISnapshotProvider snapshotProvider);
    bool RemoveProvider(ISnapshotProvider snapshotProvider);

    Task<ISnapshot> CreateSnapshotAsync(ISnapshot snapshot);
    Task RestoreSnapshotAsync(ISnapshot snapshot);

    void Add(ISnapshot snapshot);
    bool Remove(ISnapshot snapshot);
}
