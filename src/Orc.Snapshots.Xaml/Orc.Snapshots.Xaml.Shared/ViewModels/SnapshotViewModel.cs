// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots.ViewModels
{
    using Catel;
    using Catel.Fody;
    using Catel.MVVM;
    using Catel.Services;

    public class SnapshotViewModel : ViewModelBase
    {
        #region Constructors
        public SnapshotViewModel(ISnapshot snapshot, ILanguageService languageService)
        {
            Argument.IsNotNull(() => snapshot);
            Argument.IsNotNull(() => languageService);

            DeferValidationUntilFirstSaveCall = true;
            SuspendValidation = false;

            Snapshot = snapshot;

            Title = !string.IsNullOrEmpty(snapshot.Title) ? string.Format(languageService.GetString("Snapshots_EditSnapshot"), snapshot.Title) : languageService.GetString("Snapshots_CreateNewSnapshot");
        }
        #endregion

        #region Properties
        [Model]
        [Expose("SnapshotTitle", "Title")]
        public ISnapshot Snapshot { get; private set; }
        #endregion
    }
}