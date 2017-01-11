// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISnapshotManagerExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System;
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

        public static async Task<ISnapshot> CreateSnapshotAsync(this ISnapshotManager snapshotManager, string title, string category = null)
        {
            Argument.IsNotNull(() => snapshotManager);
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

        public static async Task<ISnapshot> CreateSnapshotAndSaveAsync(this ISnapshotManager snapshotManager, string title, string category = null)
        {
            Argument.IsNotNull(() => snapshotManager);
            Argument.IsNotNullOrWhitespace(() => title);

            var snapshot = await snapshotManager.CreateSnapshotAsync(title, category);

            snapshotManager.Add(snapshot);
            await snapshotManager.SaveAsync();

            return snapshot;
        }

        public static ISnapshot FindSnapshot(this ISnapshotManager snapshotManager, string title, string category = null)
        {
            Argument.IsNotNull(() => snapshotManager);
            Argument.IsNotNullOrWhitespace(() => title);

            return (from snapshot in snapshotManager.Snapshots
                    where string.Equals(snapshot.Title, title, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(snapshot.Category, category, StringComparison.OrdinalIgnoreCase)
                    select snapshot).FirstOrDefault();
        }

        public static TSnapshot FindSnapshot<TSnapshot>(this ISnapshotManager snapshotManager, string title, string category = null)
            where TSnapshot : ISnapshot
        {
            Argument.IsNotNull(() => snapshotManager);

            return (TSnapshot)FindSnapshot(snapshotManager, title, category);
        }
        #endregion
    }
}