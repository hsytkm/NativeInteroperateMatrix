namespace Nima;

public abstract class NativeMemoryContainerBase : IDisposable
{
    private protected NativePointerSizePair AllocatedMemory { get; private set; }

    private protected NativeMemoryContainerBase(NativePointerSizePair pair)
    {
        AllocatedMemory = pair;
    }

    protected NativeMemoryContainerBase(int allocateSize, bool initialize)
        : this(NativePointerSizePair.Alloc(allocateSize, initialize))
    { }

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

    // Do not return memory in deconstructor. Because memory may be inserted from the outside.
    //~NativeMemoryContainerBase() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
