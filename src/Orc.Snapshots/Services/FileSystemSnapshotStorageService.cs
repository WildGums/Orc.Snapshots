namespace Orc.Snapshots;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catel;
using Catel.Logging;
using Catel.Services;
using FileSystem;
using Microsoft.Extensions.Logging;

public class FileSystemSnapshotStorageService : SnapshotStorageServiceBase
{
    private const string SnapshotExtension = ".snp";

    private readonly ILogger<FileSystemSnapshotStorageService> _logger;
    private readonly IDirectoryService _directoryService;
    private readonly IFileService _fileService;
    private readonly IAppDataService _appDataService;

    public FileSystemSnapshotStorageService(ILogger<FileSystemSnapshotStorageService> logger, 
        IDirectoryService directoryService, IFileService fileService, IAppDataService appDataService)
        : base(logger)
    {
        _logger = logger;
        _directoryService = directoryService;
        _fileService = fileService;
        _appDataService = appDataService;

        Directory = System.IO.Path.Combine(_appDataService.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming), "snapshots");
    }

    public string Directory { get; set; }

    public override async Task<IEnumerable<ISnapshot>> LoadSnapshotsAsync()
    {
        var directory = Directory;

        _logger.LogDebug("Loading snapshots from '{Directory}'", directory);

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

        _logger.LogDebug("Loaded '{SnapshotCount}' snapshots from '{Directory}'", snapshots.Count, directory);

        return snapshots;
    }

    protected virtual async Task<ISnapshot?> LoadSnapshotAsync(string source)
    {
        Argument.IsNotNullOrEmpty(() => source);

        ISnapshot? result = null;

        try
        {
            _logger.LogDebug("Loading snapshot from '{Source}'", source);

            var bytes = await _fileService.ReadAllBytesAsync(source);
            if (bytes is not null && bytes.Length > 0)
            {
                result = await ConvertBytesToSnapshotAsync(bytes);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load snapshot from '{Source}'", source);
        }

        return result;
    }

    public override async Task SaveSnapshotsAsync(IEnumerable<ISnapshot> snapshots)
    {
        ArgumentNullException.ThrowIfNull(snapshots);

        var directory = Directory;
        _directoryService.Create(directory);

        _logger.LogDebug("Deleting previous snapshot files");

        var deleteCount = 0;

        var snapshotFileNames = snapshots.ToDictionary(x => GetSnapshotFileName(directory, x), x => x, StringComparer.OrdinalIgnoreCase);

        foreach (var snapshotFile in _directoryService.GetFiles(directory, $"*{SnapshotExtension}"))
        {
            try
            {
                var delete = !snapshotFileNames.TryGetValue(snapshotFile, out var snapshot);
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
                    _logger.LogDebug("No need to delete '{SnapshotFile}', snapshot is still in use", snapshotFile);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to delete file '{SnapshotFile}'", snapshotFile);
            }
        }

        _logger.LogDebug("Deleted '{DeleteCount}' snapshots, going to save new snapshots now", deleteCount);

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

        _logger.LogDebug("Saved '{SaveCount}' of '{TotalCount}' snapshots to disk", saveCount, snapshots.Count());
    }

    protected virtual async Task SaveSnapshotAsync(string source, ISnapshot snapshot)
    {
        Argument.IsNotNullOrEmpty(() => source);
        ArgumentNullException.ThrowIfNull(snapshot);

        _logger.LogDebug("Saving snapshot '{Snapshot}' to '{Source}'", snapshot, source);

        var bytes = await ConvertSnapshotToBytesAsync(snapshot);
        if (bytes is not null)
        {
            await _fileService.WriteAllBytesAsync(source, bytes);
        }
    }

    protected virtual string GetSnapshotFileName(string directory, ISnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        var snapshotFile = System.IO.Path.Combine(directory, $"{snapshot.Title.GetSlug()}{SnapshotExtension}");
        return snapshotFile;
    }
}
