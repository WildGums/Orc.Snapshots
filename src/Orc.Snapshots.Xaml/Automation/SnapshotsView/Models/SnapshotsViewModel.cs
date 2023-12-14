namespace Orc.Snapshots.Automation;

using Orc.Automation;

[ActiveAutomationModel]
public class SnapshotsViewModel : ControlModel
{
    public SnapshotsViewModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {              
    }

    public object? Scope { get; set; }
}
