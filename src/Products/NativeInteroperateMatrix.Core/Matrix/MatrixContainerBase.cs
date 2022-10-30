namespace Nima;

public abstract class MatrixContainerBase : NativeMemoryContainerBase, IMatrixContainer
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

    public IDisposable GetMatrixForRead(out NativeMatrix matrix)
    {
        if (IsWriting)
            throw new NotSupportedException("Someone is writing.");

        ReadCounter++;
        matrix = Matrix;
        return new DisposableAction(() => ReadCounter--);
    }

    public IDisposable GetMatrixForWrite(out NativeMatrix matrix)
    {
        if (IsWriting)
            throw new NotSupportedException("Someone is writing.");

        if (ReadCounter > 0)
            throw new NotSupportedException("Someone is reading.");

        IsWriting = true;
        matrix = Matrix;
        return new DisposableAction(() => IsWriting = false);
    }

}
