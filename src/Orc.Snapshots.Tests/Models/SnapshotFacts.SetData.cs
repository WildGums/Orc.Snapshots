namespace Orc.Snapshots.Tests.Models
{
    using System;
    using NUnit.Framework;

    public partial class SnapshotFacts
    {
        [TestFixture]
        public class TheSetDataMethod
        {
            [Test]
            public void SetsData()
            {
                var snapshot = new Snapshot();

                var testBytes = new byte[] { 1, 2, 3 };

                Assert.AreEqual(Array.Empty<byte>(), snapshot.GetData("MyData"));

                snapshot.SetData("MyData", testBytes);

                Assert.AreEqual(testBytes, snapshot.GetData("MyData"));
            }

            [Test]
            public void OverwritesData()
            {
                var snapshot = new Snapshot();

                snapshot.SetData("MyData", new byte[] { 4, 5, 6 });

                var testBytes = new byte[] { 1, 2, 3 };

                snapshot.SetData("MyData", testBytes);

                Assert.AreEqual(testBytes, snapshot.GetData("MyData"));
            }

            [Test]
            public void ClearsData()
            {
                var snapshot = new Snapshot();

                snapshot.SetData("MyData", new byte[] { 4, 5, 6 });
                snapshot.SetData("MyData", null);

                Assert.AreEqual(Array.Empty<byte>(), snapshot.GetData("MyData"));
            }
        }
    }
}
