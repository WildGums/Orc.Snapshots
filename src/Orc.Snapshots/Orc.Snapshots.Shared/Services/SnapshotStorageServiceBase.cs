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
    using System.Text;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;

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

            var dataBytesIndex = IndexOf(bytes, DataSeparatorBytes);
            if (dataBytesIndex < 0)
            {
                Log.Warning("No data separator found in snapshot data, cannot convert bytes to snapshot");
                return null;
            }

            var metadataLength = dataBytesIndex;
            var metadataBytes = new byte[dataBytesIndex];
            if (metadataLength > 0)
            {
                Buffer.BlockCopy(bytes, 0, metadataBytes, 0, metadataLength);
            }

            var dataStartIndex = dataBytesIndex + DataSeparatorBytes.Length;
            var dataLength = bytes.Length - dataStartIndex;
            var dataBytes = new byte[dataLength];
            if (dataLength > 0)
            {
                Buffer.BlockCopy(bytes, dataStartIndex, dataBytes, 0, dataLength);
            }

            // Metadata
            var metadata = ParseMetadata(metadataBytes);

            if (!metadata.TryGetValue("title", out string title))
            {
                title = "-";
            }

            if (!metadata.TryGetValue("created", out string created))
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
            var bytes = new List<byte>();

            var metadata = new Dictionary<string, string>();
            metadata["title"] = snapshot.Title;
            metadata["created"] = snapshot.Created.ToString("yyyy-MM-dd HH:mm:ss");

            var metadataBytes = GetMetadataBytes(metadata);
            bytes.AddRange(metadataBytes);

            // Data separator
            bytes.AddRange(DataSeparatorBytes);

            var snapshotBytes = await snapshot.GetAllBytesAsync();
            bytes.AddRange(snapshotBytes);

            return bytes.ToArray();
        }

        private List<byte> GetMetadataBytes(Dictionary<string, string> metadata)
        {
            var bytes = new List<byte>();

            foreach (var value in metadata)
            {
                var stringValue = $"{value.Key}{MetadataSplitter}{value.Value}";
                var stringBytes = Encoding.GetBytes(stringValue);

                bytes.AddRange(stringBytes);
                bytes.AddRange(MetadataSeparatorBytes);
            }

            return bytes;
        }

        private Dictionary<string, string> ParseMetadata(byte[] bytes)
        {
            var metadata = new Dictionary<string, string>();

            var nextIndex = 0;
            var info = FindMetadataBlockAsText(bytes, nextIndex);
            while (info != null)
            {
                var splitIndex = info.Item1.IndexOf(MetadataSplitter);
                if (splitIndex < 0)
                {
                    continue;
                }

                var key = info.Item1.Substring(0, splitIndex);
                var value = info.Item1.Substring(splitIndex + MetadataSplitter.Length);

                metadata[key] = value;

                nextIndex = info.Item2 + MetadataSeparatorBytes.Length;
                info = FindMetadataBlockAsText(bytes, nextIndex);
            }

            return metadata;
        }

        private Tuple<byte[], int> FindMetadataBlock(byte[] bytes, int startIndex)
        {
            var separatorIndex = IndexOf(bytes, MetadataSeparatorBytes, startIndex);
            if (separatorIndex < 0)
            {
                Log.Warning("No separator found in snapshot data, cannot convert bytes to snapshot");
                return null;
            }

            var length = separatorIndex - startIndex;

            var blockBytes = new byte[length];
            Buffer.BlockCopy(bytes, startIndex, blockBytes, 0, length);

            return new Tuple<byte[], int>(blockBytes, separatorIndex);
        }

        private Tuple<string, int> FindMetadataBlockAsText(byte[] bytes, int startIndex)
        {
            var dataBlock = FindMetadataBlock(bytes, startIndex);
            if (dataBlock == null)
            {
                return null;
            }

            var text = Encoding.GetString(dataBlock.Item1);

            return new Tuple<string, int>(text, dataBlock.Item2);
        }

        private static int IndexOf(byte[] data, byte[] block, int startIndex = 0)
        {
            Argument.IsNotNull("data", data);
            Argument.IsNotNull("block", block);

            for (int i = startIndex; i <= data.Length - block.Length; i++)
            {
                for (int j = 0; j < block.Length; j++)
                {
                    if (data[i + j] == block[j])
                    {
                        if (j == block.Length - 1)
                        {
                            return i;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return -1;
        }
    }
}