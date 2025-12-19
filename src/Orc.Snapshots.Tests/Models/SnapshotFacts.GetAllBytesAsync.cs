namespace Orc.Snapshots.Tests;

using System.IO;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using VerifyNUnit;

public partial class SnapshotFacts
{
    // Note: tests are explicit because zipped files are always different (date/time, etc)

    [TestFixture]
    public class TheGetAllBytesAsyncMethod
    {
        [Test, Explicit]
        public async Task ReturnsBytesForEmptySnapshotAsync()
        {
            using var fileContext = new TemporaryFilesContext("ReturnsBytesForEmptySnapshotAsync", false);
            var snapshot = new Snapshot();

            var bytes = await snapshot.GetAllBytesAsync();

            var outputFileName = fileContext.GetFile("snapshot.zip");
            await File.WriteAllBytesAsync(outputFileName, bytes);

            //Approvals.VerifyBinaryFile(bytes, "zip");
            await Verifier.VerifyFile(outputFileName);
        }

        [Test, Explicit]
        public async Task ReturnsBytesForSnapshotAsync()
        {
            using var fileContext = new TemporaryFilesContext("ReturnsBytesForSnapshotAsync", false);
            var snapshot = new Snapshot();
            snapshot.SetData("Data A", Encoding.UTF8.GetBytes("123"));
            snapshot.SetData("Data B", Encoding.UTF8.GetBytes("456"));
            snapshot.SetData("Data C", Encoding.UTF8.GetBytes("789"));
            snapshot.SetData("Large Data", LargeStringBytes);

            var bytes = await snapshot.GetAllBytesAsync();

            var outputFileName = fileContext.GetFile("snapshot.zip");
            await File.WriteAllBytesAsync(outputFileName, bytes);

            //Approvals.VerifyFile(bytes, "zip");
            await Verifier.VerifyFile(outputFileName);
        }

        [Test, Explicit]
        public async Task ReturnsBytesForUpdatedSnapshotAsync()
        {
            using var fileContext = new TemporaryFilesContext("ReturnsBytesForUpdatedSnapshotAsync", false);
            var snapshot = new Snapshot();
            snapshot.SetData("Data A", Encoding.UTF8.GetBytes("123"));
            snapshot.SetData("Large Data", LargeStringBytes);

            var bytes1 = await snapshot.GetAllBytesAsync();

            // Now we update and we should have an updated byte array

            snapshot.SetData("Data A", Encoding.UTF8.GetBytes("123"));
            snapshot.SetData("Data B", Encoding.UTF8.GetBytes("456"));
            snapshot.SetData("Data C", Encoding.UTF8.GetBytes("789"));
            snapshot.SetData("Large Data", LargeStringBytes);

            var bytes2 = await snapshot.GetAllBytesAsync();

            Assert.That(bytes2, Is.Not.EqualTo(bytes1));

            var outputFileName = fileContext.GetFile("snapshot.zip");
            await File.WriteAllBytesAsync(outputFileName, bytes2);

            //Approvals.VerifyBinaryFile(bytes2, "zip");
            await Verifier.VerifyFile(outputFileName);
        }
    }
}
