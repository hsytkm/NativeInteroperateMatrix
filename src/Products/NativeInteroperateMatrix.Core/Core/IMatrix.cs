namespace Nima;

public interface IMatrix
{
    /// <summary>
    /// Maximum number of rows
    /// </summary>
    int Rows { get; }

    /// <summary>
    /// Maximum number of columns
    /// </summary>
    int Columns { get; }

    /// <summary>
    /// Memory size of one row
    /// </summary>
    int Stride { get; }

    /// <summary>
    /// Whether the memory is contiguous
    /// </summary>
    bool IsContinuous { get; }
}
