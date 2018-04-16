// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotException.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System;

    public class SnapshotException : Exception
    {
        #region Constructors
        public SnapshotException(ISnapshot snapshot)
            : this(snapshot, string.Empty)
        {
        }

        public SnapshotException(ISnapshot snapshot, string message)
            : base(message)
        {
            Snapshot = snapshot;
        }
        #endregion

        #region Properties
        public ISnapshot Snapshot { get; private set; }
        #endregion
    }
}