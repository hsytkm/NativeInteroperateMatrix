using Nima.Core;
using Nima.Core.Imaging;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Nima.Imaging.Drawing
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

        /// <summary>PixelBgrMatrixContainer を作成して返します</summary>
        public static PixelBgrMatrixContainer ToPixelBgrMatrixContainer(this Bitmap bitmap, bool isDisposeBitmap = false)
        {
            if (bitmap.IsInvalid()) throw new ArgumentException("Invalid Image");

            var container = new PixelBgrMatrixContainer(rows: bitmap.Height, columns: bitmap.Width, false);
            var pixels = container.Matrix;

            switch (bitmap.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    Copy24bitToBgrMatrix(bitmap, pixels, isDisposeBitmap);
                    break;
                case PixelFormat.Format8bppIndexed:
                    // ◆未テスト
                    Copy8bitToBgrMatrix(bitmap, pixels, isDisposeBitmap);
                    break;
                default:
                    throw new NotImplementedException($"PixelFormat : {bitmap.PixelFormat}");
            }
            return container;
        }

        /// <summary>PixelBgrMatrix に画素値をコピーします</summary>
        public static void Copy24bitToBgrMatrix(this Bitmap bitmap, in PixelBgrMatrix pixels, bool isDisposeBitmap = false)
        {
            if (bitmap.IsInvalid()) throw new ArgumentException("Invalid Bitmap");
            if (!pixels.IsValid) throw new ArgumentException("Invalid Pixels");
            if (bitmap.Width != pixels.Columns) throw new ArgumentException("Different Width");
            if (bitmap.Height != pixels.Rows) throw new ArgumentException("Different Height");

            var srcBytesPerPixel = bitmap.GetBytesPerPixel();
            if (srcBytesPerPixel < pixels.BytesPerItem)
                throw new NotImplementedException("Different BytesPerPixel");

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
                            UnsafeUtils.MemCopy(destPtr, srcPtr, columnLength);
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
                                *(PixelBgr*)(destPtr + x * destBytesPerPixel) = *(PixelBgr*)(srcPtr + x * srcBytesPerPixel);
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

        /// <summary>PixelBgrMatrix に画素値をコピーします</summary>
        public static void Copy8bitToBgrMatrix(this Bitmap bitmap, in PixelBgrMatrix pixels, bool isDisposeBitmap = false)
        {
            if (bitmap.IsInvalid()) throw new ArgumentException("Invalid Bitmap");
            if (!pixels.IsValid) throw new ArgumentException("Invalid Pixels");
            if (bitmap.Width != pixels.Columns) throw new ArgumentException("Different Width");
            if (bitmap.Height != pixels.Rows) throw new ArgumentException("Different Height");

            if (bitmap.GetBytesPerPixel() != 1)
                throw new NotImplementedException("Different BytesPerPixel");

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

                    for (byte* srcPtr = srcHead, destPtr = destHead;
                         srcPtr < srcPtrTail;
                         srcPtr += srcStride, destPtr += destStride)
                    {
                        for (var x = 0; x < bitmap.Width; ++x)
                        {
                            *(PixelBgr*)(destPtr + x * destBytesPerPixel) = new PixelBgr(*(srcPtr + x));
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
