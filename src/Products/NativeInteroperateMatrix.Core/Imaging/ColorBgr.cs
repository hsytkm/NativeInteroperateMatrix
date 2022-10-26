namespace Nima.Imaging;

public sealed record ColorBgr : IFormattable
{
    public double B { get; }
    public double G { get; }
    public double R { get; }
    public double Y { get; }

    public ColorBgr(double b, double g, double r) => (B, G, R, Y) = (b, g, r, ToLuminanceY(b, g, r));
    public ColorBgr(byte b, byte g, byte r) : this((double)b, g, r) { }
    public ColorBgr(PixelBgr24 pixels) : this((double)pixels.Blue, pixels.Green, pixels.Red) { }
    public ColorBgr(ReadOnlySpan<double> channels)
    {
        if (channels.Length != 3)
            throw new ArgumentException("channels length is invalid.");

        B = channels[0];
        G = channels[1];
        R = channels[2];
        Y = ToLuminanceY(B, G, R);
    }

    private static double ToLuminanceY(double b, double g, double r) => 0.299 * r + 0.587 * g + 0.114 * b;

    public override string ToString() => $"B={B:f1}, G={G:f1}, R={R:f1}, Y={Y:f1}";

    public string ToString(string? format, IFormatProvider? formatProvider)
        => $"B={B.ToString(format, formatProvider)}, G={G.ToString(format, formatProvider)}, R={R.ToString(format, formatProvider)}, Y={Y.ToString(format, formatProvider)}";

}
