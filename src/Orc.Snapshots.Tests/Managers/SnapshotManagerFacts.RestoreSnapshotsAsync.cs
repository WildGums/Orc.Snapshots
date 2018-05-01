// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotManagerFacts.RestoreSnapshotsAsync.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots.Tests.Managers
{
    using System.Text;
    using System.Threading.Tasks;
    using Catel.IoC;
    using NUnit.Framework;
    using Providers;

    public partial class SnapshotManagerFacts
    {

        [TestFixture]
        public class TheRestoreSnapshotAsyncMethod
        {
            [Test]
            public async Task CreatesSnapshotAsync()
            {
                var snapshotManager = CreateSnapshotManager();
                var provider = new TestSnapshotProvider(snapshotManager, ServiceLocator.Default);

                snapshotManager.AddProvider(provider);

                provider.TestData = "1234";

                var snapshot = await snapshotManager.CreateSnapshotAsync("My title");

                provider.TestData = "5678";

                await snapshotManager.RestoreSnapshotAsync(snapshot);

                Assert.AreEqual("1234", provider.TestData);
            }
        }
    }
}