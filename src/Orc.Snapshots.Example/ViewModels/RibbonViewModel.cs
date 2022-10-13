namespace Orc.Snapshots.Example.ViewModels
{
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.Snapshots.ViewModels;

    public class RibbonViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly ISnapshotManager _snapshotManager;
        private readonly IMessageService _messageService;

        public RibbonViewModel(ISnapshotManager snapshotManager, IUIVisualizerService uiVisualizerService, IMessageService messageService)
        {
            _snapshotManager = snapshotManager;
            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;

            CreateSnapshot = new TaskCommand(OnCreateSnapshotExecuteAsync, OnCreateSnapshotCanExecute);
            CleanupSnapshots = new TaskCommand(OnCleanupSnapshotsExecuteAsync);

            Title = "Orc.Snapshots example";
        }

        public TaskCommand CreateSnapshot { get; private set; }
        
        private bool OnCreateSnapshotCanExecute()
        {
            return true;
        }

        private async Task OnCreateSnapshotExecuteAsync()
        {
            // In theory, we shouldn't have to use this one
            var snapshot = new Snapshot();

            var result = await _uiVisualizerService.ShowDialogAsync<SnapshotViewModel>(snapshot);
            if (result.DialogResult ?? false)
            {
                var existingSnapshot = _snapshotManager.FindSnapshot(snapshot.Title);
                if (existingSnapshot is not null)
                {
                    if (await _messageService.ShowAsync(
                        $"Snapshot '{snapshot}' already exists. Are you sure you want to overwrite the existing snapshot?",
                        "Are you sure?",
                        MessageButton.YesNo) != MessageResult.Yes)
                    {
                        return;
                    }

                    _snapshotManager.Remove(existingSnapshot);
                }

                await _snapshotManager.CreateSnapshotAndSaveAsync(snapshot.Title);
            }
        }

        public TaskCommand CleanupSnapshots { get; private set; }

        private Task OnCleanupSnapshotsExecuteAsync()
        {
            return _uiVisualizerService.ShowDialogAsync<SnapshotsCleanupViewModel>();
        }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await _snapshotManager.LoadAsync();
        }
    }
}
