// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots.ViewModels
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.Fody;
    using Catel.MVVM;
    using Catel.Services;

    public class SnapshotViewModel : ViewModelBase
    {
        private readonly ILanguageService _languageService;

        #region Constructors
        public SnapshotViewModel(ISnapshot snapshot, ILanguageService languageService)
        {
            Argument.IsNotNull(() => snapshot);
            Argument.IsNotNull(() => languageService);

            DeferValidationUntilFirstSaveCall = true;
            SuspendValidation = false;

            _languageService = languageService;

            Snapshot = snapshot;

            Title = !string.IsNullOrEmpty(snapshot.Title) ? string.Format(languageService.GetString("Snapshots_EditSnapshot"), snapshot.Title) : languageService.GetString("Snapshots_CreateNewSnapshot");
        }
        #endregion

        #region Properties
        [Model]
        public ISnapshot Snapshot { get; private set; }

        [ViewModelToModel("Snapshot", "Title")]
        public string SnapshotTitle { get; set; }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            if (string.IsNullOrWhiteSpace(SnapshotTitle))
            {
                var dateString = FastDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SnapshotTitle = $"{_languageService.GetString("Snapshots_Snapshot")} - {dateString}";
            }
        }
        #endregion
    }
}