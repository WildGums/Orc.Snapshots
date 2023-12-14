namespace Orc.Snapshots.Tests.Managers;

using System.Linq;
using Catel;
using NUnit.Framework;

public partial class SnapshotManagerFacts
{
    [TestFixture]
    public class TheRemoveMethod
    {
        [Test]
        public void RemovesSnapshotFromSnapshotsList()
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
        public void ReturnsFalseWhenSnapshotCannotBeRemoved()
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
        public void RaisesSnapshotRemovedEvent()
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
        public void RaisesSnapshotsChangedEvent()
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
