﻿// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY ArrayContainerTemplate.tt. DO NOT CHANGE IT.
// </auto-generated>
#nullable enable
namespace Nima;

public interface IByteArrayContainer : INativeArrayContainer
{
    Byte[] ToArray();
}

public /*sealed*/ class ByteArrayContainer : ArrayContainerBase, IByteArrayContainer
{
    public ByteArrayContainer(int length, bool initialize = true)
        : base(length, Unsafe.SizeOf<Byte>(), initialize)
    { }

    public ByteArrayContainer(int length, IEnumerable<Byte> items)
        : this(length, false)
    {
        int index = 0;
        var span = Array.AsSpan<Byte>();

#if NET6_0_OR_GREATER
        if (items.TryGetNonEnumeratedCount(out var count))
        {
            if (length < count)
            {
                throw new IndexOutOfRangeException("Items is large.");
            }
            else if (length > count)
            {
                throw new ArgumentException("Items is small.", nameof(items));
            }

            foreach (var item in items)
                span[index++] = item;
        }
        else
#endif
        {
            foreach (var item in items)
                span[index++] = item;

            if (index != length)
                throw new ArgumentException("Items is small.", nameof(items));
        }
    }
    
    public ByteArrayContainer(int length, ReadOnlySpan<Byte> items)
        : this(length, false)
    {
        if (length != items.Length)
            throw new ArgumentException("Items is different length.", nameof(items));

        int index = 0;
        var span = Array.AsSpan<Byte>();

        foreach (var item in items)
            span[index++] = item;
    }

    public Byte[] ToArray()
    {
        using var token = GetArrayForReading(out NativeArray array);
        return array.AsReadOnlySpan<Byte>().ToArray();
    }
}


public interface IInt16ArrayContainer : INativeArrayContainer
{
    Int16[] ToArray();
}

public /*sealed*/ class Int16ArrayContainer : ArrayContainerBase, IInt16ArrayContainer
{
    public Int16ArrayContainer(int length, bool initialize = true)
        : base(length, Unsafe.SizeOf<Int16>(), initialize)
    { }

    public Int16ArrayContainer(int length, IEnumerable<Int16> items)
        : this(length, false)
    {
        int index = 0;
        var span = Array.AsSpan<Int16>();

#if NET6_0_OR_GREATER
        if (items.TryGetNonEnumeratedCount(out var count))
        {
            if (length < count)
            {
                throw new IndexOutOfRangeException("Items is large.");
            }
            else if (length > count)
            {
                throw new ArgumentException("Items is small.", nameof(items));
            }

            foreach (var item in items)
                span[index++] = item;
        }
        else
#endif
        {
            foreach (var item in items)
                span[index++] = item;

            if (index != length)
                throw new ArgumentException("Items is small.", nameof(items));
        }
    }
    
    public Int16ArrayContainer(int length, ReadOnlySpan<Int16> items)
        : this(length, false)
    {
        if (length != items.Length)
            throw new ArgumentException("Items is different length.", nameof(items));

        int index = 0;
        var span = Array.AsSpan<Int16>();

        foreach (var item in items)
            span[index++] = item;
    }

    public Int16[] ToArray()
    {
        using var token = GetArrayForReading(out NativeArray array);
        return array.AsReadOnlySpan<Int16>().ToArray();
    }
}


public interface IInt32ArrayContainer : INativeArrayContainer
{
    Int32[] ToArray();
}

public /*sealed*/ class Int32ArrayContainer : ArrayContainerBase, IInt32ArrayContainer
{
    public Int32ArrayContainer(int length, bool initialize = true)
        : base(length, Unsafe.SizeOf<Int32>(), initialize)
    { }

    public Int32ArrayContainer(int length, IEnumerable<Int32> items)
        : this(length, false)
    {
        int index = 0;
        var span = Array.AsSpan<Int32>();

#if NET6_0_OR_GREATER
        if (items.TryGetNonEnumeratedCount(out var count))
        {
            if (length < count)
            {
                throw new IndexOutOfRangeException("Items is large.");
            }
            else if (length > count)
            {
                throw new ArgumentException("Items is small.", nameof(items));
            }

            foreach (var item in items)
                span[index++] = item;
        }
        else
#endif
        {
            foreach (var item in items)
                span[index++] = item;

            if (index != length)
                throw new ArgumentException("Items is small.", nameof(items));
        }
    }
    
    public Int32ArrayContainer(int length, ReadOnlySpan<Int32> items)
        : this(length, false)
    {
        if (length != items.Length)
            throw new ArgumentException("Items is different length.", nameof(items));

        int index = 0;
        var span = Array.AsSpan<Int32>();

        foreach (var item in items)
            span[index++] = item;
    }

    public Int32[] ToArray()
    {
        using var token = GetArrayForReading(out NativeArray array);
        return array.AsReadOnlySpan<Int32>().ToArray();
    }
}


public interface IInt64ArrayContainer : INativeArrayContainer
{
    Int64[] ToArray();
}

public /*sealed*/ class Int64ArrayContainer : ArrayContainerBase, IInt64ArrayContainer
{
    public Int64ArrayContainer(int length, bool initialize = true)
        : base(length, Unsafe.SizeOf<Int64>(), initialize)
    { }

    public Int64ArrayContainer(int length, IEnumerable<Int64> items)
        : this(length, false)
    {
        int index = 0;
        var span = Array.AsSpan<Int64>();

#if NET6_0_OR_GREATER
        if (items.TryGetNonEnumeratedCount(out var count))
        {
            if (length < count)
            {
                throw new IndexOutOfRangeException("Items is large.");
            }
            else if (length > count)
            {
                throw new ArgumentException("Items is small.", nameof(items));
            }

            foreach (var item in items)
                span[index++] = item;
        }
        else
#endif
        {
            foreach (var item in items)
                span[index++] = item;

            if (index != length)
                throw new ArgumentException("Items is small.", nameof(items));
        }
    }
    
    public Int64ArrayContainer(int length, ReadOnlySpan<Int64> items)
        : this(length, false)
    {
        if (length != items.Length)
            throw new ArgumentException("Items is different length.", nameof(items));

        int index = 0;
        var span = Array.AsSpan<Int64>();

        foreach (var item in items)
            span[index++] = item;
    }

    public Int64[] ToArray()
    {
        using var token = GetArrayForReading(out NativeArray array);
        return array.AsReadOnlySpan<Int64>().ToArray();
    }
}


public interface ISingleArrayContainer : INativeArrayContainer
{
    Single[] ToArray();
}

public /*sealed*/ class SingleArrayContainer : ArrayContainerBase, ISingleArrayContainer
{
    public SingleArrayContainer(int length, bool initialize = true)
        : base(length, Unsafe.SizeOf<Single>(), initialize)
    { }

    public SingleArrayContainer(int length, IEnumerable<Single> items)
        : this(length, false)
    {
        int index = 0;
        var span = Array.AsSpan<Single>();

#if NET6_0_OR_GREATER
        if (items.TryGetNonEnumeratedCount(out var count))
        {
            if (length < count)
            {
                throw new IndexOutOfRangeException("Items is large.");
            }
            else if (length > count)
            {
                throw new ArgumentException("Items is small.", nameof(items));
            }

            foreach (var item in items)
                span[index++] = item;
        }
        else
#endif
        {
            foreach (var item in items)
                span[index++] = item;

            if (index != length)
                throw new ArgumentException("Items is small.", nameof(items));
        }
    }
    
    public SingleArrayContainer(int length, ReadOnlySpan<Single> items)
        : this(length, false)
    {
        if (length != items.Length)
            throw new ArgumentException("Items is different length.", nameof(items));

        int index = 0;
        var span = Array.AsSpan<Single>();

        foreach (var item in items)
            span[index++] = item;
    }

    public Single[] ToArray()
    {
        using var token = GetArrayForReading(out NativeArray array);
        return array.AsReadOnlySpan<Single>().ToArray();
    }
}


public interface IDoubleArrayContainer : INativeArrayContainer
{
    Double[] ToArray();
}

public /*sealed*/ class DoubleArrayContainer : ArrayContainerBase, IDoubleArrayContainer
{
    public DoubleArrayContainer(int length, bool initialize = true)
        : base(length, Unsafe.SizeOf<Double>(), initialize)
    { }

    public DoubleArrayContainer(int length, IEnumerable<Double> items)
        : this(length, false)
    {
        int index = 0;
        var span = Array.AsSpan<Double>();

#if NET6_0_OR_GREATER
        if (items.TryGetNonEnumeratedCount(out var count))
        {
            if (length < count)
            {
                throw new IndexOutOfRangeException("Items is large.");
            }
            else if (length > count)
            {
                throw new ArgumentException("Items is small.", nameof(items));
            }

            foreach (var item in items)
                span[index++] = item;
        }
        else
#endif
        {
            foreach (var item in items)
                span[index++] = item;

            if (index != length)
                throw new ArgumentException("Items is small.", nameof(items));
        }
    }
    
    public DoubleArrayContainer(int length, ReadOnlySpan<Double> items)
        : this(length, false)
    {
        if (length != items.Length)
            throw new ArgumentException("Items is different length.", nameof(items));

        int index = 0;
        var span = Array.AsSpan<Double>();

        foreach (var item in items)
            span[index++] = item;
    }

    public Double[] ToArray()
    {
        using var token = GetArrayForReading(out NativeArray array);
        return array.AsReadOnlySpan<Double>().ToArray();
    }
}


