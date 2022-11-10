namespace Nima;

// Do not change the order of the struct because it is the same as C++
[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8 + (2 * sizeof(int)))]
public readonly record struct NativeArray : INativeArray
{
    public static readonly NativeArray Zero = new(IntPtr.Zero, 0, 0);

    readonly IntPtr _pointer;
    readonly int _allocateSize;
    readonly int _bytesPerItem;

    internal NativeArray(IntPtr pointer, int allocateSize, int bytesPerItem)
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
    public bool IsValid => INativeArrayEx.IsValid(this);

    // INativeArray
    public int Length => AllocateSize / BytesPerItem;

    public unsafe Span<T> AsSpan<T>() where T : struct => INativeArrayEx.AsSpan<NativeArray, T>(this);

    public ReadOnlySpan<T> AsReadOnlySpan<T>() where T : struct => AsSpan<T>();

    public T GetValue<T>(int index) where T : struct
    {
        ThrowInvalidLength(index);
        return INativeArrayEx.GetValue<NativeArray, T>(this, index);
    }

    public NativeArray GetRearrangedArray(int bytesPerItem)
    {
        if (AllocateSize < bytesPerItem)
            throw new InvalidOperationException("Allocate size is small.");

        return new(Pointer, AllocateSize, bytesPerItem);
    }

    public override string ToString() => $"Length={Length}, Pointer=0x{Pointer:x16}";
}
