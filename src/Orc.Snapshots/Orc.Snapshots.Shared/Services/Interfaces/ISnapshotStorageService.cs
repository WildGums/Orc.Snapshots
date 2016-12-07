// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISnapshotStorageService.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISnapshotStorageService
    {
        #region Methods
        Task<IEnumerable<ISnapshot>> LoadSnapshotsAsync();
        Task SaveSnapshotsAsync(IEnumerable<ISnapshot> snapshots);
        #endregion
    }
}