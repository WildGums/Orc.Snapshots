﻿namespace Orc.Snapshots.Tests.Managers;

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

            Assert.That(provider.TestData, Is.EqualTo("1234"));
        }
    }
}