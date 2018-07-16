// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidSnapshotException.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using Catel;

    public class InvalidSnapshotException : SnapshotException
    {
        #region Constructors
        public InvalidSnapshotException(ISnapshot snapshot)
            : base(snapshot, string.Format("Snapshot '{0}' is invalid at this stage", ObjectToStringHelper.ToString(snapshot)))
        {
        }
        #endregion
    }
}