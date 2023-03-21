namespace Orc.Snapshots;

using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

internal static class ZipEntryExtensions
{
    public static async Task<byte[]> GetBytesAsync(this ZipArchiveEntry entry)
    {
        await using var stream = entry.Open();
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        await memoryStream.FlushAsync();
        return memoryStream.ToArray();
    }

    public static async Task OpenAndWriteAsync(this ZipArchiveEntry entry, byte[] bytes)
    {
        await using var stream = entry.Open();
        using var memoryStream = new MemoryStream(bytes);
        await memoryStream.CopyToAsync(stream);
        await stream.FlushAsync();
    }
}
