namespace Nima;

public interface IMatrixContainer<T> : IDisposable
    where T : struct
{
    /// <summary>
    /// Matrix of ValueType managed by a container
    /// </summary>
    IMatrix<T> Matrix { get; }
}
