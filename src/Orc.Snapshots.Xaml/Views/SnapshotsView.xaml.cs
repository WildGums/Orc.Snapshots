namespace Orc.Snapshots.Views;

using System.Windows;
using System.Windows.Automation.Peers;
using Catel.MVVM.Views;
using SnapshotsViewModel = ViewModels.SnapshotsViewModel;

/// <summary>
/// Interaction logic for SnapshotsView.xaml.
/// </summary>
public partial class SnapshotsView
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SnapshotsView"/> class.
    /// </summary>
    public SnapshotsView()
    {
        InitializeComponent();
    }

    [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
    public object? Scope
    {
        get { return GetValue(ScopeProperty); }
        set { SetValue(ScopeProperty, value); }
    }

    public static readonly DependencyProperty ScopeProperty = DependencyProperty.Register(nameof(Scope), typeof(object),
        typeof(SnapshotsView), new FrameworkPropertyMetadata((sender, e) => ((SnapshotsView)sender).OnScopeChanged()));

    private void OnScopeChanged()
    {
        if (ViewModel is SnapshotsViewModel vm)
        {
            vm.Scope = Scope;
        }
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new Automation.SnapshotsViewPeer(this);
    }
}
