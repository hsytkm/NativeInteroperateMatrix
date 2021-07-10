using System;
using System.Runtime.InteropServices;

namespace Nima.Core
{
    public abstract class MatrixContainerBase<TMatrix, TValue> : IMatrixContainer<TMatrix, TValue>
        where TMatrix : struct, IMatrix<TValue>
        where TValue : struct
    {
        public TMatrix Matrix { get; }
        private readonly IntPtr _allocatedMemoryPointer;
        private readonly int _allocatedSize;

        public MatrixContainerBase(int rows, int columns, bool initialize = true)
        {
            var bytesPerData = Marshal.SizeOf<TValue>();
            var stride = columns * bytesPerData;

            _allocatedSize = stride * rows;
            _allocatedMemoryPointer = Marshal.AllocCoTaskMem(_allocatedSize);
            GC.AddMemoryPressure(_allocatedSize);

            if (initialize)
            {
                UnsafeHelper.FillZero(_allocatedMemoryPointer, _allocatedSize);
            }

            Matrix = CreateMatrix(_allocatedMemoryPointer, rows, columns, bytesPerData, stride);
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
            Marshal.FreeCoTaskMem(_allocatedMemoryPointer);
            GC.RemoveMemoryPressure(_allocatedSize);

            _disposedValue = true;
        }

        ~MatrixContainerBase() => Dispose(disposing: false);

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
