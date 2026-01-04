namespace Orc.Snapshots.ViewModels;

using System;
using System.Threading.Tasks;
using Catel;
using Catel.MVVM;
using Catel.Services;

public class SnapshotViewModel : FeaturedViewModelBase
{
    private readonly ILanguageService _languageService;

    public SnapshotViewModel(ISnapshot snapshot, IServiceProvider serviceProvider,
        ILanguageService languageService)
        : base(serviceProvider)
    {
        DeferValidationUntilFirstSaveCall = true;

        _languageService = languageService;

        Snapshot = snapshot;

        Title = !string.IsNullOrEmpty(snapshot.Title) ? string.Format(languageService.GetRequiredString("Snapshots_EditSnapshot"), snapshot.Title) : languageService.GetRequiredString("Snapshots_CreateNewSnapshot");
    }

    [Model]
    public ISnapshot Snapshot { get; private set; }

    [ViewModelToModel("Snapshot", "Title")]
    public string? SnapshotTitle { get; set; }

    protected override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        if (string.IsNullOrWhiteSpace(SnapshotTitle))
        {
            var dateString = FastDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            SnapshotTitle = $"{_languageService.GetRequiredString("Snapshots_Snapshot")} - {dateString}";
        }
    }
}
