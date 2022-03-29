namespace Orc.Snapshots.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Data;

    public class SnapshotsViewModel : ViewModelBase
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private ISnapshotManager _snapshotManager;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IServiceLocator _serviceLocator;
        private readonly IMessageService _messageService;
        private readonly ILanguageService _languageService;
        #endregion

        #region Constructors
        public SnapshotsViewModel(IUIVisualizerService uiVisualizerService, IServiceLocator serviceLocator,
            IDispatcherService dispatcherService, IMessageService messageService, ILanguageService languageService)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => serviceLocator);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => languageService);

            _uiVisualizerService = uiVisualizerService;
            _serviceLocator = serviceLocator;
            _messageService = messageService;
            _languageService = languageService;

            SnapshotCategories = new List<SnapshotCategory>();

            RestoreSnapshot = new TaskCommand<ISnapshot>(OnRestoreSnapshotExecuteAsync, OnRestoreSnapshotCanExecute);
            EditSnapshot = new TaskCommand<ISnapshot>(OnEditSnapshotExecuteAsync, OnEditSnapshotCanExecute);
            RemoveSnapshot = new TaskCommand<ISnapshot>(OnRemoveSnapshotExecuteAsync, OnRemoveSnapshotCanExecute);
        }
        #endregion

        #region Properties
        public bool HasSnapshots { get; private set; }

        public List<SnapshotCategory> SnapshotCategories { get; private set; }

        public string Filter { get; set; }

        public object Scope { get; set; }
        #endregion

        #region Commands
        public TaskCommand<ISnapshot> RestoreSnapshot { get; private set; }

        private bool OnRestoreSnapshotCanExecute(ISnapshot snapshot)
        {
            return snapshot is not null;
        }

        private async Task OnRestoreSnapshotExecuteAsync(ISnapshot snapshot)
        {
            Log.Info($"Restoring snapshot '{snapshot}'");

            await _snapshotManager.RestoreSnapshotAsync(snapshot);
        }

        public TaskCommand<ISnapshot> EditSnapshot { get; private set; }

        private bool OnEditSnapshotCanExecute(ISnapshot snapshot)
        {
            return snapshot is not null;
        }

        private async Task OnEditSnapshotExecuteAsync(ISnapshot snapshot)
        {
            var modelValidation = snapshot as IValidatable;

            void OnSnapshotValidating(object sender, ValidationEventArgs e)
            {
                if (_snapshotManager.Snapshots.Any(x => x.Title.EqualsIgnoreCase(snapshot.Title) && x != snapshot))
                {
                    e.ValidationContext.Add(FieldValidationResult.CreateError("Title", _languageService.GetString("Snapshots_SnapshotWithCurrentTitleAlreadyExists")));
                }
            }

            if (modelValidation is not null)
            {
                modelValidation.Validating += OnSnapshotValidating;
            }

            if (await _uiVisualizerService.ShowDialogAsync<SnapshotViewModel>(snapshot) ?? false)
            {
                if (modelValidation is not null)
                {
                    modelValidation.Validating -= OnSnapshotValidating;
                }

                await _snapshotManager.SaveAsync();
            }
        }

        public TaskCommand<ISnapshot> RemoveSnapshot { get; private set; }

        private bool OnRemoveSnapshotCanExecute(ISnapshot snapshot)
        {
            return snapshot is not null;
        }

        private async Task OnRemoveSnapshotExecuteAsync(ISnapshot snapshot)
        {
            if (await _messageService.ShowAsync(string.Format(_languageService.GetString("Snapshots_AreYouSureYouWantToRemoveTheSnapshot"), snapshot.Title),
                _languageService.GetString("Snapshots_AreYouSure"), MessageButton.YesNo, MessageImage.Question) == MessageResult.No)
            {
                return;
            }

            _snapshotManager.Remove(snapshot);

            await _snapshotManager.SaveAsync();
        }
        #endregion

        #region Methods
        private void OnFilterChanged()
        {
            UpdateSnapshots();
        }

#pragma warning disable AsyncFixer03 // Avoid fire & forget async void methods
#pragma warning disable AvoidAsyncVoid
        private async void OnScopeChanged()
        {
            var scope = Scope;

            Log.Debug($"Scope has changed to '{scope}'");

            await DeactivateSnapshotManagerAsync();
            ActivateSnapshotManager();
        }
#pragma warning restore AsyncFixer03 // Avoid fire & forget async void methods
#pragma warning restore AvoidAsyncVoid

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            ActivateSnapshotManager();
        }

        protected override async Task CloseAsync()
        {
            await DeactivateSnapshotManagerAsync(false);

            await base.CloseAsync();
        }

        private void OnSnapshotsLoaded(object sender, EventArgs e)
        {
            var snapshotManager = _snapshotManager;

            Log.Debug($"Snapshots have been loaded, updating snapshots, current snapshot manager scope is '{snapshotManager.Scope}'");

            UpdateSnapshots();
        }

        private void OnSnapshotsChanged(object sender, EventArgs e)
        {
            var snapshotManager = _snapshotManager;

            Log.Debug($"Snapshots have changed, updating snapshots, current snapshot manager scope is '{snapshotManager.Scope}'");

            UpdateSnapshots();
        }

        private void SetSnapshotManager(ISnapshotManager snapshotManager)
        {
            var previousSnapshotManager = _snapshotManager;
            if (ReferenceEquals(snapshotManager, previousSnapshotManager))
            {
                return;
            }

            if (previousSnapshotManager is not null)
            {
                previousSnapshotManager.Loaded -= OnSnapshotsLoaded;
                previousSnapshotManager.SnapshotsChanged -= OnSnapshotsChanged;
            }

            Log.Debug($"Updating current snapshot manager with scope '{snapshotManager?.Scope}' to new instance with '{snapshotManager?.Snapshots.Count() ?? 0}' snapshots");

            _snapshotManager = snapshotManager;

            if (snapshotManager is not null)
            {
                snapshotManager.Loaded += OnSnapshotsLoaded;
                snapshotManager.SnapshotsChanged += OnSnapshotsChanged;
            }
        }

        private void ActivateSnapshotManager()
        {
            var scope = Scope;

            Log.Debug($"Activating snapshot manager using scope '{scope}'");

            var snapshotManager = _serviceLocator.ResolveType<ISnapshotManager>(scope);
            SetSnapshotManager(snapshotManager);

            UpdateSnapshots();
        }

        private async Task DeactivateSnapshotManagerAsync(bool setToNull = true)
        {
            Log.Debug($"Deactivating snapshot manager");

            var snapshotManager = _snapshotManager;
            if (snapshotManager is not null)
            {
                snapshotManager.Loaded -= OnSnapshotsLoaded;
                snapshotManager.SnapshotsChanged -= OnSnapshotsChanged;

                if (setToNull)
                {
                    _snapshotManager = null;
                }
            }

            SnapshotCategories.Clear();
        }

        private void UpdateSnapshots()
        {
            var filter = Filter;

            var allSnapshots = _snapshotManager.Snapshots;

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

            Log.Debug($"Updating available snapshots using snapshot manager with scope '{_snapshotManager?.Scope}', '{finalItems.Count}' snapshot categories available");

            SnapshotCategories = finalItems;
        }
        #endregion
    }
}
