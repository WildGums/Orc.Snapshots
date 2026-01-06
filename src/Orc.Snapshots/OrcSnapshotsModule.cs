namespace Orc
{
    using Catel.Services;
    using Catel.ThirdPartyNotices;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Orc.Snapshots;

    /// <summary>
    /// Core module which allows the registration of default services in the service collection.
    /// </summary>
    public static class OrcSnapshotsModule
    {
        public static IServiceCollection AddOrcSnapshots(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<ISnapshotManager, SnapshotManager>();
            serviceCollection.TryAddSingleton<ISnapshotStorageService, FileSystemSnapshotStorageService>();

            serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orc.Snapshots", "Orc.Snapshots.Properties", "Resources"));

            serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new LibraryThirdPartyNotice("Orc.Snapshots", "https://github.com/wildgums/orc.snapshots"));

            return serviceCollection;
        }
    }
}
