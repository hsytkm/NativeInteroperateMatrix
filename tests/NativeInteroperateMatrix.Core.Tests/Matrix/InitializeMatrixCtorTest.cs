using Xunit;

namespace Nima.Core.Tests.Matrix;

/// <summary>
/// 初期値あり ctor のテスト（代表して byte 型のみ）
/// </summary>
public class InitializeMatrixCtorTest
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
        var array = CreateTestByteArray(rows * columns);
        using var container = new ByteMatrixContainer(rows, columns, array);
        using var token = container.GetMatrixForWrite(out NativeMatrix matrix);

        for (var r = 0; r < matrix.Rows; r++)
        {
            for (var c = 0; c < matrix.Columns; c++)
            {
                matrix.GetValue<byte>(r, c).Is(array[(r * columns) + c]);
            }
        }

        Assert.Throws<ArgumentException>(() =>
        {
            var overArray = CreateTestByteArray(rows * columns + 1);
            using var container = new ByteMatrixContainer(rows, columns, overArray);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var shortArray = CreateTestByteArray(rows * columns - 1);
            using var container = new ByteMatrixContainer(rows, columns, shortArray);
        });
    }

}
