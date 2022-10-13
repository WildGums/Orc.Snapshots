namespace Orc.Snapshots.Models
{
    public class Project
    {
        public Project()
        {
            Person = new Person();
            Company = new Company();
        }

        public Person Person { get; private set; }

        public Company Company { get; private set; }
    }
}
