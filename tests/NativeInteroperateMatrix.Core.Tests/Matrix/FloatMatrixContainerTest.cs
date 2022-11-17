using Xunit;

namespace Nima.Core.Tests.Matrix;

/// <summary>
/// 空の Container インスタンス作成（小数専用）
/// </summary>
public abstract class FloatMatrixContainerTest<TContainer, TType>
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

        using var token = container.GetMatrixForWriting(out NativeMatrix matrix);
        GetSum<TType>(matrix).Is(0);

        var rc = new List<(int row, int col)>();
        {
            (var centerRow, var centerCol) = ((rows - 1) / 2, (columns - 1) / 2);
            rc.Add((centerRow, centerCol));
            if (rows > 1) rc.Add((centerRow + 1, centerCol));
            if (columns > 1) rc.Add((centerRow, centerCol + 1));
            if (rows > 1 && columns > 1) rc.Add((centerRow + 1, centerCol + 1));
        }

        var writeValue = WriteValue;
        foreach (var (row, col) in rc)
        {
            var rowSpan = matrix.AsRowSpan<TType>(row);
            rowSpan[col] = writeValue;
            rowSpan[col].Is(writeValue);
        }

        var expected = (double)(dynamic)writeValue * rc.Count;
        GetSum<TType>(matrix).Is(expected);
    }

    static double GetSum<T>(NativeMatrix matrix) where T : struct
    {
        double sum = 0;
        for (var row = 0; row < matrix.Rows; row++)
        {
            ReadOnlySpan<T> span = matrix.AsRowSpan<T>(row);

            for (var i = 0; i < span.Length; i++)
                sum += (dynamic)span[i];    // ジェネリクスを無理やり加算
        }
        return sum;
    }
}

public class SingleMatrixContainerTest : FloatMatrixContainerTest<SingleMatrixContainer, float>
{
    protected override float WriteValue { get; } = float.MaxValue / 128f;  // overflow 対策

    protected override SingleMatrixContainer CreateContainer(int rows, int columns, bool initialize) =>
        new(rows, columns, initialize);
}

public class DoubleMatrixContainerTest : FloatMatrixContainerTest<DoubleMatrixContainer, double>
{
    protected override double WriteValue { get; } = double.MaxValue / 128d;  // overflow 対策

    protected override DoubleMatrixContainer CreateContainer(int rows, int columns, bool initialize) =>
        new(rows, columns, initialize);
}
