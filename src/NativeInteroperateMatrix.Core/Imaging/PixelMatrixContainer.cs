using NativeInteroperateMatrix.Core.Imaging;
using System;

namespace NativeInteroperateMatrix.Core.Imaging
{
    public class PixelMatrixContainer : MatrixContainerBase<Pixel3Matrix, Pixel3ch>
    {
        public PixelMatrixContainer(int width, int height) : base(width, height) { }

        //protected override Pixel3Matrix CreateMatrix(int width, int height, int bytesPerData, int stride, IntPtr intPtr)
        //    => new Pixel3Matrix(width, height, bytesPerData, stride, intPtr);
    }
}
