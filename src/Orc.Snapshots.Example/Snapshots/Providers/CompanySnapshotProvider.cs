namespace Orc.Snapshots.Snapshots.Providers;

using System;
using System.IO;
using System.Threading.Tasks;
using Models;

public class CompanySnapshotProvider : SnapshotProviderBase
{
    private readonly Project _project;

    public CompanySnapshotProvider(Project project)
    {
        _project = project;
    }

    public override async Task StoreDataToSnapshotAsync(string name, Stream stream)
    {
        using (var writer = new StreamWriter(stream))
        {
            var company = _project.Company;

            await writer.WriteLineAsync(company.Name);
        }
    }

    public override async Task RestoreDataFromSnapshotAsync(string name, Stream stream)
    {
        using (var reader = new StreamReader(stream))
        {
            var company = _project.Company;

            company.Name = await reader.ReadLineAsync();
        }
    }
}
