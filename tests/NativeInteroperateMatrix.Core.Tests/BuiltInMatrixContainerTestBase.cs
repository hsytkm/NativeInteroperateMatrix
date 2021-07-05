using NativeInteroperateMatrix.Core.Imaging;
using System;
using System.Runtime.InteropServices;
using Xunit;

namespace NativeInteroperateMatrix.Core.Tests
{
    public abstract class BuiltInMatrixContainerTestBase<TContainer, TMatrix, TValue>
        where TContainer : class, IMatrixContainer<TMatrix, TValue>
        where TMatrix : struct, IMatrix<TValue>
        where TValue : struct
    {
        [Theory]
        [InlineData(4, 3)]
        [InlineData(161, 121)]
        public void Create(int rows, int columns, bool initialize = true)
        {
            var container = MatrixContainerFactory.Create<TContainer, TMatrix, TValue>(rows, columns, initialize);
            var matrix = container.Matrix;

            matrix.Pointer.IsNot(IntPtr.Zero);
            matrix.Rows.Is(rows);
            matrix.Columns.Is(columns);
            matrix.BytesPerItem.Is(Marshal.SizeOf<TValue>());
            matrix.Stride.IsNot(0);         // tekito-

            matrix.Width.Is(matrix.Columns);
            matrix.Height.Is(matrix.Rows);
            matrix.AllocatedSize.IsNot(0);  // tekito-
            matrix.BitsPerItem.Is(matrix.BytesPerItem * 8);

            matrix.IsValid.IsTrue();
            matrix.IsInvalid.IsFalse();
            matrix.IsContinuous.IsTrue();   // must be true
        }

#if false
        [Theory]
        [InlineData(4, 3)]
        [InlineData(161, 121)]
        public void ReadWrite(int rows, int columns, bool initialize = true)
        {
            var container = MatrixContainerFactory.Create<TContainer, TMatrix, TValue>(rows, columns, initialize);
            var matrix = container.Matrix;

            // ílÇÃì«Ç›èëÇ´
            var value = 1.234;
            (var row, var col) = (rows / 2, columns / 2);
            GetSum(matrix).Is(0);           // initialized
            matrix.WriteValue(value, row, col);
            matrix.ReadValue(row, col).Is(value);
            GetSum(matrix).Is(value);

            static TValue GetSum(IMatrix<TValue> matrix)
            {
                TValue sum = default;
                for (var row = 0; row < matrix.Rows; row++)
                {
                    var span = matrix.GetRoRowSpan(row);
                    for (var i = 0; i < span.Length; i++)
                    {
                        sum += span[i];
                    }
                }
                return sum;
            }
        }
#endif
    }

    public class ByteMatrixContainerTest : BuiltInMatrixContainerTestBase<ByteMatrixContainer, ByteMatrix, byte>
    {
    }

    public class Int16MatrixContainerTest : BuiltInMatrixContainerTestBase<Int16MatrixContainer, Int16Matrix, short>
    {
    }

    public class Int32MatrixContainerTest : BuiltInMatrixContainerTestBase<Int32MatrixContainer, Int32Matrix, int>
    {
    }

    public class Int64MatrixContainerTest : BuiltInMatrixContainerTestBase<Int64MatrixContainer, Int64Matrix, long>
    {
    }

    public class SingleMatrixContainerTest : BuiltInMatrixContainerTestBase<SingleMatrixContainer, SingleMatrix, float>
    {
    }

    public class DoubleMatrixContainerTest : BuiltInMatrixContainerTestBase<DoubleMatrixContainer, DoubleMatrix, double>
    {
    }

    public class PixelBgrMatrixContainerTest : BuiltInMatrixContainerTestBase<PixelBgrMatrixContainer, PixelBgrMatrix, PixelBgr>
    {
    }

}
