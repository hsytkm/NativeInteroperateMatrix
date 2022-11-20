using Xunit;

namespace Nima.Core.Tests.Array;

/// <summary>
/// 初期値あり ctor のテスト（代表して byte 型のみ）
/// </summary>
public class NativeArrayTest
{
    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void CtorItems(int rows, int columns)
    {
        static IReadOnlyList<byte> CreateTestByteArray(int count)
        {
            var array = new byte[count];
            for (var i = 0; i < array.Length; i++)
                array[i] = (byte)(i & 0xff);
            return array;
        }

        var length = rows * columns;
        var sourceArray = CreateTestByteArray(length);
        using var container = new ByteArrayContainer(length, sourceArray);
        using var token = container.GetArrayForWriting(out NativeArray nativeArray);

        for (var i = 0; i < nativeArray.Length; i++)
        {
            nativeArray.GetValue<byte>(i).Is(sourceArray[i]);
        }

        Assert.Throws<IndexOutOfRangeException>(() =>
        {
            var overArray = CreateTestByteArray(length + 1);
            using var container = new ByteArrayContainer(length, overArray);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var shortArray = CreateTestByteArray(length - 1);
            using var container = new ByteArrayContainer(length, shortArray);
        });
    }

    [Fact]
    public void LockRead()
    {
        using var container = new ByteArrayContainer(10);

        // Multiple Read
        using (var token1 = container.GetArrayForReading(out NativeArray nativeArray1))
        using (var token2 = container.GetArrayForReading(out NativeArray nativeArray2))
        {
            Assert.Throws<InvalidMemoryAccessException>(() =>
            {
                using var token3 = container.GetArrayForWriting(out NativeArray nativeArray3);
            });
        }

        // Writing
        {
            using var token1 = container.GetArrayForWriting(out NativeArray nativeArray1);
            Assert.Throws<InvalidMemoryAccessException>(() =>
            {
                using var token2 = container.GetArrayForReading(out NativeArray nativeArray2);
            });
        }
    }

    [Fact]
    public void LockWrite()
    {
        using var container = new ByteArrayContainer(10);

        // Multiple Write
        {
            using var token1 = container.GetArrayForWriting(out NativeArray nativeArray1);
            Assert.Throws<InvalidMemoryAccessException>(() =>
            {
                using var token2 = container.GetArrayForWriting(out NativeArray nativeArray2);
            });
        }

        // Reading
        {
            using var token1 = container.GetArrayForReading(out NativeArray nativeArray1);
            Assert.Throws<InvalidMemoryAccessException>(() =>
            {
                using var token2 = container.GetArrayForWriting(out NativeArray nativeArray2);
            });
        }
    }

}
