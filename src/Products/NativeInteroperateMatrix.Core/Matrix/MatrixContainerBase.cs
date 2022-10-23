namespace Nima;

public /*sealed*/ class MatrixContainerBase : NativeMemoryContainerBase
{
    /// <summary>
    /// 外部公開用の配列(NativeMemoryをWrapしています)
    /// </summary>
    protected NativeMatrix Matrix { get; }

    // メモリ読み出し中カウンタ
    int _readCounter;

    // メモリ書き込み中フラグ
    bool _isWriting;

    protected MatrixContainerBase(int rows, int columns, int bytesPerItem, bool initialize)
        : base(rows * columns * bytesPerItem, initialize)
    {
        int stride = rows * bytesPerItem;
        int allocateSize = columns * stride;
        Matrix = new NativeMatrix(AllocatedMemory.Pointer, allocateSize, bytesPerItem, rows, columns, stride);
    }

    public IDisposable GetArrayForRead(out NativeMatrix matrix)
    {
        if (_isWriting)
            throw new NotSupportedException("Someone is writing.");

        _readCounter++;
        matrix = Matrix;
        return new DisposableAction(() => _readCounter--);
    }

    public IDisposable GetArrayForWrite(out NativeMatrix matrix)
    {
        if (_isWriting)
            throw new NotSupportedException("Someone is writing.");

        if (_readCounter > 0)
            throw new NotSupportedException("Someone is reading.");

        _isWriting = true;
        matrix = Matrix;
        return new DisposableAction(() => _isWriting = false);
    }

}
