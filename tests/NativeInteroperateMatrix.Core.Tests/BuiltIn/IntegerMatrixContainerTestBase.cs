using Xunit;

namespace Nima.Core.Tests.BuiltIn;

/// <summary>
/// 空の Container インスタンス作成（整数専用）
/// </summary>
public abstract class IntegerMatrixContainerTestBase<TContainer, TType>
    : BuiltInMatrixContainerTestBase<TContainer, TType>
        where TContainer : notnull, IMatrixContainer<TType>
        where TType : struct
{
    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void ReadWrite(int rows, int columns)
    {
        using var container = CreateContainer(rows, columns, initialize: true);
        var matrix = container.Matrix;

        GetSum(matrix).Is(0);

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
            matrix[row, col] = writeValue;
            matrix[row, col].Is(writeValue);
        }

        var expected = (long)(dynamic)writeValue * rc.Length;
        GetSum(matrix).Is(expected);
    }

    private static long GetSum<T>(IMatrix<T> matrix) where T : struct
    {
        long sum = 0;
        for (var row = 0; row < matrix.Rows; row++)
        {
            ReadOnlySpan<T> span = matrix.AsRowSpan(row);

            for (var i = 0; i < span.Length; i++)
                sum += (dynamic)span[i];    // ジェネリクスを無理やり加算
        }
        return sum;
    }
}

public class ByteMatrixContainerTest : IntegerMatrixContainerTestBase<ByteMatrixContainer, byte>
{
    protected override byte WriteValue { get; } = byte.MaxValue;

    protected override IMatrixContainer<byte> CreateContainer(int rows, int columns, bool initialize)
         => new ByteMatrixContainer(rows, columns, initialize);
}

public class Int8MatrixContainerTest : IntegerMatrixContainerTestBase<Int8MatrixContainer, sbyte>
{
    protected override sbyte WriteValue { get; } = sbyte.MaxValue;

    protected override IMatrixContainer<sbyte> CreateContainer(int rows, int columns, bool initialize)
         => new Int8MatrixContainer(rows, columns, initialize);
}

public class Int16MatrixContainerTest : IntegerMatrixContainerTestBase<Int16MatrixContainer, short>
{
    protected override short WriteValue { get; } = short.MaxValue;

    protected override IMatrixContainer<short> CreateContainer(int rows, int columns, bool initialize)
         => new Int16MatrixContainer(rows, columns, initialize);
}

public class Int32MatrixContainerTest : IntegerMatrixContainerTestBase<Int32MatrixContainer, int>
{
    protected override int WriteValue { get; } = int.MaxValue;

    protected override IMatrixContainer<int> CreateContainer(int rows, int columns, bool initialize)
         => new Int32MatrixContainer(rows, columns, initialize);
}

public class Int64MatrixContainerTest : IntegerMatrixContainerTestBase<Int64MatrixContainer, long>
{
    protected override long WriteValue { get; } = long.MaxValue / 64;   // overflow 対策

    protected override IMatrixContainer<long> CreateContainer(int rows, int columns, bool initialize)
         => new Int64MatrixContainer(rows, columns, initialize);
}
