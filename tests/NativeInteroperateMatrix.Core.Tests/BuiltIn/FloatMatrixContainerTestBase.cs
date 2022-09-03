using Xunit;

namespace Nima.Core.Tests.BuiltIn;

/// <summary>
/// 空の Container インスタンス作成（小数専用）
/// </summary>
public abstract class FloatMatrixContainerTestBase<TContainer, TType>
    where TContainer : notnull, IMatrixContainer<TType>
    where TType : struct
{
    protected abstract TType WriteValue { get; }
    protected abstract TContainer CreateContainer(int rows, int columns, bool initialize);

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void ReadWrite(int rows, int columns)
    {
        using var container = CreateContainer(rows, columns, initialize: true);
        var matrix = container.MatrixT;

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

        var expected = (double)(dynamic)writeValue * rc.Length;
        GetSum(matrix).Is(expected);
    }

    private static double GetSum<T>(IMatrix<T> matrix) where T : struct
    {
        double sum = 0;
        for (var row = 0; row < matrix.Rows; row++)
        {
            ReadOnlySpan<T> span = matrix.AsRowSpan(row);

            for (var i = 0; i < span.Length; i++)
                sum += (dynamic)span[i];    // ジェネリクスを無理やり加算
        }
        return sum;
    }
}

public class SingleMatrixContainerTest : FloatMatrixContainerTestBase<SingleMatrixContainer, float>
{
    protected override float WriteValue { get; } = float.MaxValue / 128f;  // overflow 対策

    protected override SingleMatrixContainer CreateContainer(int rows, int columns, bool initialize) =>
        new(rows, columns, initialize);
}

public class DoubleMatrixContainerTest : FloatMatrixContainerTestBase<DoubleMatrixContainer, double>
{
    protected override double WriteValue { get; } = double.MaxValue / 128d;  // overflow 対策

    protected override DoubleMatrixContainer CreateContainer(int rows, int columns, bool initialize) =>
        new(rows, columns, initialize);
}
