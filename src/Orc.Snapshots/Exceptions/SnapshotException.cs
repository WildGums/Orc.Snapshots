﻿namespace Orc.Snapshots;

using System;

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

    public ISnapshot? Snapshot { get; private set; }

}
