namespace Orc.Snapshots.Views;

using System.Windows.Automation.Peers;
using Automation;
using ViewModels;

public partial class SnapshotWindow
{
    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new SnapshotWindowPeer(this);
    }
}
