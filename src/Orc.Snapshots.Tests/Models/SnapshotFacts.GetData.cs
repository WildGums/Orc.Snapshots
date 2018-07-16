// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotFacts.GetData.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots.Tests.Models
{
    using NUnit.Framework;

    public partial class SnapshotFacts
    {
        [TestFixture]
        public class TheGetDataMethod
        {
            [Test]
            public void ReturnsNullIfNotExists()
            {
                var snapshot = new Snapshot();

                Assert.AreEqual(null, snapshot.GetData("MyData"));
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
}