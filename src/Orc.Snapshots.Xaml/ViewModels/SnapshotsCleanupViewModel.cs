namespace Orc.Snapshots.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Data;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;

    public class SnapshotsCleanupViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ISnapshotManager _snapshotManager;

        public SnapshotsCleanupViewModel(ISnapshotManager snapshotManager, ILanguageService languageService)
        {
            ArgumentNullException.ThrowIfNull(snapshotManager);
            ArgumentNullException.ThrowIfNull(languageService);

            _snapshotManager = snapshotManager;

            Snapshots = new List<SnapshotCleanup>(from snapshot in _snapshotManager.Snapshots
                                                  select new SnapshotCleanup(snapshot));

            MaxSnapshotAge = 7;

            Title = languageService.GetRequiredString("Snapshots_CleanupTitle");
        }

        public List<SnapshotCleanup> Snapshots { get; private set; }

        public int MaxSnapshotAge { get; set; }

        public int NumberOfSnapshotsToCleanup { get; set; }

        public bool IncludeAllInCleanup { get; set; }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            foreach (var snapshot in Snapshots)
            {
                snapshot.PropertyChanged += OnSnapshotPropertyChanged;
            }
        }

        protected override async Task CloseAsync()
        {
            foreach (var snapshot in Snapshots)
            {
                snapshot.PropertyChanged -= OnSnapshotPropertyChanged;
            }

            await base.CloseAsync();
        }

        protected override async Task<bool> SaveAsync()
        {
            Log.Info("Cleaning up snapshots");

            foreach (var snapshotCleanup in Snapshots)
            {
                if (snapshotCleanup.IncludeInCleanup)
                {
                    var snapshot = snapshotCleanup.Snapshot;

                    Log.Info($"Cleaning up snapshot '{snapshot}'");

                    _snapshotManager.Remove(snapshot);
                }
            }

            await _snapshotManager.SaveAsync();

            Log.Info("Cleaned up snapshots");

            return true;
        }

        private void OnSnapshotPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateSnapshotCount();
            using (SuspendChangeCallbacks())
            {
                IncludeAllInCleanup = NumberOfSnapshotsToCleanup == Snapshots.Count;
            }
        }

        private void UpdateSnapshotCount()
        {
            NumberOfSnapshotsToCleanup = Snapshots.Count(x => x.IncludeInCleanup);
        }

        private void OnMaxSnapshotAgeChanged()
        {
            var snapshots = Snapshots;
            if (snapshots is null)
            {
                return;
            }

            var now = FastDateTime.Now;
            var minCreated = now.AddDays(MaxSnapshotAge * -1);

            foreach (var snapshot in snapshots)
            {
                snapshot.IncludeInCleanup = snapshot.Snapshot.Created < minCreated;
            }
        }

        private void OnIncludeAllInCleanupChanged()
        {
            foreach (var snapshot in Snapshots)
            {
                snapshot.PropertyChanged -= OnSnapshotPropertyChanged;
                snapshot.IncludeInCleanup = IncludeAllInCleanup;
                snapshot.PropertyChanged += OnSnapshotPropertyChanged;
            }

            UpdateSnapshotCount();
        }
    }
}
