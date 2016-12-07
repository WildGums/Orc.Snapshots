// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snapshot.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using FileSystem;
    using Ionic.Zip;
    using CompressionLevel = Ionic.Zlib.CompressionLevel;

    public class Snapshot : ISnapshot
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, byte[]> _data = new Dictionary<string, byte[]>();

        private byte[] _allData;
        private bool _isDirty = true;

        #region Constructors
        public Snapshot()
        {
        }
        #endregion

        #region Properties
        public string Title { get; set; }
        #endregion

        #region Methods
        public override string ToString()
        {
            return Title;
        }

        public async Task InitializeFromBytesAsync(byte[] bytes)
        {
            Argument.IsNotNull(() => bytes);

            _data.Clear();

            var data = await LoadSnapshotDataAsync(bytes);

            foreach (var dataItem in data)
            {
                _data[dataItem.Key] = dataItem.Value;
            }

            _isDirty = true;
        }

        public async Task<byte[]> GetAllBytesAsync()
        {
            if (_isDirty)
            {
                Log.Debug($"Data for '{this}' is outdated, generating new data");

                _allData = await SaveSnapshotDataAsync(_data.ToList());
            }

            return _allData;
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

                _isDirty = true;
            }
        }

        protected virtual async Task<List<KeyValuePair<string, byte[]>>> LoadSnapshotDataAsync(byte[] bytes)
        {
            var data = new List<KeyValuePair<string, byte[]>>();

            using (var memoryStream = new MemoryStream())
            {
                using (var zipStream = new ZipInputStream(memoryStream))
                {
                    var entry = zipStream.GetNextEntry();
                    while (entry != null)
                    {
                        var key = entry.FileName;
                        byte[] dataBytes;

                        using (var reader = entry.OpenReader())
                        {
                            dataBytes = await reader.ReadAllBytesAsync();
                        }

                        data.Add(new KeyValuePair<string, byte[]>(key, dataBytes));

                        entry = zipStream.GetNextEntry();
                    }
                }
            }

            return data;
        }

        protected virtual async Task<byte[]> SaveSnapshotDataAsync(List<KeyValuePair<string, byte[]>> data)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Note: unfortunately not yet fully async, we could rewrite this to ZipOutputStream if that would make sense
                using (var zip = new ZipFile())
                {
                    zip.CompressionLevel = CompressionLevel.BestSpeed;

                    foreach (var dataItem in data)
                    {
                        var bytes = dataItem.Value;
                        if (bytes != null && bytes.Length > 0)
                        {
                            zip.AddEntry(dataItem.Key, bytes);
                        }
                    }

                    zip.Save(memoryStream);
                }

                return memoryStream.ToArray();
            }
        }
        #endregion
    }
}