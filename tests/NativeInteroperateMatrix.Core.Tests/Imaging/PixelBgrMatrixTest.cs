using Nima.Imaging;
using Xunit;

namespace Nima.Core.Tests.Imaging;

public class PixelBgrMatrixTest
{
    const string TempPath = "_temp.bmp";

    public PixelBgrMatrixTest()
    {
        if (File.Exists(TempPath))
            File.Delete(TempPath);
    }

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void CtorInit(int rows, int columns)
    {
        using var container = new PixelBgr24MatrixContainer(rows, columns, true);

        var color = new ColorBgr(0, 0, 0);
        container.GetChannelsAverageOfEntire().Is(color);
    }

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void FillAllPixels(int rows, int columns)
    {
        using var container = new PixelBgr24MatrixContainer(rows, columns, false);

        var bgr = PixelBgr24.FromBgr(255, 0, 128);    // teketo-
        container.FillAllPixels(bgr);
        container.GetChannelsAverageOfEntire().Is(new ColorBgr(bgr));
    }

    [Theory]
    [ClassData(typeof(ImagePathTestData))]
    public async Task ReadWrite(string sourcePath)
    {
        using var container = PixelBgr24MatrixContainer.Create(sourcePath);
        container.ToBmpFile(TempPath);

        var isMatch = await FileComparator.IsMatchAsync(sourcePath, TempPath);
        isMatch.IsTrue();

        File.Delete(TempPath);
    }

    [Theory]
    [ClassData(typeof(ImagePathTestData))]
    public async Task ReadWriteAsync(string sourcePath)
    {
        using var container = PixelBgr24MatrixContainer.Create(sourcePath);
        await container.ToBmpFileAsync(TempPath);

        var isMatch = await FileComparator.IsMatchAsync(sourcePath, TempPath);
        isMatch.IsTrue();

        File.Delete(TempPath);
    }

}
