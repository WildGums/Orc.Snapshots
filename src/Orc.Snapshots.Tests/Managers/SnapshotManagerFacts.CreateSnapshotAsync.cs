namespace Orc.Snapshots.Tests.Managers;

using System.Text;
using System.Threading.Tasks;
using Catel.IoC;
using NUnit.Framework;
using Providers;

public partial class SnapshotManagerFacts
{
    [TestFixture]
    public class TheCreateSnapshotAsyncMethod
    {
        [Test]
        public async Task CreatesSnapshotAsync()
        {
            var snapshotManager = CreateSnapshotManager();
            var provider = new TestSnapshotProvider(snapshotManager, ServiceLocator.Default);

            snapshotManager.AddProvider(provider);

            provider.TestData = "1234";
            var testBytes = Encoding.UTF8.GetBytes(provider.TestData);

            var snapshot = await snapshotManager.CreateSnapshotAsync("My title");

            Assert.AreEqual("My title", snapshot.Title);
            Assert.AreEqual(testBytes, snapshot.GetData(provider.Name));
        }
    }
}