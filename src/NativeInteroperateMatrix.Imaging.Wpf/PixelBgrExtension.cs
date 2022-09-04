namespace Nima.Imaging.Wpf;

public static class PixelBgrExtension
{
    /// <summary>色を変換します</summary>
    public static PixelBgr24 ToPixelBgr(this System.Windows.Media.Color color) =>
        PixelBgr24.FromBgr(color.B, color.G, color.R);
}

public static class MediaColorExtension
{
    /// <summary>色を変換します</summary>
    public static System.Windows.Media.Color ToMediaColor(in this PixelBgr24 pixel) =>
        System.Windows.Media.Color.FromRgb(pixel.Red, pixel.Green, pixel.Blue);
}
