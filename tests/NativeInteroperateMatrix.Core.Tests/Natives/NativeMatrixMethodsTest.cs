﻿using System.Runtime.InteropServices;
using Nima;
using Nima.Core.Tests;
using Xunit;

namespace NativeInteroperateMatrix.Core.Tests.Natives;

internal static class NativeMatrixMethods
{
    private const string DllName = "NativeInteroperateMatrix.Tests.Natives.Core.dll";

    // Byte
    [DllImport(DllName, EntryPoint = "SumByteMatrix")]
    public static extern long Sum(in ByteMatrix matrix);
    [DllImport(DllName, EntryPoint = "ClearByteMatrix")]
    public static extern void Clear(in ByteMatrix matrix);

    // Int8
    [DllImport(DllName, EntryPoint = "SumInt8Matrix")]
    public static extern long Sum(in Int8Matrix matrix);
    [DllImport(DllName, EntryPoint = "ClearInt8Matrix")]
    public static extern void Clear(in Int8Matrix matrix);

    // Int16
    [DllImport(DllName, EntryPoint = "SumInt16Matrix")]
    public static extern long Sum(in Int16Matrix matrix);
    [DllImport(DllName, EntryPoint = "ClearInt16Matrix")]
    public static extern void Clear(in Int16Matrix matrix);

    // Int32
    [DllImport(DllName, EntryPoint = "SumInt32Matrix")]
    public static extern long Sum(in Int32Matrix matrix);
    [DllImport(DllName, EntryPoint = "ClearInt32Matrix")]
    public static extern void Clear(in Int32Matrix matrix);

    // Int64
    [DllImport(DllName, EntryPoint = "SumInt64Matrix")]
    public static extern long Sum(in Int64Matrix matrix);
    [DllImport(DllName, EntryPoint = "ClearInt64Matrix")]
    public static extern void Clear(in Int64Matrix matrix);

    // Single
    [DllImport(DllName, EntryPoint = "SumSingleMatrix")]
    public static extern double Sum(in SingleMatrix matrix);
    [DllImport(DllName, EntryPoint = "ClearSingleMatrix")]
    public static extern void Clear(in SingleMatrix matrix);

    // Double
    [DllImport(DllName, EntryPoint = "SumDoubleMatrix")]
    public static extern double Sum(in DoubleMatrix matrix);
    [DllImport(DllName, EntryPoint = "ClearDoubleMatrix")]
    public static extern void Clear(in DoubleMatrix matrix);
}

public class NativeMatrixMethodsTest
{
    private static long GetMaxValue<T>(long count)
    {
        var type = typeof(T);
        long max = 0;
        if (type == typeof(byte)) max = byte.MaxValue;
        else if (type == typeof(short)) max = short.MaxValue;
        else if (type == typeof(int)) max = int.MaxValue;
        else if (type == typeof(long)) max = long.MaxValue;

        var value = max / count;
        return Math.Clamp(value, 1, max);
    }

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void SumInt8Matrix(int rows, int columns)
    {
        var max = GetMaxValue<byte>(rows * columns);
        var items = Enumerable.Range(0, rows * columns).Select(x => (sbyte)(x % max)).ToArray();
        using var container = new Int8MatrixContainer(rows, columns, items.AsSpan());

        var expected = items.Select(x => (long)x).Sum();
        NativeMatrixMethods.Sum(container.Matrix).Is(expected);

        NativeMatrixMethods.Clear(container.Matrix);
        NativeMatrixMethods.Sum(container.Matrix).Is(0);
    }

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void SumInt16Matrix(int rows, int columns)
    {
        var max = GetMaxValue<short>(rows * columns);
        var items = Enumerable.Range(0, rows * columns).Select(x => (short)(x % max)).ToArray();
        using var container = new Int16MatrixContainer(rows, columns, items.AsSpan());

        var expected = items.Select(x => (long)x).Sum();
        NativeMatrixMethods.Sum(container.Matrix).Is(expected);

        NativeMatrixMethods.Clear(container.Matrix);
        NativeMatrixMethods.Sum(container.Matrix).Is(0);
    }

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void SumInt32Matrix(int rows, int columns)
    {
        var max = GetMaxValue<int>(rows * columns);
        var items = Enumerable.Range(0, rows * columns).Select(x => (int)(x % max)).ToArray();
        using var container = new Int32MatrixContainer(rows, columns, items.AsSpan());

        var expected = items.Select(x => (long)x).Sum();
        NativeMatrixMethods.Sum(container.Matrix).Is(expected);

        NativeMatrixMethods.Clear(container.Matrix);
        NativeMatrixMethods.Sum(container.Matrix).Is(0);
    }

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void SumInt64Matrix(int rows, int columns)
    {
        var max = GetMaxValue<long>(rows * columns);
        var items = Enumerable.Range(0, rows * columns).Select(x => (long)(x % max)).ToArray();
        using var container = new Int64MatrixContainer(rows, columns, items.AsSpan());

        var expected = items.Select(x => (long)x).Sum();
        NativeMatrixMethods.Sum(container.Matrix).Is(expected);

        NativeMatrixMethods.Clear(container.Matrix);
        NativeMatrixMethods.Sum(container.Matrix).Is(0);
    }

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void SumSingleMatrix(int rows, int columns)
    {
        var denom = 100f;
        var max = GetMaxValue<int>(rows * columns);
        var items = Enumerable.Range(0, rows * columns).Select(x => x % max).Select(x => x / denom).ToArray();
        using var container = new SingleMatrixContainer(rows, columns, items.AsSpan());

        var expected = items.Select(x => (double)x).Sum();
        NativeMatrixMethods.Sum(container.Matrix).Is(expected);

        NativeMatrixMethods.Clear(container.Matrix);
        NativeMatrixMethods.Sum(container.Matrix).Is(0);
    }

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void SumDoubleMatrix(int rows, int columns)
    {
        var denom = 1000d;
        var max = GetMaxValue<int>(rows * columns);
        var items = Enumerable.Range(0, rows * columns).Select(x => x % max).Select(x => x / denom).ToArray();
        using var container = new DoubleMatrixContainer(rows, columns, items.AsSpan());

        var expected = items.Select(x => (double)x).Sum();
        NativeMatrixMethods.Sum(container.Matrix).Is(expected);

        NativeMatrixMethods.Clear(container.Matrix);
        NativeMatrixMethods.Sum(container.Matrix).Is(0);
    }

    //[Theory]
    //[ClassData(typeof(RowColPairTestData))]
    //public void SumPixelBgrMatrix(int rows, int columns)
    //{
    //    //NotImplemented
    //}
}
