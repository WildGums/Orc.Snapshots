// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snapshot.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System.Collections.Generic;
    using Catel;
    using Catel.Logging;

    public class Snapshot : ISnapshot
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, byte[]> _data = new Dictionary<string, byte[]>();

        #region Constructors
        public Snapshot()
        {
        }

        public Snapshot(byte[] snapshotBytes)
        {
            Argument.IsNotNull(() => snapshotBytes);
        }
        #endregion

        #region Properties
        public string Title { get; set; }

        public byte[] Data { get; }
        #endregion

        #region Methods
        public override string ToString()
        {
            return Title;
        }

        public byte[] GetData(string key)
        {
            Argument.IsNotNullOrWhitespace(() => key);

            lock (_data)
            {
                byte[] data;
                if (!_data.TryGetValue(key, out data))
                {
                    Log.Warning($"Key '{key}' not found in snapshot");
                }

                return data;
            }
        }

        public void SetData(string key, byte[] data)
        {
            Argument.IsNotNullOrWhitespace(() => key);
            Argument.IsNotNull(() => data);

            lock (_data)
            {
                _data[key] = data;
            }
        }
        #endregion
    }
}