// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snapshot.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;

    public class Snapshot : ISnapshot
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private const string InternalFileExtension = ".dat";
        private readonly Dictionary<string, byte[]> _data = new Dictionary<string, byte[]>();
        private string[] _dataKeys = Array.Empty<string>();

        private string _contentHash;
        private byte[] _allData;
        private bool _isDirty = true;

        #region Constructors
        public Snapshot()
        {
        }
        #endregion

        #region Properties
        public string Title { get; set; }

        public string Category { get; set; }

        public DateTime Created { get; set; }

        public string[] Keys
        {
            get { return _dataKeys; }
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"{Title} (Category = {Category})";
        }

        public async Task<string> GetContentHashAsync()
        {
            if (string.IsNullOrWhiteSpace(_contentHash))
            {
                await GetAllBytesAsync();
            }

            return _contentHash;
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

            _dataKeys = _data.Keys.ToArray();

            _contentHash = null;
            _isDirty = true;
        }

        public async Task<byte[]> GetAllBytesAsync()
        {
            if (_isDirty)
            {
                Log.Debug($"Data for '{this}' is outdated, generating new data");

                _allData = await SaveSnapshotDataAsync(_data.ToList());
                _contentHash = Md5Helper.ComputeMd5(_allData);
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

            lock (_data)
            {
                _data[key] = data;

                _contentHash = null;
                _isDirty = true;
            }
        }

        public void ClearData(string key)
        {
            SetData(key, null);
        }

        protected virtual async Task<List<KeyValuePair<string, byte[]>>> LoadSnapshotDataAsync(byte[] bytes)
        {
            var data = new List<KeyValuePair<string, byte[]>>();

            _contentHash = string.Empty;
            using (var compressedStream = new MemoryStream(bytes))
            {
                using (var archive = new ZipArchive(compressedStream, ZipArchiveMode.Read))
                {
                    foreach (var entry in archive.Entries)
                    {
                        var fileName = entry.Name;
                        var key = fileName.Substring(0, fileName.Length - InternalFileExtension.Length).Replace("/", "\\");
                        var dataBytes = await entry.GetBytesAsync();

                        data.Add(new KeyValuePair<string, byte[]>(key, dataBytes));
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
                using (ZipArchive archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var dataItem in data)
                    {
                        var bytes = dataItem.Value;

                        // Even store empty data
                        if (bytes is null)
                        {
                            bytes = new byte[] { };
                        }
                        var entry = archive.CreateEntry($"{dataItem.Key}{InternalFileExtension}", CompressionLevel.Fastest);
                        await entry.OpenAndWriteAsync(bytes);
                    }
                }

                var allBytes = memoryStream.ToArray();
                return allBytes;
            }
        }
        #endregion
    }
}
