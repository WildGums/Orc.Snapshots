// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotEventArgs.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System;

    public class SnapshotEventArgs : EventArgs
    {
        #region Constructors
        public SnapshotEventArgs(ISnapshot snapshot)
        {
            Snapshot = snapshot;
        }
        #endregion

        #region Properties
        public ISnapshot Snapshot { get; private set; }
        #endregion
    }
}