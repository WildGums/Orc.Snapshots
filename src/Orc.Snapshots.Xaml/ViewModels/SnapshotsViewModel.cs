namespace Orc.Snapshots.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catel;
using Catel.Data;
using Catel.Logging;
using Catel.MVVM;
using Catel.Services;
using Microsoft.Extensions.Logging;

public class SnapshotsViewModel : ViewModelBase
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(SnapshotsViewModel));

    private readonly ISnapshotManager _snapshotManager;
    private readonly IUIVisualizerService _uiVisualizerService;
    private readonly IMessageService _messageService;
    private readonly ILanguageService _languageService;

    public SnapshotsViewModel(IUIVisualizerService uiVisualizerService, IServiceProvider serviceProvider,
        IDispatcherService dispatcherService, IMessageService messageService, ILanguageService languageService,
        ISnapshotManager snapshotManager)
        : base(serviceProvider)
    {
        _uiVisualizerService = uiVisualizerService;
        _messageService = messageService;
        _languageService = languageService;
        _snapshotManager = snapshotManager;

        SnapshotCategories = new List<SnapshotCategory>();
        Filter = string.Empty;

        RestoreSnapshot = new TaskCommand<ISnapshot>(serviceProvider, OnRestoreSnapshotExecuteAsync, OnRestoreSnapshotCanExecute);
        EditSnapshot = new TaskCommand<ISnapshot>(serviceProvider, OnEditSnapshotExecuteAsync, OnEditSnapshotCanExecute);
        RemoveSnapshot = new TaskCommand<ISnapshot>(serviceProvider, OnRemoveSnapshotExecuteAsync, OnRemoveSnapshotCanExecute);
    }

    public bool HasSnapshots { get; private set; }

    public List<SnapshotCategory> SnapshotCategories { get; private set; }

    public string Filter { get; set; }

    public TaskCommand<ISnapshot> RestoreSnapshot { get; private set; }

    private bool OnRestoreSnapshotCanExecute(ISnapshot? snapshot)
    {
        return snapshot is not null;
    }

    private async Task OnRestoreSnapshotExecuteAsync(ISnapshot? snapshot)
    {
        if (snapshot is null)
        {
            return;
        }

        Logger.LogInformation($"Restoring snapshot '{snapshot}'");

        var snapshotManager = _snapshotManager;
        if (snapshotManager is not null)
        {
            await snapshotManager.RestoreSnapshotAsync(snapshot);
        }
    }

    public TaskCommand<ISnapshot> EditSnapshot { get; private set; }

    private bool OnEditSnapshotCanExecute(ISnapshot? snapshot)
    {
        return snapshot is not null;
    }

    private async Task OnEditSnapshotExecuteAsync(ISnapshot? snapshot)
    {
        if (snapshot is null)
        {
            return;
        }

        var snapshotManager = _snapshotManager;
        if (snapshotManager is null)
        {
            return;
        }

        var modelValidation = snapshot as IValidatable;

        void OnSnapshotValidating(object? sender, ValidationEventArgs e)
        {
            if (snapshotManager.Snapshots.Any(x => x.Title.EqualsIgnoreCase(snapshot.Title) && x != snapshot))
            {
                e.ValidationContext.Add(FieldValidationResult.CreateError("Title", _languageService.GetRequiredString("Snapshots_SnapshotWithCurrentTitleAlreadyExists")));
            }
        }

        if (modelValidation is not null)
        {
            modelValidation.Validating += OnSnapshotValidating;
        }

        var result = await _uiVisualizerService.ShowDialogAsync<SnapshotViewModel>(snapshot);
        if (result.DialogResult ?? false)
        {
            if (modelValidation is not null)
            {
                modelValidation.Validating -= OnSnapshotValidating;
            }

            await snapshotManager.SaveAsync();
        }
    }

    public TaskCommand<ISnapshot> RemoveSnapshot { get; private set; }

    private bool OnRemoveSnapshotCanExecute(ISnapshot? snapshot)
    {
        return snapshot is not null;
    }

    private async Task OnRemoveSnapshotExecuteAsync(ISnapshot? snapshot)
    {
        if (snapshot is null)
        {
            return;
        }

        var snapshotManager = _snapshotManager;
        if (snapshotManager is null)
        {
            return;
        }

        if (await _messageService.ShowAsync(string.Format(_languageService.GetRequiredString("Snapshots_AreYouSureYouWantToRemoveTheSnapshot"), snapshot.Title),
                _languageService.GetRequiredString("Snapshots_AreYouSure"), MessageButton.YesNo, MessageImage.Question) == MessageResult.No)
        {
            return;
        }

        snapshotManager.Remove(snapshot);

        await snapshotManager.SaveAsync();
    }

    private void OnFilterChanged()
    {
        UpdateSnapshots();
    }

    protected override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        _snapshotManager.Loaded += OnSnapshotsLoaded;
        _snapshotManager.SnapshotsChanged += OnSnapshotsChanged;

        UpdateSnapshots();
    }

    protected override async Task CloseAsync()
    {
        _snapshotManager.Loaded -= OnSnapshotsLoaded;
        _snapshotManager.SnapshotsChanged -= OnSnapshotsChanged;

        await base.CloseAsync();
    }

    private void OnSnapshotsLoaded(object? sender, EventArgs e)
    {
        var snapshotManager = _snapshotManager;

        Logger.LogDebug($"Snapshots have been loaded, updating snapshots");

        UpdateSnapshots();
    }

    private void OnSnapshotsChanged(object? sender, EventArgs e)
    {
        var snapshotManager = _snapshotManager;

        Logger.LogDebug($"Snapshots have changed, updating snapshots");

        UpdateSnapshots();
    }

    private void UpdateSnapshots()
    {
        var snapshotManager = _snapshotManager;
        if (snapshotManager is null)
        {
            return;
        }

        var filter = Filter;

        var allSnapshots = snapshotManager.Snapshots;

        HasSnapshots = allSnapshots.Any();

        var finalItems = new List<SnapshotCategory>();

        var groupedSnapshots = allSnapshots.OrderBy(x => x.Category).GroupBy(x => x.Category);

        foreach (var category in groupedSnapshots)
        {
            var snapshotCategory = new SnapshotCategory
            {
                Category = category.Key
            };

            var categoryItems = category.Select(x => x);

            if (!string.IsNullOrWhiteSpace(filter))
            {
                categoryItems = category.Where(x => x.Title.ContainsIgnoreCase(filter));
            }

            snapshotCategory.Snapshots.AddRange(categoryItems.OrderByDescending(x => x.Created));

            if (snapshotCategory.Snapshots.Count > 0)
            {
                finalItems.Add(snapshotCategory);
            }
        }

        Logger.LogDebug($"Updating available snapshots using snapshot manager, '{finalItems.Count}' snapshot categories available");

        SnapshotCategories = finalItems;
    }
}
