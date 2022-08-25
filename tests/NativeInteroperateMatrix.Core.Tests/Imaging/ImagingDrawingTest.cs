﻿using System.Drawing;
using Nima.Imaging.Drawing;
using Xunit;

namespace Nima.Core.Tests.Imaging.Drawing;

public class ImagingDrawingTest
{
    private const string TempPath = "_temp.bmp";
    public ImagingDrawingTest()
    {
        if (File.Exists(TempPath))
            File.Delete(TempPath);
    }

    [Theory]
    [ClassData(typeof(ImagePathTestData))]
    public async Task ToContainer(string sourcePath)
    {
        try
        {
#pragma warning disable CA1416    // 'Bitmap' は 'windows' でのみサポートされています
            using var drawing = new Bitmap(sourcePath);
#pragma warning restore CA1416
            using var container = drawing.ToPixelBgrMatrixContainer();

            await container.Matrix.ToBmpFileAsync(TempPath);
            var isMatch = await FileComparator.IsMatchAsync(sourcePath, TempPath);
            isMatch.IsTrue();
        }
        finally
        {
            File.Delete(TempPath);
        }
    }

}
