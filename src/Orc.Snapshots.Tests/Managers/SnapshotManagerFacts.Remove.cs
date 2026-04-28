namespace Orc.Snapshots.Tests.Managers;

using System.Linq;
using Catel;
using NUnit.Framework;

public partial class SnapshotManagerFacts
{
    [TestFixture]
    public class The_Remove_Method
    {
        [Test]
        public void Removes_Snapshot_From_Snapshots_List()
        {
            var snapshot = new Snapshot
            {
                Title = "Test"
            };

            var snapshotManager = CreateSnapshotManager();
            snapshotManager.Add(snapshot);

            Assert.That(snapshotManager.Snapshots.Count(), Is.EqualTo(1));

            var result = snapshotManager.Remove(snapshot);

            Assert.That(result, Is.True);
            Assert.That(snapshotManager.Snapshots.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Returns_False_When_Snapshot_Cannot_Be_Removed()
        {
            var snapshot = new Snapshot
            {
                Title = "Test"
            };

            var snapshotManager = CreateSnapshotManager();

            Assert.That(snapshotManager.Snapshots.Count(), Is.EqualTo(0));

            var result = snapshotManager.Remove(snapshot);

            Assert.That(result, Is.False);
            Assert.That(snapshotManager.Snapshots.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Raises_SnapshotRemoved_Event()
        {
            var snapshot = new Snapshot
            {
                Title = "Test"
            };

            var snapshotManager = CreateSnapshotManager();
            snapshotManager.Add(snapshot);

            var isInvoked = false;

            snapshotManager.SnapshotRemoved += (sender, e) =>
            {
                isInvoked = e.Snapshot.Title.EqualsIgnoreCase(snapshot.Title);
            };

            snapshotManager.Remove(snapshot);

            Assert.That(isInvoked, Is.True);
        }

        [Test]
        public void Raises_SnapshotsChanged_Event()
        {
            var snapshot = new Snapshot
            {
                Title = "Test"
            };

            var snapshotManager = CreateSnapshotManager();
            snapshotManager.Add(snapshot);

            var isInvoked = false;

            snapshotManager.SnapshotsChanged += (sender, e) =>
            {
                isInvoked = true;
            };

            snapshotManager.Remove(snapshot);

            Assert.That(isInvoked, Is.True);
        }
    }
}
