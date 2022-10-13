namespace Orc.Snapshots.Views
{
    using System.Windows.Automation.Peers;
    using Automation;
    using ViewModels;

    public partial class SnapshotWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SnapshotWindow"/> class.
        /// </summary>
        public SnapshotWindow()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapshotWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public SnapshotWindow(SnapshotViewModel? viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new SnapshotWindowPeer(this);
        }
    }
}
