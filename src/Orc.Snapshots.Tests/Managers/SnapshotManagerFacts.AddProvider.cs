namespace Orc.Snapshots.Tests.Managers;

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

            Assert.That(snapshotManager.Providers.Count(), Is.EqualTo(0));

            snapshotManager.AddProvider(provider);

            var providers = snapshotManager.Providers.ToList();
            Assert.That(providers.Count, Is.EqualTo(1));
            Assert.That(providers[0].Name, Is.EqualTo(provider.Name));
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

            Assert.That(isInvoked, Is.True);
        }
    }
}