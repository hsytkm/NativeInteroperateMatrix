namespace Nima;

public interface IMatrixContainer<T> : IDisposable
    where T : struct
{
    IMatrix<T> Matrix { get; }
}
