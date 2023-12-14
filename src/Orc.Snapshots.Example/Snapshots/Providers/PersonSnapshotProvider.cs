namespace Orc.Snapshots.Snapshots.Providers;

using System;
using System.IO;
using System.Threading.Tasks;
using Catel.IoC;
using Models;

public class PersonSnapshotProvider : SnapshotProviderBase
{
    private readonly Project _project;

    public PersonSnapshotProvider(Project project, ISnapshotManager snapshotManager, IServiceLocator serviceLocator) 
        : base(snapshotManager, serviceLocator)
    {
        ArgumentNullException.ThrowIfNull(project);

        _project = project;
    }

    public override async Task StoreDataToSnapshotAsync(string name, Stream stream)
    {
        using (var writer = new StreamWriter(stream))
        {
            var person = _project.Person;

            await writer.WriteLineAsync(person.FirstName);
            await writer.WriteLineAsync(person.LastName);
        }
    }

    public override async Task RestoreDataFromSnapshotAsync(string name, Stream stream)
    {
        using (var reader = new StreamReader(stream))
        {
            var person = _project.Person;

            var allText = await reader.ReadToEndAsync();
            var allLines = allText.Split(new [] { Environment.NewLine }, StringSplitOptions.None);

            person.FirstName = allLines[0];
            person.LastName = allLines[1];
        }
    }
}
