// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZipEntryExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System.IO;
    using Ionic.Zip;

    internal static class ZipEntryExtensions
    {
        public static byte[] GetBytes(this ZipEntry entry)
        {
            using (var memoryStream = new MemoryStream())
            {
                entry.Extract(memoryStream);

                return memoryStream.ToArray();
            }
        }
    }
}