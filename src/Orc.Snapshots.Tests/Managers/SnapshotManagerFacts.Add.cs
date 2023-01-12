namespace Orc.Snapshots.Tests.Managers
{
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

                Assert.AreEqual(0, snapshotManager.Snapshots.Count());

                snapshotManager.Add(snapshot);

                var snapshots = snapshotManager.Snapshots.ToList();
                Assert.AreEqual(1, snapshots.Count);
                Assert.AreEqual(snapshot.Title, snapshots[0].Title);
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

                Assert.IsTrue(isInvoked);
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

                Assert.IsTrue(isInvoked);
            }
            #endregion
        }
        #endregion
    }
}