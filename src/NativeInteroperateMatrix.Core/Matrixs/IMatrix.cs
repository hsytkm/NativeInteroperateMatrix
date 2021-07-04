using System;

namespace NativeInteroperateMatrix.Core
{
    public interface IMatrix<TValue>
        where TValue : struct
    {
        IntPtr Pointer { get; }
        int Rows { get; }
        int Columns { get; }
        int BytesPerItem { get; }
        int BitsPerItem { get; }
        int Stride { get; }
        Span<TValue> GetRowSpan(int row);
        ReadOnlySpan<TValue> GetRoRowSpan(int row);

    }
}
