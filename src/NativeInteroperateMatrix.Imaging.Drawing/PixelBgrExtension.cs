namespace Nima.Imaging.Drawing;

public static class PixelBgrExtension
{
    /// <summary>色を変換します</summary>
    public static PixelBgr ToPixelBgr(in this System.Drawing.Color color) =>
        PixelBgr.FromBgr(color.B, color.G, color.R);
}

public static class DrawingColorExtension
{
    /// <summary>色を変換します</summary>
    public static System.Drawing.Color ToDrawingColor(in this PixelBgr pixel) =>
        System.Drawing.Color.FromArgb(pixel.Red, pixel.Green, pixel.Blue);
}
