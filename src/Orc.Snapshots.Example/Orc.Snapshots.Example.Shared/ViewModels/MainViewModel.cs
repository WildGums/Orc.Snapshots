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
        }

        public Person Person { get; private set; }

        public Company Company { get; private set; }
    }
}