namespace Orc.Snapshots
{
    using System;

    public class SnapshotProviderEventArgs : EventArgs
    {
        public SnapshotProviderEventArgs(ISnapshotProvider snapshotProvider)
        {
            ArgumentNullException.ThrowIfNull(snapshotProvider);

            SnapshotProvider = snapshotProvider;
        }
        
        public ISnapshotProvider SnapshotProvider { get; private set; }
    }
}
