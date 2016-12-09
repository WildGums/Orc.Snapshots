// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotStorageServiceBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using FileSystem;
    using Ionic.Zip;

    public abstract class SnapshotStorageServiceBase : ISnapshotStorageService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private const string MetadataSplitter = "=";
        private static readonly byte[] MetadataSeparatorBytes = Encoding.UTF8.GetBytes("-|-");
        private static readonly byte[] DataSeparatorBytes = Encoding.UTF8.GetBytes("_|_");

        private static readonly Encoding Encoding = Encoding.UTF8;

        public abstract Task<IEnumerable<ISnapshot>> LoadSnapshotsAsync();
        public abstract Task SaveSnapshotsAsync(IEnumerable<ISnapshot> snapshots);

        protected virtual async Task<ISnapshot> ConvertBytesToSnapshotAsync(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                Log.Warning("No bytes in snapshot data, cannot convert bytes to snapshot");
                return null;
            }

            Dictionary<string, string> metadata;
            byte[] dataBytes;

            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var zip = ZipFile.Read(memoryStream))
                {
                    var metadataEntry = zip["metadata"];
                    var metadataBytes = metadataEntry.GetBytes();
                    metadata = ParseMetadata(metadataBytes);

                    var dataEntry = zip["data"];
                    dataBytes = dataEntry.GetBytes();
                }
            }

            string title;
            if (!metadata.TryGetValue("title", out title))
            {
                title = "-";
            }

            string created;
            if (!metadata.TryGetValue("created", out created))
            {
                created = FastDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }

            var snapshot = new Snapshot
            {
                Title = title,
                Created = DateTime.ParseExact(created, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            };

            // Data
            await snapshot.InitializeFromBytesAsync(dataBytes);

            return snapshot;
        }

        protected virtual async Task<byte[]> ConvertSnapshotToBytesAsync(ISnapshot snapshot)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var zip = new ZipFile())
                {
                    var metadata = new Dictionary<string, string>();
                    metadata["title"] = snapshot.Title;
                    metadata["created"] = snapshot.Created.ToString("yyyy-MM-dd HH:mm:ss");

                    zip.AddEntry("metadata", GetMetadataBytes(metadata));

                    var snapshotBytes = await snapshot.GetAllBytesAsync();
                    zip.AddEntry("data", snapshotBytes);

                    zip.Save(memoryStream);
                }

                return memoryStream.ToArray();
            }
        }

        private byte[] GetMetadataBytes(Dictionary<string, string> metadata)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream))
                {
                    foreach (var value in metadata)
                    {
                        var stringValue = $"{value.Key}{MetadataSplitter}{value.Value}";
                        writer.WriteLine(stringValue);
                    }
                }

                return memoryStream.ToArray();
            }
        }

        private Dictionary<string, string> ParseMetadata(byte[] bytes)
        {
            var metadata = new Dictionary<string, string>();

            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var reader = new StreamReader(memoryStream))
                {
                    var allText = reader.ReadToEnd();
                    var allLines = allText.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var line in allLines)
                    {
                        var splitIndex = line.IndexOf(MetadataSplitter);
                        if (splitIndex < 0)
                        {
                            continue;
                        }

                        var key = line.Substring(0, splitIndex);
                        var value = line.Substring(splitIndex + MetadataSplitter.Length);

                        metadata[key] = value;
                    }
                }
            }

            return metadata;
        }
    }
}