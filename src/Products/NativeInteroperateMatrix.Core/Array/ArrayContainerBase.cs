namespace Nima;

public abstract class ArrayContainerBase : NativeMemoryContainerBase, INativeArrayContainer
{
    /// <summary>
    /// 外部公開用の配列(NativeMemoryをWrapしています)
    /// </summary>
    protected NativeArray Array { get; }

    // メモリ読み出し中カウンタ
    int _readCounter;

    // メモリ書き込み中フラグ
    bool _isWriting;

    protected ArrayContainerBase(int length, int bytesPerItem, bool initialize)
        : base(length * bytesPerItem, initialize)
    {
        int allocateSize = length * bytesPerItem;
        Array = new NativeArray(AllocatedMemory.Pointer, allocateSize, bytesPerItem);
    }

    public IDisposable GetArrayForRead(out NativeArray array)
    {
        if (_isWriting)
            throw new NotSupportedException("Someone is writing.");

        _readCounter++;
        array = Array;
        return new DisposableAction(() => _readCounter--);
    }

    public IDisposable GetArrayForWrite(out NativeArray array)
    {
        if (_isWriting)
            throw new NotSupportedException("Someone is writing.");

        if (_readCounter > 0)
            throw new NotSupportedException("Someone is reading.");

        _isWriting = true;
        array = Array;
        return new DisposableAction(() => _isWriting = false);
    }

}
