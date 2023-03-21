namespace Orc.Snapshots.Automation;

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
        var element = Element;

        var childElements = element.GetChildElements()
            .ToList();

        var categories = element.FindAll<Text>(id: "SnapshotCategoryTextBlock")
            .Select(x => new
            {
                Element = x,
                Index = childElements.IndexOf(x.Element),
                CategoryItem = new SnapshotCategoryItem
                {
                    CategoryName = x.Value
                }
            })
            .ToList();

        if (!categories.Any())
        {
            return new List<SnapshotCategoryItem>();
        }

        var titleTextBlocks = element.FindAll<Text>(id: "SnapshotTitleLabel")
            .Select(x => new
            {
                Element = x,
                Index = childElements.IndexOf(x.Element)
            })
            .ToList();
        var restoreButtons = element.FindAll<Button>(id: "RestoreSnapshotButton").ToList();
        var editButtons = element.FindAll<Button>(id: "EditSnapshotButton").ToList();
        var removeButtons = element.FindAll<Button>(id: "RemoveSnapshotButton").ToList();
            
        for (var i = 0; i < titleTextBlocks.Count; i++)
        {
            var startIndex = titleTextBlocks[i].Index;

            var categoryItem = categories.Where((x, index) => x.Index < startIndex && (index == categories.Count - 1 || categories[index + 1].Index > startIndex))
                .Select(x => x.CategoryItem)
                .FirstOrDefault();

            var snapshotItemMap = new SnapshotItemMap 
            { 
                TitleText = titleTextBlocks[i].Element,
                RestoreButton = restoreButtons[i], 
                EditButton = editButtons[i], 
                RemoveButton = removeButtons[i]
            };

            categoryItem?.Items.Add(new SnapshotItem(snapshotItemMap));
        }

        return categories.Select(x => x.CategoryItem)
            .ToList();
    }
}
