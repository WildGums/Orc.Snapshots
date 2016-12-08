// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISnapshotManagerExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;

    public static class ISnapshotManagerExtensions
    {
        #region Methods
        public static void AddProvider<TSnapshotProvider>(this ISnapshotManager snapshotManager)
            where TSnapshotProvider : ISnapshotProvider
        {
            Argument.IsNotNull(() => snapshotManager);

            var snapshotProvider = TypeFactory.Default.CreateInstance<TSnapshotProvider>();
            snapshotManager.AddProvider(snapshotProvider);
        }

        public static async Task CreateSnapshotAndSaveAsync(this ISnapshotManager snapshotManager, string title)
        {
            Argument.IsNotNull(() => snapshotManager);
            Argument.IsNotNullOrWhitespace(() => title);

            var snapshot = await snapshotManager.CreateSnapshotAsync(title);

            snapshotManager.Add(snapshot);
            await snapshotManager.SaveAsync();
        }

        public static ISnapshot FindSnapshot(this ISnapshotManager snapshotManager, string title)
        {
            Argument.IsNotNull(() => snapshotManager);
            Argument.IsNotNullOrWhitespace(() => title);

            return (from snapshot in snapshotManager.Snapshots
                    where string.Equals(snapshot.Title, title)
                    select snapshot).FirstOrDefault();
        }

        public static TSnapshot FindSnapshot<TSnapshot>(this ISnapshotManager snapshotManager, string title)
            where TSnapshot : ISnapshot
        {
            Argument.IsNotNull(() => snapshotManager);

            return (TSnapshot)FindSnapshot(snapshotManager, title);
        }
        #endregion
    }
}