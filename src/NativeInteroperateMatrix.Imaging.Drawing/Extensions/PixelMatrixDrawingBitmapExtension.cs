using NativeInteroperateMatrix.Core;
using NativeInteroperateMatrix.Core.Imaging;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace NativeInteroperateMatrix.Imaging.Drawing.Extensions
{
    public static class PixelMatrixDrawingBitmapExtension
    {
        /// <summary>Bitmapに異常がないかチェックします</summary>
        public static bool IsValid(this Bitmap bitmap)
        {
            if (bitmap.Width == 0 || bitmap.Height == 0) return false;
            return true;
        }

        /// <summary>Bitmapに異常がないかチェックします</summary>
        public static bool IsInvalid(this Bitmap bitmap) => !IsValid(bitmap);

        /// <summary>1PixelのByte数を取得します</summary>
        public static int GetBytesPerPixel(this Bitmap bitmap)
        {
            if (bitmap.IsInvalid()) throw new ArgumentException("Invalid Image");
            return Ceiling(Image.GetPixelFormatSize(bitmap.PixelFormat), 8);

            static int Ceiling(int value, int div) => (value + (div - 1)) / div;
        }

        /// <summary>Pixel3chMatrixContainer を作成して返します</summary>
        public static Pixel3chMatrixContainer ToPixel3chMatrixContainer(this Bitmap bitmap, bool isDisposeBitmap = false)
        {
            if (bitmap.IsInvalid()) throw new ArgumentException("Invalid Image");

            var container = new Pixel3chMatrixContainer(bitmap.Width, bitmap.Height);
            var pixels = container.Matrix;
            CopyTo(bitmap, pixels, isDisposeBitmap);

            return container;
        }

        /// <summary>Pixel3chMatrix に画素値をコピーします</summary>
        public static void CopyTo(this Bitmap bitmap, in Pixel3chMatrix pixels, bool isDisposeBitmap = false)
        {
            if (bitmap.IsInvalid()) throw new ArgumentException("Invalid Bitmap");
            if (pixels.IsInvalid) throw new ArgumentException("Invalid Pixels");
            if (bitmap.Width != pixels.Columns) throw new ArgumentException("Different Width");
            if (bitmap.Height != pixels.Rows) throw new ArgumentException("Different Height");

            var srcBytesPerPixel = bitmap.GetBytesPerPixel();
            if (srcBytesPerPixel < pixels.BytesPerItem) throw new NotImplementedException("Different BytesPerPixel");

            var bitmapData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            try
            {
                unsafe
                {
                    var srcHead = (byte*)bitmapData.Scan0;
                    var srcStride = bitmapData.Stride;
                    var srcPtrTail = srcHead + (bitmap.Height * srcStride);

                    var destHead = (byte*)pixels.Pointer;
                    var destStride = pixels.Stride;
                    var destBytesPerPixel = pixels.BytesPerItem;

                    var isSameLength = srcBytesPerPixel == destBytesPerPixel;

                    if (isSameLength)
                    {
                        // BytesPerPixel の一致を前提に行を丸ごとコピー
                        var columnLength = bitmap.Width * srcBytesPerPixel;

                        for (byte* srcPtr = srcHead, destPtr = destHead;
                             srcPtr < srcPtrTail;
                             srcPtr += srcStride, destPtr += destStride)
                        {
                            UnsafeHelper.MemCopy(destPtr, srcPtr, columnLength);
                        }
                    }
                    else
                    {
                        for (byte* srcPtr = srcHead, destPtr = destHead;
                             srcPtr < srcPtrTail;
                             srcPtr += srcStride, destPtr += destStride)
                        {
                            for (var x = 0; x < bitmap.Width; ++x)
                            {
                                *(Pixel3ch*)(destPtr + x * destBytesPerPixel) = *(Pixel3ch*)(srcPtr + x * srcBytesPerPixel);
                            }
                        }
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }

            if (isDisposeBitmap) bitmap.Dispose();
        }

    }
}
