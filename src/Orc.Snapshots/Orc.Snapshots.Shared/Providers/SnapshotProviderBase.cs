// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotProviderBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System.IO;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Data;
    using Catel.IoC;
    using Catel.Logging;

    /// <summary>
    /// Base implementation for snapshot providers.
    /// </summary>
    public abstract class SnapshotProviderBase : ISnapshotProvider
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        protected readonly IServiceLocator ServiceLocator;

        private object _scope;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SnapshotProviderBase" /> class.
        /// </summary>
        /// <param name="snapshotManager">The snapshot manager.</param>
        /// <param name="serviceLocator">The service locator.</param>
        protected SnapshotProviderBase(ISnapshotManager snapshotManager, IServiceLocator serviceLocator)
        {
            Argument.IsNotNull(() => snapshotManager);
            Argument.IsNotNull(() => serviceLocator);

            ServiceLocator = serviceLocator;

            SnapshotManager = snapshotManager;

            Name = GetType().Name;
        }
        #endregion

        #region Properties
        protected ISnapshotManager SnapshotManager { get; set; }

        public virtual object Scope
        {
            get { return _scope; }
            set
            {
                var snapshotManager = ServiceLocator.ResolveType<ISnapshotManager>(value);
                if (snapshotManager == null)
                {
                    throw new PropertyNotNullableException("SnapshotManager", typeof(ISnapshotManager));
                }

                SnapshotManager = snapshotManager;
                _scope = value;
            }
        }

        public virtual string Name { get; protected set; }

        public object Tag { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Stores the data into the stream that will be stored inside the snapshot.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public abstract Task StoreDataToSnapshotAsync(Stream stream);

        /// <summary>
        /// Restores the snapshot data from the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public abstract Task RestoreDataFromSnapshotAsync(Stream stream);
        #endregion
    }
}