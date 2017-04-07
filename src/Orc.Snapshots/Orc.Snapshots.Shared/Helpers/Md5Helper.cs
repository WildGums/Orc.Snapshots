// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Md5Helper.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    internal static class Md5Helper
    {
        #region Methods
        public static string ComputeMd5ForFile(string fileName)
        {
            using (var stream = File.OpenRead(fileName))
            {
                return ComputeMd5(stream);
            }
        }

        public static string ComputeMd5(byte[] input)
        {
            using (var stream = new MemoryStream(input))
            {
                return ComputeMd5(stream);
            }
        }

        public static string ComputeMd5(Stream input)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(input);
                var finalHash = BitConverter.ToString(hash).Replace("-", "").ToLower();

                return finalHash;
            }
        }
        #endregion
    }
}