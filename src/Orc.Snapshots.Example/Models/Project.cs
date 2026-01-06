namespace Orc.Snapshots.Models;

using Catel.IoC;

public class Project : IConstructAtStartup
{
    public Project()
    {
        Person = new Person();
        Company = new Company();
    }

    public Person Person { get; }

    public Company Company { get; }
}
