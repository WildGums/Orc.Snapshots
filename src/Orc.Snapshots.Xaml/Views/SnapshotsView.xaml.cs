namespace Orc.Snapshots.Views;

using System.Windows.Automation.Peers;

/// <summary>
/// Interaction logic for SnapshotsView.xaml.
/// </summary>
public partial class SnapshotsView
{
    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new Automation.SnapshotsViewPeer(this);
    }
}
