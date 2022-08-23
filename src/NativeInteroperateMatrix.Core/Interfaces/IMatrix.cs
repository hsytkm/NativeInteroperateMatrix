using System;

namespace Nima.Core;

public interface IMatrix<TValue> : INativeMemory<TValue>
    where TValue : struct
{
    int Rows { get; }
    int Columns { get; }
    int Stride { get; }

    int Width => Columns;
    int Height => Rows;
    bool IsContinuous { get; }

    Span<TValue> AsSpan(int row);

    /// <summary>指定行列の値を読み出します</summary>
    TValue ReadValue(int row, int column);

    /// <summary>指定行列の値を書き出します</summary>
    void WriteValue(int row, int column, in TValue value);

}
