namespace Nima.Imaging;

#if false
// SourceGenerator 内で Container も一緒に生成しちゃっています(手抜き)
public /*sealed*/ partial class PixelBgr24MatrixContainer
//: IMatrixContainer<PixelBgr24>, IDisposable
{
    private PixelBgr24MatrixContainer(in PixelBgr24Matrix matrix)
        : this(matrix.Rows, matrix.Columns, false)
    {
        Matrix = matrix;
    }

    /// <summary>
    /// 確保メモリを変更せずに小さい画像サイズに変更します
    /// </summary>
    public PixelBgr24MatrixContainer ShrinkToNewMatrix(int newRows, int newColumns, int newBytesPerPixel, int newStride)
    {
        if (newStride * newRows > Matrix.AllocateSize)
            throw new NotSupportedException();

        var newMatrix = new PixelBgr24Matrix(Matrix.Pointer, Matrix.AllocateSize, newRows, newColumns, newBytesPerPixel, newStride);
        return new(newMatrix);
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

        var rows = header.Height;
        var cols = header.Width;
        var stride = header.ImageStride;
        var bytesPerPixel = header.BytesPerPixel;
        var headerSize = Unsafe.SizeOf<BitmapHeader>();
        var colLength = cols * bytesPerPixel;

        var container = new PixelBgr24MatrixContainer(rows, cols, false);
        var destPtr = container.Matrix.Pointer;
        var array = new byte[colLength];

        for (var row = 0; row < rows; row++)
        {
            stream.Position = headerSize + (rows - 1 - row) * stride;
            stream.Read(array);

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

        var rows = header.Height;
        var cols = header.Width;
        var stride = header.ImageStride;
        var bytesPerPixel = header.BytesPerPixel;
        var headerSize = Unsafe.SizeOf<BitmapHeader>();
        var colLength = cols * bytesPerPixel;

        var container = new PixelBgr24MatrixContainer(rows, cols, false);
        var destPtr = container.Matrix.Pointer;
        var array = new byte[colLength];

        for (var row = 0; row < rows; row++)
        {
            stream.Position = headerSize + (rows - 1 - row) * stride;
            await stream.ReadAsync(array, cancellationToken);

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
    public static PixelBgr24MatrixContainer Clone(in PixelBgr24Matrix pixels)
    {
        var container = new PixelBgr24MatrixContainer(pixels.Rows, pixels.Columns, false);
        container.Matrix.CopyFrom(pixels);
        return container;
    }

    public bool CanReuseContainer(in PixelBgr24Matrix pixels)
    {
        if (Matrix.Columns != pixels.Columns
            || Matrix.Rows != pixels.Rows
            || Matrix.BytesPerPixel != pixels.BytesPerPixel)
            return false;

        return true;
    }

}
#endif
