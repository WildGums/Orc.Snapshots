namespace Orc.Snapshots.Example.ViewModels;

using System;
using Catel.MVVM;
using Models;

public class MainViewModel : ViewModelBase
{
    public MainViewModel(Project project, IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(project);

        Person = project.Person;
        Company = project.Company;

        FillData = new Command(serviceProvider, OnFillDataExecute);
        ClearData = new Command(serviceProvider, OnClearDataExecute);
    }

    public Person Person { get; }

    public Company Company { get; }

    public Command FillData { get; }

    private void OnFillDataExecute()
    {
        Person.FirstName = "John";
        Person.LastName = "Doe";
        Company.Name = "Some company";
    }

    public Command ClearData { get; }

    private void OnClearDataExecute()
    {
        Person.FirstName = string.Empty;
        Person.LastName = string.Empty;
        Company.Name = string.Empty;
    }
}
