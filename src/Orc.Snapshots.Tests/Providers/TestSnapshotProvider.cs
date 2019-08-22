// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestSnapshotProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots.Tests.Providers
{
    using System.IO;
    using System.Threading.Tasks;
    using Catel.IoC;

    public class TestSnapshotProvider : SnapshotProviderBase
    {
        public TestSnapshotProvider(ISnapshotManager snapshotManager, IServiceLocator serviceLocator) 
            : base(snapshotManager, serviceLocator)
        {
        }

        public string TestData { get; set; }

        public override async Task StoreDataToSnapshotAsync(string name, Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                await writer.WriteAsync(TestData);
            }
        }

        public override async Task RestoreDataFromSnapshotAsync(string name, Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                TestData = await reader.ReadToEndAsync();
            }
        }
    }
}
