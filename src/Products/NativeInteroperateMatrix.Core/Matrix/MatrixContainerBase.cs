using System.Diagnostics;

namespace Nima;

[DebuggerDisplay("[{Columns}, {Rows}], R={ReadCounter}, W={IsWriting}")]
public abstract class MatrixContainerBase : NativeMemoryContainerBase, INativeMatrixContainer
{
    readonly object _lockObject = new();
    Action _readLockReleaseAction;
    Action _writeLockReleaseAction;

    /// <summary>
    /// 外部公開用の2次元配列(NativeMemoryをWrapしています)
    /// </summary>
    protected NativeMatrix Matrix { get; }

    /// <summary>
    /// メモリ読み出し中カウンタ
    /// </summary>
    public int ReadCounter { get; private set; }

    /// <summary>
    /// メモリ書き込み中フラグ
    /// </summary>
    public bool IsWriting { get; private set; }

    public int Rows { get; }    // DebuggerDisplay用に結果を保持します
    public int Columns { get; } // DebuggerDisplay用に結果を保持します

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Width => Matrix.Width;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Height => Matrix.Height;

    protected MatrixContainerBase(int rows, int columns, int bytesPerItem, bool initialize)
        : base(rows * columns * bytesPerItem, initialize)
    {
        int stride = columns * bytesPerItem;
        int allocateSize = rows * stride;
        Matrix = new NativeMatrix(AllocatedMemory.Pointer, allocateSize, bytesPerItem, rows, columns, stride);
        (Rows, Columns) = (Matrix.Rows, Matrix.Columns);
        SetLockReleaseAction();
    }

    private protected MatrixContainerBase(in NativeMatrix matrix)
        : base(new NativePointerSizePair(matrix.Pointer, matrix.AllocateSize))
    {
        Matrix = matrix;
        (Rows, Columns) = (Matrix.Rows, Matrix.Columns);
        SetLockReleaseAction();
    }

    [System.Diagnostics.CodeAnalysis.MemberNotNull(nameof(_readLockReleaseAction), nameof(_writeLockReleaseAction))]
    void SetLockReleaseAction()
    {
        _readLockReleaseAction = new(() =>
        {
            lock (_lockObject)
            {
                ReadCounter--;
            }
        });
        _writeLockReleaseAction = new(() =>
        {
            lock (_lockObject)
            {
                IsWriting = false;
            }
        });
    }

    /// <summary>
    /// Get native matrix for reading.
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    /// <exception cref="InvalidMemoryAccessException"></exception>
    public IDisposable GetMatrixForReading(out NativeMatrix matrix)
    {
        ThrowObjectDisposedException();

        if (IsWriting)
            throw new InvalidMemoryAccessException("Someone is writing.");

        matrix = Matrix;

        lock (_lockObject)
        {
            ReadCounter++;
        }
        return new DisposableAction(_readLockReleaseAction);
    }

    /// <summary>
    /// Get native matrix for writing.
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    /// <exception cref="InvalidMemoryAccessException"></exception>
    public IDisposable GetMatrixForWriting(out NativeMatrix matrix)
    {
        ThrowObjectDisposedException();

        if (IsWriting)
            throw new InvalidMemoryAccessException("Someone is writing.");

        if (ReadCounter > 0)
            throw new InvalidMemoryAccessException($"Someone is reading. (Count={ReadCounter})");

        matrix = Matrix;

        lock (_lockObject)
        {
            IsWriting = true;
        }
        return new DisposableAction(_writeLockReleaseAction);
    }

    /// <summary>
    /// Get native matrix (don't manage memory references)
    /// </summary>
    /// <returns>NativeMatrix</returns>
    public NativeMatrix DangerousGetNativeMatrix()
    {
        ThrowObjectDisposedException();
        return Matrix;
    }
}
