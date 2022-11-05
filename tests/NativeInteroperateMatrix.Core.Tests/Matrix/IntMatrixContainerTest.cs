using Xunit;

namespace Nima.Core.Tests.Matrix;

/// <summary>
/// 空の Container インスタンス作成（整数専用）
/// </summary>
public abstract class IntMatrixContainerTest<TContainer, TType>
    where TContainer : notnull, INativeMatrixContainer, IDisposable
    where TType : struct
{
    protected abstract TType WriteValue { get; }

    protected abstract TContainer CreateContainer(int rows, int columns, bool initialize);

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void ReadWrite(int rows, int columns)
    {
        using var container = CreateContainer(rows, columns, initialize: true);

        using var token = container.GetMatrixForWrite(out NativeMatrix matrix);
        GetSum<TType>(matrix).Is(0);

        (var centerRow, var centerCol) = ((rows - 1) / 2, (columns - 1) / 2);
        var rc = new (int row, int col)[]
        {
            (centerRow, centerCol),
            (centerRow, centerCol + 1),
            (centerRow + 1, centerCol),
            (centerRow + 1, centerCol + 1)
        };

        var writeValue = WriteValue;
        foreach (var (row, col) in rc)
        {
            var rowSpan = matrix.AsRowSpan<TType>(row);
            rowSpan[col] = writeValue;
            rowSpan[col].Is(writeValue);
        }

        var expected = (long)(dynamic)writeValue * rc.Length;
        GetSum<TType>(matrix).Is(expected);
    }

    static long GetSum<T>(NativeMatrix matrix) where T : struct
    {
        long sum = 0;
        for (var row = 0; row < matrix.Rows; row++)
        {
            ReadOnlySpan<T> span = matrix.AsRowSpan<T>(row);

            for (var i = 0; i < span.Length; i++)
                sum += (dynamic)span[i];    // ジェネリクスを無理やり加算
        }
        return sum;
    }
}

public class ByteMatrixContainerTest : IntMatrixContainerTest<ByteMatrixContainer, byte>
{
    protected override byte WriteValue { get; } = byte.MaxValue;

    protected override ByteMatrixContainer CreateContainer(int rows, int columns, bool initialize) =>
        new(rows, columns, initialize);
}

public class Int16MatrixContainerTest : IntMatrixContainerTest<Int16MatrixContainer, short>
{
    protected override short WriteValue { get; } = short.MaxValue;

    protected override Int16MatrixContainer CreateContainer(int rows, int columns, bool initialize) =>
        new(rows, columns, initialize);
}

public class Int32MatrixContainerTest : IntMatrixContainerTest<Int32MatrixContainer, int>
{
    protected override int WriteValue { get; } = int.MaxValue;

    protected override Int32MatrixContainer CreateContainer(int rows, int columns, bool initialize) =>
        new(rows, columns, initialize);
}

public class Int64MatrixContainerTest : IntMatrixContainerTest<Int64MatrixContainer, long>
{
    protected override long WriteValue { get; } = long.MaxValue / 64;   // overflow 対策

    protected override Int64MatrixContainer CreateContainer(int rows, int columns, bool initialize) =>
        new(rows, columns, initialize);
}
