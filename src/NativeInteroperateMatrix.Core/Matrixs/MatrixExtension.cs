using System;

namespace NativeInteroperateMatrix.Core
{
    // 構造体を型制約付きジェネリクスで受け取ると boxing が起こらない。  https://ikorin2.hatenablog.jp/entry/2021/05/03/172217
    public static class MatrixExtension
    {
        public static int GetAllocatedSize<TMatrix, TValue>(this TMatrix matrix)
            where TMatrix : struct, IMatrix<TValue> where TValue : struct
        {
            return matrix.Columns * matrix.BytesPerItem * matrix.Rows;  // Strideは見ない
        }

        public static bool IsContinuous<TMatrix, TValue>(this TMatrix matrix)
            where TMatrix : struct, IMatrix<TValue> where TValue : struct
        {
            return (matrix.Columns * matrix.BytesPerItem) == matrix.Stride;
        }

        public static bool IsValid<TMatrix, TValue>(this TMatrix matrix)
            where TMatrix : struct, IMatrix<TValue> where TValue : struct
        {
            if (matrix.Pointer == IntPtr.Zero) return false;
            if (matrix.Columns <= 0 || matrix.Rows <= 0) return false;
            if (matrix.Stride < matrix.Columns * matrix.BytesPerItem) return false;
            if (matrix.GetAllocatedSize<TMatrix, TValue>() < matrix.Columns * matrix.BytesPerItem * matrix.Rows) return false;
            return true;    //valid
        }

        /// <summary>指定行のSpanを取得します</summary>
        public static unsafe Span<TValue> GetRowSpan<TMatrix, TValue>(this TMatrix matrix, int row)
            where TMatrix : struct, IMatrix<TValue> where TValue : struct
        {
            if (row < 0 || matrix.Rows - 1 < row)
                throw new ArgumentException("invalid row");

            var ptr = matrix.Pointer + (row * matrix.Stride);
            return new Span<TValue>(ptr.ToPointer(), matrix.Columns);
        }

        /// <summary>指定行のReadOnlySpanを取得します</summary>
        public static ReadOnlySpan<TValue> GetRoRowSpan<TMatrix, TValue>(this TMatrix matrix, int row)
            where TMatrix : struct, IMatrix<TValue> where TValue : struct
        {
            return GetRowSpan<TMatrix, TValue>(matrix, row);
        }

    }
}
