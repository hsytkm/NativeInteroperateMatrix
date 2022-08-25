namespace Nima;

public interface INativeMemory<T>
    where T : struct
{
    IntPtr Pointer { get; }
    int AllocatedSize { get; }

    int BytesPerItem { get; }
    int BitsPerItem { get; }

    bool IsValid { get; }
}
