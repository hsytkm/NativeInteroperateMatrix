namespace Nima;

public interface INativeMemory
{
    /// <summary>
    /// Pointer
    /// </summary>
    IntPtr Pointer { get; }

    /// <summary>
    /// Allocated memory size (byte)
    /// </summary>
    int AllocateSize { get; }

    /// <summary>
    /// byte size of each item
    /// </summary>
    int BytesPerItem { get; }

    /// <summary>
    /// bit size of each item
    /// </summary>
    int BitsPerItem { get; }

    /// <summary>
    /// Validity of allocated memory
    /// </summary>
    bool Valid { get; }
}
