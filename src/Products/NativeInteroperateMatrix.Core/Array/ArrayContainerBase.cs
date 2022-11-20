namespace Nima;

public abstract class ArrayContainerBase : NativeMemoryContainerBase, INativeArrayContainer
{
    readonly object _lockObject = new();
    readonly Action _readLockReleaseAction;
    readonly Action _writeLockReleaseAction;

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

        _readLockReleaseAction = new(() =>
        {
            lock (_lockObject)
            {
                ReadCounter--;
            }
        });
        _writeLockReleaseAction = new(() =>
        {
            lock (_lockObject)
            {
                IsWriting = false;
            }
        });
    }

    /// <summary>
    /// Get native array for reading.
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    /// <exception cref="InvalidMemoryAccessException"></exception>
    public IDisposable GetArrayForReading(out NativeArray array)
    {
        ThrowObjectDisposedException();

        if (IsWriting)
            throw new InvalidMemoryAccessException("Someone is writing.");

        array = Array;

        lock (_lockObject)
        {
            ReadCounter++;
        }
        return new DisposableAction(_readLockReleaseAction);
    }

    /// <summary>
    /// Get native array for writing.
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    /// <exception cref="InvalidMemoryAccessException"></exception>
    public IDisposable GetArrayForWriting(out NativeArray array)
    {
        ThrowObjectDisposedException();

        if (IsWriting)
            throw new InvalidMemoryAccessException("Someone is writing.");

        if (ReadCounter > 0)
            throw new InvalidMemoryAccessException($"Someone is reading. (Count={ReadCounter})");

        array = Array;

        lock (_lockObject)
        {
            IsWriting = true;
        }
        return new DisposableAction(_writeLockReleaseAction);
    }

    /// <summary>
    /// Get native array (don't manage memory references)
    /// </summary>
    /// <returns>NativeArray</returns>
    public NativeArray DangerousGetNativeArray()
    {
        ThrowObjectDisposedException();
        return Array;
    }
}
