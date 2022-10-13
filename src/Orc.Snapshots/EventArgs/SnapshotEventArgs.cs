namespace Orc.Snapshots
{
    using System;

    public class SnapshotEventArgs : EventArgs
    {
        public SnapshotEventArgs(ISnapshot snapshot)
        {
            ArgumentNullException.ThrowIfNull(snapshot);

            Snapshot = snapshot;
        }
        
        public ISnapshot Snapshot { get; private set; }
    }
}
