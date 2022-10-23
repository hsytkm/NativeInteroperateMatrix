using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Nima;

// Do not change the order of the struct because it is the same as C++
[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8 + (3 * 4))]
public readonly record struct NativeArray : IArray, INativeMemory
{
    //public static NativeArray Zero = new(IntPtr.Zero, 0, 0);

    readonly IntPtr _pointer;
    readonly int _allocSize;
    readonly int _count;
    readonly int _bytesPerItem;

    public NativeArray(IntPtr pointer, int allocSize, int bytesPerItem)
    {
        if (IntPtr.Size != 8)
            throw new NotSupportedException("Must be x64");

        _pointer = pointer;
        _allocSize = allocSize;
        _count = allocSize / bytesPerItem;
        _bytesPerItem = bytesPerItem;
    }

    public NativeArray GetRearrangedArray(int bytesPerItem) => new(_pointer, _allocSize, bytesPerItem);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void ThrowInvalidLength(int index)
    {
        if (index < 0 || _count - 1 < index)
            throw new ArgumentException($"Invalid length={index}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IntPtr GetIntPtr(int index)
    {
        ThrowInvalidLength(index);
        return _pointer + (index * _bytesPerItem);
    }

    // INativeMemory
    public IntPtr Pointer => _pointer;
    public int AllocatedSize => _allocSize;
    public int BytesPerItem => _bytesPerItem;
    public int BitsPerItem => _bytesPerItem * 8;
    public bool IsValid
    {
        get
        {
            if (_pointer == IntPtr.Zero) return false;
            if (_count <= 0) return false;
            if (_bytesPerItem <= 0) return false;
            if (_allocSize < _count * _bytesPerItem) return false;
            return true;    //valid
        }
    }

    // IArray
    public int Count => _count;

    // IMatrix<T>
    public unsafe ref Byte this[int index]
    {
        get
        {
            IntPtr ptr = GetIntPtr(index);
            return ref Unsafe.AsRef<Byte>(ptr.ToPointer());
        }
    }

    public unsafe Span<Byte> AsSpan() => new(_pointer.ToPointer(), _count);
    public unsafe Span<Byte> AsReadOnlySpan() => AsSpan();

    public override string ToString() => $"Length={_count}, Pointer=0x{_pointer:x16}";
}
