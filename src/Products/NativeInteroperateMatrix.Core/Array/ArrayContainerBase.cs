namespace Nima;

public abstract class ArrayContainerBase : NativeMemoryContainerBase, INativeArrayContainer
{
    /// <summary>
    /// 外部公開用の1次元配列(NativeMemoryをWrapしています)
    /// </summary>
    protected NativeArray Array { get; }

    /// <summary>
    /// メモリ読み出し中カウンタ
    /// </summary>
    public int ReadCounter { get; private set; }

    /// <summary>
    /// メモリ書き込み中フラグ
    /// </summary>
    public bool IsWriting { get; private set; }

    public int Length => Array.Length;

    protected ArrayContainerBase(int length, int bytesPerItem, bool initialize)
        : base(length * bytesPerItem, initialize)
    {
        int allocateSize = length * bytesPerItem;
        Array = new NativeArray(AllocatedMemory.Pointer, allocateSize, bytesPerItem);
    }

    public IDisposable GetArrayForRead(out NativeArray array)
    {
        if (IsWriting)
            throw new NotSupportedException("Someone is writing.");

        ReadCounter++;
        array = Array;
        return new DisposableAction(() => ReadCounter--);
    }

    public IDisposable GetArrayForWrite(out NativeArray array)
    {
        if (IsWriting)
            throw new NotSupportedException("Someone is writing.");

        if (ReadCounter > 0)
            throw new NotSupportedException("Someone is reading.");

        IsWriting = true;
        array = Array;
        return new DisposableAction(() => IsWriting = false);
    }

}
