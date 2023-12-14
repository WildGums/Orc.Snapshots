namespace Orc.Snapshots.Tests.Managers;

using Catel.IoC;

public partial class SnapshotManagerFacts
{
    private static ISnapshotManager CreateSnapshotManager(IServiceLocator serviceLocator = null)
    {
        var snapshotStorageService = new InMemorySnapshotStorageService();
        var snapshotManager = new SnapshotManager(snapshotStorageService, serviceLocator ?? ServiceLocator.Default);

        return snapshotManager;
    }
}