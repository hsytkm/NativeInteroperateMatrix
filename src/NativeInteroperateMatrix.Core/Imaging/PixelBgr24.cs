using System.Runtime.InteropServices;

namespace Nima.Imaging;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 3)]
public readonly record struct PixelBgr24
{
    public byte Blue { get; }
    public byte Green { get; }
    public byte Red { get; }

    public const int Size = 3;

    private PixelBgr24(byte blue, byte green, byte red) => (Blue, Green, Red) = (blue, green, red);

    public static PixelBgr24 FromBgr(byte blue, byte green, byte red) => new(blue, green, red);

    public static PixelBgr24 FromGray(byte value) => new(value, value, value);

    public override string ToString() => $"Bgr=0x{Blue:x2}{Green:x2}{Red:x2}";
}

public static class PixelBgr24Color
{
    public static PixelBgr24 White { get; } = PixelBgr24.FromGray(0xff);
    public static PixelBgr24 Gray { get; } = PixelBgr24.FromGray(0x80);
    public static PixelBgr24 Black { get; } = PixelBgr24.FromGray(0x00);
    public static PixelBgr24 Red { get; } = PixelBgr24.FromBgr(0x00, 0x00, 0xff);
    public static PixelBgr24 Green { get; } = PixelBgr24.FromBgr(0x00, 0xff, 0x00);
    public static PixelBgr24 Blue { get; } = PixelBgr24.FromBgr(0xff, 0x00, 0x00);

}
