namespace Orc.Snapshots
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;

    public static class ISnapshotManagerExtensions
    {
        public static void AddProvider<TSnapshotProvider>(this ISnapshotManager snapshotManager)
            where TSnapshotProvider : ISnapshotProvider
        {
            ArgumentNullException.ThrowIfNull(snapshotManager);

            var snapshotProvider = TypeFactory.Default.CreateRequiredInstance<TSnapshotProvider>();
            snapshotManager.AddProvider(snapshotProvider);
        }

        public static async Task<ISnapshot?> CreateSnapshotAsync(this ISnapshotManager snapshotManager, string title, string category = "")
        {
            ArgumentNullException.ThrowIfNull(snapshotManager);
            Argument.IsNotNullOrWhitespace(() => title);

            var snapshot = new Snapshot
            {
                Title = title,
                Category = category,
                Created = FastDateTime.Now
            };

            await snapshotManager.CreateSnapshotAsync(snapshot);

            return snapshot;
        }

        public static async Task<ISnapshot?> CreateSnapshotAndSaveAsync(this ISnapshotManager snapshotManager, string title, string category = "")
        {
            ArgumentNullException.ThrowIfNull(snapshotManager);
            Argument.IsNotNullOrWhitespace(() => title);

            var snapshot = await snapshotManager.CreateSnapshotAsync(title, category);
            if (snapshot is not null)
            {
                snapshotManager.Add(snapshot);
                await snapshotManager.SaveAsync();
            }

            return snapshot;
        }

        public static ISnapshot? FindSnapshot(this ISnapshotManager snapshotManager, string title, string category = "")
        {
            ArgumentNullException.ThrowIfNull(snapshotManager);
            Argument.IsNotNullOrWhitespace(() => title);

            return (from snapshot in snapshotManager.Snapshots
                    where string.Equals(snapshot.Title, title, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(snapshot.Category, category, StringComparison.OrdinalIgnoreCase)
                    select snapshot).FirstOrDefault();
        }

        public static TSnapshot? FindSnapshot<TSnapshot>(this ISnapshotManager snapshotManager, string title, string category = "")
            where TSnapshot : ISnapshot
        {
            ArgumentNullException.ThrowIfNull(snapshotManager);

            return (TSnapshot?)FindSnapshot(snapshotManager, title, category);
        }
    }
}
