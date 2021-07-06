using System;
using Xunit;

namespace NativeInteroperateMatrix.Core.Tests.BuiltIn
{
    /// <summary>
    /// 空の Container インスタンス作成（整数専用）
    /// </summary>
    public abstract class IntegerMatrixContainerTestBase<TContainer, TMatrix, TValue>
        : BuiltInMatrixContainerTestBase<TContainer, TMatrix, TValue>
            where TContainer : class, IMatrixContainer<TMatrix, TValue>
            where TMatrix : struct, IMatrix<TValue>
            where TValue : struct
    {
        [Theory]
        [ClassData(typeof(RowColPairTestData))]
        public void ReadWrite(int rows, int columns)
        {
            var container = CreateContainer(rows, columns, initialize: true);
            var matrix = container.Matrix;

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
                matrix.WriteValue(writeValue, row, col);
                matrix.ReadValue(row, col).Is(writeValue);
            }

            var expected = (long)(dynamic)writeValue * rc.Length;
            GetSum(matrix).Is(expected);

            (container as IDisposable)?.Dispose();
        }

        private static long GetSum<T>(IMatrix<T> matrix) where T : struct
        {
            long sum = 0;
            for (var row = 0; row < matrix.Rows; row++)
            {
                var span = matrix.GetRoRowSpan(row);

                for (var i = 0; i < span.Length; i++)
                    sum += (dynamic)span[i];    // ジェネリクスを無理やり加算
            }
            return sum;
        }
    }

    public class Int8MatrixContainerTest : IntegerMatrixContainerTestBase<Int8MatrixContainer, Int8Matrix, byte>
    {
        protected override byte WriteValue { get; } = byte.MaxValue;

        protected override IMatrixContainer<Int8Matrix, byte> CreateContainer(int rows, int columns, bool initialize)
             => new Int8MatrixContainer(rows, columns, initialize);
    }

    public class Int16MatrixContainerTest : IntegerMatrixContainerTestBase<Int16MatrixContainer, Int16Matrix, short>
    {
        protected override short WriteValue { get; } = short.MaxValue;

        protected override IMatrixContainer<Int16Matrix, short> CreateContainer(int rows, int columns, bool initialize)
             => new Int16MatrixContainer(rows, columns, initialize);
    }

    public class Int32MatrixContainerTest : IntegerMatrixContainerTestBase<Int32MatrixContainer, Int32Matrix, int>
    {
        protected override int WriteValue { get; } = int.MaxValue;

        protected override IMatrixContainer<Int32Matrix, int> CreateContainer(int rows, int columns, bool initialize)
             => new Int32MatrixContainer(rows, columns, initialize);
    }

    public class Int64MatrixContainerTest : IntegerMatrixContainerTestBase<Int64MatrixContainer, Int64Matrix, long>
    {
        protected override long WriteValue { get; } = long.MaxValue / 64;   // overflow 対策

        protected override IMatrixContainer<Int64Matrix, long> CreateContainer(int rows, int columns, bool initialize)
             => new Int64MatrixContainer(rows, columns, initialize);
    }
}
