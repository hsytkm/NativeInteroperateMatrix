namespace Nima.Imaging;

public /*sealed*/ class PixelBgr24MatrixContainer : MatrixContainerBase, IPixelBgr24MatrixContainer
{
    public PixelBgr24MatrixContainer(int rows, int columns, bool initialize)
        : base(rows, columns, Unsafe.SizeOf<NativeMatrix>(), initialize)
    { }

    /// <summary>
    /// 確保メモリを変更せずに小さい画像サイズに変更します
    /// </summary>
    public PixelBgr24MatrixContainer ShrinkToNewMatrix(int newRows, int newColumns, int newBytesPerPixel, int newStride)
    {
        if (newStride * newRows > Matrix.AllocateSize)
            throw new NotSupportedException();

        var newMatrix = new NativeMatrix(Matrix.Pointer, Matrix.AllocateSize, newRows, newColumns, newBytesPerPixel, newStride);
        return new(newMatrix.Rows, newMatrix.Columns, false);
    }

    /// <summary>Bitmapファイルから PixelBgrMatrixContainer を生成します</summary>
    /// <param name="filePath">File path of Bitmap</param>
    /// <returns></returns>
    public static PixelBgr24MatrixContainer Create(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException(filePath);

        if (Path.GetExtension(filePath) != ".bmp")
            throw new NotSupportedException(filePath);

        using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Create(fileStream);
    }

    /// <summary>Bitmap の Stream から PixelBgrMatrixContainer を生成します</summary>
    /// <param name="stream">Stream of Bitmap</param>
    /// <returns></returns>
    public static PixelBgr24MatrixContainer Create(Stream stream)
    {
        var header = UnsafeUtils.ReadStruct<BitmapHeader>(stream);
        if (!header.CanRead)
            throw new InvalidDataException("Invalid bitmap format.");

        int rows = header.Height;
        int cols = header.Width;
        int stride = header.ImageStride;
        int bytesPerPixel = header.BytesPerPixel;
        int headerSize = Unsafe.SizeOf<BitmapHeader>();
        int colLength = cols * bytesPerPixel;

        PixelBgr24MatrixContainer container = new(rows, cols, false);
        IntPtr destPtr = container.Matrix.Pointer;
        byte[] array = new byte[colLength];

        for (int row = 0; row < rows; row++)
        {
            stream.Position = headerSize + (rows - 1 - row) * stride;
            _ = stream.Read(array);

            unsafe
            {
                fixed (byte* srcPtr = array)
                {
                    UnsafeUtils.MemCopy(destPtr + row * colLength, srcPtr, colLength);
                }
            }
        }
        return container;
    }

    /// <summary>Bitmapファイルから PixelBgrMatrixContainer を生成します</summary>
    /// <param name="filePath">File path of Bitmap</param>
    /// <returns></returns>
    public static async Task<PixelBgr24MatrixContainer> CreateAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException(filePath);

        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return await CreateAsync(stream, cancellationToken);
    }

    /// <summary>Bitmap の Stream から PixelBgrMatrixContainer を生成します</summary>
    /// <param name="stream">Stream of Bitmap</param>
    /// <returns></returns>
    public static async Task<PixelBgr24MatrixContainer> CreateAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var header = await UnsafeUtils.ReadStructAsync<BitmapHeader>(stream, cancellationToken);
        if (!header.CanRead)
            throw new InvalidDataException("Invalid bitmap format.");

        int rows = header.Height;
        int cols = header.Width;
        int stride = header.ImageStride;
        int bytesPerPixel = header.BytesPerPixel;
        int headerSize = Unsafe.SizeOf<BitmapHeader>();
        int colLength = cols * bytesPerPixel;

        PixelBgr24MatrixContainer container = new(rows, cols, false);
        IntPtr destPtr = container.Matrix.Pointer;
        byte[] array = new byte[colLength];

        for (int row = 0; row < rows; row++)
        {
            stream.Position = headerSize + (rows - 1 - row) * stride;
            _ = await stream.ReadAsync(array, cancellationToken);

            unsafe
            {
                fixed (byte* srcPtr = array)
                {
                    UnsafeUtils.MemCopy(destPtr + row * colLength, srcPtr, colLength);
                }
            }
        }
        return container;
    }

    /// <summary>
    /// 指定と同じ画像のコンテナを新規に作成します
    /// </summary>
    public static PixelBgr24MatrixContainer Clone(NativeMatrix pixels)
    {
        PixelBgr24MatrixContainer container = new(pixels.Rows, pixels.Columns, false);
        container.Matrix.CopyFrom(pixels);
        return container;
    }

    public bool CanReuseContainer(NativeMatrix pixels)
    {
        if (Matrix.Columns != pixels.Columns
            || Matrix.Rows != pixels.Rows
            || Matrix.BytesPerItem != pixels.BytesPerItem)
            return false;

        return true;
    }

}
