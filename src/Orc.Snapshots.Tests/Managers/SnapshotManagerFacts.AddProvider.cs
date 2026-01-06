namespace Orc.Snapshots.Tests.Managers;

using Catel;
using NUnit.Framework;
using System.Linq;
using Catel.IoC;
using Providers;

public partial class SnapshotManagerFacts
{
    [TestFixture]
    public class The_AddProvider_Method
    {
        [Test]
        public void Adds_Provider_To_Providers_List()
        {
            var snapshotManager = CreateSnapshotManager();
            var provider = new TestSnapshotProvider();

            Assert.That(snapshotManager.Providers.Count(), Is.EqualTo(0));

            snapshotManager.AddProvider(provider);

            var providers = snapshotManager.Providers.ToList();
            Assert.That(providers.Count, Is.EqualTo(1));
            Assert.That(providers[0].Name, Is.EqualTo(provider.Name));
        }

        [Test]
        public void Raises_Snapshot_Provider_Added_Event()
        {
            var snapshotManager = CreateSnapshotManager();
            var provider = new TestSnapshotProvider();

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
