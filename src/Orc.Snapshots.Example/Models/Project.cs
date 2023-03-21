namespace Orc.Snapshots.Models;

public class Project
{
    public Project()
    {
        Person = new Person();
        Company = new Company();
    }

    public Person Person { get; }

    public Company Company { get; }
}
