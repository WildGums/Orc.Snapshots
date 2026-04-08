namespace Orc.Snapshots.Tests;

using Catel;
using Microsoft.Extensions.DependencyInjection;
using Orc.Snapshots;

internal static class ServiceCollectionHelper
{
    public static IServiceCollection CreateServiceCollection()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddLogging();
        serviceCollection.AddCatelCore();
        serviceCollection.AddCatelMvvm();
        serviceCollection.AddOrcSnapshots();
        serviceCollection.AddOrcSnapshotsXaml();

        return serviceCollection;
    }
}
