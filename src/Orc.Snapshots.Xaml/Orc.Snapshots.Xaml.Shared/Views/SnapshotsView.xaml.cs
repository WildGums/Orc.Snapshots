// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotsView.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Snapshots.Views
{
    using System;
    using System.Windows;
    using Catel.MVVM.Views;
    using ViewModels;

    /// <summary>
    /// Interaction logic for SnapshotsView.xaml.
    /// </summary>
    public partial class SnapshotsView
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SnapshotsView"/> class.
        /// </summary>
        public SnapshotsView()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public object Scope
        {
            get { return GetValue(ScopeProperty); }
            set { SetValue(ScopeProperty, value); }
        }

        public static readonly DependencyProperty ScopeProperty = DependencyProperty.Register("Scope", typeof(object),
            typeof(SnapshotsView), new FrameworkPropertyMetadata((sender, e) => ((SnapshotsView)sender).OnScopeChanged(e)));
        #endregion

        #region Methods
        private void OnScopeChanged(DependencyPropertyChangedEventArgs e)
        {
            var vm = ViewModel as SnapshotsViewModel;
            if (vm != null)
            {
                vm.Scope = Scope;
            }
        }
        #endregion
    }
}