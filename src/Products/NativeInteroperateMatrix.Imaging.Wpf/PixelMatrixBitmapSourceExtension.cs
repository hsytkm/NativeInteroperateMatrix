using System.Buffers;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Nima.Imaging.Wpf;

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

    /// <summary>ToPixelBgrMatrixContainer を作成して返します</summary>
    public static IPixelBgr24MatrixContainer ToPixelBgr24MatrixContainer(this BitmapSource bitmap)
    {
        if (bitmap.IsInvalid()) throw new ArgumentException("Invalid Image");

        var container = new PixelBgr24MatrixContainer(rows: bitmap.PixelHeight, columns: bitmap.PixelWidth, false);
        container.CopyFrom(bitmap);
        return container;
    }

    /// <summary>BitmapSource から PixelBgr24Matrix に画素値をコピーします</summary>
    public static void CopyFrom(this IPixelBgr24MatrixContainer container, BitmapSource bitmap)
    {
        using var disposable = container.GetMatrixForRead(out var matrix);

        if (bitmap.IsInvalid()) throw new ArgumentException("Invalid Bitmap");
        if (!matrix.IsValid) throw new ArgumentException("Invalid Pixels");
        if (bitmap.PixelWidth != matrix.Width) throw new ArgumentException("Different Width");
        if (bitmap.PixelHeight != matrix.Height) throw new ArgumentException("Different Height");

        var bytesPerPixel = bitmap.GetBytesPerPixel();
        if (bytesPerPixel < matrix.BytesPerItem) throw new ArgumentException("Invalid BytesPerPixel");

        var rect1Line = new Int32Rect(0, 0, bitmap.PixelWidth, height: 1);

        // 1行ずつメモリに読み出して処理する(ヒープ使用量の削減)
        var bufferSize = bitmap.PixelWidth * bytesPerPixel;
        var bufferArray = ArrayPool<byte>.Shared.Rent(bufferSize); // Sliceしていません。
        try
        {
            unsafe
            {
                var destHead = (byte*)matrix.Pointer;

                fixed (byte* head = bufferArray)
                {
                    var tail = head + bufferSize;

                    for (var y = 0; y < bitmap.PixelHeight; ++y)
                    {
                        rect1Line.Y = y;
                        bitmap.CopyPixels(rect1Line, bufferArray, bufferSize, 0);

                        var dest = (PixelBgr24*)(destHead + y * matrix.Stride);
                        for (var ptr = head; ptr < tail; ptr += bytesPerPixel)
                        {
                            *(dest++) = *((PixelBgr24*)ptr);
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
