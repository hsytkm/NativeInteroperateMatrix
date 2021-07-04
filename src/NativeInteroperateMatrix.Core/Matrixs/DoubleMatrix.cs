using System;
using System.Runtime.InteropServices;
//using Matrix.SourceGenerator;

namespace NativeInteroperateMatrix.Core
{
    //[MatrixGenerator]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8 + (4 * 4))]
    public readonly partial struct DoubleMatrix : IEquatable<DoubleMatrix>, IMatrix<double>
    {
        private readonly IntPtr _pointer;
        //private readonly int _allocSize;  // = rows * stride;
        private readonly int _rows;         // height
        private readonly int _columns;      // width
        private readonly int _bytesPerItem;
        private readonly int _stride;

        public DoubleMatrix(int rows, int columns, int bytesPerItem, int stride, IntPtr intPtr)
        {
            if (IntPtr.Size != 8)
                throw new NotSupportedException();

            if (bytesPerItem != sizeof(double))
                throw new ArgumentException(nameof(bytesPerItem));

            _rows = rows;
            _columns = columns;
            _bytesPerItem = bytesPerItem;
            _stride = stride;
            _pointer = intPtr;
        }

        // IMatrix<T>
        public IntPtr Pointer => _pointer;
        public int Rows => _rows;
        public int Columns => _columns;
        public int BytesPerItem => _bytesPerItem;
        public int BitsPerItem => _bytesPerItem * 8;
        public int Stride => _stride;
        public int Width => _columns;
        public int Height => _rows;

        // MatrixExtension
        public int AllocatedSize => this.GetAllocatedSize<DoubleMatrix, double>();
        public bool IsContinuous => this.IsContinuous<DoubleMatrix, double>();
        public bool IsValid => this.IsValid<DoubleMatrix, double>();
        public bool IsInvalid => !IsValid;
        public Span<double> GetRowSpan(int row) => this.GetRowSpan<DoubleMatrix, double>(row);
        public ReadOnlySpan<double> GetRoRowSpan(int row) => this.GetRowSpan<DoubleMatrix, double>(row);

        // IEquatable<T>
        public bool Equals(DoubleMatrix other) => this == other;
        public override bool Equals(object? obj) => (obj is DoubleMatrix other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(_pointer, _rows, _columns, _bytesPerItem, _stride);

        public static bool operator ==(in DoubleMatrix left, in DoubleMatrix right)
             => (left._pointer, left._rows, left._columns, left._bytesPerItem, left._stride)
                == (right._pointer, right._rows, right._columns, right._bytesPerItem, right._stride);

        public static bool operator !=(in DoubleMatrix left, in DoubleMatrix right) => !(left == right);

    }
}
