namespace Orc.Snapshots.Models;

using Catel.Data;

public class Company : ModelBase
{
    public Company()
    {
        Name = string.Empty;
    }

    public string Name { get; set; }
}
