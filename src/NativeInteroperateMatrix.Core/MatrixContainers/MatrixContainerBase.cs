using NativeInteroperateMatrix.Core.Imaging;
using System;
using System.Runtime.InteropServices;

namespace NativeInteroperateMatrix.Core
{
    public abstract class MatrixContainerBase<TMatrix, TValue> : IMatrixContainer<TMatrix, TValue>, IDisposable
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

            Matrix = CreateMatrix(rows, columns, bytesPerData, stride, _allocatedMemoryPointer);
        }

        private static TMatrix CreateMatrix(int width, int height, int bytesPerData, int stride, IntPtr intPtr)
        {
            var t = typeof(TValue);

            if (t == typeof(Pixel3ch))
                return (TMatrix)(IMatrix<Pixel3ch>)new Pixel3Matrix(width, height, bytesPerData, stride, intPtr);

            if (t == typeof(double))
                return (TMatrix)(IMatrix<double>)new DoubleMatrix(width, height, bytesPerData, stride, intPtr);

            throw new NotImplementedException();
        }

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
