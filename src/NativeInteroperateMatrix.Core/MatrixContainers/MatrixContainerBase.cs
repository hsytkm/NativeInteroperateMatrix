using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Nima.Core;

public abstract class MatrixContainerBase<TMatrix, TValue> : IMatrixContainer<TMatrix, TValue>
    where TMatrix : struct, IMatrix<TValue>
    where TValue : struct
{
    public TMatrix Matrix { get; }

    IntPtr _allocatedMemoryPointer;
    readonly int _allocatedSize;

    public MatrixContainerBase(int rows, int columns, bool initialize = true)
    {
        var bytesPerData = Unsafe.SizeOf<TValue>();
        var stride = columns * bytesPerData;

        _allocatedSize = stride * rows;
        _allocatedMemoryPointer = Alloc(_allocatedSize);
        GC.AddMemoryPressure(_allocatedSize);

        if (initialize)
        {
            UnsafeUtils.FillZero(_allocatedMemoryPointer, _allocatedSize);
        }

        Matrix = CreateMatrix(_allocatedMemoryPointer, rows, columns, bytesPerData, stride);
    }

    static IntPtr Alloc(int size)
    {
#if NET6_0_OR_GREATER
        unsafe { return (IntPtr)NativeMemory.Alloc((nuint)size); }
#else
        return Marshal.AllocCoTaskMem(size);
#endif
    }

    static void Free(IntPtr ptr)
    {
#if NET6_0_OR_GREATER
        unsafe { NativeMemory.Free((void*)ptr); }
#else
        Marshal.FreeCoTaskMem(ptr);
#endif
    }

    protected abstract TMatrix CreateMatrix(IntPtr intPtr, int width, int height, int bytesPerData, int stride);

    #region IDisposable
    private bool _disposedValue;
    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue) return;

        if (disposing)
        {
            // TODO: dispose managed state (managed objects).
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        if (_allocatedMemoryPointer != IntPtr.Zero)
        {
            Free(_allocatedMemoryPointer);
            GC.RemoveMemoryPressure(_allocatedSize);
            _allocatedMemoryPointer = IntPtr.Zero;
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
