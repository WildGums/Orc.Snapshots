namespace Orc.Snapshots.Tests;

using System.IO;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

public partial class SnapshotFacts
{
    [TestFixture]
    public class TheInitializeFromBytesAsyncMethod
    {
        [Test]
        public async Task InitializesSnapshotWithoutDataAsync()
        {
            var snapshot = new Snapshot();

            var fileName = GetExampleFileName("Snapshots.Empty.zip");
            var bytes = await File.ReadAllBytesAsync(fileName);

            await snapshot.InitializeFromBytesAsync(bytes);

            Assert.That(snapshot.Keys.Length, Is.EqualTo(0));
        }

        [Test]
        public async Task InitializesSnapshotWithDataAsync()
        {
            var snapshot = new Snapshot();

            var fileName = GetExampleFileName("Snapshots.WithData.zip");
            var bytes = await File.ReadAllBytesAsync(fileName);

            await snapshot.InitializeFromBytesAsync(bytes);

            Assert.That(snapshot.Keys.Length, Is.EqualTo(4));
            Assert.That(snapshot.GetData("Data A"), Is.EqualTo(Encoding.UTF8.GetBytes("123")));
            Assert.That(snapshot.GetData("Data B"), Is.EqualTo(Encoding.UTF8.GetBytes("456")));
            Assert.That(snapshot.GetData("Data C"), Is.EqualTo(Encoding.UTF8.GetBytes("789")));
            Assert.That(snapshot.GetData("Large Data"), Is.EqualTo(LargeStringBytes));
        }

        private string GetExampleFileName(string relativeFileName)
        {
            var rootDirectory = AssemblyDirectoryHelper.GetCurrentDirectory();

            var path = System.IO.Path.Combine(rootDirectory, "Models", relativeFileName);
            return path;
        }
    }
}
