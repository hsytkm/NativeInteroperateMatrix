using NativeInteroperateMatrix.Core.Imaging;
using System;
using System.Windows.Media;

namespace NativeInteroperateMatrix.Imaging.Wpf.Extensions
{
    public static class Pixel3chExtension
    {
        /// <summary>色を変換します</summary>
        public static Pixel3ch ToPixel3ch(this Color color) => new(color.B, color.G, color.R);
    }

    public static class MediaColorExtension
    {
        /// <summary>色を変換します</summary>
        public static Color ToMediaColor(in this Pixel3ch pixel) => new() { B = pixel.Ch0, G = pixel.Ch1, R = pixel.Ch2 };
    }
}
