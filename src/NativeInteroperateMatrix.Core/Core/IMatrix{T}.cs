namespace Nima;

public interface IMatrix<T> : IMatrix, INativeMemory
    where T : struct
{
    /// <summary>
    /// Get reference of the specified matrix
    /// </summary>
    /// <param name="row">Specified row</param>
    /// <param name="column">Specified column</param>
    /// <returns>ref value</returns>
    ref T this[int row, int column] { get; }

    /// <summary>
    /// Return the raw Span
    /// </summary>
    /// <returns></returns>
    Span<T> AsSpan();

    /// <summary>
    /// Return the Span of the specified row
    /// </summary>
    /// <param name="row">Specified row</param>
    /// <returns>value</returns>
    Span<T> AsRowSpan(int row);
}
