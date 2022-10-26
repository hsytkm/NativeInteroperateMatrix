namespace Nima;

public /*sealed*/ class ByteArrayContainer : ArrayContainerBase
{
    static readonly int _bytesPerItem = Unsafe.SizeOf<Byte>();

    public ByteArrayContainer(int length, bool initialize = true)
        : base(length, _bytesPerItem, initialize)
    { }

    public ByteArrayContainer(int length, IEnumerable<Byte> items)
        : this(length, false)
    {
        int index = 0;
        var span = Array.AsSpan<Byte>();

        foreach (var item in items)
        {
            span[index++] = item;

            if (index > length)
                throw new ArgumentException("Items is large.", nameof(items));
        }

        if (index != length - 1)
            throw new ArgumentException("Items is small.", nameof(items));
    }

    public ByteArrayContainer(int length, ReadOnlySpan<Byte> items)
        : this(length, false)
    {
        int index = 0;
        var span = Array.AsSpan<Byte>();

        foreach (var item in items)
        {
            span[index++] = item;

            if (index > length)
                throw new ArgumentException("Items is large.", nameof(items));
        }

        if (index != length - 1)
            throw new ArgumentException("Items is small.", nameof(items));
    }

}
