namespace Nima.Imaging;

public /*sealed*/ class PixelBgr24MatrixContainer : MatrixContainerBase, IPixelBgr24MatrixContainer
{
    public PixelBgr24MatrixContainer(int rows, int columns, bool initialize)
        : base(rows, columns, Unsafe.SizeOf<PixelBgr24>(), initialize)
    { }

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
    public static PixelBgr24MatrixContainer Clone(IPixelBgr24MatrixContainer sourceContainer)
    {
        using var token = sourceContainer.GetMatrixForRead(out var sourceMatrix);
        PixelBgr24MatrixContainer newContainer = new(sourceMatrix.Rows, sourceMatrix.Columns, false);
        newContainer.CopyFrom(sourceContainer);
        return newContainer;
    }

    /// <summary>
    /// 確保済メモリを変更せずに小さい画像サイズに変更します
    /// </summary>
    public PixelBgr24MatrixContainer ShrinkToNewMatrix(int newRows, int newColumns, int newBytesPerPixel, int newStride)
    {
        if (newStride * newRows > Matrix.AllocateSize)
            throw new NotSupportedException();

        var newMatrix = new NativeMatrix(Matrix.Pointer, Matrix.AllocateSize, newRows, newColumns, newBytesPerPixel, newStride);
        return new(newMatrix.Rows, newMatrix.Columns, false);
    }

    /// <summary>
    /// 引数コンテナと比較して、コンテナを再利用できるかを判定します
    /// </summary>
    public bool CanReuseContainer(IPixelBgr24MatrixContainer sourceContainer)
    {
        using var token1 = sourceContainer.GetMatrixForRead(out var source);
        using var token2 = GetMatrixForRead(out var self);

        if (self.Columns != source.Columns
            || self.Rows != source.Rows
            || self.BytesPerItem != source.BytesPerItem)
            return false;

        return true;
    }

    /// <summary>
    /// 引数コンテナの内容をコピーします(コンテナを再作成しません)
    /// </summary>
    public void CopyFrom(IPixelBgr24MatrixContainer sourceContainer)
    {
        using var token1 = sourceContainer.GetMatrixForRead(out var source);
        using var token2 = GetMatrixForWrite(out var self);
        self.CopyFrom(source.Pointer, source.Rows, source.Columns, source.Stride);
    }

    /// <summary>指定領域における各チャンネルの画素平均値を取得します</summary>
    public ColorBgr GetChannelsAverage(int x, int y, int width, int height)
    {
        using var token = GetMatrixForRead(out var matrix);

        if (!matrix.IsValid) throw new ArgumentException("Invalid image.");
        if (width * height == 0) throw new ArgumentException("Area is zero.");
        if (matrix.Columns < x + width) throw new ArgumentException("Width over.");
        if (matrix.Rows < y + height) throw new ArgumentException("Height over.");

        var bytesPerPixel = matrix.BytesPerItem;
        Span<ulong> sumChannels = stackalloc ulong[bytesPerPixel];

        unsafe
        {
            var stride = matrix.Stride;
            var rowHead = (byte*)matrix.Pointer + y * stride;
            var rowTail = rowHead + height * stride;
            var columnLength = width * bytesPerPixel;

            for (byte* rowPtr = rowHead; rowPtr < rowTail; rowPtr += stride)
            {
                for (var ptr = rowPtr; ptr < rowPtr + columnLength; ptr += bytesPerPixel)
                {
                    for (var c = 0; c < bytesPerPixel; ++c)
                    {
                        sumChannels[c] += *(ptr + c);
                    }
                }
            }
        }

        Span<double> aveChannels = stackalloc double[sumChannels.Length];
        var count = (double)(width * height);

        for (var i = 0; i < aveChannels.Length; ++i)
        {
            aveChannels[i] = sumChannels[i] / count;
        }
        return new ColorBgr(aveChannels);
    }

    /// <summary>画面全体における各チャンネルの画素平均値を取得します</summary>
    public ColorBgr GetChannelsAverageOfEntire()
    {
        using var token = GetMatrixForRead(out var matrix);
        return GetChannelsAverage(0, 0, matrix.Columns, matrix.Rows);
    }

    /// <summary>指定領域の画素を塗りつぶします</summary>
    public void FillRectangle(PixelBgr24 pixelBgr, int x, int y, int width, int height)
    {
        using var token = GetMatrixForWrite(out var matrix);

        if (matrix.Columns < x + width) throw new ArgumentException("vertical direction");
        if (matrix.Rows < y + height) throw new ArgumentException("horizontal direction");

        unsafe
        {
            var stride = matrix.Stride;
            var lineHeadPtr = (byte*)matrix.GetIntPtr(y, x);
            var lineTailPtr = lineHeadPtr + height * stride;
            var widthOffset = width * matrix.BytesPerItem;

            for (var linePtr = lineHeadPtr; linePtr < lineTailPtr; linePtr += stride)
            {
                for (var p = (PixelBgr24*)linePtr; p < linePtr + widthOffset; p++)
                    *p = pixelBgr;
            }
        }
    }

    /// <summary>指定枠を描画します</summary>
    public void DrawRectangle(PixelBgr24 pixel, int x, int y, int width, int height)
    {
        using var token = GetMatrixForWrite(out var matrix);

        if (matrix.Columns < x + width) throw new ArgumentException("vertical direction");
        if (matrix.Rows < y + height) throw new ArgumentException("horizontal direction");

        unsafe
        {
            var stride = matrix.Stride;
            var bytesPerPixel = matrix.BytesPerItem;
            var widthOffset = (width - 1) * bytesPerPixel;
            var rectHeadPtr = (byte*)matrix.GetIntPtr(y, x);

            // 上ライン
            for (var ptr = rectHeadPtr; ptr < rectHeadPtr + widthOffset; ptr += bytesPerPixel)
                *(PixelBgr24*)ptr = pixel;

            // 下ライン
            var bottomHeadPtr = rectHeadPtr + (height - 1) * stride;
            for (var ptr = bottomHeadPtr; ptr < bottomHeadPtr + widthOffset; ptr += bytesPerPixel)
                *(PixelBgr24*)ptr = pixel;

            // 左ライン
            var leftTailPtr = rectHeadPtr + height * stride;
            for (var ptr = rectHeadPtr; ptr < leftTailPtr; ptr += stride)
                *(PixelBgr24*)ptr = pixel;

            // 右ライン
            var rightHeadPtr = rectHeadPtr + widthOffset;
            var rightTailPtr = rightHeadPtr + height * stride;
            for (var ptr = rightHeadPtr; ptr < rightTailPtr; ptr += stride)
                *(PixelBgr24*)ptr = pixel;
        }
    }

    public unsafe void FillAllPixels(PixelBgr24 pixel)
    {
        using var token = GetMatrixForWrite(out var matrix);

        var pixelsHead = (byte*)matrix.Pointer;
        var stride = matrix.Stride;
        var pixelsTail = pixelsHead + matrix.Rows * stride;
        var widthOffset = matrix.Columns * matrix.BytesPerItem;

        for (var line = pixelsHead; line < pixelsTail; line += stride)
        {
            var lineTail = line + widthOffset;
            for (var p = (PixelBgr24*)line; p < lineTail; ++p)
            {
                *p = pixel;
            }
        }
    }

}
