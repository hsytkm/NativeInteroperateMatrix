using System.Runtime.CompilerServices;
using Xunit;

namespace Nima.Core.Tests.BuiltIn;

/// <summary>
/// 空の Container インスタンス作成（整数/小数共通部）
/// </summary>
public abstract class BuiltInMatrixContainerTestBase<TContainer, TMatrix, TValue>
    where TContainer : class, IMatrixContainer<TMatrix, TValue>
    where TMatrix : struct, IMatrix<TValue>
    where TValue : struct
{
    protected abstract TValue WriteValue { get; }

    protected abstract IMatrixContainer<TMatrix, TValue> CreateContainer(int rows, int columns, bool initialize);

    [Theory]
    [ClassData(typeof(RowColPairTestData))]
    public void Create(int rows, int columns)
    {
        using var container = CreateContainer(rows, columns, initialize: true);
        var matrix = container.Matrix;

        matrix.Pointer.IsNot(IntPtr.Zero);
        matrix.Rows.Is(rows);
        matrix.Columns.Is(columns);
        matrix.BytesPerItem.Is(Unsafe.SizeOf<TValue>());
        matrix.Stride.IsNot(0);         // tekito-

        matrix.Width.Is(matrix.Columns);
        matrix.Height.Is(matrix.Rows);
        matrix.AllocatedSize.IsNot(0);  // tekito-
        matrix.BitsPerItem.Is(matrix.BytesPerItem * 8);

        matrix.IsContinuous.IsTrue();   // must be true
        matrix.IsValid.IsTrue();
    }
}
