namespace Orc.Snapshots.Automation
{
    using System.Collections.Generic;
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [AutomatedControl(Class = typeof(SnapshotsView))]
    public class SnapshotsView : FrameworkElement<SnapshotsViewModel, SnapshotsViewMap>
    {
        public SnapshotsView(AutomationElement element) 
            : base(element)
        {
        }

        public IReadOnlyList<SnapshotCategoryItem> SnapshotCategories
            => Map.SnapshotCategoryList?.GetCategoryItems() ?? new List<SnapshotCategoryItem>();
    }
}
