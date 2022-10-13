namespace Orc.Snapshots
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class SnapshotException : Exception
    {
        public SnapshotException()
        {

        }

        public SnapshotException(ISnapshot snapshot)
            : this(snapshot, string.Empty)
        {
        }

        public SnapshotException(ISnapshot snapshot, string message)
            : base(message)
        {
            Snapshot = snapshot;
        }

        public SnapshotException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        public ISnapshot? Snapshot { get; private set; }

    }
}
