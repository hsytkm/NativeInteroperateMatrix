namespace Nima;

public abstract class MatrixContainerBase : NativeMemoryContainerBase, INativeMatrixContainer
{
    /// <summary>
    /// 外部公開用の2次元配列(NativeMemoryをWrapしています)
    /// </summary>
    protected NativeMatrix Matrix { get; }

    /// <summary>
    /// メモリ読み出し中カウンタ
    /// </summary>
    public int ReadCounter { get; private set; }

    /// <summary>
    /// メモリ書き込み中フラグ
    /// </summary>
    public bool IsWriting { get; private set; }

    public int Rows => Matrix.Rows;
    public int Columns => Matrix.Columns;
    public int Width => Matrix.Width;
    public int Height => Matrix.Height;

    protected MatrixContainerBase(int rows, int columns, int bytesPerItem, bool initialize)
        : base(rows * columns * bytesPerItem, initialize)
    {
        int stride = columns * bytesPerItem;
        int allocateSize = rows * stride;
        Matrix = new NativeMatrix(AllocatedMemory.Pointer, allocateSize, bytesPerItem, rows, columns, stride);
    }

    private protected MatrixContainerBase(in NativeMatrix matrix)
        : base(new NativePointerSizePair(matrix.Pointer, matrix.AllocateSize))
    {
        Matrix = matrix;
    }

    /// <summary>
    /// Get native matrix for reading.
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    /// <exception cref="InvalidMemoryAccessException"></exception>
    public IDisposable GetMatrixForReading(out NativeMatrix matrix)
    {
        if (IsWriting)
            throw new InvalidMemoryAccessException("Someone is writing.");

        matrix = Matrix;
        ReadCounter++;
        return new DisposableAction(() => ReadCounter--);
    }

    /// <summary>
    /// Get native matrix for writing.
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    /// <exception cref="InvalidMemoryAccessException"></exception>
    public IDisposable GetMatrixForWriting(out NativeMatrix matrix)
    {
        if (IsWriting)
            throw new InvalidMemoryAccessException("Someone else is writing.");

        if (ReadCounter > 0)
            throw new InvalidMemoryAccessException($"Someone is reading. (Count={ReadCounter})");

        matrix = Matrix;
        IsWriting = true;
        return new DisposableAction(() => IsWriting = false);
    }

    /// <summary>
    /// Get native matrix (don't manage memory references)
    /// </summary>
    /// <returns>NativeMatrix</returns>
    public NativeMatrix DangerousGetNativeMatrix() => Matrix;
}
