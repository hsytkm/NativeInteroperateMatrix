using System.IO;
using System.Windows.Media.Imaging;
using Nima.Core.Tests;
using Nima.Core.Tests.Imaging;
using Nima.Imaging.Wpf;
using Xunit;

namespace NativeInteroperateMatrix.Imaging.Wpf.Tests;

public class BitmapSourceTest : ImagePathTestData
{
    const string TempPath = "_temp.bmp";

    public BitmapSourceTest()
    {
        if (File.Exists(TempPath))
            File.Delete(TempPath);
    }

    static BitmapSource Create(string imagePath)
    {
        static BitmapImage ToBitmapImage(Stream stream)
        {
            if (stream is null) throw new ArgumentNullException(nameof(stream));

            var bi = new BitmapImage();
            bi.BeginInit();
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.StreamSource = stream;
            bi.EndInit();
            bi.Freeze();

            if (bi.Width == 1 && bi.Height == 1) throw new OutOfMemoryException();
            return bi;
        }

        using var stream = File.Open(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return ToBitmapImage(stream);
    }

    [Theory]
    [ClassData(typeof(ImagePathTestData))]
    public async Task ToContainer(string sourcePath)
    {
        try
        {
            BitmapSource bitmap = Create(sourcePath);
            using var container = bitmap.ToPixelBgr24MatrixContainer();

            await container.ToBmpFileAsync(TempPath);
            var isMatch = await FileComparator.IsMatchAsync(sourcePath, TempPath);
            isMatch.IsTrue();
        }
        finally
        {
            File.Delete(TempPath);
        }
    }
}
