namespace Orc.Snapshots.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class SnapshotStorageServiceMock : ISnapshotStorageService
    {
        private List<ISnapshot> _snapshotCategories;

        public object Scope { get; set; }

        public async Task<IEnumerable<ISnapshot>> LoadSnapshotsAsync()
        {
            _snapshotCategories ??= SnapshotTestData.GetSnapshotCategories(Scope)
                .SelectMany(x => x.Snapshots)
                .ToList();

            await File.AppendAllLinesAsync(@"C:\Temps\Log\log1.txt",
                _snapshotCategories?.Select(x => $"{Scope} - {x.Title}"));

            return _snapshotCategories;
        }

        public async Task SaveSnapshotsAsync(IEnumerable<ISnapshot> snapshots)
        {
            _snapshotCategories = snapshots?.ToList();

            await File.AppendAllLinesAsync(@"C:\Temps\Log\log.txt",
                _snapshotCategories.Select(x => x.Title));

            //   Enumerable.Range(0, 1).Select(x => $"{_snapshotCategories.Count}\r\n"));
        }
    }
}
