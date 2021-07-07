using Nima.Core.Imaging;
using System;
using Xunit;

namespace Nima.Core.Tests.Imaging.Drawing
{
    public class PixelBgrMatrixTest
    {
        [Theory]
        [ClassData(typeof(RowColPairTestData))]
        public void CtorInit(int rows, int columns)
        {
            using var container = new PixelBgrMatrixContainer(rows, columns, true);
            var matrix = container.Matrix;

            var color = new ColorBgr(0, 0, 0);
            matrix.GetChannelsAverageOfEntire().Is(color);
        }

        [Theory]
        [ClassData(typeof(RowColPairTestData))]
        public void FillAllPixels(int rows, int columns)
        {
            using var container = new PixelBgrMatrixContainer(rows, columns, false);
            var matrix = container.Matrix;

            var bgr = new PixelBgr(255, 0, 128);    // teketo-
            matrix.FillAllPixels(bgr);
            matrix.GetChannelsAverageOfEntire().Is(new ColorBgr(bgr));
        }

    }
}
