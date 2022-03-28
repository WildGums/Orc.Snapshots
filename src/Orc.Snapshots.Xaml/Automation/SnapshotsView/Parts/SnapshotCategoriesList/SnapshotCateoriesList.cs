namespace Orc.Snapshots.Automation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [AutomatedControl(ControlTypeName = nameof(ControlType.Pane))]
    public class SnapshotCategoriesList : FrameworkElement
    {
        public SnapshotCategoriesList(AutomationElement element) 
            : base(element)
        {
        }

        public IReadOnlyList<SnapshotCategoryItem> GetCategoryItems()
        {
            var childElements = Element.GetChildElements()
                .ToList();

            return new List<SnapshotCategoryItem>();
        }
    }
}
