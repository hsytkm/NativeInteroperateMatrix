using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Nima;

internal readonly record struct NativePointerSizePair
{
    public static readonly NativePointerSizePair Zero = new(IntPtr.Zero, 0);

    public IntPtr Pointer { get; }
    public int Size { get; }
    public NativePointerSizePair(IntPtr ptr, int size) => (Pointer, Size) = (ptr, size);
    public void Deconstruct(out IntPtr ptr, out int size) => (ptr, size) = (Pointer, Size);

    public unsafe Span<T> AsSpan<T>() where T : struct =>
        new(Pointer.ToPointer(), Size / Unsafe.SizeOf<T>());

    public static NativePointerSizePair Alloc(int size, bool initialize = true)
    {
        IntPtr intPtr;
#if NET6_0_OR_GREATER
        unsafe { intPtr = (IntPtr)NativeMemory.Alloc((nuint)size); }
#else
        intPtr = Marshal.AllocCoTaskMem(size);
#endif
        if (initialize)
            UnsafeUtils.FillZero(intPtr, size);

        GC.AddMemoryPressure(size);
        return new(intPtr, size);
    }

    public void Free()
    {
#if NET6_0_OR_GREATER
        unsafe { NativeMemory.Free((void*)Pointer); }
#else
        Marshal.FreeCoTaskMem(Pointer);
#endif
        GC.RemoveMemoryPressure(Size);
    }
}
