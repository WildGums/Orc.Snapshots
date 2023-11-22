namespace Orc.Snapshots;

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
        : base(snapshot, $"Snapshot '{ObjectToStringHelper.ToString(snapshot)}' is invalid at this stage")
    {
    }
}
