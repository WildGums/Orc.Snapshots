namespace Orc.Snapshots;

using System.Collections.Generic;

public class SnapshotCategory
{
    public SnapshotCategory()
    {
        Category = string.Empty;
        Snapshots = new List<ISnapshot>();
    }

    public string Category { get; set; }

    public List<ISnapshot> Snapshots { get; private set; }
}
