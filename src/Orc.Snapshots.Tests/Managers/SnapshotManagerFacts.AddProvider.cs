// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotManager.AddProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------



namespace Orc.Snapshots.Tests.Managers
{
    using Catel;
    using NUnit.Framework;
    using System.Linq;
    using Catel.IoC;
    using Providers;

    public partial class SnapshotManagerFacts
    {
        [TestFixture]
        public class TheAddProviderMethod
        {
            [Test]
            public void AddsProviderToProvidersList()
            {
                var snapshotManager = CreateSnapshotManager();
                var provider = new TestSnapshotProvider(snapshotManager, ServiceLocator.Default);

                Assert.AreEqual(0, snapshotManager.Providers.Count());

                snapshotManager.AddProvider(provider);

                var providers = snapshotManager.Providers.ToList();
                Assert.AreEqual(1, providers.Count);
                Assert.AreEqual(provider.Name, providers[0].Name);
            }

            [Test]
            public void RaisesSnapshotProviderAddedEvent()
            {
                var snapshotManager = CreateSnapshotManager();
                var provider = new TestSnapshotProvider(snapshotManager, ServiceLocator.Default);

                var isInvoked = false;

                snapshotManager.SnapshotProviderAdded += (sender, e) =>
                {
                    isInvoked = e.SnapshotProvider.Name.EqualsIgnoreCase(provider.Name);
                };

                snapshotManager.AddProvider(provider);

                Assert.IsTrue(isInvoked);
            }
        }
    }
}
