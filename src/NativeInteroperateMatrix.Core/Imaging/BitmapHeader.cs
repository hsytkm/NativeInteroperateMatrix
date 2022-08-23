using System;
using System.Runtime.InteropServices;

namespace Nima.Core.Imaging;

// http://www.umekkii.jp/data/computer/file_format/bitmap.cgi
[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal readonly struct BitmapHeader
{
    // Bitmap File Header
    public readonly Int16 FileType;
    public readonly Int32 FileSize;
    public readonly Int16 Reserved1;
    public readonly Int16 Reserved2;
    public readonly Int32 OffsetBytes;

    // Bitmap Information Header
    public readonly Int32 InfoSize;
    public readonly Int32 Width;
    public readonly Int32 Height;
    public readonly Int16 Planes;
    public readonly Int16 BitCount;
    public readonly Int32 Compression;
    public readonly Int32 SizeImage;
    public readonly Int32 XPixPerMete;
    public readonly Int32 YPixPerMete;
    public readonly Int32 ClrUsed;
    public readonly Int32 CirImportant;

    private const int _pixelPerMeter = 3780;    // pixel/meter (96dpi / 2.54cm * 100m)
    private const int _fileHeaderSize = 14;
    private const int _infoHeaderSize = 40;
    private const int _totalHeaderSize = _fileHeaderSize + _infoHeaderSize;

    public BitmapHeader(int width, int height, int bitsPerPixel)
    {
        var imageSize = GetImageSize(width, height, bitsPerPixel);

        FileType = 0x4d42;  // 'B','M'
        FileSize = _totalHeaderSize + imageSize;
        Reserved1 = 0;
        Reserved2 = 0;
        OffsetBytes = _totalHeaderSize;

        InfoSize = _infoHeaderSize;
        Width = width;
        Height = height;
        Planes = 1;
        BitCount = (Int16)bitsPerPixel;
        Compression = 0;
        SizeImage = 0;      // 無圧縮の場合、ファイルサイズでなく 0 を設定するみたい
        XPixPerMete = _pixelPerMeter;
        YPixPerMete = _pixelPerMeter;
        ClrUsed = 0;
        CirImportant = 0;
    }
    private static int BitsToBytes(int bits) => (int)Math.Ceiling(bits / 8d);
    public int BytesPerPixel => BitsToBytes(BitCount);

    /// <summary>Bytes per row</summary>
    public int ImageStride => GetImageStride(Width, BitCount);

    private static int GetImageStride(int width, int bitsPerPixel)
    {
        var bytesPerPixel = BitsToBytes(bitsPerPixel);
        return (int)Math.Ceiling(width * bytesPerPixel / 4d) * 4;   // strideは4の倍数
    }

    private static int GetImageSize(int width, int height, int bitsPerPixel)
        => GetImageStride(width, bitsPerPixel) * height;

    public bool CanRead
    {
        get
        {
            if (FileType != 0x4d42) return false;
            if (OffsetBytes != _totalHeaderSize) return false;
            if (InfoSize != _infoHeaderSize) return false;
            if (BitCount != 8 * 3) return false;
            if (FileSize != GetImageSize(Width, Height, BitCount) + _totalHeaderSize) return false;
            return true;
        }
    }

}
