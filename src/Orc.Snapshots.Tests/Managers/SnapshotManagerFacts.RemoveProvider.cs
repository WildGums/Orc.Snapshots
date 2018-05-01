// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotManagerFacts.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots.Tests.Managers
{
    using System.Linq;
    using Catel;
    using Catel.IoC;
    using NUnit.Framework;
    using Providers;

    public partial class SnapshotManagerFacts
    {
        [TestFixture]
        public class TheRemoveProviderMethod
        {
            [Test]
            public void RemovesProviderFromProvidersList()
            {
                var snapshotManager = CreateSnapshotManager();
                var provider = new TestSnapshotProvider(snapshotManager, ServiceLocator.Default);

                snapshotManager.AddProvider(provider);

                Assert.AreEqual(1, snapshotManager.Providers.Count());

                var result = snapshotManager.RemoveProvider(provider);

                Assert.IsTrue(result);
                Assert.AreEqual(0, snapshotManager.Providers.Count());
            }

            [Test]
            public void ReturnsFalseWhenProviderCannotBeRemoved()
            {
                var snapshotManager = CreateSnapshotManager();
                var provider = new TestSnapshotProvider(snapshotManager, ServiceLocator.Default);

                Assert.AreEqual(0, snapshotManager.Providers.Count());

                var result = snapshotManager.RemoveProvider(provider);

                Assert.IsFalse(result);
                Assert.AreEqual(0, snapshotManager.Providers.Count());
            }

            [Test]
            public void RaisesSnapshotProviderRemovedEvent()
            {
                var snapshotManager = CreateSnapshotManager();
                var provider = new TestSnapshotProvider(snapshotManager, ServiceLocator.Default);

                snapshotManager.AddProvider(provider);

                var isInvoked = false;

                snapshotManager.SnapshotProviderRemoved += (sender, e) =>
                {
                    isInvoked = e.SnapshotProvider.Name.EqualsIgnoreCase(provider.Name);
                };

                snapshotManager.RemoveProvider(provider);

                Assert.IsTrue(isInvoked);
            }
        }
    }
}