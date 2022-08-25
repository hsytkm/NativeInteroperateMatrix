using System.Runtime.InteropServices;

namespace Nima.Imaging;

// http://www.umekkii.jp/data/computer/file_format/bitmap.cgi
[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal readonly struct BitmapHeader
{
    // Bitmap File Header
    public readonly short FileType;
    public readonly int FileSize;
    public readonly short Reserved1;
    public readonly short Reserved2;
    public readonly int OffsetBytes;

    // Bitmap Information Header
    public readonly int InfoSize;
    public readonly int Width;
    public readonly int Height;
    public readonly short Planes;
    public readonly short BitCount;
    public readonly int Compression;
    public readonly int SizeImage;
    public readonly int XPixPerMete;
    public readonly int YPixPerMete;
    public readonly int ClrUsed;
    public readonly int CirImportant;

    private const int PixelPerMeter = 3780;    // pixel/meter (96dpi / 2.54cm * 100m)
    private const int FileHeaderSize = 14;
    private const int InfoHeaderSize = 40;
    private const int TotalHeaderSize = FileHeaderSize + InfoHeaderSize;

    public BitmapHeader(int width, int height, int bitsPerPixel)
    {
        var imageSize = GetImageSize(width, height, bitsPerPixel);

        FileType = 0x4d42;  // 'B','M'
        FileSize = TotalHeaderSize + imageSize;
        Reserved1 = 0;
        Reserved2 = 0;
        OffsetBytes = TotalHeaderSize;

        InfoSize = InfoHeaderSize;
        Width = width;
        Height = height;
        Planes = 1;
        BitCount = (short)bitsPerPixel;
        Compression = 0;
        SizeImage = 0;      // 無圧縮の場合、ファイルサイズでなく 0 を設定するみたい
        XPixPerMete = PixelPerMeter;
        YPixPerMete = PixelPerMeter;
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
            if (OffsetBytes != TotalHeaderSize) return false;
            if (InfoSize != InfoHeaderSize) return false;
            if (BitCount != 8 * 3) return false;
            if (FileSize != GetImageSize(Width, Height, BitCount) + TotalHeaderSize) return false;
            return true;
        }
    }

}
