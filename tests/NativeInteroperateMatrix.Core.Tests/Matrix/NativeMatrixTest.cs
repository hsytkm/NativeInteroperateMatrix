using Xunit;

namespace Nima.Core.Tests.Matrix;

/// <summary>
/// 初期値あり ctor のテスト（代表して byte 型のみ）
/// </summary>
public class NativeMatrixTest
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

        var array = CreateTestByteArray(rows * columns);
        using var container = new ByteMatrixContainer(rows, columns, array);
        using var token = container.GetMatrixForWriting(out NativeMatrix matrix);

        for (var r = 0; r < matrix.Rows; r++)
        {
            for (var c = 0; c < matrix.Columns; c++)
            {
                matrix.GetValue<byte>(r, c).Is(array[(r * columns) + c]);
            }
        }

        Assert.Throws<IndexOutOfRangeException>(() =>
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

    [Fact]
    public void LockRead()
    {
        using var container = new ByteMatrixContainer(3, 4);

        // Multiple Read
        using (var token1 = container.GetMatrixForReading(out NativeMatrix nativeMatrix1))
        using (var token2 = container.GetMatrixForReading(out NativeMatrix nativeMatrix2))
        {
            Assert.Throws<NotSupportedException>(() =>
            {
                using var token3 = container.GetMatrixForWriting(out NativeMatrix nativeMatrix3);
            });
        }

        // Writing
        {
            using var token1 = container.GetMatrixForWriting(out NativeMatrix nativeMatrix1);
            Assert.Throws<NotSupportedException>(() =>
            {
                using var token2 = container.GetMatrixForReading(out NativeMatrix nativeMatrix2);
            });
        }
    }

    [Fact]
    public void LockWrite()
    {
        using var container = new ByteMatrixContainer(3, 4);

        // Multiple Write
        {
            using var token1 = container.GetMatrixForWriting(out NativeMatrix nativeMatrix1);
            Assert.Throws<NotSupportedException>(() =>
            {
                using var token2 = container.GetMatrixForWriting(out NativeMatrix nativeMatrix2);
            });
        }

        // Reading
        {
            using var token1 = container.GetMatrixForReading(out NativeMatrix nativeMatrix1);
            Assert.Throws<NotSupportedException>(() =>
            {
                using var token2 = container.GetMatrixForWriting(out NativeMatrix nativeMatrix2);
            });
        }
    }

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void CopyMatrix(int rows, int columns)
    {
        var span = Enumerable.Range(0, rows * columns).ToArray().AsSpan();
        using var srcContainer = new Int32MatrixContainer(rows, columns, span);
        using var destContainer1 = new Int32MatrixContainer(rows, columns, false);
        using var destContainer2 = new Int32MatrixContainer(rows, columns, false);

        using (var token0 = srcContainer.GetMatrixForReading(out var srcMatrix))
        using (var token1 = destContainer1.GetMatrixForWriting(out var destMatrix1))
        using (var token2 = destContainer2.GetMatrixForWriting(out var destMatrix2))
        {
            srcMatrix.CopyTo(destMatrix1);
            destMatrix1.AsReadOnlySpan<int>().SequenceEqual(srcMatrix.AsReadOnlySpan<int>()).IsTrue();
        }
    }
}
