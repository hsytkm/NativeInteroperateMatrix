namespace Nima;

// Do not change the order of the struct because it is the same as C++
[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8 + (5 * sizeof(int)))]
public readonly record struct NativeMatrix : INativeMatrix, IEnumerable<NativeArray>
{
    public static readonly NativeMatrix Zero = new(IntPtr.Zero, 0, 0, 0, 0, 0);

    readonly IntPtr _pointer;
    readonly int _allocateSize;
    readonly int _rows;         // height
    readonly int _columns;      // width
    readonly int _bytesPerItem;
    readonly int _stride;

    internal NativeMatrix(IntPtr pointer, int allocateSize, int bytesPerItem, int rows, int columns, int stride)
    {
        if (IntPtr.Size != 8)
            throw new NotSupportedException("Must be x64.");

        _pointer = pointer;
        _allocateSize = allocateSize;
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
        if (row < 0)
            throw new ArgumentException($"Invalid row={row}");
        if (Rows - 1 < row)
            throw new IndexOutOfRangeException($"Invalid row={row}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void ThrowInvalidColumn(int column)
    {
        if (column < 0)
            throw new ArgumentException($"Invalid column={column}");
        if (Columns - 1 < column)
            throw new IndexOutOfRangeException($"Invalid column={column}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IntPtr GetIntPtr(int row, int column)
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
    public bool IsValid => INativeMatrixEx.IsValid(this);

    // INativeMemory
    public int Rows => _rows;
    public int Columns => _columns;
    public int Width => _columns;
    public int Height => _rows;
    public int Stride => _stride;
    public bool IsContinuous => (Columns * BytesPerItem) == Stride;

    public unsafe Span<T> AsSpan<T>() where T : struct => INativeMatrixEx.AsSpan<NativeMatrix, T>(this);

    public ReadOnlySpan<T> AsReadOnlySpan<T>() where T : struct => AsSpan<T>();

    public unsafe Span<T> AsRowSpan<T>(int row) where T : struct
    {
        ThrowInvalidRow(row);
        return INativeMatrixEx.AsRowSpan<NativeMatrix, T>(this, row);
    }

    public ReadOnlySpan<T> AsRowReadOnlySpan<T>(int row) where T : struct => AsRowSpan<T>(row);

    public T GetValue<T>(int row, int column) where T : struct =>
        INativeMatrixEx.GetValue<NativeMatrix, T>(this, row, column);

    /// <summary>引数から値をコピーします</summary>
    internal void CopyFrom(IntPtr pointer, int rows, int columns, int stride, int bytesPerItem) =>
        INativeMatrixEx.CopyFrom(this, pointer, rows, columns, stride, bytesPerItem);

    /// <summary>引数に値をコピーします</summary>
    internal void CopyTo(NativeMatrix dest) => INativeMatrixEx.CopyTo(this, dest);

    public override string ToString() => $"Rows={Rows}, Cols={Columns}, Pointer=0x{Pointer:x16}";

    public IEnumerator<NativeArray> GetEnumerator()
    {
        IntPtr head = Pointer;
        var length = Columns * BytesPerItem;

        for (int row = 0; row < Rows; row++)
        {
            IntPtr rowHeadPtr = head + (row * Stride);
            yield return new NativeArray(rowHeadPtr, length, BytesPerItem);
        }
    }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}
