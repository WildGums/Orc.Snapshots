namespace Orc.Snapshots.Tests.Models;

using System;
using NUnit.Framework;

public partial class SnapshotFacts
{
    [TestFixture]
    public class TheGetDataMethod
    {
        [Test]
        public void ReturnsEmptyIfNotExists()
        {
            var snapshot = new Snapshot();

            Assert.AreEqual(Array.Empty<byte>(), snapshot.GetData("MyData"));
        }

        [Test]
        public void ReturnsDataIfExists()
        {
            var snapshot = new Snapshot();

            var testBytes = new byte[] { 1, 2, 3 };
            snapshot.SetData("MyData", testBytes);

            Assert.AreEqual(testBytes, snapshot.GetData("MyData"));
        }
    }
}