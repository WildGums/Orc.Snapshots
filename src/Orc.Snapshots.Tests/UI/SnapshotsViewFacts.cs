namespace Orc.Snapshots.Tests
{
    using Automation;
    using NUnit.Framework;
    using Orc.Automation;

    [Explicit]
    [TestFixture]
    public class SnapshotsViewFacts : StyledControlTestFacts<Views.SnapshotsView>
    {
        [Target]
        public SnapshotsView Target { get; set; }

        protected override void InitializeTarget(string id)
        {
            base.InitializeTarget(id);

            var target = Target;

            //target.Execute<InitWorkspacesViewMethodRun>();
        }

        [Test]
        public void Correctly()
        {
            var target = Target;

            var categories = target.SnapshotCategories;
        }
    }
}
