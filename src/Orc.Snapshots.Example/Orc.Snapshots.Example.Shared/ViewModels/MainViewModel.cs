// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots.Example.ViewModels
{
    using Catel;
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
            Argument.IsNotNull(() => project);

            Person = project.Person;
            Company = project.Company;

            FillData = new Command(OnFillDataExecute);
            ClearData = new Command(OnClearDataExecute);
        }

        public Person Person { get; private set; }

        public Company Company { get; private set; }

        #region Commands
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
        #endregion
    }
}