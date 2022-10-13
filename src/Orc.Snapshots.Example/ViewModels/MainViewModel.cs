namespace Orc.Snapshots.Example.ViewModels
{
    using System;
    using Catel.Logging;
    using Catel.MVVM;
    using Models;

    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public MainViewModel(Project project)
        {
            ArgumentNullException.ThrowIfNull(project);

            Person = project.Person;
            Company = project.Company;

            FillData = new Command(OnFillDataExecute);
            ClearData = new Command(OnClearDataExecute);
        }

        public Person Person { get; private set; }

        public Company Company { get; private set; }

        public Command FillData { get; private set; }

        private void OnFillDataExecute()
        {
            Person.FirstName = "Geert";
            Person.LastName = "van Horrik";
            Company.Name = "CatenaLogic";
        }

        public Command ClearData { get; private set; }

        private void OnClearDataExecute()
        {
            Person.FirstName = string.Empty;
            Person.LastName = string.Empty;
            Company.Name = string.Empty;
        }
    }
}
