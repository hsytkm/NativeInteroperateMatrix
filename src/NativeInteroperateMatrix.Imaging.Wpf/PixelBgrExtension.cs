using System.Windows.Media;
using Nima.Core.Imaging;

namespace Nima.Imaging.Wpf;

public static class PixelBgrExtension
{
    /// <summary>色を変換します</summary>
    public static PixelBgr ToPixelBgr(this Color color) => new(color.B, color.G, color.R);
}

public static class MediaColorExtension
{
    /// <summary>色を変換します</summary>
    public static Color ToMediaColor(in this PixelBgr pixel) => new() { B = pixel.Ch0, G = pixel.Ch1, R = pixel.Ch2 };
}
