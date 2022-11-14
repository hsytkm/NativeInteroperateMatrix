using Nima;
using Nima.Core.Tests;
using Xunit;

namespace NativeInteroperateMatrix.Core.Tests.Natives;

public class NativeMatrixMethodsTest
{
    static long GetMaxValue<T>(long denominator)
    {
        var type = typeof(T);
        long max = 0;

        if (type == typeof(byte))
            max = byte.MaxValue;

        if (type == typeof(short))
            max = short.MaxValue;

        if (type == typeof(int))
            max = int.MaxValue;

        if (type == typeof(long))
            max = long.MaxValue;

        var value = max / denominator;
        return Math.Clamp(value, 1, max);
    }

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void SumByteMatrix(int rows, int columns)
    {
        var max = GetMaxValue<byte>(rows * columns);
        var items = Enumerable.Range(0, rows * columns).Select(x => (byte)(x % max)).ToArray();
        using var container = new ByteMatrixContainer(rows, columns, items.AsSpan());

        var expected = items.Select(x => (long)x).Sum();
        using (var token = container.GetMatrixForRead(out var matrix))
        {
            NativeMatrixMethods.SumByte(matrix).Is(expected);
        }

        using (var token = container.GetMatrixForWrite(out var matrix))
        {
            NativeMatrixMethods.ClearByte(matrix);
            NativeMatrixMethods.SumByte(matrix).Is(0);
        }
    }

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void SumInt16Matrix(int rows, int columns)
    {
        var max = GetMaxValue<short>(rows * columns);
        var items = Enumerable.Range(0, rows * columns).Select(x => (short)(x % max)).ToArray();
        using var container = new Int16MatrixContainer(rows, columns, items.AsSpan());

        var expected = items.Select(x => (long)x).Sum();
        using (var token = container.GetMatrixForRead(out var matrix))
        {
            var bs1 = matrix.AsSpan<byte>().ToArray();
            var bs2 = matrix.AsSpan<short>().ToArray();

            NativeMatrixMethods.SumInt16(matrix).Is(expected);
        }

        using (var token = container.GetMatrixForWrite(out var matrix))
        {
            NativeMatrixMethods.ClearInt16(matrix);
            NativeMatrixMethods.SumInt16(matrix).Is(0);
        }
    }

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void SumInt32Matrix(int rows, int columns)
    {
        var max = GetMaxValue<int>(rows * columns);
        var items = Enumerable.Range(0, rows * columns).Select(x => (int)(x % max)).ToArray();
        using var container = new Int32MatrixContainer(rows, columns, items.AsSpan());

        var expected = items.Select(x => (long)x).Sum();
        using (var token = container.GetMatrixForRead(out var matrix))
        {
            NativeMatrixMethods.SumInt32(matrix).Is(expected);
        }

        using (var token = container.GetMatrixForWrite(out var matrix))
        {
            NativeMatrixMethods.ClearInt32(matrix);
            NativeMatrixMethods.SumInt32(matrix).Is(0);
        }
    }

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void SumInt64Matrix(int rows, int columns)
    {
        var max = GetMaxValue<long>(rows * columns);
        var items = Enumerable.Range(0, rows * columns).Select(x => (long)(x % max)).ToArray();
        using var container = new Int64MatrixContainer(rows, columns, items.AsSpan());

        var expected = items.Select(x => (long)x).Sum();
        using (var token = container.GetMatrixForRead(out var matrix))
        {
            NativeMatrixMethods.SumInt64(matrix).Is(expected);
        }

        using (var token = container.GetMatrixForWrite(out var matrix))
        {
            NativeMatrixMethods.ClearInt64(matrix);
            NativeMatrixMethods.SumInt64(matrix).Is(0);
        }
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
        using (var token = container.GetMatrixForRead(out var matrix))
        {
            NativeMatrixMethods.SumSingle(matrix).Is(expected);
        }

        using (var token = container.GetMatrixForWrite(out var matrix))
        {
            NativeMatrixMethods.ClearSingle(matrix);
            NativeMatrixMethods.SumSingle(matrix).Is(0);
        }
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
        using (var token = container.GetMatrixForRead(out var matrix))
        {
            NativeMatrixMethods.SumDouble(matrix).Is(expected);
        }

        using (var token = container.GetMatrixForWrite(out var matrix))
        {
            NativeMatrixMethods.ClearDouble(matrix);
            NativeMatrixMethods.SumDouble(matrix).Is(0);
        }
    }

    //[Theory]
    //[ClassData(typeof(RowColPairTestData))]
    //public void SumPixelBgrMatrix(int rows, int columns)
    //{
    //    //NotImplemented
    //}
}
