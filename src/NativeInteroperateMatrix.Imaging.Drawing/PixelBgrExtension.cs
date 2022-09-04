namespace Nima.Imaging.Drawing;

public static class PixelBgrExtension
{
    /// <summary>色を変換します</summary>
    public static PixelBgr24 ToPixelBgr(in this System.Drawing.Color color) =>
        PixelBgr24.FromBgr(color.B, color.G, color.R);
}

public static class DrawingColorExtension
{
    /// <summary>色を変換します</summary>
    public static System.Drawing.Color ToDrawingColor(in this PixelBgr24 pixel) =>
        System.Drawing.Color.FromArgb(pixel.Red, pixel.Green, pixel.Blue);
}
