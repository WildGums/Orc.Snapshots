namespace Orc.Snapshots;

using System;

[Serializable]
public class SnapshotManagementInitializationException : Exception
{
    public SnapshotManagementInitializationException()
    {

    }

    public SnapshotManagementInitializationException(ISnapshotManager snapshotManager)
        : base("Unable to initialize SnapshotManager. Probably initialization was canceled.")
    {
        SnapshotManager = snapshotManager;
    }

    public ISnapshotManager? SnapshotManager { get; private set; }
}
