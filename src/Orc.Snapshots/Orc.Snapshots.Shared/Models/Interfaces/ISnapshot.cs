// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISnapshot.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISnapshot
    {
        #region Properties
        string Title { get; set; }
        string Category { get; set; }

        List<string> Keys { get; }
        DateTime Created { get; set; }
        #endregion

        Task InitializeFromBytesAsync(byte[] bytes);
        Task<byte[]> GetAllBytesAsync();
        byte[] GetData(string key);
        void SetData(string key, byte[] data);
        void ClearData(string key);
    }
}