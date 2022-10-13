namespace Orc.Snapshots
{
    using System;
    using System.Runtime.Serialization;
    using Catel;

    [Serializable]
    public class InvalidSnapshotException : SnapshotException
    {
        public InvalidSnapshotException()
        {

        }

        public InvalidSnapshotException(ISnapshot snapshot)
            : base(snapshot, string.Format("Snapshot '{0}' is invalid at this stage", ObjectToStringHelper.ToString(snapshot)))
        {
        }

        public InvalidSnapshotException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
