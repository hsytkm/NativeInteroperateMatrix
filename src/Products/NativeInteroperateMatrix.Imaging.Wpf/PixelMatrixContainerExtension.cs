using System.Windows.Media.Imaging;

namespace Nima.Imaging.Wpf;

public static class PixelMatrixContainerExtension
{
    public static bool CanReuseContainer(this IPixelBgr24MatrixContainer container, BitmapSource bitmap)
    {
        if (container.Width != bitmap.PixelWidth || container.Height != bitmap.PixelHeight)
            return false;

        return true;
    }

}
