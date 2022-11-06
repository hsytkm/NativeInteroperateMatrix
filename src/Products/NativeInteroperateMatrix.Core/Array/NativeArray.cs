namespace Nima;

// Do not change the order of the struct because it is the same as C++
[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8 + (2 * sizeof(int)))]
public readonly record struct NativeArray : INativeArray
{
    public static readonly NativeArray Zero = new(IntPtr.Zero, 0, 0);

    readonly IntPtr _pointer;
    readonly int _allocateSize;
    readonly int _bytesPerItem;

    public NativeArray(IntPtr pointer, int allocateSize, int bytesPerItem)
    {
        if (IntPtr.Size != 8)
            throw new NotSupportedException("Must be x64.");

        _pointer = pointer;
        _allocateSize = allocateSize;
        _bytesPerItem = bytesPerItem;

        if (!IsValid)
            throw new NotSupportedException($"Invalid {nameof(NativeArray)} ctor.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void ThrowInvalidLength(int index)
    {
        if (index < 0)
            throw new ArgumentException($"Invalid length={index}.");
        if (Length - 1 < index)
            throw new IndexOutOfRangeException($"Invalid length={index}.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IntPtr GetIntPtr(int index)
    {
        ThrowInvalidLength(index);
        return Pointer + (index * BytesPerItem);
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
            if (Length <= 0) return false;
            if (BytesPerItem <= 0) return false;
            if (AllocateSize < Length * BytesPerItem) return false;
            return true;    //valid
        }
    }

    // IArray
    public int Length => AllocateSize / BytesPerItem;

    // IMatrix<T>
    public unsafe ref Byte this[int index]
    {
        get
        {
            IntPtr ptr = GetIntPtr(index);
            return ref Unsafe.AsRef<Byte>(ptr.ToPointer());
        }
    }

    public unsafe Span<T> AsSpan<T>() where T : struct => new(Pointer.ToPointer(), Length);

    public ReadOnlySpan<T> AsReadOnlySpan<T>() where T : struct => AsSpan<T>();

    public T GetValue<T>(int index) where T : struct
    {
        IntPtr ptr = Pointer + index * BytesPerItem;
        return Marshal.PtrToStructure<T>(ptr);
    }

    public NativeArray GetRearrangedArray(int bytesPerItem) => new(Pointer, AllocateSize, bytesPerItem);

    public override string ToString() => $"Length={Length}, Pointer=0x{Pointer:x16}";
}
