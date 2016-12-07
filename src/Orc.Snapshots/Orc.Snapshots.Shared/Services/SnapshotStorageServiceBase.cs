// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotStorageServiceBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;

    public abstract class SnapshotStorageServiceBase : ISnapshotStorageService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static readonly byte[] SeparatorBytes = new byte[] { };
        private static readonly Encoding Encoding = Encoding.UTF8;

        public abstract Task<IEnumerable<ISnapshot>> LoadSnapshotsAsync();
        public abstract Task SaveSnapshotsAsync(IEnumerable<ISnapshot> snapshots);

        protected virtual ISnapshot ConvertBytesToSnapshot(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                Log.Warning("No bytes in snapshot data, cannot convert bytes to snapshot");
                return null;
            }

            var separatorIndex = IndexOf(bytes, SeparatorBytes);
            if (separatorIndex < 0)
            {
                Log.Warning("No separator found in snapshot data, cannot convert bytes to snapshot");
                return null;
            }

            var titleBytes = new byte[separatorIndex];
            var title = Encoding.GetString(titleBytes);

            var dataIndex = separatorIndex + SeparatorBytes.Length;
            var dataLength = bytes.Length - dataIndex;
            var data = new byte[dataLength];

            if (dataLength > 0)
            {
                Buffer.BlockCopy(bytes, dataIndex, data, 0, dataLength);
            }

            var snapshot = new Snapshot
            {
                Title = title
            };

            return snapshot;
        }

        protected virtual async Task<byte[]> ConvertSnapshotToBytesAsync(ISnapshot snapshot)
        {
            var bytes = new List<byte>();

            var titleBytes = Encoding.GetBytes(snapshot.Title);
            bytes.AddRange(titleBytes);

            bytes.AddRange(SeparatorBytes);

            var snapshotBytes = await snapshot.GetAllBytesAsync();
            bytes.AddRange(snapshotBytes);

            return bytes.ToArray();
        }

        private static int IndexOf(byte[] data, byte[] block)
        {
            Argument.IsNotNull("data", data);
            Argument.IsNotNull("block", block);

            for (int i = 0; i < data.Length - block.Length; i++)
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