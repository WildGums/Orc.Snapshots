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
    using NUnit.Framework;
    using Path = Catel.IO.Path;

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
                var bytes = File.ReadAllBytes(fileName);

                await snapshot.InitializeFromBytesAsync(bytes);

                Assert.AreEqual(0, snapshot.Keys.Count);
            }

            [Test]
            public async Task InitializesSnapshotWithDataAsync()
            {
                var snapshot = new Snapshot();

                var fileName = GetExampleFileName("Snapshots.WithData.zip");
                var bytes = File.ReadAllBytes(fileName);

                await snapshot.InitializeFromBytesAsync(bytes);

                Assert.AreEqual(4, snapshot.Keys.Count);
                Assert.AreEqual(Encoding.UTF8.GetBytes("123"), snapshot.GetData("Data A"));
                Assert.AreEqual(Encoding.UTF8.GetBytes("456"), snapshot.GetData("Data B"));
                Assert.AreEqual(Encoding.UTF8.GetBytes("789"), snapshot.GetData("Data C"));
                Assert.AreEqual(LargeStringBytes, snapshot.GetData("Large Data"));
            }

            private string GetExampleFileName(string relativeFileName)
            {
                var rootDirectory = AssemblyDirectoryHelper.GetCurrentDirectory();

                var path = Path.Combine(rootDirectory, "Models", relativeFileName);
                return path;
            }
        }
    }
}