namespace Orc.Snapshots;

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

/// <summary>
/// Provider that can be registered in the snapshot manager to interact with a snapshot.
/// </summary>
public interface ISnapshotProvider
{
    string Name { get; }

    /// <summary>
    /// Gets the names of the values that needs to be written to a stream.
    /// </summary>
    /// <returns></returns>
    List<string> GetNames();

    /// <summary>
    /// Called when a snapshot manager is about to create a snapshot.
    /// </summary>
    /// <param name="snapshot">The snapshot.</param>
    /// <returns></returns>
    Task CreatingSnapshotAsync(ISnapshot snapshot);

    /// <summary>
    /// Called when a snapshot manager has just created a snapshot.
    /// </summary>
    /// <param name="snapshot">The snapshot.</param>
    /// <returns></returns>
    Task CreatedSnapshotAsync(ISnapshot snapshot);

    /// <summary>
    /// Called when a snapshot manager is about to restore a snapshot.
    /// </summary>
    /// <param name="snapshot">The snapshot.</param>
    /// <returns></returns>
    Task RestoringSnapshotAsync(ISnapshot snapshot);

    /// <summary>
    /// Called when a snapshot manager has just restored a snapshot.
    /// </summary>
    /// <param name="snapshot">The snapshot.</param>
    /// <returns></returns>
    Task RestoredSnapshotAsync(ISnapshot snapshot);

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
}
