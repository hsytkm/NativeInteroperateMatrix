using System.Runtime.CompilerServices;

namespace Nima;

public /*sealed*/ class ByteArrayContainer : IArrayContainer, IDisposable
{
    /// <summary>
    /// 確保したNativeMemory
    /// </summary>
    private protected NativePointerSizePair AllocatedMemory;

    /// <summary>
    /// 外部公開用の配列(NativeMemoryをWrapしています)
    /// </summary>
    protected NativeArray Array { get; }

    // メモリ読み出し中カウンタ
    int _readCounter;

    // メモリ書き込み中フラグ
    bool _isWriting;

    protected ByteArrayContainer(int length, int bytesPerItem, bool initialize)
    {
        AllocatedMemory = NativePointerSizePair.Alloc(length * bytesPerItem, initialize);
        Array = new NativeArray(AllocatedMemory.Pointer, length * bytesPerItem, bytesPerItem);
    }

    public ByteArrayContainer(int length, bool initialize = true)
        : this(length, Unsafe.SizeOf<Byte>(), initialize)
    { }

    public ByteArrayContainer(int length, IEnumerable<Byte> items)
        : this(length, false)
    {
        int index = 0;
        var span = Array.AsSpan();

        foreach (var item in items)
        {
            span[index++] = item;

            if (index > length)
                throw new ArgumentException("Items is large.", nameof(items));
        }

        if (index != length - 1)
            throw new ArgumentException("Items is small.", nameof(items));
    }

    public ByteArrayContainer(int length, ReadOnlySpan<Byte> items)
        : this(length, false)
    {
        int index = 0;
        var span = Array.AsSpan();

        foreach (var item in items)
        {
            span[index++] = item;

            if (index > length)
                throw new ArgumentException("Items is large.", nameof(items));
        }

        if (index != length - 1)
            throw new ArgumentException("Items is small.", nameof(items));
    }


    sealed class DisposableAction : IDisposable
    {
        readonly Action _action;
        public DisposableAction(Action action) => _action = action;
        public void Dispose() => _action.Invoke();
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


    bool _disposedValue;
    public void Dispose(bool disposing)
    {
        if (_disposedValue)
            return;

        if (disposing)
        {
            // TODO: dispose managed state (managed objects).
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        if (AllocatedMemory != NativePointerSizePair.Zero)
        {
            AllocatedMemory.Free();
            AllocatedMemory = NativePointerSizePair.Zero;
        }

        _disposedValue = true;
    }

    ~ByteArrayContainer() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
