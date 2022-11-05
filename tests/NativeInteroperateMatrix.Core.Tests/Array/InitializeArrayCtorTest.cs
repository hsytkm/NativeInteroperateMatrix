using Xunit;

namespace Nima.Core.Tests.Array;

/// <summary>
/// 初期値あり ctor のテスト（代表して byte 型のみ）
/// </summary>
public class InitializeArrayCtorTest
{
    static IReadOnlyList<byte> CreateTestByteArray(int count)
    {
        var array = new byte[count];
        for (var i = 0; i < array.Length; i++)
            array[i] = (byte)(i & 0xff);
        return array;
    }

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void CtorItems(int rows, int columns)
    {
        var length = rows * columns;
        var sourceArray = CreateTestByteArray(length);
        using var container = new ByteArrayContainer(length, sourceArray);
        using var token = container.GetArrayForWrite(out NativeArray nativeArray);

        for (var i = 0; i < sourceArray.Count; i++)
        {
            nativeArray.GetValue<byte>(i).Is(sourceArray[i]);
        }

        Assert.Throws<ArgumentException>(() =>
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

}
