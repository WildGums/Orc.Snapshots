namespace Orc.Snapshots.Example.Services
{
    using System;
    using Orchestra.Services;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using Catel.IoC;
    using Models;
    using Snapshots.Providers;
    using Orc.Theming;

    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        private readonly IServiceLocator _serviceLocator;

        public ApplicationInitializationService(IServiceLocator serviceLocator)
        {
            ArgumentNullException.ThrowIfNull(serviceLocator);

            _serviceLocator = serviceLocator;
        }

        public override Task InitializeBeforeCreatingShellAsync()
        {
            InitializeFonts();
            RegisterTypes();

            return Task.CompletedTask;
        }

        private void InitializeFonts()
        {
            FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/Orc.Snapshots.Example;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));

            FontImage.DefaultBrush = new SolidColorBrush(Color.FromArgb(255, 87, 87, 87));
            FontImage.DefaultFontFamily = "FontAwesome";
        }

        private void RegisterTypes()
        {
            // Singleton project, we recommend to use Orc.ProjectManagement for real projects
            _serviceLocator.RegisterTypeAndInstantiate<Project>();

            // Snapshot Providers
            var snapshotManager = _serviceLocator.ResolveRequiredType<ISnapshotManager>();
            snapshotManager.AddProvider<CompanySnapshotProvider>();
            snapshotManager.AddProvider<PersonSnapshotProvider>();

            // Watchers
            //_serviceLocator.RegisterTypeAndInstantiate<ShowNotificationOnSnapshotEventsWatcher>();
        }
    }
}
