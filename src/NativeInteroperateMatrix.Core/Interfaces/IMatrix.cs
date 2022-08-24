namespace Nima.Core;

public interface IMatrix<T> : INativeMemory<T>
    where T : struct
{
    int Rows { get; }
    int Columns { get; }
    int Stride { get; }

    int Width => Columns;
    int Height => Rows;
    bool IsContinuous { get; }

    Span<T> AsSpan(int row);

    /// <summary>指定行列の値を読み出します</summary>
    T ReadValue(int row, int column);

    /// <summary>指定行列の値を書き出します</summary>
    void WriteValue(int row, int column, in T value);

}
