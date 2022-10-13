namespace Orc.Snapshots
{
    using System;
    using System.Runtime.Serialization;

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

        public SnapshotManagementInitializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        public ISnapshotManager? SnapshotManager { get; private set; }
    }
}
