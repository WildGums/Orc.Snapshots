namespace Orc.Snapshots;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface ISnapshotStorageService
{
    Task<IEnumerable<ISnapshot>> LoadSnapshotsAsync();
    Task SaveSnapshotsAsync(IEnumerable<ISnapshot> snapshots);
}
