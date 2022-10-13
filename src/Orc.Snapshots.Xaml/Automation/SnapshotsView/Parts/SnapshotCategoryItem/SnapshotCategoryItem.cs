namespace Orc.Snapshots.Automation
{
    using System.Collections.Generic;

    public class SnapshotCategoryItem
    {
        public SnapshotCategoryItem()
        {
            CategoryName = string.Empty;
            Items = new List<SnapshotItem>();
        }

        public string CategoryName { get; set; }
        public List<SnapshotItem> Items { get; set; }
    }
}
