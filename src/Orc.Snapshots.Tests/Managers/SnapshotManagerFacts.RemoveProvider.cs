namespace Orc.Snapshots.Tests.Managers;

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

            Assert.That(snapshotManager.Providers.Count(), Is.EqualTo(1));

            var result = snapshotManager.RemoveProvider(provider);

            Assert.That(result, Is.True);
            Assert.That(snapshotManager.Providers.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ReturnsFalseWhenProviderCannotBeRemoved()
        {
            var snapshotManager = CreateSnapshotManager();
            var provider = new TestSnapshotProvider(snapshotManager, ServiceLocator.Default);

            Assert.That(snapshotManager.Providers.Count(), Is.EqualTo(0));

            var result = snapshotManager.RemoveProvider(provider);

            Assert.That(result, Is.False);
            Assert.That(snapshotManager.Providers.Count(), Is.EqualTo(0));
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

            Assert.That(isInvoked, Is.True);
        }
    }
}