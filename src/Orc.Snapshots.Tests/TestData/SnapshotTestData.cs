namespace Orc.Snapshots.Tests;

using System;
using System.Collections.Generic;
using System.Linq;

public static class SnapshotTestData
{
    public static List<string> AvailableScopes = new()
    {
        TestScopeWith0CustomRecords,
        TestScopeWith3CustomRecords,
        TestScopeWith5CustomRecords
    };

    public const string TestScopeWith0CustomRecords = nameof(TestScopeWith0CustomRecords);
    public const string TestScopeWith3CustomRecords = nameof(TestScopeWith3CustomRecords);
    public const string TestScopeWith5CustomRecords = nameof(TestScopeWith5CustomRecords);

    public static List<SnapshotCategory> GetSnapshotCategories(object scope)
    {
        return GetSnapshots(scope)
            .GroupBy(x => x.Category)
            .Select(x =>
            {
                var snapshotCategory = new SnapshotCategory { Category = x.Key };
                snapshotCategory.Snapshots.AddRange(x.AsEnumerable());
                return snapshotCategory;
            })
            .ToList();
    }

    private static IEnumerable<ISnapshot> GetSnapshots(object scope)
    {
        if (Equals(scope, TestScopeWith3CustomRecords))
        {
            return new List<ISnapshot>
            {
                new Snapshot
                {
                    Title = "Snapshot_1",
                    Category = "Category",
                    Created = DateTime.Today
                },
                new Snapshot
                {
                    Title = "Snapshot_2",
                    Category = "Category",
                    Created = DateTime.Today
                },
                new Snapshot
                {
                    Title = "Snapshot_3",
                    Category = "Category",
                    Created = DateTime.Today
                }
            };
        }

        if (Equals(scope, TestScopeWith5CustomRecords))
        {
            return new List<ISnapshot>
            {
                new Snapshot
                {
                    Title = "Snapshot_1",
                    Category = "Category_1",
                    Created = DateTime.Now - TimeSpan.FromDays(10)
                },
                new Snapshot
                {
                    Title = "Snapshot_2",
                    Category = "Category_1",
                    Created = DateTime.Now - TimeSpan.FromDays(8)
                },
                new Snapshot
                {
                    Title = "Snapshot_3",
                    Category = "Category_2",
                    Created = DateTime.Now - TimeSpan.FromDays(7)
                },
                new Snapshot
                {
                    Title = "Snapshot_4",
                    Category = "Category_2",
                    Created = DateTime.Now - TimeSpan.FromDays(5)
                },
                new Snapshot
                {
                    Title = "Snapshot_5",
                    Category = "Category_2",
                    Created = DateTime.Now - TimeSpan.FromDays(2)
                },
                new Snapshot
                {
                    Title = "Snapshot_6",
                    Category = "Category_3",
                    Created = DateTime.Now - TimeSpan.FromDays(1)
                },
                new Snapshot
                {
                    Title = "Snapshot_7",
                    Category = "Category_3",
                    Created = DateTime.Now
                }
            };
        }

        return Array.Empty<ISnapshot>();
    }
}