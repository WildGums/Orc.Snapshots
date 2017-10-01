// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PublicApiFacts.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots.Tests
{
    using ApiApprover;
    using NUnit.Framework;
    using Views;

    [TestFixture]
    public class PublicApiFacts
    {
        [Test]
        public void Orc_Snapshots_HasNoBreakingChanges()
        {
            var assembly = typeof(SnapshotManager).Assembly;

            PublicApiApprover.ApprovePublicApi(assembly);
        }

        [Test]
        public void Orc_Snapshots_Xaml_HasNoBreakingChanges()
        {
            var assembly = typeof(SnapshotsView).Assembly;

            PublicApiApprover.ApprovePublicApi(assembly);
        }
    }
}