namespace Orc.Snapshots.Tests;

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

            Assert.That(snapshot.GetData("MyData"), Is.EqualTo(Array.Empty<byte>()));
        }

        [Test]
        public void ReturnsDataIfExists()
        {
            var snapshot = new Snapshot();

            var testBytes = new byte[] { 1, 2, 3 };
            snapshot.SetData("MyData", testBytes);

            Assert.That(snapshot.GetData("MyData"), Is.EqualTo(testBytes));
        }
    }
}
