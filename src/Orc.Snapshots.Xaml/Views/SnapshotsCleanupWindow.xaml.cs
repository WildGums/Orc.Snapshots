namespace Orc.Snapshots.Views;

public sealed partial class SnapshotsCleanupWindow
{
    partial void OnInitializingComponent()
    {
        Mode = Catel.Windows.DataWindowMode.Close;
    }
}
