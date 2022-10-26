namespace Nima.Imaging;

#if false
// Do not change the order of the struct because it is the same as C++
[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8 + (5 * sizeof(int)))]
public readonly partial record struct PixelBgr24Matrix : INativeMatrix
{
    readonly IntPtr _pointer;
    readonly int _allocateSize;
    readonly int _rows;         // height
    readonly int _columns;      // width
    readonly int _bytesPerItem;
    readonly int _stride;

    public PixelBgr24Matrix(IntPtr pointer, int allocateSize, int bytesPerItem, int rows, int columns, int stride)
    {
        if (IntPtr.Size != 8)
            throw new NotSupportedException("Must be x64.");

        if (bytesPerItem != Unsafe.SizeOf<Byte>())
            throw new ArgumentException(nameof(bytesPerItem));

        _pointer = pointer;
        _allocateSize = allocateSize;
        _rows = rows;
        _columns = columns;
        _bytesPerItem = bytesPerItem;
        _stride = stride;

        if (!IsValid)
            throw new NotSupportedException($"Invalid {nameof(NativeMatrix)} ctor.");
    }

    public PixelBgr24Matrix(NativeMatrix matrix)
        : this(matrix.Pointer, matrix.AllocateSize, matrix.BytesPerItem, matrix.Rows, matrix.Columns, matrix.Stride)
    { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void ThrowInvalidRow(int row)
    {
        if (row < 0 || Rows - 1 < row)
            throw new ArgumentException($"Invalid row={row}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void ThrowInvalidColumn(int column)
    {
        if (column < 0 || Columns - 1 < column)
            throw new ArgumentException($"Invalid column={column}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IntPtr GetIntPtr(int row, int column)
    {
        ThrowInvalidRow(row);
        ThrowInvalidColumn(column);
        return Pointer + (row * Stride) + (column * BytesPerItem);
    }

    // INativeMemory
    public IntPtr Pointer => _pointer;
    public int AllocateSize => _allocateSize;
    public int BytesPerItem => _bytesPerItem;
    public int BitsPerItem => _bytesPerItem * 8;
    public bool IsValid
    {
        get
        {
            if (Pointer == IntPtr.Zero) return false;
            if (Columns <= 0 || Rows <= 0) return false;
            if (BytesPerItem <= 0) return false;
            if (Stride < Columns * BytesPerItem) return false;
            return true;    //valid
        }
    }

    // IMatrix
    public int Rows => _rows;
    public int Columns => _columns;
    public int Stride => _stride;
    public bool IsContinuous => (Columns * BytesPerItem) == Stride;

    public override string ToString() => $"Rows={Rows}, Cols={Columns}, Pointer=0x{Pointer:x16}";
}
#endif
