namespace Orc.Snapshots.Views;

using System.Windows.Automation.Peers;
using Automation;
using ViewModels;

public partial class SnapshotWindow
{
    partial void OnInitializingComponent()
    {
        Mode = Catel.Windows.DataWindowMode.OkCancel;
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new SnapshotWindowPeer(this);
    }
}
