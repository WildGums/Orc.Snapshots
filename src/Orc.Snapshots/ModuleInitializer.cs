using System.Runtime.CompilerServices;
using Catel.IoC;
using Catel.Services;
using Orc.Snapshots;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    /// Initializes the module.
    /// </summary>
    [ModuleInitializer]
    public static void Initialize()
    {
        var serviceLocator = ServiceLocator.Default;

        serviceLocator.RegisterType<ISnapshotManager, SnapshotManager>();
        serviceLocator.RegisterType<ISnapshotStorageService, FileSystemSnapshotStorageService>();

        var languageService = serviceLocator.ResolveRequiredType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orc.Snapshots", "Orc.Snapshots.Properties", "Resources"));
    }
}
