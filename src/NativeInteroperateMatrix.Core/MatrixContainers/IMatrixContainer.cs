using System;

namespace Nima.Core
{
    public interface IMatrixContainer<TMatrix, TValue>
        where TMatrix : struct, IMatrix<TValue>
        where TValue : struct
    {
        TMatrix Matrix { get; }
    }
}
