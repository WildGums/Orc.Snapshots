namespace Orc;

using Catel.Services;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Core module which allows the registration of default services in the service collection.
/// </summary>
public static class OrcSnapshotsXamlModule
{
    public static IServiceCollection AddOrcSnapshotsXaml(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orc.Snapshots.Xaml", "Orc.Snapshots.Properties", "Resources"));

        return serviceCollection;
    }
}
