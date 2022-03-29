// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InMemorySnapshotStorageService.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;

    public class InMemorySnapshotStorageService : SnapshotStorageServiceBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, byte[]> _snapshots = new Dictionary<string, byte[]>();

        public override async Task<IEnumerable<ISnapshot>> LoadSnapshotsAsync()
        {
            Log.Debug($"Loading snapshots");

            var snapshots = new List<ISnapshot>();

            foreach (var snapshotData in _snapshots)
            {
                var snapshot = await LoadSnapshotAsync(snapshotData.Key);
                if (snapshot is not null)
                {
                    snapshots.Add(snapshot);
                }
            }

            Log.Debug($"Loaded '{snapshots.Count}' snapshots");

            return snapshots;
        }

        protected virtual async Task<ISnapshot> LoadSnapshotAsync(string source)
        {
            Argument.IsNotNullOrEmpty(() => source);

            ISnapshot result = null;

            try
            {
                Log.Debug($"Loading snapshot from '{source}'");

                byte[] bytes;
                if (_snapshots.TryGetValue(source, out bytes))
                {
                    result = await ConvertBytesToSnapshotAsync(bytes);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to load snapshot from '{source}'");
            }

            return result;
        }

        public override async Task SaveSnapshotsAsync(IEnumerable<ISnapshot> snapshots)
        {
            Argument.IsNotNull(() => snapshots);

            Log.Debug("Deleting previous snapshot files");

            _snapshots.Clear();

            foreach (var snapshot in snapshots)
            {
                await SaveSnapshotAsync(snapshot.Title, snapshot);
            }
        }

        protected virtual async Task SaveSnapshotAsync(string source, ISnapshot snapshot)
        {
            Argument.IsNotNullOrEmpty(() => source);
            Argument.IsNotNull(() => snapshot);

            Log.Debug($"Saving snapshot '{snapshot}' to '{source}'");

            var bytes = await ConvertSnapshotToBytesAsync(snapshot);
            if (bytes is not null)
            {
                _snapshots[source] = bytes;
            }
        }
    }
}