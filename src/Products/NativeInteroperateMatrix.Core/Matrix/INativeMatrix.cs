namespace Nima;

public interface INativeMatrix : INativeMemory
{
    /// <summary>
    /// Maximum number of rows
    /// </summary>
    int Rows { get; }

    /// <summary>
    /// Maximum number of columns
    /// </summary>
    int Columns { get; }

    int Width { get; }
    int Height { get; }

    /// <summary>
    /// Memory size of one row
    /// </summary>
    int Stride { get; }

    /// <summary>
    /// Whether the memory is contiguous
    /// </summary>
    bool IsContinuous { get; }

    /// <summary>
    /// Return the raw Span
    /// </summary>
    Span<T> AsSpan<T>() where T : struct;

    ReadOnlySpan<T> AsReadOnlySpan<T>() where T : struct;

    /// <summary>
    /// Return the Span of the specified row
    /// </summary>
    /// <param name="row">Specified row</param>
    Span<T> AsRowSpan<T>(int row) where T : struct;

    ReadOnlySpan<T> AsRowReadOnlySpan<T>(int row) where T : struct;

    T GetValue<T>(int row, int column) where T : struct;
}
