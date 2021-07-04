using System;
using System.Linq;
using Xunit;

namespace NativeInteroperateMatrix.Core.Tests
{
    public class DoubleMatrixTest
    {
        [Theory]
        [InlineData(4, 3)]
        public void Test1(int rows, int columns)
        {
            using var container = new DoubleMatrixContainer(rows, columns);
            var matrix = container.Matrix;

            matrix.Rows.Is(rows);
            matrix.Columns.Is(columns);
            GetSum(matrix).Is(0);

            static double GetSum(in IMatrix<double> matrix)
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
