using NativeInteroperateMatrix.Core.Imaging;
using System;

namespace NativeInteroperateMatrix.Core.Tests
{
    public static class MatrixContainerFactory
    {
        public static TContainer Create<TContainer, TMatrix, TValue>(int rows, int columns, bool initialize)
            where TContainer : class, IMatrixContainer<TMatrix, TValue>
            where TMatrix : struct, IMatrix<TValue>
            where TValue : struct
        {
            //var args = new object[] { rows, columns, initialize };
            //var obj = Activator.CreateInstance(typeof(TContainer), BindingFlags.CreateInstance, null, args, null);
            var t = typeof(TValue);

            if (t == typeof(PixelBgr))
                return (TContainer)(IMatrixContainer<PixelBgrMatrix, PixelBgr>)new PixelBgrMatrixContainer(rows, columns, initialize);
            if (t == typeof(double))
                return (TContainer)(IMatrixContainer<DoubleMatrix, double>)new DoubleMatrixContainer(rows, columns, initialize);
            if (t == typeof(float))
                return (TContainer)(IMatrixContainer<SingleMatrix, float>)new SingleMatrixContainer(rows, columns, initialize);
            if (t == typeof(long))
                return (TContainer)(IMatrixContainer<Int64Matrix, long>)new Int64MatrixContainer(rows, columns, initialize);
            if (t == typeof(int))
                return (TContainer)(IMatrixContainer<Int32Matrix, int>)new Int32MatrixContainer(rows, columns, initialize);
            if (t == typeof(short))
                return (TContainer)(IMatrixContainer<Int16Matrix, short>)new Int16MatrixContainer(rows, columns, initialize);
            if (t == typeof(byte))
                return (TContainer)(IMatrixContainer<ByteMatrix, byte>)new ByteMatrixContainer(rows, columns, initialize);

            throw new NotImplementedException();
        }
    }
}
