namespace Orc.Snapshots.Tests.Managers;

using Microsoft.Extensions.DependencyInjection;

public partial class SnapshotManagerFacts
{
    private static ISnapshotManager CreateSnapshotManager()
    {
        var serviceCollection = ServiceCollectionHelper.CreateServiceCollection();

        serviceCollection.AddSingleton<ISnapshotStorageService, InMemorySnapshotStorageService>();

        using var serviceProvider = serviceCollection.BuildServiceProvider();

        var snapshotManager = serviceProvider.GetRequiredService<ISnapshotManager>();

        return snapshotManager;
    }
}
