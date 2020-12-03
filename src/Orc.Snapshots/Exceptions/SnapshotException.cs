// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotException.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class SnapshotException : Exception
    {
        #region Constructors
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
        #endregion

        #region Properties
        public ISnapshot Snapshot { get; private set; }
        #endregion
    }
}
