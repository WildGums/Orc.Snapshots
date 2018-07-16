// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotCleanup.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using Catel;
    using Catel.Data;

    public class SnapshotCleanup : ModelBase
    {
        public SnapshotCleanup(ISnapshot snapshot)
        {
            Argument.IsNotNull(() => snapshot);

            Snapshot = snapshot;
        }

        public bool IncludeInCleanup { get; set; }

        public ISnapshot Snapshot { get; private set; }
    }
}