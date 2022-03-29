namespace Orc.Snapshots.Tests
{
    using System.Windows;
    using Catel.IoC;
    using Catel.Services;
    using Catel.Threading;
    using Orc.Automation;

    public class InitSnapshotsViewMethodRun : NamedAutomationMethodRun
    {
        public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
        {
            result = AutomationValue.FromValue(true);

            if (owner is not Views.SnapshotsView snapshotsView)
            {
                return true;
            }

#pragma warning disable IDISP001 // Dispose created
            var serviceLocator = this.GetServiceLocator();
#pragma warning restore IDISP001 // Dispose created

            var languageService = serviceLocator.ResolveType<ILanguageService>();
            languageService.RegisterLanguageSource(new LanguageResourceSource("Orc.Snapshots.Xaml", "Orc.Snapshots.Properties", "Resources"));

            serviceLocator.RegisterType<ISnapshotManager, SnapshotManager>();
            serviceLocator.RegisterType<ISnapshotStorageService, SnapshotStorageServiceMock>();

            foreach (var scope in SnapshotTestData.AvailableScopes)
            {
                RegisterScope(scope);
            }
            
            var vm = this.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion<ViewModels.SnapshotsViewModel>();
            snapshotsView.DataContext = vm;

            return true;
        }

        private void RegisterScope(object scope)
        {
#pragma warning disable IDISP001 // Dispose created
            var serviceLocator = this.GetServiceLocator();
            var typeFactory = this.GetTypeFactory();
#pragma warning restore IDISP001 // Dispose created

            var snapshotStorageService = new SnapshotStorageServiceMock
            {
                Scope = scope
            };
            serviceLocator.RegisterInstance(typeof(ISnapshotStorageService), snapshotStorageService, scope);

            var snapshotManager = typeFactory.CreateInstanceWithParametersAndAutoCompletionWithTag<SnapshotManager>(scope);
            snapshotManager.Scope = scope;

            TaskHelper.RunAndWaitAsync(async () => await snapshotManager.LoadAsync());

            serviceLocator.RegisterInstance(typeof(ISnapshotManager), snapshotManager, scope);
        }
    }
}
