namespace Orc.Snapshots.Tests
{
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using PublicApiGenerator;
    using VerifyNUnit;
    using Views;

    [TestFixture]
    public class PublicApiFacts
    {
        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public async Task Orc_Snapshots_HasNoBreakingChanges_Async()
        {
            var assembly = typeof(SnapshotManager).Assembly;

            await PublicApiApprover.ApprovePublicApiAsync(assembly);
        }

        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public async Task Orc_Snapshots_Xaml_HasNoBreakingChanges_Async()
        {
            var assembly = typeof(SnapshotsView).Assembly;

            await PublicApiApprover.ApprovePublicApiAsync(assembly);
        }

        internal static class PublicApiApprover
        {
            public static async Task ApprovePublicApiAsync(Assembly assembly)
            {
                var publicApi = ApiGenerator.GeneratePublicApi(assembly, new ApiGeneratorOptions());
                await Verifier.Verify(publicApi);
            }
        }
    }
}
