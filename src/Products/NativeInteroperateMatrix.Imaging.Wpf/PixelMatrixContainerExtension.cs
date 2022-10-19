using System.Windows.Media.Imaging;

namespace Nima.Imaging.Wpf;

public static class PixelMatrixContainerExtension
{
    public static bool CanReuseContainer(this PixelBgr24MatrixContainer container, BitmapSource bitmap)
    {
        if (container.Matrix.Width != bitmap.PixelWidth
            || container.Matrix.Height != bitmap.PixelHeight)
            return false;

        return true;
    }

}
