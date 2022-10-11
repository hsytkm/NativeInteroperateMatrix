namespace Nima.Imaging.Wpf;

public static class PixelMatrixExtension
{
    static readonly double _dpiX = 96.0;
    static readonly double _dpiY = _dpiX;

    /// <summary>System.Windows.Media.Imaging.BitmapSource に変換します</summary>
    public static System.Windows.Media.Imaging.BitmapSource ToBitmapSource(in this PixelBgr24Matrix pixel, bool isFreeze = true)
    {
        if (!pixel.IsValid) throw new ArgumentException("Invalid ImagePixels");
        if (pixel.BytesPerPixel != PixelBgr24.Size) throw new NotSupportedException("Invalid BytesPerPixel");

        var bitmapSource = System.Windows.Media.Imaging.BitmapSource.Create(
            pixel.Width, pixel.Height, _dpiX, _dpiY,
            System.Windows.Media.PixelFormats.Bgr24, null,
            pixel.Pointer, pixel.Stride * pixel.Width, pixel.Stride);

        if (isFreeze) bitmapSource.Freeze();
        return bitmapSource;
    }

    /// <summary>System.Windows.Media.Imaging.WriteableBitmap の画素値を更新します(遅いです)</summary>
    public static void CopyFrom(this System.Windows.Media.Imaging.WriteableBitmap writeableBitmap, in PixelBgr24Matrix pixel, bool isFreeze = false)
    {
        if (!pixel.IsValid) throw new ArgumentException("Invalid Image");

        if (writeableBitmap.IsFrozen) throw new ArgumentException("WriteableBitmap is frozen");
        if (writeableBitmap.IsInvalid()) throw new ArgumentException("Invalid Image");
        if (writeableBitmap.PixelWidth != pixel.Width) throw new ArgumentException("Different Width");
        if (writeableBitmap.PixelHeight != pixel.Height) throw new ArgumentException("Different Height");
        if (writeableBitmap.GetBytesPerPixel() != pixel.BytesPerPixel) throw new ArgumentException("Different BytesPerPixel");

        writeableBitmap.WritePixels(
            new System.Windows.Int32Rect(0, 0, pixel.Width, pixel.Height),
            pixel.Pointer, pixel.Stride * pixel.Width, pixel.Stride);

        if (isFreeze) writeableBitmap.Freeze();
    }

    /// <summary>System.Windows.Media.Imaging.WriteableBitmap に変換します</summary>
    public static System.Windows.Media.Imaging.WriteableBitmap ToWriteableBitmap(in this PixelBgr24Matrix pixel, bool isFreeze = false)
    {
        if (!pixel.IsValid) throw new ArgumentException("Invalid ImagePixels");
        if (pixel.BytesPerPixel != PixelBgr24.Size) throw new NotSupportedException("Invalid BytesPerPixel");

        var writeableBitmap = new System.Windows.Media.Imaging.WriteableBitmap(
            pixel.Width, pixel.Height, _dpiX, _dpiY,
            System.Windows.Media.PixelFormats.Bgr24, null);

        CopyFrom(writeableBitmap, pixel, isFreeze);
        return writeableBitmap;
    }

}
