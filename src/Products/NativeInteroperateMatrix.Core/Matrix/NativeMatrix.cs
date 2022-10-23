namespace Nima;

// Do not change the order of the struct because it is the same as C++
[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8 + (5 * sizeof(int)))]
public readonly record struct NativeMatrix : IMatrix<Byte>
{
    readonly IntPtr _pointer;
    readonly int _allocSize;
    readonly int _rows;         // height
    readonly int _columns;      // width
    readonly int _bytesPerItem;
    readonly int _stride;

    public NativeMatrix(IntPtr pointer, int allocSize, int bytesPerItem, int rows, int columns, int stride)
    {
        if (IntPtr.Size != 8)
            throw new NotSupportedException("Must be x64.");

        if (bytesPerItem != Unsafe.SizeOf<Byte>())
            throw new ArgumentException(nameof(bytesPerItem));

        _pointer = pointer;
        _allocSize = allocSize;
        _rows = rows;
        _columns = columns;
        _bytesPerItem = bytesPerItem;
        _stride = stride;

        if (!IsValid)
            throw new NotSupportedException($"Invalid {nameof(NativeMatrix)} ctor.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void ThrowInvalidRow(int row)
    {
        if (row < 0 || _rows - 1 < row)
            throw new ArgumentException($"Invalid row={row}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void ThrowInvalidColumn(int column)
    {
        if (column < 0 || _columns - 1 < column)
            throw new ArgumentException($"Invalid column={column}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IntPtr GetIntPtr(int row, int column)
    {
        ThrowInvalidRow(row);
        ThrowInvalidColumn(column);
        return _pointer + (row * _stride) + (column * _bytesPerItem);
    }

    // INativeMemory
    public IntPtr Pointer => _pointer;
    public int AllocateSize => _allocSize;
    public int BytesPerItem => _bytesPerItem;
    public int BitsPerItem => _bytesPerItem * 8;
    public bool IsValid
    {
        get
        {
            if (_pointer == IntPtr.Zero) return false;
            if (_columns <= 0 || _rows <= 0) return false;
            if (_stride < _columns * _bytesPerItem) return false;
            return true;    //valid
        }
    }

    // IMatrix
    public int Rows => _rows;
    public int Columns => _columns;
    public int Stride => _stride;
    public bool IsContinuous => (_columns * _bytesPerItem) == _stride;

    // IMatrix<T>
    public unsafe ref Byte this[int row, int column]
    {
        get
        {
            IntPtr ptr = GetIntPtr(row, column);
            return ref Unsafe.AsRef<Byte>(ptr.ToPointer());
        }
    }

    public unsafe Span<Byte> AsSpan()
    {
        int length = _rows * _stride;
        return new Span<Byte>(_pointer.ToPointer(), length);
    }

    public unsafe Span<Byte> AsRowSpan(int row)
    {
        ThrowInvalidRow(row);

        IntPtr ptr = _pointer + (row * _stride);
        return new Span<Byte>(ptr.ToPointer(), _columns);
    }

    public override string ToString() => $"Rows={_rows}, Cols={_columns}, Pointer=0x{_pointer:x16}";
}
