using Xunit;

namespace Nima.Core.Tests.Common;

/// <summary>
/// 初期値あり ctor のテスト（代表して byte 型のみ）
/// </summary>
public class InitializedCtorTest
{
    private static IReadOnlyList<byte> CreateByteArray(int count)
    {
        var array = new byte[count];
        byte data = 0;
        for (var i = 0; i < count; i++)
        {
            array[i] = data;
            data = (data >= 0xff) ? (byte)0 : data++;
        }
        return array;
    }

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void CtorItems(int rows, int columns)
    {
        var array = CreateByteArray(rows * columns);
        using var container = new ByteMatrixContainer(rows, columns, array);
        var matrix = container.Matrix;

        for (var r = 0; r < matrix.Rows; r++)
        {
            for (var c = 0; c < matrix.Columns; c++)
            {
                matrix.ReadValue(r, c).Is(array[(r * columns) + c]);
            }
        }

        Assert.Throws<ArgumentException>(() =>
        {
            var overArray = CreateByteArray(rows * columns + 1);
            using var container = new ByteMatrixContainer(rows, columns, overArray);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var shortArray = CreateByteArray(rows * columns - 1);
            using var container = new ByteMatrixContainer(rows, columns, shortArray);
        });
    }

}
