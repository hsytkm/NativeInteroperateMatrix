using System;

namespace NativeInteroperateMatrix.Core
{
    public class DoubleMatrixContainer : MatrixContainerBase<DoubleMatrix, double>
    {
        public DoubleMatrixContainer(int rows, int columns) : base(rows, columns) { }

        public static DoubleMatrixContainer CreateContainer(ValueArray2d<double> array2d)
        {
            var container = new DoubleMatrixContainer(array2d.Columns, array2d.Rows);
            var matrix = container.Matrix;

            for (var r = 0; r < matrix.Rows; r++)
            {
                var line = matrix.GetRowSpan(r);
                for (var c = 0; c < line.Length; c++)
                {
                    line[c] = array2d[r, c];
                }
            }
            return container;
        }

        //protected override DoubleMatrix CreateMatrix(int rows, int columns, int bytesPerData, int stride, IntPtr intPtr)
        //    => new DoubleMatrix(rows, columns, bytesPerData, stride, intPtr);
    }

}
