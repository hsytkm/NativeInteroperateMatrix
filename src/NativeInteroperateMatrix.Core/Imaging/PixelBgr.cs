using System;
using System.Runtime.InteropServices;

namespace Nima.Core.Imaging
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 3)]
    public readonly struct PixelBgr : IEquatable<PixelBgr>
    {
        public readonly byte Ch0;
        public readonly byte Ch1;
        public readonly byte Ch2;

        public const int Size = 3;

        public PixelBgr(byte ch0, byte ch1, byte ch2) => (Ch0, Ch1, Ch2) = (ch0, ch1, ch2);
        public PixelBgr(byte level) : this(level, level, level) { }
        public PixelBgr(uint value)
            : this((byte)((value >> 16) & 0xff), (byte)((value >> 8) & 0xff), (byte)(value & 0xff))
        { }

        #region IEquatable<T>
        public bool Equals(PixelBgr other) => this == other;
        public override bool Equals(object? obj) => (obj is PixelBgr other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Ch0, Ch1, Ch2);
        public static bool operator ==(in PixelBgr left, in PixelBgr right) => (left.Ch0, left.Ch1, left.Ch2) == (right.Ch0, right.Ch1, right.Ch2);
        public static bool operator !=(in PixelBgr left, in PixelBgr right) => !(left == right);
        #endregion

        public override string ToString() => $"#{Ch2:x2}{Ch1:x2}{Ch0:x2}";
    }

    public static class PixelBgrs
    {
        public static readonly PixelBgr White = new(0xff);
        public static readonly PixelBgr Gray = new(0x80);
        public static readonly PixelBgr Black = new(0x00);
        public static readonly PixelBgr Red = new(0x00, 0x00, 0xff);
        public static readonly PixelBgr Green = new(0x00, 0xff, 0x00);
        public static readonly PixelBgr Blue = new(0xff, 0x00, 0x00);

    }
}
