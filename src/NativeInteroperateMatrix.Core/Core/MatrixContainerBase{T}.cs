using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Nima;

public abstract class MatrixContainerBase<T> : IMatrixContainer<T>
    where T : struct
{
    public IMatrix<T> Matrix { get; }

    readonly record struct PointerSizePair
    {
        public static readonly PointerSizePair Zero = new(IntPtr.Zero, 0);
        public IntPtr Pointer { get; }
        public int Size { get; }
        public PointerSizePair(IntPtr ptr, int size) => (Pointer, Size) = (ptr, size);
    }

    PointerSizePair _allocatedMemory;

    protected MatrixContainerBase(int rows, int columns) : this(rows, columns, true) { }

    protected MatrixContainerBase(int rows, int columns, bool initialize)
    {
        int bytesPerData = Unsafe.SizeOf<T>();
        int stride = columns * bytesPerData;
        _allocatedMemory = Alloc(stride * rows);

        if (initialize)
        {
            UnsafeUtils.FillZero(_allocatedMemory.Pointer, _allocatedMemory.Size);
        }

        Matrix = CreateMatrix(_allocatedMemory.Pointer, rows, columns, bytesPerData, stride);
    }

    static PointerSizePair Alloc(int size)
    {
        IntPtr intPtr;
#if NET6_0_OR_GREATER
        unsafe { intPtr = (IntPtr)NativeMemory.Alloc((nuint)size); }
#else
        intPtr = Marshal.AllocCoTaskMem(size);
#endif
        GC.AddMemoryPressure(size);
        return new(intPtr, size);
    }

    static void Free(in PointerSizePair pair)
    {
#if NET6_0_OR_GREATER
        unsafe { NativeMemory.Free((void*)pair.Pointer); }
#else
        Marshal.FreeCoTaskMem(pair.Pointer);
#endif
        GC.RemoveMemoryPressure(pair.Size);
    }

    protected abstract IMatrix<T> CreateMatrix(IntPtr intPtr, int width, int height, int bytesPerData, int stride);

    #region IDisposable
    bool _disposedValue;
    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue) return;

        if (disposing)
        {
            // TODO: dispose managed state (managed objects).
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        if (_allocatedMemory != PointerSizePair.Zero)
        {
            Free(_allocatedMemory);
            _allocatedMemory = PointerSizePair.Zero;
        }

        _disposedValue = true;
    }

    ~MatrixContainerBase() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
