namespace Nima;

/// <summary>
/// Matrix構造体のメソッド群
/// </summary>
internal static class INativeMatrixEx
{
    internal static bool Valid<TMatrix>(TMatrix matrix)
        where TMatrix : struct, INativeMatrix
    {
        if (matrix.Pointer == IntPtr.Zero) return false;
        if (matrix.Columns <= 0 || matrix.Rows <= 0) return false;
        if (matrix.BytesPerItem <= 0) return false;
        if (matrix.Stride < matrix.Columns * matrix.BytesPerItem) return false;
        return true;    //valid
    }

    internal static unsafe Span<TType> AsSpan<TMatrix, TType>(TMatrix matrix)
        where TMatrix : struct, INativeMatrix
        where TType : struct
    {
        int length = matrix.AllocateSize / Unsafe.SizeOf<TType>();
        return new(matrix.Pointer.ToPointer(), length);
    }

    internal static unsafe Span<TType> AsRowSpan<TMatrix, TType>(TMatrix matrix, int row)
        where TMatrix : struct, INativeMatrix
        where TType : struct
    {
        IntPtr ptr = matrix.Pointer + (row * matrix.Stride);
        return new(ptr.ToPointer(), matrix.Columns);
    }

    internal static TType GetValue<TMatrix, TType>(TMatrix matrix, int row, int column)
        where TMatrix : struct, INativeMatrix
        where TType : struct
    {
        IntPtr ptr = matrix.Pointer + (row * matrix.Stride) + (column * matrix.BytesPerItem);
        return Marshal.PtrToStructure<TType>(ptr);
    }

    /// <summary>引数から値をコピーします</summary>
    internal static void CopyFrom<TMatrix>(TMatrix srcMatrix, IntPtr intPtr, int rows, int columns, int stride, int bytesPerItem)
        where TMatrix : struct, INativeMatrix
    {
        int srcSize = rows * stride;
        if (srcMatrix.AllocateSize < srcSize)
            throw new NotSupportedException("Allocated size is short.");

        int destStride = srcMatrix.Stride;
        if (destStride == stride)
        {
            UnsafeUtils.MemCopy(srcMatrix.Pointer, intPtr, srcSize);
        }
        else
        {
            IntPtr destPtr = srcMatrix.Pointer;
            IntPtr srcPtr = intPtr;
            int length = columns * bytesPerItem;

            for (int row = 0; row < rows; row++)
            {
                UnsafeUtils.MemCopy(destPtr, srcPtr, length);
                destPtr += destStride;
                srcPtr += stride;
            }
        }
    }

    /// <summary>引数から値をコピーします</summary>
    internal static void CopyFrom<TMatrix>(TMatrix destMatrix, TMatrix srcMatrix)
        where TMatrix : struct, INativeMatrix
    {
        CopyFrom(destMatrix, srcMatrix.Pointer, srcMatrix.Rows, srcMatrix.Columns, srcMatrix.Stride, srcMatrix.BytesPerItem);
    }

    /// <summary>引数に値をコピーします</summary>
    internal static void CopyTo<TMatrix>(TMatrix srcMatrix, TMatrix destMatrix)
        where TMatrix : struct, INativeMatrix
    {
        CopyFrom(destMatrix, srcMatrix);
    }
}
