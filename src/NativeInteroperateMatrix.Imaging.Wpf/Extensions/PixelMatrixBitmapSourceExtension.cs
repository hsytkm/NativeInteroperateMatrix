using NativeInteroperateMatrix.Core.Imaging;
using System;
using System.Buffers;
using System.Windows;
using System.Windows.Media.Imaging;

namespace NativeInteroperateMatrix.Imaging.Wpf.Extensions
{
    public static class PixelMatrixBitmapSourceExtension
    {
        /// <summary>BitmapSource に異常がないかチェックします</summary>
        internal static bool IsValid(this BitmapSource bitmap) => bitmap.PixelWidth > 0 && bitmap.PixelHeight > 0;

        /// <summary>BitmapSource に異常がないかチェックします</summary>
        internal static bool IsInvalid(this BitmapSource bitmap) => !IsValid(bitmap);

        /// <summary>1PixelのByte数を取得します</summary>
        public static int GetBytesPerPixel(this BitmapSource bitmap)
        {
            if (bitmap.IsInvalid()) throw new ArgumentException("Invalid Image");
            return (int)Math.Ceiling(bitmap.Format.BitsPerPixel / 8d);
        }

        /// <summary>ToPixel3chMatrixContainer を作成して返します</summary>
        public static Pixel3chMatrixContainer ToPixel3chMatrixContainer(this BitmapSource bitmap)
        {
            if (bitmap.IsInvalid()) throw new ArgumentException("Invalid Image");

            var container = new Pixel3chMatrixContainer(bitmap.PixelWidth, bitmap.PixelHeight);
            container.Matrix.CopyTo(bitmap);
            return container;
        }

        /// <summary>ImagePixels に画素値をコピーします</summary>
        internal static void CopyTo(in this Pixel3chMatrix pixel, BitmapSource bitmap)
        {
            if (bitmap.IsInvalid()) throw new ArgumentException("Invalid Bitmap");
            if (pixel.IsInvalid) throw new ArgumentException("Invalid Pixels");
            if (bitmap.PixelWidth != pixel.Columns) throw new ArgumentException("Different Width");
            if (bitmap.PixelHeight != pixel.Rows) throw new ArgumentException("Different Height");

            var bytesPerPixel = bitmap.GetBytesPerPixel();
            if (bytesPerPixel < pixel.BytesPerPixel) throw new ArgumentException("Invalid BytesPerPixel");

            var rect1Line = new Int32Rect(0, 0, bitmap.PixelWidth, height: 1);

            // 1行ずつメモリに読み出して処理する(ヒープ使用量の削減)
            var bufferSize = bitmap.PixelWidth * bytesPerPixel;
            var bufferArray = ArrayPool<byte>.Shared.Rent(bufferSize);
            try
            {
                unsafe
                {
                    var destHead = (byte*)pixel.Pointer;

                    fixed (byte* head = bufferArray)
                    {
                        var tail = head + bufferSize;

                        for (var y = 0; y < bitmap.PixelHeight; ++y)
                        {
                            rect1Line.Y = y;
                            bitmap.CopyPixels(rect1Line, bufferArray, bufferSize, 0);

                            var dest = (Pixel3ch*)(destHead + y * pixel.Stride);
                            for (var ptr = head; ptr < tail; ptr += bytesPerPixel)
                            {
                                *(dest++) = *((Pixel3ch*)ptr);
                            }
                        }
                    }
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(bufferArray, clearArray: true);
            }
        }
    }
}
