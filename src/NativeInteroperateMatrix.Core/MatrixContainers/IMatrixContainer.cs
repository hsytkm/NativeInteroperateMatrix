using System;

namespace NativeInteroperateMatrix.Core
{
    public interface IMatrixContainer<TMatrix, TValue>
        where TMatrix : struct, IMatrix<TValue>
        where TValue : struct
    {
        TMatrix Matrix { get; }
    }
}
