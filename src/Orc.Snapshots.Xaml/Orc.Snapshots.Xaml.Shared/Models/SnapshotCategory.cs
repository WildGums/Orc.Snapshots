// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotCategory.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System.Collections.Generic;

    public class SnapshotCategory
    {
        public SnapshotCategory()
        {
            Snapshots = new List<ISnapshot>();
        }

        public string Category { get; set; }

        public List<ISnapshot> Snapshots { get; private set; }
    }
}