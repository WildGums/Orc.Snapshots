﻿namespace Orc.Snapshots.Tests;

using System;

internal static class AssemblyDirectoryHelper
{
    public static string GetCurrentDirectory()
    {
        var directory = AppDomain.CurrentDomain.BaseDirectory;
        return directory;
    }
}