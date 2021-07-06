using System;
using Xunit;

namespace NativeInteroperateMatrix.Core.Tests.BuiltIn
{
    /// <summary>
    /// 空の Container インスタンス作成（小数専用）
    /// </summary>
    public abstract class DoubleMatrixContainerTestBase<TContainer, TMatrix, TValue>
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

            var expected = (double)(dynamic)writeValue * rc.Length;
            GetSum(matrix).Is(expected);

            (container as IDisposable)?.Dispose();
        }

        private static double GetSum<T>(IMatrix<T> matrix) where T : struct
        {
            double sum = 0;
            for (var row = 0; row < matrix.Rows; row++)
            {
                var span = matrix.GetRoRowSpan(row);

                for (var i = 0; i < span.Length; i++)
                    sum += (dynamic)span[i];    // ジェネリクスを無理やり加算
            }
            return sum;
        }
    }

    public class SingleMatrixContainerTest : DoubleMatrixContainerTestBase<SingleMatrixContainer, SingleMatrix, float>
    {
        protected override float WriteValue { get; } = float.MaxValue / 128f;  // overflow 対策

        protected override IMatrixContainer<SingleMatrix, float> CreateContainer(int rows, int columns, bool initialize)
             => new SingleMatrixContainer(rows, columns, initialize);
    }

    public class DoubleMatrixContainerTest : DoubleMatrixContainerTestBase<DoubleMatrixContainer, DoubleMatrix, double>
    {
        protected override double WriteValue { get; } = double.MaxValue / 128d;  // overflow 対策

        protected override IMatrixContainer<DoubleMatrix, double> CreateContainer(int rows, int columns, bool initialize)
             => new DoubleMatrixContainer(rows, columns, initialize);
    }
}
