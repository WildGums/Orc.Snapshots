// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotWindow.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots.Views
{
    using ViewModels;

    public partial class SnapshotWindow
    {
        #region Constructors
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
        public SnapshotWindow(SnapshotViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
        #endregion
    }
}