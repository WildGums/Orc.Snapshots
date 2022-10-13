namespace Orc.Snapshots
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;

    public abstract class SnapshotStorageServiceBase : ISnapshotStorageService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private const string MetadataSplitter = "=";

        public abstract Task<IEnumerable<ISnapshot>> LoadSnapshotsAsync();
        public abstract Task SaveSnapshotsAsync(IEnumerable<ISnapshot> snapshots);

        protected virtual async Task<ISnapshot?> ConvertBytesToSnapshotAsync(byte[] bytes)
        {
            if (bytes is null || bytes.Length == 0)
            {
                Log.Warning("No bytes in snapshot data, cannot convert bytes to snapshot");
                return null;
            }

            Dictionary<string, string>? metadata = null;
            byte[]? dataBytes = null;

            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
                {
                    var metadataEntry = archive.GetEntry("metadata");
                    if (metadataEntry is not null)
                    {
                        var metadataBytes = await metadataEntry.GetBytesAsync();
                        metadata = ParseMetadata(metadataBytes);

                        var dataEntry = archive.GetEntry("data");
                        if (dataEntry is not null)
                        {
                            dataBytes = await dataEntry.GetBytesAsync();
                        }
                    }
                }
            }

            if (metadata is null || dataBytes is null)
            {
                return null;
            }

            if (!metadata.TryGetValue("title", out var title))
            {
                title = "-";
            }

            if (!metadata.TryGetValue("category", out var category))
            {
                category = null;
            }

            if (!metadata.TryGetValue("created", out var created))
            {
                created = FastDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }

            var snapshot = new Snapshot
            {
                Title = title,
                Category = category ?? string.Empty,
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
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var metadata = new Dictionary<string, string>();
                    metadata["title"] = snapshot.Title;
                    metadata["category"] = snapshot.Category ?? string.Empty;
                    metadata["created"] = snapshot.Created.ToString("yyyy-MM-dd HH:mm:ss");

                    var metadataEntry = archive.CreateEntry("metadata");
                    await metadataEntry.OpenAndWriteAsync(GetMetadataBytes(metadata));

                    var snapshotBytes = await snapshot.GetAllBytesAsync();
                    var dataEntry = archive.CreateEntry("data");
                    await dataEntry.OpenAndWriteAsync(snapshotBytes);
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
                    var allLines = allText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

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
