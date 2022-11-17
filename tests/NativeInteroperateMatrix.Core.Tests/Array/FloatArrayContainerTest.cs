using Xunit;

namespace Nima.Core.Tests.Array;

/// <summary>
/// 空の Container インスタンス作成（小数専用）
/// </summary>
public abstract class FloatArrayContainerTest<TContainer, TType>
    where TContainer : notnull, INativeArrayContainer, IDisposable
    where TType : struct
{
    protected abstract TType WriteValue { get; }

    protected abstract TContainer CreateContainer(int length, bool initialize);

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void ReadWrite(int rows, int columns)
    {
        var length = rows * columns;
        using var container = CreateContainer(length, initialize: true);

        using var token = container.GetArrayForWriting(out NativeArray array);
        GetSum<TType>(array).Is(0);

        var indexes = new List<int>();
        {
            var centerIndex = length / 2;
            if (centerIndex > 1) indexes.Add(centerIndex - 1);
            indexes.Add(centerIndex);
            if (centerIndex + 1 < length) indexes.Add(centerIndex + 1);
        }

        var writeValue = WriteValue;
        var span = array.AsSpan<TType>();
        foreach (var index in indexes)
        {
            span[index] = writeValue;
            span[index].Is(writeValue);
        }

        var expected = (double)(dynamic)writeValue * indexes.Count;
        GetSum<TType>(array).Is(expected);
    }

    static double GetSum<T>(NativeArray array) where T : struct
    {
        double sum = 0;
        ReadOnlySpan<T> span = array.AsSpan<T>();

        for (var i = 0; i < span.Length; i++)
            sum += (dynamic)span[i];    // ジェネリクスを無理やり加算
        return sum;
    }
}

public class SingleArrayContainerTest : FloatArrayContainerTest<SingleArrayContainer, float>
{
    protected override float WriteValue { get; } = float.MaxValue / 128f;  // overflow 対策

    protected override SingleArrayContainer CreateContainer(int length, bool initialize) =>
        new(length, initialize);
}

public class DoubleArrayContainerTest : FloatArrayContainerTest<DoubleArrayContainer, double>
{
    protected override double WriteValue { get; } = double.MaxValue / 128d;  // overflow 対策

    protected override DoubleArrayContainer CreateContainer(int length, bool initialize) =>
        new(length, initialize);
}
