using Catel.IoC;
using Orc.Snapshots.Example.Services;
using Orc.Snapshots.Models;
using Orchestra.Services;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        var serviceLocator = ServiceLocator.Default;

        // Singleton project, we recommend to use Orc.ProjectManagement for real projects
        serviceLocator.RegisterTypeAndInstantiate<Project>();

        serviceLocator.RegisterType<IRibbonService, RibbonService>();
        serviceLocator.RegisterType<IApplicationInitializationService, ApplicationInitializationService>();
    }
}