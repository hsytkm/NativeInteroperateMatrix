namespace Nima.Imaging.Wpf;

public static class PixelBgrExtension
{
    /// <summary>色を変換します</summary>
    public static PixelBgr ToPixelBgr(this System.Windows.Media.Color color) =>
        PixelBgr.FromBgr(color.B, color.G, color.R);
}

public static class MediaColorExtension
{
    /// <summary>色を変換します</summary>
    public static System.Windows.Media.Color ToMediaColor(in this PixelBgr pixel) =>
        System.Windows.Media.Color.FromRgb(pixel.Red, pixel.Green, pixel.Blue);
}
