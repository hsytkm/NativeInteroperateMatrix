using System.Runtime.InteropServices;

namespace Nima.Core.Imaging;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 3)]
public readonly record struct PixelBgr
{
    public byte Blue { get; }
    public byte Green { get; }
    public byte Red { get; }

    public const int Size = 3;

    private PixelBgr(byte blue, byte green, byte red) => (Blue, Green, Red) = (blue, green, red);

    public static PixelBgr FromBgr(byte blue, byte green, byte red) => new(blue, green, red);

    public static PixelBgr FromGray(byte value) => new(value, value, value);

    public override string ToString() => $"Bgr=0x{Blue:x2}{Green:x2}{Red:x2}";
}

public static class PixelBgrs
{
    public static PixelBgr White { get; } = PixelBgr.FromGray(0xff);
    public static PixelBgr Gray { get; } = PixelBgr.FromGray(0x80);
    public static PixelBgr Black { get; } = PixelBgr.FromGray(0x00);
    public static PixelBgr Red { get; } = PixelBgr.FromBgr(0x00, 0x00, 0xff);
    public static PixelBgr Green { get; } = PixelBgr.FromBgr(0x00, 0xff, 0x00);
    public static PixelBgr Blue { get; } = PixelBgr.FromBgr(0xff, 0x00, 0x00);

}
