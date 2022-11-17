namespace Nima.Imaging.Wpf;

public static class PixelMatrixExtension
{
    static readonly double _dpiX = 96.0;
    static readonly double _dpiY = _dpiX;

    /// <summary>System.Windows.Media.Imaging.BitmapSource に変換します</summary>
    public static System.Windows.Media.Imaging.BitmapSource ToBitmapSource(this IPixelBgr24MatrixContainer container, bool isFreeze = true)
    {
        using var token = container.GetMatrixForReading(out NativeMatrix matrix);

        if (!matrix.IsValid) throw new ArgumentException("Invalid ImagePixels");
        if (matrix.BytesPerItem != PixelBgr24.Size) throw new NotSupportedException("Invalid BytesPerPixel");

        var bitmapSource = System.Windows.Media.Imaging.BitmapSource.Create(
            matrix.Width, matrix.Height, _dpiX, _dpiY,
            System.Windows.Media.PixelFormats.Bgr24, null,
            matrix.Pointer, matrix.Stride * matrix.Height, matrix.Stride);

        if (isFreeze) bitmapSource.Freeze();
        return bitmapSource;
    }

    /// <summary>System.Windows.Media.Imaging.WriteableBitmap の画素値を更新します(遅いです)</summary>
    public static void CopyFrom(this System.Windows.Media.Imaging.WriteableBitmap writeableBitmap, IPixelBgr24MatrixContainer container, bool isFreeze = false)
    {
        using var token = container.GetMatrixForReading(out NativeMatrix matrix);

        if (!matrix.IsValid) throw new ArgumentException("Invalid Image");

        if (writeableBitmap.IsFrozen) throw new ArgumentException("WriteableBitmap is frozen");
        if (writeableBitmap.IsInvalid()) throw new ArgumentException("Invalid Image");
        if (writeableBitmap.PixelWidth != matrix.Width) throw new ArgumentException("Different Width");
        if (writeableBitmap.PixelHeight != matrix.Height) throw new ArgumentException("Different Height");
        if (writeableBitmap.GetBytesPerPixel() != matrix.BytesPerItem) throw new ArgumentException("Different BytesPerPixel");

        writeableBitmap.WritePixels(
            new System.Windows.Int32Rect(0, 0, matrix.Width, matrix.Height),
            matrix.Pointer, matrix.Stride * matrix.Height, matrix.Stride);

        if (isFreeze) writeableBitmap.Freeze();
    }

    /// <summary>System.Windows.Media.Imaging.WriteableBitmap に変換します</summary>
    public static System.Windows.Media.Imaging.WriteableBitmap ToWriteableBitmap(this IPixelBgr24MatrixContainer container, bool isFreeze = false)
    {
        using var token = container.GetMatrixForReading(out NativeMatrix matrix);

        if (!matrix.IsValid) throw new ArgumentException("Invalid ImagePixels");
        if (matrix.BytesPerItem != PixelBgr24.Size) throw new NotSupportedException("Invalid BytesPerPixel");

        var writeableBitmap = new System.Windows.Media.Imaging.WriteableBitmap(
            matrix.Width, matrix.Height, _dpiX, _dpiY,
            System.Windows.Media.PixelFormats.Bgr24, null);

        writeableBitmap.CopyFrom(container, isFreeze);
        return writeableBitmap;
    }

}
