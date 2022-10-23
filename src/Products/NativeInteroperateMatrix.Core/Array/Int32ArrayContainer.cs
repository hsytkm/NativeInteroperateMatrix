using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Nima;

public /*sealed*/ class Int32ArrayContainer : ByteArrayContainer
{
    public Int32ArrayContainer(int length, bool initialize = true)
        : base(length, Unsafe.SizeOf<Int32>(), initialize)
    { }

    public Int32ArrayContainer(int length, IEnumerable<Int32> items)
        : this(length, false)
    {
        int index = 0;
        var span = AllocatedMemory.AsSpan<Int32>();

        foreach (var item in items)
        {
            span[index++] = item;

            if (index > length)
                throw new ArgumentException("Items is large.", nameof(items));
        }

        if (index != length - 1)
            throw new ArgumentException("Items is small.", nameof(items));
    }

    public Int32ArrayContainer(int length, ReadOnlySpan<Int32> items)
        : this(length, false)
    {
        int index = 0;
        var span = AllocatedMemory.AsSpan<Int32>();

        foreach (var item in items)
        {
            span[index++] = item;

            if (index > length)
                throw new ArgumentException("Items is large.", nameof(items));
        }

        if (index != length - 1)
            throw new ArgumentException("Items is small.", nameof(items));
    }

    public Int32ArrayContainer(int length, ReadOnlySpan<byte> items)
        : this(length, MemoryMarshal.Cast<byte, Int32>(items))
    { }

}
