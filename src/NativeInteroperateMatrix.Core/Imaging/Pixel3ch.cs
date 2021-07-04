using System;
using System.Runtime.InteropServices;

namespace NativeInteroperateMatrix.Core.Imaging
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 3)]
    public readonly struct Pixel3ch : IEquatable<Pixel3ch>
    {
        public readonly byte Ch0;
        public readonly byte Ch1;
        public readonly byte Ch2;

        public Pixel3ch(byte ch0, byte ch1, byte ch2) => (Ch0, Ch1, Ch2) = (ch0, ch1, ch2);
        public Pixel3ch(byte level) : this(level, level, level) { }
        public Pixel3ch(uint value) : this((byte)((value >> 16) & 0xff), (byte)((value >> 8) & 0xff), (byte)(value & 0xff)) { }

        public static readonly Pixel3ch White = new(0xff);
        public static readonly Pixel3ch Gray = new(0x80);
        public static readonly Pixel3ch Black = new(0x00);

        #region IEquatable<T>
        public bool Equals(Pixel3ch other) => this == other;
        public override bool Equals(object? obj) => (obj is Pixel3ch other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Ch0, Ch1, Ch2);
        public static bool operator ==(in Pixel3ch left, in Pixel3ch right) => (left.Ch0, left.Ch1, left.Ch2) == (right.Ch0, right.Ch1, right.Ch2);
        public static bool operator !=(in Pixel3ch left, in Pixel3ch right) => !(left == right);
        #endregion

    }
}
