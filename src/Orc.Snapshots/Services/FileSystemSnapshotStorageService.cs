// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemSnapshotStorageService.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Catel.Services;
    using FileSystem;
    using Path = Catel.IO.Path;

    public class FileSystemSnapshotStorageService : SnapshotStorageServiceBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private const string SnapshotExtension = ".snp";

        private readonly IDirectoryService _directoryService;
        private readonly IFileService _fileService;
        private readonly IAppDataService _appDataService;

        public FileSystemSnapshotStorageService(IDirectoryService directoryService, IFileService fileService,
            IAppDataService appDataService)
        {
            Argument.IsNotNull(() => directoryService);
            Argument.IsNotNull(() => fileService);
            Argument.IsNotNull(() => appDataService);

            _directoryService = directoryService;
            _fileService = fileService;
            _appDataService = appDataService;

            Directory = Path.Combine(_appDataService.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming), "snapshots");
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
                    if (snapshot is not null)
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
                if (bytes is not null && bytes.Length > 0)
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

            var deleteCount = 0;

            var snapshotFileNames = snapshots.ToDictionary(x => GetSnapshotFileName(directory, x), x => x, StringComparer.OrdinalIgnoreCase);

            foreach (var snapshotFile in _directoryService.GetFiles(directory, $"*{SnapshotExtension}"))
            {
                try
                {
                    ISnapshot snapshot = null;
                    var delete = !snapshotFileNames.TryGetValue(snapshotFile, out snapshot);
                    if (!delete)
                    {
                        // Note: we cannot yet use this method because we add additional contents to the file
                        // when writing to disk. Therefore we must assume that snapshots will never change once
                        // written to disk.
                    }

                    if (delete)
                    {
                        _fileService.Delete(snapshotFile);
                        deleteCount++;
                    }
                    else
                    {
                        Log.Debug($"No need to delete '{snapshotFile}', snapshot is still in use");
                    }
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, $"Failed to delete file '{snapshotFile}'");
                }
            }

            Log.Debug($"Deleted '{deleteCount}' snapshots, going to save new snapshots now");

            var saveCount = 0;

            foreach (var snapshot in snapshots)
            {
                var fileName = GetSnapshotFileName(directory, snapshot);
                if (!_fileService.Exists(fileName))
                {
                    await SaveSnapshotAsync(fileName, snapshot);
                    saveCount++;
                }
            }

            Log.Debug($"Saved '{saveCount}' of '{snapshots.Count()}' snapshots to disk");
        }

        protected virtual async Task SaveSnapshotAsync(string source, ISnapshot snapshot)
        {
            Argument.IsNotNullOrEmpty(() => source);
            Argument.IsNotNull(() => snapshot);

            Log.Debug($"Saving snapshot '{snapshot}' to '{source}'");

            var bytes = await ConvertSnapshotToBytesAsync(snapshot);
            if (bytes is not null)
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
