// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISnapshotProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Provider that can be registered in the snapshot manager to interact with a snapshot.
    /// </summary>
    public interface ISnapshotProvider
    {
        #region Properties
        string Name { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the names of the values that needs to be written to a stream.
        /// </summary>
        /// <returns></returns>
        List<string> GetNames();

        /// <summary>
        /// Stores the data into the stream that will be stored inside the snapshot.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        Task StoreDataToSnapshotAsync(string name, Stream stream);

        /// <summary>
        /// Restores the snapshot data from the stream.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        Task RestoreDataFromSnapshotAsync(string name, Stream stream);
        #endregion
    }
}