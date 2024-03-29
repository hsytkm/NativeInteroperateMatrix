﻿using System.Security.Cryptography;

namespace Nima.Core.Tests;

public static class FileComparator
{
    /// <summary>
    /// Returns the hash string for the file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static async Task<string> GetFileHashAsync(string filePath)
    {
        // see https://mseeeen.msen.jp/compute-hash-string-of-files/
#pragma warning disable SYSLIB0021    //  'SHA1CryptoServiceProvider' は旧形式です
        using var hashProvider = new SHA1CryptoServiceProvider();
#pragma warning restore SYSLIB0021
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
#if NET5_0_OR_GREATER
        var bs = await hashProvider.ComputeHashAsync(fs);
#else
        await Task.Yield();
        var bs = hashProvider.ComputeHash(fs);
#endif
        return BitConverter.ToString(bs).ToLowerInvariant().Replace("-", "");
    }

    public static async Task<bool> IsMatchAsync(string filePath1, string filePath2)
    {
        if (filePath1 == filePath2) return true;

        var task1 = GetFileHashAsync(filePath1);
        var task2 = GetFileHashAsync(filePath2);
        var hash = await Task.WhenAll(task1, task2);
        return hash[0] == hash[1];
    }
}
