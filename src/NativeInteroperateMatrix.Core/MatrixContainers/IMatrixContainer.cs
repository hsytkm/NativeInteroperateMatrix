﻿namespace Nima;

public interface IMatrixContainer<TMatrix, TValue> : IDisposable
    where TMatrix : struct, IMatrix<TValue>
    where TValue : struct
{
    TMatrix Matrix { get; }
}
