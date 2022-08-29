namespace Nima.Core.Tests;

//internal static class MatrixContainerFactory
//{
//    public static TContainer Create<TContainer, TMatrix, TType>(int rows, int columns, bool initialize)
//        where TContainer : class, IMatrixContainer<TMatrix, TType>
//        where TMatrix : struct, IMatrix<TType>
//        where TType : struct
//    {
//        var t = typeof(TType);

//        if (t == typeof(PixelBgr))
//            return (TContainer)(IMatrixContainer<PixelBgrMatrix, PixelBgr>)new PixelBgrMatrixContainer(rows, columns, initialize);
//        if (t == typeof(double))
//            return (TContainer)(IMatrixContainer<DoubleMatrix, double>)new DoubleMatrixContainer(rows, columns, initialize);
//        if (t == typeof(float))
//            return (TContainer)(IMatrixContainer<SingleMatrix, float>)new SingleMatrixContainer(rows, columns, initialize);
//        if (t == typeof(long))
//            return (TContainer)(IMatrixContainer<Int64Matrix, long>)new Int64MatrixContainer(rows, columns, initialize);
//        if (t == typeof(int))
//            return (TContainer)(IMatrixContainer<Int32Matrix, int>)new Int32MatrixContainer(rows, columns, initialize);
//        if (t == typeof(short))
//            return (TContainer)(IMatrixContainer<Int16Matrix, short>)new Int16MatrixContainer(rows, columns, initialize);
//        if (t == typeof(sbyte))
//            return (TContainer)(IMatrixContainer<Int8Matrix, byte>)new Int8MatrixContainer(rows, columns, initialize);
//        if (t == typeof(byte))
//            return (TContainer)(IMatrixContainer<ByteMatrix, byte>)new Int8MatrixContainer(rows, columns, initialize);

//        throw new NotImplementedException();
//    }
//}
