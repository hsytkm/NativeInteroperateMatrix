using NativeInteroperateMatrix.Core.Imaging;
using System;

namespace NativeInteroperateMatrix.Imaging.Wpf.Extensions
{
    public static class PixelMatrixExtension
    {
        internal static double _dpiX = 96.0;
        internal static double _dpiY = _dpiX;

        /// <summary>System.Windows.Media.Imaging.BitmapSource に変換します</summary>
        public static System.Windows.Media.Imaging.BitmapSource ToBitmapSource(in this PixelBgrMatrix pixel, bool isFreeze = true)
        {
            if (pixel.IsInvalid) throw new ArgumentException("Invalid ImagePixels");
            if (pixel.BytesPerPixel != PixelBgr.Size) throw new NotSupportedException("Invalid BytesPerPixel");

            var bitmapSource = System.Windows.Media.Imaging.BitmapSource.Create(
                pixel.Columns, pixel.Rows, _dpiX, _dpiY,
                System.Windows.Media.PixelFormats.Bgr24, null,
                pixel.Pointer, pixel.Stride * pixel.Rows, pixel.Stride);

            if (isFreeze) bitmapSource.Freeze();
            return bitmapSource;
        }

        /// <summary>System.Windows.Media.Imaging.WriteableBitmap の画素値を更新します(遅いです)</summary>
        public static void CopyTo(this System.Windows.Media.Imaging.WriteableBitmap writeableBitmap, in PixelBgrMatrix pixel, bool isFreeze = false)
        {
            if (pixel.IsInvalid) throw new ArgumentException("Invalid Image");

            if (writeableBitmap.IsFrozen) throw new ArgumentException("WriteableBitmap is frozen");
            if (writeableBitmap.IsInvalid()) throw new ArgumentException("Invalid Image");
            if (writeableBitmap.PixelWidth != pixel.Columns) throw new ArgumentException("Different Width");
            if (writeableBitmap.PixelHeight != pixel.Rows) throw new ArgumentException("Different Height");
            if (writeableBitmap.GetBytesPerPixel() != pixel.BytesPerPixel) throw new ArgumentException("Different BytesPerPixel");

            writeableBitmap.WritePixels(
                new System.Windows.Int32Rect(0, 0, pixel.Columns, pixel.Rows),
                pixel.Pointer, pixel.Stride * pixel.Rows, pixel.Stride);

            if (isFreeze) writeableBitmap.Freeze();
        }

        /// <summary>System.Windows.Media.Imaging.WriteableBitmap に変換します</summary>
        public static System.Windows.Media.Imaging.WriteableBitmap ToWriteableBitmap(in this PixelBgrMatrix pixel, bool isFreeze = false)
        {
            if (pixel.IsInvalid) throw new ArgumentException("Invalid ImagePixels");
            if (pixel.BytesPerPixel != PixelBgr.Size) throw new NotSupportedException("Invalid BytesPerPixel");

            var writeableBitmap = new System.Windows.Media.Imaging.WriteableBitmap(
                pixel.Columns, pixel.Rows, _dpiX, _dpiY,
                System.Windows.Media.PixelFormats.Bgr24, null);

            CopyTo(writeableBitmap, pixel, isFreeze);
            return writeableBitmap;
        }

    }
}
