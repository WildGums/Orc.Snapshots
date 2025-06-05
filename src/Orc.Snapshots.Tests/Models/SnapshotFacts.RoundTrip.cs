namespace Orc.Snapshots.Tests.Models;

using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

public partial class SnapshotFacts
{
    [TestFixture]
    public class TheRoundTripBehavior
    {
        [Test]
        public async Task SaveAndLoadSnapshotWithEmptyDataAsync()
        {
            var originalSnapshot = new Snapshot
            {
                Title = "Test Snapshot",
                Category = "Test Category"
            };

            var bytes = await originalSnapshot.GetAllBytesAsync();

            var loadedSnapshot = new Snapshot();
            await loadedSnapshot.InitializeFromBytesAsync(bytes);

            Assert.That(loadedSnapshot.Keys.Length, Is.EqualTo(0));
        }

        [Test]
        public async Task SaveAndLoadSnapshotWithSingleDataItemAsync()
        {
            var originalSnapshot = new Snapshot
            {
                Title = "Test Snapshot",
                Category = "Test Category"
            };
            var testData = Encoding.UTF8.GetBytes("Test data content");
            originalSnapshot.SetData("TestKey", testData);

            var bytes = await originalSnapshot.GetAllBytesAsync();

            var loadedSnapshot = new Snapshot();
            await loadedSnapshot.InitializeFromBytesAsync(bytes);

            Assert.That(loadedSnapshot.Keys.Length, Is.EqualTo(1));
            Assert.That(loadedSnapshot.Keys[0], Is.EqualTo("TestKey"));
            Assert.That(loadedSnapshot.GetData("TestKey"), Is.EqualTo(testData));
        }

        [Test]
        public async Task SaveAndLoadSnapshotWithMultipleDataItemsAsync()
        {
            var originalSnapshot = new Snapshot
            {
                Title = "Multi Data Snapshot",
                Category = "Test Category"
            };

            var testData1 = Encoding.UTF8.GetBytes("First test data");
            var testData2 = Encoding.UTF8.GetBytes("Second test data");
            var testData3 = new byte[] { 0x01, 0x02, 0x03, 0x04, 0xFF };

            originalSnapshot.SetData("Key1", testData1);
            originalSnapshot.SetData("Key2", testData2);
            originalSnapshot.SetData("BinaryKey", testData3);

            var bytes = await originalSnapshot.GetAllBytesAsync();

            var loadedSnapshot = new Snapshot();
            await loadedSnapshot.InitializeFromBytesAsync(bytes);

            Assert.That(loadedSnapshot.Keys.Length, Is.EqualTo(3));
            Assert.That(loadedSnapshot.GetData("Key1"), Is.EqualTo(testData1));
            Assert.That(loadedSnapshot.GetData("Key2"), Is.EqualTo(testData2));
            Assert.That(loadedSnapshot.GetData("BinaryKey"), Is.EqualTo(testData3));
        }

        [Test]
        public async Task SaveAndLoadSnapshotWithLargeDataAsync()
        {
            var originalSnapshot = new Snapshot
            {
                Title = "Large Data Snapshot",
                Category = "Performance Test"
            };

            originalSnapshot.SetData("SmallKey", Encoding.UTF8.GetBytes("Small data"));
            originalSnapshot.SetData("LargeKey", LargeStringBytes);

            var bytes = await originalSnapshot.GetAllBytesAsync();

            var loadedSnapshot = new Snapshot();
            await loadedSnapshot.InitializeFromBytesAsync(bytes);

            Assert.That(loadedSnapshot.Keys.Length, Is.EqualTo(2));
            Assert.That(loadedSnapshot.GetData("SmallKey"), Is.EqualTo(Encoding.UTF8.GetBytes("Small data")));
            Assert.That(loadedSnapshot.GetData("LargeKey"), Is.EqualTo(LargeStringBytes));
        }

        [Test]
        public async Task SaveAndLoadSnapshotWithEmptyDataValueAsync()
        {
            var originalSnapshot = new Snapshot();
            originalSnapshot.SetData("EmptyKey", new byte[0]);
            originalSnapshot.SetData("NormalKey", Encoding.UTF8.GetBytes("Normal data"));

            var bytes = await originalSnapshot.GetAllBytesAsync();

            var loadedSnapshot = new Snapshot();
            await loadedSnapshot.InitializeFromBytesAsync(bytes);

            Assert.That(loadedSnapshot.Keys.Length, Is.EqualTo(2));
            Assert.That(loadedSnapshot.GetData("EmptyKey"), Is.EqualTo(new byte[0]));
            Assert.That(loadedSnapshot.GetData("NormalKey"), Is.EqualTo(Encoding.UTF8.GetBytes("Normal data")));
        }

        [Test]
        public async Task SaveAndLoadSnapshotPreservesContentHashAsync()
        {
            var originalSnapshot = new Snapshot();
            originalSnapshot.SetData("Key1", Encoding.UTF8.GetBytes("Test data"));
            originalSnapshot.SetData("Key2", LargeStringBytes);

            var originalHash = await originalSnapshot.GetContentHashAsync();
            var bytes = await originalSnapshot.GetAllBytesAsync();

            var loadedSnapshot = new Snapshot();
            await loadedSnapshot.InitializeFromBytesAsync(bytes);

            var loadedHash = await loadedSnapshot.GetContentHashAsync();

            Assert.That(loadedHash, Is.EqualTo(originalHash));
        }

        [Test]
        public async Task MultipleRoundTripsProduceSameResultAsync()
        {
            var snapshot1 = new Snapshot();
            snapshot1.SetData("Key1", Encoding.UTF8.GetBytes("Data 1"));
            snapshot1.SetData("Key2", Encoding.UTF8.GetBytes("Data 2"));

            var bytes1 = await snapshot1.GetAllBytesAsync();

            var snapshot2 = new Snapshot();
            await snapshot2.InitializeFromBytesAsync(bytes1);
            var bytes2 = await snapshot2.GetAllBytesAsync();

            var snapshot3 = new Snapshot();
            await snapshot3.InitializeFromBytesAsync(bytes2);
            var bytes3 = await snapshot3.GetAllBytesAsync();

            Assert.That(bytes2, Is.EqualTo(bytes1));
            Assert.That(bytes3, Is.EqualTo(bytes1));
            Assert.That(snapshot3.GetData("Key1"), Is.EqualTo(Encoding.UTF8.GetBytes("Data 1")));
            Assert.That(snapshot3.GetData("Key2"), Is.EqualTo(Encoding.UTF8.GetBytes("Data 2")));
        }

        [Test]
        public async Task LoadSnapshotWithNestedDirectoryStructureAsync()
        {
            using var memoryStream = new System.IO.MemoryStream();
            using (var archive = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
            {
                var entry1 = archive.CreateEntry("folder1/subfolder/file1.dat");
                using (var entryStream = entry1.Open())
                {
                    var data = Encoding.UTF8.GetBytes("Data in nested folder");
                    await entryStream.WriteAsync(data, 0, data.Length);
                }

                var entry2 = archive.CreateEntry("folder2/file2.dat");
                using (var entryStream = entry2.Open())
                {
                    var data = Encoding.UTF8.GetBytes("Data in folder2");
                    await entryStream.WriteAsync(data, 0, data.Length);
                }

                var entry3 = archive.CreateEntry("rootfile.dat");
                using (var entryStream = entry3.Open())
                {
                    var data = Encoding.UTF8.GetBytes("Root level data");
                    await entryStream.WriteAsync(data, 0, data.Length);
                }
            }

            var zipBytes = memoryStream.ToArray();
            var snapshot = new Snapshot();
            await snapshot.InitializeFromBytesAsync(zipBytes);

            Assert.That(snapshot.Keys.Length, Is.EqualTo(3));

            Assert.That(snapshot.GetData(@"folder1\subfolder\file1"), Is.EqualTo(Encoding.UTF8.GetBytes("Data in nested folder")));
            Assert.That(snapshot.GetData(@"folder2\file2"), Is.EqualTo(Encoding.UTF8.GetBytes("Data in folder2")));
            Assert.That(snapshot.GetData("rootfile"), Is.EqualTo(Encoding.UTF8.GetBytes("Root level data")));
        }
    }
}