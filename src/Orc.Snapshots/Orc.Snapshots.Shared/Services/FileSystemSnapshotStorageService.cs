// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemSnapshotStorageService.cs" company="WildGums">
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
    using FileSystem;
    using Path = Catel.IO.Path;

    public class FileSystemSnapshotStorageService : SnapshotStorageServiceBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private const string SnapshotExtension = ".snp";

        private readonly IDirectoryService _directoryService;
        private readonly IFileService _fileService;

        public FileSystemSnapshotStorageService(IDirectoryService directoryService, IFileService fileService)
        {
            Argument.IsNotNull(() => directoryService);
            Argument.IsNotNull(() => fileService);

            _directoryService = directoryService;
            _fileService = fileService;

            Directory = Path.Combine(Path.GetApplicationDataDirectory(), "snapshots");
        }

        public string Directory { get; set; }

        public override async Task<IEnumerable<ISnapshot>> LoadSnapshotsAsync()
        {
            var directory = Directory;

            Log.Debug($"Loading snapshots from '{directory}'");

            var snapshots = new List<ISnapshot>();

            if (_directoryService.Exists(directory))
            {
                foreach (var snapshotFile in _directoryService.GetFiles(directory, $"*{SnapshotExtension}"))
                {
                    var snapshot = await LoadSnapshotAsync(snapshotFile);
                    if (snapshot != null)
                    {
                        snapshots.Add(snapshot);
                    }
                }
            }

            Log.Debug($"Loaded '{snapshots.Count}' snapshots from '{directory}'");

            return snapshots;
        }

        protected virtual async Task<ISnapshot> LoadSnapshotAsync(string source)
        {
            Argument.IsNotNullOrEmpty(() => source);

            ISnapshot result = null;

            try
            {
                Log.Debug($"Loading snapshot from '{source}'");

                var bytes = await _fileService.ReadAllBytesAsync(source);
                if (bytes != null && bytes.Length > 0)
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

            var directory = Directory;
            _directoryService.Create(directory);

            Log.Debug("Deleting previous snapshot files");

            foreach (var snapshotFile in _directoryService.GetFiles(directory, $"*{SnapshotExtension}"))
            {
                try
                {
                    _fileService.Delete(snapshotFile);
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, $"Failed to delete file '{snapshotFile}'");
                }
            }

            foreach (var snapshot in snapshots)
            {
                var fileName = GetSnapshotFileName(directory, snapshot);
                await SaveSnapshotAsync(fileName, snapshot);
            }
        }

        protected virtual async Task SaveSnapshotAsync(string source, ISnapshot snapshot)
        {
            Argument.IsNotNullOrEmpty(() => source);
            Argument.IsNotNull(() => snapshot);

            Log.Debug($"Saving snapshot '{snapshot}' to '{source}'");

            var bytes = await ConvertSnapshotToBytesAsync(snapshot);
            if (bytes != null)
            {
                await _fileService.WriteAllBytesAsync(source, bytes);
            }
        }

        protected virtual string GetSnapshotFileName(string directory, ISnapshot snapshot)
        {
            Argument.IsNotNull(() => snapshot);

            var snapshotFile = Path.Combine(directory, $"{snapshot.Title.GetSlug()}{SnapshotExtension}");
            return snapshotFile;
        }
    }
}