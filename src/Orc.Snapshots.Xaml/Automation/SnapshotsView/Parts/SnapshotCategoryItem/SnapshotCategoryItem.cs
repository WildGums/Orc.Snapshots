namespace Orc.Snapshots.Automation
{
    using System.Collections.Generic;

    public class SnapshotCategoryItem
    {
        public string CategoryName { get; set; }
        public IReadOnlyList<SnapshotItem> Items { get; set; }
    }
}
