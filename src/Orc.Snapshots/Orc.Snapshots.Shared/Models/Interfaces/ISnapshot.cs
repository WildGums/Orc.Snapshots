// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISnapshot.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    public interface ISnapshot
    {
        #region Properties
        string Title { get; set; }

        byte[] Data { get; }
        #endregion

        byte[] GetData(string key);
        void SetData(string key, byte[] data);
    }
}