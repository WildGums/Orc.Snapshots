namespace Orc.Snapshots;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Catel;
using Catel.Data;
using Catel.IoC;
using Catel.Logging;
using Catel.Threading;

/// <summary>
/// Base implementation for snapshot providers.
/// </summary>
public abstract class SnapshotProviderBase : ISnapshotProvider
{
    protected readonly IServiceLocator ServiceLocator;

    private object? _scope;

    /// <summary>
    /// Initializes a new instance of the <see cref="SnapshotProviderBase" /> class.
    /// </summary>
    /// <param name="snapshotManager">The snapshot manager.</param>
    /// <param name="serviceLocator">The service locator.</param>
    protected SnapshotProviderBase(ISnapshotManager snapshotManager, IServiceLocator serviceLocator)
    {
        ArgumentNullException.ThrowIfNull(snapshotManager);
        ArgumentNullException.ThrowIfNull(serviceLocator);

        ServiceLocator = serviceLocator;
        SnapshotManager = snapshotManager;

        Name = GetType().Name;
    }

    protected ISnapshotManager SnapshotManager { get; set; }

    public virtual object? Scope
    {
        get { return _scope; }
        set
        {
            var snapshotManager = ServiceLocator.ResolveType<ISnapshotManager>(value);
            if (snapshotManager is null)
            {
                throw new PropertyNotNullableException("SnapshotManager", typeof(ISnapshotManager));
            }

            SnapshotManager = snapshotManager;
            _scope = value;
        }
    }

    public virtual string Name { get; protected set; }

    public object? Tag { get; set; }

    /// <summary>
    /// Called when a snapshot manager is about to create a snapshot.
    /// </summary>
    /// <param name="snapshot">The snapshot.</param>
    /// <returns></returns>
    public virtual Task CreatingSnapshotAsync(ISnapshot snapshot)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Called when a snapshot manager has just created a snapshot.
    /// </summary>
    /// <param name="snapshot">The snapshot.</param>
    /// <returns></returns>
    public virtual Task CreatedSnapshotAsync(ISnapshot snapshot)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Called when a snapshot manager is about to restore a snapshot.
    /// </summary>
    /// <param name="snapshot">The snapshot.</param>
    /// <returns></returns>
    public virtual Task RestoringSnapshotAsync(ISnapshot snapshot)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Called when a snapshot manager has just restored a snapshot.
    /// </summary>
    /// <param name="snapshot">The snapshot.</param>
    /// <returns></returns>
    public virtual Task RestoredSnapshotAsync(ISnapshot snapshot)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Stores the data into the stream that will be stored inside the snapshot.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="stream">The stream.</param>
    /// <returns></returns>
    public abstract Task StoreDataToSnapshotAsync(string name, Stream stream);

    /// <summary>
    /// Restores the snapshot data from the stream.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="stream">The stream.</param>
    /// <returns></returns>
    public abstract Task RestoreDataFromSnapshotAsync(string name, Stream stream);

    /// <summary>
    /// Gets the names of the values that needs to be written to a stream.
    /// </summary>
    /// <returns></returns>
    public virtual List<string> GetNames()
    {
        return new List<string>(new[] { Name });
    }
}
