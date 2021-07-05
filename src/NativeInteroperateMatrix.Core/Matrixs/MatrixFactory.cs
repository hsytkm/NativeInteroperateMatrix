using System;
using Matrix.SourceGenerator;
using NativeInteroperateMatrix.Core.Imaging;

namespace NativeInteroperateMatrix.Core
{
    public static class MatrixFactory
    {
        public static TMatrix Create<TMatrix, TValue>(IntPtr intPtr, int width, int height, int bytesPerData, int stride)
            where TMatrix : struct, IMatrix<TValue>
            where TValue : struct
        {
            var t = typeof(TValue);

            if (t == typeof(PixelBgr))
                return (TMatrix)(IMatrix<PixelBgr>)new PixelBgrMatrix(intPtr, width, height, bytesPerData, stride);
            if (t == typeof(double))
                return (TMatrix)(IMatrix<double>)new DoubleMatrix(intPtr, width, height, bytesPerData, stride);
            if (t == typeof(float))
                return (TMatrix)(IMatrix<float>)new SingleMatrix(intPtr, width, height, bytesPerData, stride);
            if (t == typeof(long))
                return (TMatrix)(IMatrix<long>)new Int64Matrix(intPtr, width, height, bytesPerData, stride);
            if (t == typeof(int))
                return (TMatrix)(IMatrix<int>)new Int32Matrix(intPtr, width, height, bytesPerData, stride);
            if (t == typeof(short))
                return (TMatrix)(IMatrix<short>)new Int16Matrix(intPtr, width, height, bytesPerData, stride);
            if (t == typeof(byte))
                return (TMatrix)(IMatrix<byte>)new ByteMatrix(intPtr, width, height, bytesPerData, stride);

            throw new NotImplementedException();
        }
    }

    // SourceGenerator 内で Matrix の Container も一緒に生成しています(手抜き)
    [MatrixGenerator]
    public readonly partial struct ByteMatrix { }

    [MatrixGenerator]
    public readonly partial struct Int16Matrix { }

    [MatrixGenerator]
    public readonly partial struct Int32Matrix { }

    [MatrixGenerator]
    public readonly partial struct Int64Matrix { }

    [MatrixGenerator]
    public readonly partial struct SingleMatrix { }

    [MatrixGenerator]
    public readonly partial struct DoubleMatrix { }

}
