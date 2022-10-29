using System.Windows.Media.Imaging;

namespace Nima.Imaging.Wpf;

public static class PixelMatrixContainerExtension
{
    public static bool CanReuseContainer(this IPixelBgr24MatrixContainer container, BitmapSource bitmap)
    {
        using var token = container.GetMatrixForRead(out NativeMatrix matrix);

        if (matrix.Width != bitmap.PixelWidth || matrix.Height != bitmap.PixelHeight)
            return false;

        return true;
    }

}
