namespace Orc.Snapshots.Tests;

using System;
using System.IO;
using Catel.Logging;
using Catel.Reflection;
using Microsoft.Extensions.Logging;

public sealed class TemporaryFilesContext : IDisposable
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(TemporaryFilesContext));

    private readonly Guid _randomGuid = Guid.NewGuid();
    private readonly string _rootDirectory;
    private readonly bool _cleanUp;

    public TemporaryFilesContext(string name = null, bool cleanUp = true)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            name = _randomGuid.ToString();
        }

        _cleanUp = cleanUp;

        _rootDirectory = Path.Combine(Path.GetTempPath(), GetType().Assembly.Title(), name);

        Directory.CreateDirectory(_rootDirectory);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        if (!_cleanUp)
        {
            return;
        }

        Logger.LogInformation("Deleting temporary files from '{0}'", _rootDirectory);

        try
        {
            if (Directory.Exists(_rootDirectory))
            {
                Directory.Delete(_rootDirectory, true);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to delete temporary files");
        }
    }

    public string GetDirectory(string relativeDirectoryName)
    {
        var fullPath = Path.Combine(_rootDirectory, relativeDirectoryName);

        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }

        return fullPath;
    }

    public string GetFile(string relativeFilePath, bool deleteIfExists = false)
    {
        var fullPath = Path.Combine(_rootDirectory, relativeFilePath);

        var directory = Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (deleteIfExists)
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        return fullPath;
    }
}
