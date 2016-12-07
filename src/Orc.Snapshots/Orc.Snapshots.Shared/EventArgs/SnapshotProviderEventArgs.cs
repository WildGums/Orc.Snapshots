// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotProviderEventArgs.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System;

    public class SnapshotProviderEventArgs : EventArgs
    {
        #region Constructors
        public SnapshotProviderEventArgs(ISnapshotProvider snapshotProvider)
        {
            SnapshotProvider = snapshotProvider;
        }
        #endregion

        #region Properties
        public ISnapshotProvider SnapshotProvider { get; private set; }
        #endregion
    }
}