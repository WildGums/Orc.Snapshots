namespace Orc.Snapshots.Tests.Managers;

using System.Linq;
using Catel;
using Catel.IoC;
using NUnit.Framework;
using Providers;

public partial class SnapshotManagerFacts
{
    [TestFixture]
    public class The_RemoveProvider_Method
    {
        [Test]
        public void Removes_Provider_From_Providers_List()
        {
            var snapshotManager = CreateSnapshotManager();
            var provider = new TestSnapshotProvider(snapshotManager);

            snapshotManager.AddProvider(provider);

            Assert.That(snapshotManager.Providers.Count(), Is.EqualTo(1));

            var result = snapshotManager.RemoveProvider(provider);

            Assert.That(result, Is.True);
            Assert.That(snapshotManager.Providers.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Returns_False_When_Provider_Cannot_Be_Removed()
        {
            var snapshotManager = CreateSnapshotManager();
            var provider = new TestSnapshotProvider(snapshotManager);

            Assert.That(snapshotManager.Providers.Count(), Is.EqualTo(0));

            var result = snapshotManager.RemoveProvider(provider);

            Assert.That(result, Is.False);
            Assert.That(snapshotManager.Providers.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Raises_SnapshotProviderRemoved_Event()
        {
            var snapshotManager = CreateSnapshotManager();
            var provider = new TestSnapshotProvider(snapshotManager);

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
