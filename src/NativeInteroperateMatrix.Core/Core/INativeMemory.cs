namespace Nima;

public interface INativeMemory
{
    IntPtr Pointer { get; }
    int AllocatedSize { get; }

    int BytesPerItem { get; }
    int BitsPerItem { get; }

    bool IsValid { get; }
}
