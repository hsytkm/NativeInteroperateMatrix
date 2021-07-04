using System;
using System.Linq;
using Xunit;

namespace NativeInteroperateMatrix.Core.Tests
{
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

            matrix.IsValid.IsTrue();
            matrix.IsInvalid.IsFalse();
            matrix.IsContinuous.IsTrue();

            matrix.Rows.Is(rows);
            matrix.Columns.Is(columns);
            matrix.Width.Is(matrix.Columns);
            matrix.Height.Is(matrix.Rows);
            matrix.BytesPerItem.Is(sizeof(double));
            matrix.BitsPerItem.Is(matrix.BytesPerItem * 8);

            GetSum(matrix).Is(0);

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
}
