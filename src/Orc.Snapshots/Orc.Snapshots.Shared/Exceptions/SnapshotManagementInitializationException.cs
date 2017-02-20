// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotManagementInitializationException.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System;

    public class SnapshotManagementInitializationException : Exception
    {
        #region Constructors
        public SnapshotManagementInitializationException(ISnapshotManager snapshotManager)
            : base("Unable to initialize SnapshotManager. Probably initialization was canceled.")
        {
            SnapshotManager = snapshotManager;
        }
        #endregion

        #region Properties
        public ISnapshotManager SnapshotManager { get; private set; }
        #endregion
    }
}