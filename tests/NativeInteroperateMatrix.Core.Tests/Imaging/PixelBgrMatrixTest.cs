using System.IO;
using System.Threading.Tasks;
using Nima.Core.Imaging;
using Xunit;

namespace Nima.Core.Tests.Imaging.Drawing;

public class PixelBgrMatrixTest
{
    private const string TempPath = "_temp.bmp";

    public PixelBgrMatrixTest()
    {
        if (File.Exists(TempPath))
            File.Delete(TempPath);
    }

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void CtorInit(int rows, int columns)
    {
        using var container = new PixelBgrMatrixContainer(rows, columns, true);
        var matrix = container.Matrix;

        var color = new ColorBgr(0, 0, 0);
        matrix.GetChannelsAverageOfEntire().Is(color);
    }

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void FillAllPixels(int rows, int columns)
    {
        using var container = new PixelBgrMatrixContainer(rows, columns, false);
        var matrix = container.Matrix;

        var bgr = PixelBgr.FromBgr(255, 0, 128);    // teketo-
        matrix.FillAllPixels(bgr);
        matrix.GetChannelsAverageOfEntire().Is(new ColorBgr(bgr));
    }

    [Theory]
    [ClassData(typeof(ImagePathTestData))]
    public async Task ReadWrite(string sourcePath)
    {
        using var container = PixelBgrMatrixContainer.Create(sourcePath);
        container.Matrix.ToBmpFile(TempPath);

        var isMatch = await FileComparator.IsMatchAsync(sourcePath, TempPath);
        isMatch.IsTrue();

        File.Delete(TempPath);
    }

    [Theory]
    [ClassData(typeof(ImagePathTestData))]
    public async Task ReadWriteAsync(string sourcePath)
    {
        using var container = await PixelBgrMatrixContainer.CreateAsync(sourcePath);
        await container.Matrix.ToBmpFileAsync(TempPath);

        var isMatch = await FileComparator.IsMatchAsync(sourcePath, TempPath);
        isMatch.IsTrue();

        File.Delete(TempPath);
    }

}
