﻿namespace Nima.Imaging;

partial record struct PixelBgr24Matrix
{
    public int Width => Columns;
    public int Height => Rows;
    public int BytesPerPixel => BytesPerItem;
    public int BitsPerPixel => BitsPerItem;

    public unsafe Span<PixelBgr24> AsSpan()
    {
        int length = AllocateSize / Unsafe.SizeOf<PixelBgr24>();
        return new Span<PixelBgr24>(Pointer.ToPointer(), length);
    }

    public unsafe Span<PixelBgr24> AsRowSpan(int row)
    {
        ThrowInvalidRow(row);

        IntPtr ptr = Pointer + (row * Stride);
        int length = Columns * BytesPerItem / Unsafe.SizeOf<PixelBgr24>();
        return new Span<PixelBgr24>(ptr.ToPointer(), length);
    }

    public unsafe void CopyFrom(IntPtr intPtr, int width, int height, int stride)
    {
        var srcSize = height * stride;
        if (AllocateSize < srcSize)
            throw new NotSupportedException("Allocated size is short.");

        var destStride = Stride;
        if (destStride == stride)
        {
            UnsafeUtils.MemCopy(Pointer, intPtr, srcSize);
        }
        else
        {
            var destPtr = (byte*)Pointer.ToPointer();
            var srcPtr = (byte*)intPtr.ToPointer();

            for (int row = 0; row < height; row++)
            {
                UnsafeUtils.MemCopy(destPtr, srcPtr, stride);
                destPtr += destStride;
                srcPtr += stride;
            }
        }
    }

    public void CopyFrom(in PixelBgr24Matrix dest) =>
        CopyFrom(dest.Pointer, dest.Width, dest.Height, dest.Stride);

    /// <summary>指定領域における各チャンネルの画素平均値を取得します</summary>
    public ColorBgr GetChannelsAverage(int x, int y, int width, int height)
    {
        if (!IsValid) throw new ArgumentException("Invalid image.");
        if (width * height == 0) throw new ArgumentException("Area is zero.");
        if (_columns < x + width) throw new ArgumentException("Width over.");
        if (_rows < y + height) throw new ArgumentException("Height over.");

        var bytesPerPixel = _bytesPerItem;
        Span<ulong> sumChannels = stackalloc ulong[bytesPerPixel];

        unsafe
        {
            var stride = _stride;
            var rowHead = (byte*)_pointer + y * stride;
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
    public ColorBgr GetChannelsAverageOfEntire() => GetChannelsAverage(0, 0, _columns, _rows);

    /// <summary>指定の画素値で画像全体を埋めます</summary>
    public void FillAllPixels(in PixelBgr24 pixel)
    {
        unsafe
        {
            var pixelsHead = (byte*)_pointer;
            var stride = _stride;
            var pixelsTail = pixelsHead + _rows * stride;
            var widthOffset = _columns * _bytesPerItem;

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

    /// <summary>指定領域の画素を塗りつぶします</summary>
    public void FillRectangle(in PixelBgr24 pixel, int x, int y, int width, int height)
    {
        if (_columns < x + width) throw new ArgumentException("vertical direction");
        if (_rows < y + height) throw new ArgumentException("horizontal direction");

        unsafe
        {
            var lineHeadPtr = (byte*)GetIntPtr(y, x);
            var lineTailPtr = lineHeadPtr + height * _stride;
            var widthOffset = width * _bytesPerItem;

            for (var linePtr = lineHeadPtr; linePtr < lineTailPtr; linePtr += _stride)
            {
                for (var p = (PixelBgr24*)linePtr; p < linePtr + widthOffset; p++)
                    *p = pixel;
            }
        }
    }

    /// <summary>指定枠を描画します</summary>
    public void DrawRectangle(in PixelBgr24 pixel, int x, int y, int width, int height)
    {
        if (_columns < x + width) throw new ArgumentException("vertical direction");
        if (_rows < y + height) throw new ArgumentException("horizontal direction");

        unsafe
        {
            var stride = _stride;
            var bytesPerPixel = _bytesPerItem;
            var widthOffset = (width - 1) * bytesPerPixel;
            var rectHeadPtr = (byte*)GetIntPtr(y, x);

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

    /// <summary>画像の一部を切り出した子画像を取得します</summary>
    public PixelBgr24Matrix CutOutPixelMatrix(int x, int y, int width, int height)
    {
        if (_columns < x + width) throw new ArgumentException("vertical direction");
        if (_rows < y + height) throw new ArgumentException("horizontal direction");

        var bytesPerData = Unsafe.SizeOf<PixelBgr24>();
        var size = AllocateSize - (y * Stride) - (x * BytesPerItem);
        return new(GetIntPtr(y, x), size, height, width, bytesPerData, _stride);
    }

    /// <summary>画素値をコピーします</summary>
    public void CopyTo(in PixelBgr24Matrix destPixels)
    {
        if (_columns != destPixels._columns || _rows != destPixels._rows) throw new ArgumentException("size is different.");
        if (_pointer == destPixels._pointer) throw new ArgumentException("same pointer.");

        CopyToInternal(this, destPixels);

        // 画素値のコピー（サイズチェックなし）
        static void CopyToInternal(in PixelBgr24Matrix srcPixels, in PixelBgr24Matrix destPixels)
        {
            // メモリが連続していれば memcopy
            if (srcPixels.IsContinuous && destPixels.IsContinuous)
            {
                UnsafeUtils.MemCopy(destPixels._pointer, srcPixels._pointer, srcPixels.AllocateSize);
                return;
            }

            unsafe
            {
                var (width, height, bytesPerPixel) = (srcPixels._columns, srcPixels._rows, srcPixels._bytesPerItem);
                var srcHeadPtr = (byte*)srcPixels._pointer;
                var srcStride = srcPixels._stride;
                var dstHeadPtr = (byte*)destPixels._pointer;
                var dstStride = destPixels._stride;

                for (var y = 0; y < height; y++)
                {
                    var src = srcHeadPtr + y * srcStride;
                    var dst = dstHeadPtr + y * dstStride;

                    for (var x = 0; x < width * bytesPerPixel; x += bytesPerPixel)
                    {
                        *(PixelBgr24*)(dst + x) = *(PixelBgr24*)(src + x);
                    }
                }
            }
        }
    }

    /// <summary>画素値を拡大コピーします</summary>
    public void CopyToWithScaleUp(in PixelBgr24Matrix destination)
    {
        if (_bytesPerItem != PixelBgr24.Size || destination._bytesPerItem != PixelBgr24.Size)
            throw new ArgumentException("bytes/pixel error.");

        if (destination._columns % _columns != 0 || destination._rows % _rows != 0)
            throw new ArgumentException("must be an integral multiple.");

        var widthRatio = destination._columns / _columns;
        var heightRatio = destination._rows / _rows;
        if (widthRatio != heightRatio) throw new ArgumentException("magnifications are different.");

        var magnification = widthRatio;
        if (magnification <= 1) throw new ArgumentException("ratio must be greater than 1.");

        ScaleUp(this, destination, magnification);

        static unsafe void ScaleUp(in PixelBgr24Matrix source, in PixelBgr24Matrix destination, int magnification)
        {
            var bytesPerPixel = source._bytesPerItem;
            var srcPixelHead = (byte*)source._pointer;
            var srcStride = source._stride;
            var srcWidth = source._columns;
            var srcHeight = source._rows;

            var destPixelHead = (byte*)destination._pointer;
            var destStride = destination._stride;

            for (var y = 0; y < srcHeight; y++)
            {
                var src = srcPixelHead + srcStride * y;
                var dest0 = destPixelHead + destStride * y * magnification;

                for (var x = 0; x < srcWidth * bytesPerPixel; x += bytesPerPixel)
                {
                    var pixel = (PixelBgr24*)(src + x);
                    var dest1 = dest0 + x * magnification;

                    for (byte* dest2 = dest1; dest2 < dest1 + destStride * magnification; dest2 += destStride)
                    {
                        for (var dest3 = dest2; dest3 < dest2 + bytesPerPixel * magnification; dest3 += bytesPerPixel)
                            *(PixelBgr24*)dest3 = *pixel;
                    }
                }
            }
        }
    }

    /// <summary>画像をbmpファイルに保存します</summary>
    public void ToBmpFile(string savePath)
    {
        using var ms = ToBitmapMemoryStream(savePath);
        using var fs = new FileStream(savePath, FileMode.Create);
        fs.Seek(0, SeekOrigin.Begin);

        ms.WriteTo(fs);
    }

    /// <summary>画像をbmpファイルに保存します</summary>
    public async Task ToBmpFileAsync(string savePath, CancellationToken token = default)
    {
        using var ms = ToBitmapMemoryStream(savePath);
        using var fs = new FileStream(savePath, FileMode.Create);
        fs.Seek(0, SeekOrigin.Begin);

        await ms.CopyToAsync(fs, token);
    }

    /// <summary>画像をbmpファイルに保存します</summary>
    MemoryStream ToBitmapMemoryStream(string savePath)
    {
        if (!IsValid) throw new ArgumentException("Invalid image.");
        if (File.Exists(savePath)) throw new SystemException("File is exists.");

        var bitmapBytes = GetBitmapBinary(this);
        var ms = new MemoryStream(bitmapBytes);
        ms.Seek(0, SeekOrigin.Begin);
        return ms;

        // Bitmapのバイナリ配列を取得します
        static byte[] GetBitmapBinary(in PixelBgr24Matrix pixel)
        {
            var height = pixel._rows;
            var srcStride = pixel._stride;
            var destHeader = new BitmapHeader(pixel._columns, height, pixel.BitsPerItem);
            var destBuffer = new byte[destHeader.FileSize];     // さずがにデカすぎるのでbyte[]

            // bufferにheaderを書き込む
            UnsafeUtils.CopyStructToArray(destHeader, destBuffer);

            // 画素は左下から右上に向かって記録する
            unsafe
            {
                var srcHead = (byte*)pixel._pointer;
                fixed (byte* pointer = destBuffer)
                {
                    var destHead = pointer + destHeader.OffsetBytes;
                    var destStride = destHeader.ImageStride;
                    System.Diagnostics.Debug.Assert(srcStride <= destStride);

                    for (var y = 0; y < height; ++y)
                    {
                        var src = srcHead + (height - 1 - y) * srcStride;
                        var dest = destHead + y * destStride;
                        UnsafeUtils.MemCopy(dest, src, srcStride);
                    }
                }
            }
            return destBuffer;
        }
    }

}
