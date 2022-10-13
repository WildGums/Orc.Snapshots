namespace Orc.Snapshots
{
    using System;
    using Catel.Data;

    public class SnapshotCleanup : ModelBase
    {
        public SnapshotCleanup(ISnapshot snapshot)
        {
            ArgumentNullException.ThrowIfNull(snapshot);

            Snapshot = snapshot;
        }

        public bool IncludeInCleanup { get; set; }

        public ISnapshot Snapshot { get; private set; }
    }
}
