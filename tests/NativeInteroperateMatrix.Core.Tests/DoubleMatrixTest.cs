using System;
using System.Linq;
using Xunit;

namespace NativeInteroperateMatrix.Core.Tests
{
#if false
    public class DoubleMatrixTest
    {
        // ジェネリクスをテストしたい

        [Theory]
        [InlineData(4, 3)]
        [InlineData(256, 128)]
        public void Create(int rows, int columns)
        {
            using var container = new DoubleMatrixContainer(rows, columns, initialize: true);
            var matrix = container.Matrix;

            matrix.Pointer.IsNot(IntPtr.Zero);
            matrix.Rows.Is(rows);
            matrix.Columns.Is(columns);
            matrix.BytesPerItem.Is(sizeof(double));
            matrix.Stride.IsNot(0);         // tekito-

            matrix.Width.Is(matrix.Columns);
            matrix.Height.Is(matrix.Rows);
            matrix.AllocatedSize.IsNot(0);  // tekito-
            matrix.BitsPerItem.Is(matrix.BytesPerItem * 8);

            matrix.IsValid.IsTrue();
            matrix.IsInvalid.IsFalse();
            matrix.IsContinuous.IsTrue();   // must be true

            // 値の読み書き
            var value = 1.234;
            (var row, var col) = (rows / 2, columns / 2);
            GetSum(matrix).Is(0);           // initialized
            matrix.WriteValue(value, row, col);
            matrix.ReadValue(row, col).Is(value);
            GetSum(matrix).Is(value);

            static double GetSum(IMatrix<double> matrix)
            {
                double sum = 0;
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

    }
#endif
}
