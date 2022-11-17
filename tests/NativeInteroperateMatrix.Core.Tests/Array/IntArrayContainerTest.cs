using Xunit;

namespace Nima.Core.Tests.Array;

/// <summary>
/// 空の Container インスタンス作成（整数専用）
/// </summary>
public abstract class IntArrayContainerTest<TContainer, TType>
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

        var expected = (long)(dynamic)writeValue * indexes.Count;
        GetSum<TType>(array).Is(expected);
    }

    static long GetSum<T>(NativeArray array) where T : struct
    {
        long sum = 0;
        ReadOnlySpan<T> span = array.AsSpan<T>();

        for (var i = 0; i < span.Length; i++)
            sum += (dynamic)span[i];    // ジェネリクスを無理やり加算
        return sum;
    }
}

public class ByteArrayContainerTest : IntArrayContainerTest<ByteArrayContainer, byte>
{
    protected override byte WriteValue { get; } = byte.MaxValue;

    protected override ByteArrayContainer CreateContainer(int length, bool initialize) =>
        new(length, initialize);
}

public class Int16ArrayContainerTest : IntArrayContainerTest<Int16ArrayContainer, short>
{
    protected override short WriteValue { get; } = short.MaxValue;

    protected override Int16ArrayContainer CreateContainer(int length, bool initialize) =>
        new(length, initialize);
}

public class Int32ArrayContainerTest : IntArrayContainerTest<Int32ArrayContainer, int>
{
    protected override int WriteValue { get; } = int.MaxValue;

    protected override Int32ArrayContainer CreateContainer(int length, bool initialize) =>
        new(length, initialize);
}

public class Int64ArrayContainerTest : IntArrayContainerTest<Int64ArrayContainer, long>
{
    protected override long WriteValue { get; } = long.MaxValue / 64;   // overflow 対策

    protected override Int64ArrayContainer CreateContainer(int length, bool initialize) =>
        new(length, initialize);
}
