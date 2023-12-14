namespace Orc.Snapshots.Tests;

using System.Linq;
using Automation;
using NUnit.Framework;
using Orc.Automation;

[Explicit]
[TestFixture]
public class SnapshotsViewFacts : StyledControlTestFacts<Views.SnapshotsView>
{
    [Target]
    public SnapshotsView Target { get; set; }

    protected override void InitializeTarget(string id)
    {
        base.InitializeTarget(id);

        var target = Target;

        target.Execute<InitSnapshotsViewMethodRun>();
    }

    [TestCase(SnapshotTestData.TestScopeWith0CustomRecords)]
    [TestCase(SnapshotTestData.TestScopeWith3CustomRecords)]
    [TestCase(SnapshotTestData.TestScopeWith5CustomRecords)]
    public void CorrectlySetScope(string scope)
    {
        var target = Target;
        var current = target.Current;

        current.Scope = scope;

        var categories = target.SnapshotCategories;
        var expectedCategories = SnapshotTestData.GetSnapshotCategories(scope);

        Assert.That(categories, Is.EquivalentTo(expectedCategories)
            .Using<SnapshotCategoryItem, SnapshotCategory>((x, y) 
                => Equals(x.CategoryName, y.Category) && x.Items.Select(item => item.Title)
                    .OrderBy(title => title).SequenceEqual(y.Snapshots.Select(item => item.Title).OrderBy(title => title))));
    }

    [Test]
    public void CorrectlyEditSnapshot()
    {
        var target = Target;
        var current = target.Current;

        //Initialize scope items
        current.Scope = SnapshotTestData.TestScopeWith3CustomRecords;

        Wait.UntilResponsive();

        var snapshotItems = target.SnapshotCategories[0].Items;
        foreach (var snapshotItem in snapshotItems)
        {
            var editWindow = snapshotItem.Edit();

            var itemText = snapshotItem.Title;

            Assert.That(editWindow, Is.Not.Null);
            Assert.That(editWindow.Title, Is.EqualTo(itemText));

            editWindow.Title = itemText + "_test";

            editWindow.Accept();

            Wait.UntilResponsive();
        }

        snapshotItems = target.SnapshotCategories[0].Items;

        Assert.That(snapshotItems, Has.All
            .Property(nameof(SnapshotItem.Title)).EndWith("_test"));
    }

    [Test]
    public void CorrectlyRemoveWorkspace()
    {
        var target = Target;
        var current = target.Current;

        //Initialize scope items
        current.Scope = SnapshotTestData.TestScopeWith3CustomRecords;

        Wait.UntilResponsive();

        var workspaceItems = target.SnapshotCategories[0].Items;
        var deleteItem = workspaceItems[2];

        deleteItem.Remove();

        Wait.UntilResponsive();

        workspaceItems = target.SnapshotCategories[0].Items;

        Assert.That(workspaceItems, Does.Not.Contains(deleteItem)
            .Using<SnapshotItem, SnapshotItem>((x, y) => Equals(x.Title, y.Title)));
    }
}