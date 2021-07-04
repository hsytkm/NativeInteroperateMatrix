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
        int Stride { get; }

        int Width { get; }
        int Height { get; }
        int AllocatedSize { get; }
        int BitsPerItem { get; }
        public bool IsContinuous { get; }
        public bool IsValid { get; }
        public bool IsInvalid { get; }

        Span<TValue> GetRowSpan(int row);
        ReadOnlySpan<TValue> GetRoRowSpan(int row);

    }
}
