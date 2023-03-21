namespace Orc.Snapshots.Models;

using Catel.Data;

public class Person : ModelBase
{
    public Person()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
    }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}
