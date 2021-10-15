using System;

namespace Nima.Core
{
    public interface INativeMemory<TValue>
        where TValue : struct
    {
        IntPtr Pointer { get; }
        int AllocatedSize { get; }

        int BytesPerItem { get; }
        int BitsPerItem { get; }

        bool IsValid { get; }
    }
}
