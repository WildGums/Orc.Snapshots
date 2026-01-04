namespace Orc.Snapshots.Tests.Managers;

using System.Threading.Tasks;
using Catel.IoC;
using NUnit.Framework;
using Providers;

public partial class SnapshotManagerFacts
{

    [TestFixture]
    public class The_RestoreSnapshotAsync_Method
    {
        [Test]
        public async Task Restores_Snapshot_Async()
        {
            var snapshotManager = CreateSnapshotManager();
            var provider = new TestSnapshotProvider(snapshotManager);

            snapshotManager.AddProvider(provider);

            provider.TestData = "1234";

            var snapshot = await snapshotManager.CreateSnapshotAsync("My title");

            provider.TestData = "5678";

            await snapshotManager.RestoreSnapshotAsync(snapshot);

            Assert.That(provider.TestData, Is.EqualTo("1234"));
        }
    }
}
