namespace Orc.Snapshots.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;

    public class SnapshotsViewMap : AutomationBase
    {
        public SnapshotsViewMap(AutomationElement element)
            : base(element)
        {
        }

        public SnapshotCategoriesList? SnapshotCategoryList => By.One<SnapshotCategoriesList>();
    }
}
