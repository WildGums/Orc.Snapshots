// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotFacts.GetData.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots.Tests.Models
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using ApprovalTests;
    using NUnit.Framework;

    public partial class SnapshotFacts
    {
        // Note: tests are explicit because zipped files are always different (date/time, etc)

        [TestFixture]
        public class TheGetAllBytesAsyncMethod
        {
            [Test, Explicit]
            public async Task ReturnsBytesForEmptySnapshotAsync()
            {
                using (var fileContext = new TemporaryFilesContext("ReturnsBytesForEmptySnapshotAsync", false))
                {
                    var snapshot = new Snapshot();

                    var bytes = await snapshot.GetAllBytesAsync();

                    var outputFileName = fileContext.GetFile("snapshot.zip");
                    File.WriteAllBytes(outputFileName, bytes);

                    Approvals.VerifyBinaryFile(bytes, "zip");
                    //Approvals.VerifyFile(outputFileName);
                }
            }

            [Test, Explicit]
            public async Task ReturnsBytesForSnapshotAsync()
            {
                using (var fileContext = new TemporaryFilesContext("ReturnsBytesForSnapshotAsync", false))
                {
                    var snapshot = new Snapshot();
                    snapshot.SetData("Data A", Encoding.UTF8.GetBytes("123"));
                    snapshot.SetData("Data B", Encoding.UTF8.GetBytes("456"));
                    snapshot.SetData("Data C", Encoding.UTF8.GetBytes("789"));
                    snapshot.SetData("Large Data", LargeStringBytes);

                    var bytes = await snapshot.GetAllBytesAsync();

                    var outputFileName = fileContext.GetFile("snapshot.zip");
                    File.WriteAllBytes(outputFileName, bytes);

                    Approvals.VerifyBinaryFile(bytes, "zip");
                    //Approvals.VerifyFile(outputFileName);
                }
            }

            [Test, Explicit]
            public async Task ReturnsBytesForUpdatedSnapshotAsync()
            {
                using (var fileContext = new TemporaryFilesContext("ReturnsBytesForUpdatedSnapshotAsync", false))
                {
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

                    Assert.AreNotEqual(bytes1, bytes2);

                    var outputFileName = fileContext.GetFile("snapshot.zip");
                    File.WriteAllBytes(outputFileName, bytes2);

                    Approvals.VerifyBinaryFile(bytes2, "zip");
                    //Approvals.VerifyFile(outputFileName);
                }
            }

        }
    }
}