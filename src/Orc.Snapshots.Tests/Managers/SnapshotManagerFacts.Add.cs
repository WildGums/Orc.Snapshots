namespace Orc.Snapshots.Tests.Managers;

using Catel;
using NUnit.Framework;
using System.Linq;

public partial class SnapshotManagerFacts
{
    #region Nested type: TheAddMethod
    [TestFixture]
    public class TheAddMethod
    {
        #region Methods
        [Test]
        public void AddsSnapshotToSnapshotsList()
        {
            var snapshot = new Snapshot
            {
                Title = "Test"
            };

            var snapshotManager = CreateSnapshotManager();

            Assert.That(snapshotManager.Snapshots.Count(), Is.EqualTo(0));

            snapshotManager.Add(snapshot);

            var snapshots = snapshotManager.Snapshots.ToList();
            Assert.That(snapshots.Count, Is.EqualTo(1));
            Assert.That(snapshots[0].Title, Is.EqualTo(snapshot.Title));
        }

        [Test]
        public void RaisesSnapshotAddedEvent()
        {
            var snapshot = new Snapshot
            {
                Title = "Test"
            };

            var snapshotManager = CreateSnapshotManager();

            var isInvoked = false;

            snapshotManager.SnapshotAdded += (sender, e) => { isInvoked = e.Snapshot.Title.EqualsIgnoreCase(snapshot.Title); };

            snapshotManager.Add(snapshot);

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

            var isInvoked = false;

            snapshotManager.SnapshotsChanged += (sender, e) => { isInvoked = true; };

            snapshotManager.Add(snapshot);

            Assert.That(isInvoked, Is.True);
        }
        #endregion
    }
    #endregion
}
