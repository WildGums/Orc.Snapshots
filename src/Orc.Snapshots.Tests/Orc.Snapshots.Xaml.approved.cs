[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v5.0", FrameworkDisplayName="")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.wildgums.com/orc/snapshots", "Orc.Snapshots")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.wildgums.com/orc/snapshots", "Orc.Snapshots.Converters")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.wildgums.com/orc/snapshots", "Orc.Snapshots.Views")]
[assembly: System.Windows.Markup.XmlnsPrefix("http://schemas.wildgums.com/orc/snapshots", "orcsnapshots")]
[assembly: System.Windows.ThemeInfo(System.Windows.ResourceDictionaryLocation.None, System.Windows.ResourceDictionaryLocation.SourceAssembly)]
public static class ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orc.Snapshots.Converters
{
    public class TriggerConverter : System.Windows.Data.IMultiValueConverter
    {
        public TriggerConverter() { }
        public object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) { }
        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) { }
    }
    public class UnderscoreToDoubleUnderscoresStringConverter : Catel.MVVM.Converters.ValueConverterBase<string>
    {
        public UnderscoreToDoubleUnderscoresStringConverter() { }
        protected override object Convert(string value, System.Type targetType, object parameter) { }
    }
}
namespace Orc.Snapshots
{
    public class SnapshotCategory
    {
        public SnapshotCategory() { }
        public string Category { get; set; }
        public System.Collections.Generic.List<Orc.Snapshots.ISnapshot> Snapshots { get; }
    }
    public class SnapshotCleanup : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData IncludeInCleanupProperty;
        public static readonly Catel.Data.PropertyData SnapshotProperty;
        public SnapshotCleanup(Orc.Snapshots.ISnapshot snapshot) { }
        public bool IncludeInCleanup { get; set; }
        public Orc.Snapshots.ISnapshot Snapshot { get; }
    }
}
namespace Orc.Snapshots.ViewModels
{
    public class SnapshotViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData SnapshotProperty;
        public static readonly Catel.Data.PropertyData SnapshotTitleProperty;
        public SnapshotViewModel(Orc.Snapshots.ISnapshot snapshot, Catel.Services.ILanguageService languageService) { }
        [Catel.MVVM.Model]
        public Orc.Snapshots.ISnapshot Snapshot { get; }
        [Catel.MVVM.ViewModelToModel("Snapshot", "Title")]
        public string SnapshotTitle { get; set; }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
    }
    public class SnapshotsCleanupViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData IncludeAllInCleanupProperty;
        public static readonly Catel.Data.PropertyData MaxSnapshotAgeProperty;
        public static readonly Catel.Data.PropertyData NumberOfSnapshotsToCleanupProperty;
        public static readonly Catel.Data.PropertyData SnapshotsProperty;
        public SnapshotsCleanupViewModel(Orc.Snapshots.ISnapshotManager snapshotManager, Catel.Services.ILanguageService languageService) { }
        public bool IncludeAllInCleanup { get; set; }
        public int MaxSnapshotAge { get; set; }
        public int NumberOfSnapshotsToCleanup { get; set; }
        public System.Collections.Generic.List<Orc.Snapshots.SnapshotCleanup> Snapshots { get; }
        protected override System.Threading.Tasks.Task CloseAsync() { }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
        protected override System.Threading.Tasks.Task<bool> SaveAsync() { }
    }
    public class SnapshotsViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData FilterProperty;
        public static readonly Catel.Data.PropertyData HasSnapshotsProperty;
        public static readonly Catel.Data.PropertyData ScopeProperty;
        public static readonly Catel.Data.PropertyData SnapshotCategoriesProperty;
        public SnapshotsViewModel(Catel.Services.IUIVisualizerService uiVisualizerService, Catel.IoC.IServiceLocator serviceLocator, Catel.Services.IDispatcherService dispatcherService, Catel.Services.IMessageService messageService, Catel.Services.ILanguageService languageService) { }
        public Catel.MVVM.TaskCommand<Orc.Snapshots.ISnapshot> EditSnapshot { get; }
        public string Filter { get; set; }
        public bool HasSnapshots { get; }
        public Catel.MVVM.TaskCommand<Orc.Snapshots.ISnapshot> RemoveSnapshot { get; }
        public Catel.MVVM.TaskCommand<Orc.Snapshots.ISnapshot> RestoreSnapshot { get; }
        public object Scope { get; set; }
        public System.Collections.Generic.List<Orc.Snapshots.SnapshotCategory> SnapshotCategories { get; }
        protected override System.Threading.Tasks.Task CloseAsync() { }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
    }
}
namespace Orc.Snapshots.Views
{
    public class SnapshotWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public SnapshotWindow() { }
        public SnapshotWindow(Orc.Snapshots.ViewModels.SnapshotViewModel viewModel) { }
        public void InitializeComponent() { }
    }
    public sealed class SnapshotsCleanupWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public SnapshotsCleanupWindow() { }
        public void InitializeComponent() { }
    }
    public class SnapshotsView : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty ScopeProperty;
        public SnapshotsView() { }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.ViewToViewModel)]
        public object Scope { get; set; }
        public void InitializeComponent() { }
    }
}