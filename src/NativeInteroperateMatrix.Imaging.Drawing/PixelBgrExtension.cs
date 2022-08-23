using System.Drawing;
using Nima.Core.Imaging;

namespace Nima.Imaging.Drawing;

public static class PixelBgrExtension
{
    /// <summary>色を変換します</summary>
    public static PixelBgr ToPixelBgr(in this Color color) => new(color.B, color.G, color.R);
}

public static class DrawingColorExtension
{
    /// <summary>色を変換します</summary>
    public static Color ToDrawingColor(in this PixelBgr pixel) => Color.FromArgb(pixel.Ch0, pixel.Ch1, pixel.Ch2);
}
