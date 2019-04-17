// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationInitializationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Snapshots.Example.Services
{
    using System;
    using Orchestra.Services;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using Catel;
    using Catel.IoC;
    using Catel.Threading;
    using Models;
    using Orchestra.Markup;
    using Snapshots;
    using Snapshots.Providers;
    using Watchers;

    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        private readonly IServiceLocator _serviceLocator;

        public ApplicationInitializationService(IServiceLocator serviceLocator)
        {
            Argument.IsNotNull(() => serviceLocator);

            _serviceLocator = serviceLocator;
        }

        public override Task InitializeBeforeCreatingShellAsync()
        {
            InitializeFonts();
            RegisterTypes();

            return TaskHelper.Completed;
        }

        private void InitializeFonts()
        {
            FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/Orc.Snapshots.Example.NET;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));

            FontImage.DefaultBrush = new SolidColorBrush(Color.FromArgb(255, 87, 87, 87));
            FontImage.DefaultFontFamily = "FontAwesome";
        }

        private void RegisterTypes()
        {
            // Singleton project, we recommend to use Orc.ProjectManagement for real projects
            _serviceLocator.RegisterTypeAndInstantiate<Project>();

            // Snapshot Providers
            var snapshotManager = _serviceLocator.ResolveType<ISnapshotManager>();
            snapshotManager.AddProvider<CompanySnapshotProvider>();
            snapshotManager.AddProvider<PersonSnapshotProvider>();

            // Watchers
            //_serviceLocator.RegisterTypeAndInstantiate<ShowNotificationOnSnapshotEventsWatcher>();
        }
    }
}
