using NativeInteroperateMatrix.Core.Imaging;
using System;
using System.Drawing;

namespace NativeInteroperateMatrix.Imaging.Drawing.Extensions
{
    public static class Pixel3chExtension
    {
        /// <summary>色を変換します</summary>
        public static Pixel3ch ToPixel3ch(in this Color color) => new(color.B, color.G, color.R);
    }

    public static class DrawingColorExtension
    {
        /// <summary>色を変換します</summary>
        public static Color ToDrawingColor(in this Pixel3ch pixel) => Color.FromArgb(pixel.Ch0, pixel.Ch1, pixel.Ch2);
    }
}
