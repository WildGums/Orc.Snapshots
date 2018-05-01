// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotManagerFacts.Remove.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots.Tests.Managers
{
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

                Assert.AreEqual(1, snapshotManager.Snapshots.Count());

                var result = snapshotManager.Remove(snapshot);

                Assert.IsTrue(result);
                Assert.AreEqual(0, snapshotManager.Snapshots.Count());
            }

            [Test]
            public void ReturnsFalseWhenSnapshotCannotBeRemoved()
            {
                var snapshot = new Snapshot
                {
                    Title = "Test"
                };

                var snapshotManager = CreateSnapshotManager();

                Assert.AreEqual(0, snapshotManager.Snapshots.Count());

                var result = snapshotManager.Remove(snapshot);

                Assert.IsFalse(result);
                Assert.AreEqual(0, snapshotManager.Snapshots.Count());
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
                snapshotManager.Add(snapshot);

                var isInvoked = false;

                snapshotManager.SnapshotsChanged += (sender, e) =>
                {
                    isInvoked = true;
                };

                snapshotManager.Remove(snapshot);

                Assert.IsTrue(isInvoked);
            }
        }
    }
}