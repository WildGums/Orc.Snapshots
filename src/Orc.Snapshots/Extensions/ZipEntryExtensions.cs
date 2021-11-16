// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZipEntryExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System.IO;
    using System.IO.Compression;

    internal static class ZipEntryExtensions
    {
        public static byte[] GetBytes(this ZipArchiveEntry entry)
        {
            using (var stream = entry.Open())
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        public static void OpenAndWrite(this ZipArchiveEntry entry, byte[] bytes)
        {
            using (var stream = entry.Open())
            {
                using (var streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write(bytes);
                }
            }
        }
    }
}
