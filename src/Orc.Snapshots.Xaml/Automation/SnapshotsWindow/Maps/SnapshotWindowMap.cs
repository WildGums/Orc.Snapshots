namespace Orc.Snapshots.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public class SnapshotWindowMap : AutomationBase
{
    public SnapshotWindowMap(AutomationElement element) 
        : base(element)
    {
    }

    //   public Text TitleText => By.One<Text>();
    public Edit? TitleEdit => By.One<Edit>();
    public Button? OkButton => By.Name("OK").One<Button>();
    public Button? CancelButton => By.Name("Cancel").One<Button>();
}
