using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace NativeInteroperateMatrix.Core.Imaging
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 24)]
    public readonly struct Pixel3Matrix : IEquatable<Pixel3Matrix>, IMatrix<Pixel3ch>
    {
        public const int Channel = 3;

        private readonly IntPtr _pointer;
        //private readonly int _allocSize; //= Height * Stride;
        private readonly int _width;
        private readonly int _height;
        private readonly int _bytesPerData;
        private readonly int _stride;

        public Pixel3Matrix(int width, int height, int bytesPerData, int stride, IntPtr intPtr)
        {
            if (IntPtr.Size != 8) throw new NotSupportedException();
            if (bytesPerData != Channel) throw new NotSupportedException();

            _width = width;
            _height = height;
            _bytesPerData = bytesPerData;
            _stride = stride;
            _pointer = intPtr;
        }

        #region IMatrix<T>
        public int Columns => _width;
        public int Rows => _height;
        public IntPtr Pointer => _pointer;
        public int Width => _width;
        public int Height => _height;
        public int BytesPerItem => _bytesPerData;
        public int BitsPerItem => _bytesPerData * 8;
        public int Stride => _stride;
        #endregion

        public int BytesPerPixel => BytesPerItem;
        public int BitsPerPixel => BitsPerItem;

        #region MatrixExtension
        public int AllocatedSize => this.GetAllocatedSize<Pixel3Matrix, Pixel3ch>();
        public bool IsContinuous => this.IsContinuous<Pixel3Matrix, Pixel3ch>();
        public bool IsValid => this.IsValid<Pixel3Matrix, Pixel3ch>();
        public bool IsInvalid => !IsValid;
        public Span<Pixel3ch> GetRowSpan(int row) => this.GetRowSpan<Pixel3Matrix, Pixel3ch>(row);
        public ReadOnlySpan<Pixel3ch> GetRoRowSpan(int row) => this.GetRoRowSpan<Pixel3Matrix, Pixel3ch>(row);
        #endregion

        #region IEquatable<T>
        public bool Equals(Pixel3Matrix other) => this == other;
        public override bool Equals(object? obj) => (obj is Pixel3Matrix other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(_pointer, _width, _height, _bytesPerData, _stride);
        public static bool operator ==(in Pixel3Matrix left, in Pixel3Matrix right)
             => (left._pointer, left._width, left._height, left._bytesPerData, left._stride)
                == (right._pointer, right._width, right._height, right._bytesPerData, right._stride);

        public static bool operator !=(in Pixel3Matrix left, in Pixel3Matrix right) => !(left == right);
        #endregion

        #region GetChannelsAverage
        /// <summary>指定領域における各チャンネルの画素平均値を取得します</summary>
        public ColorBgr GetChannelsAverage(int x, int y, int width, int height)
        {
            if (IsInvalid) throw new ArgumentException("Invalid image.");
            if (width * height == 0) throw new ArgumentException("Area is zero.");
            if (_width < x + width) throw new ArgumentException("Width over.");
            if (_height < y + height) throw new ArgumentException("Height over.");

            var bytesPerPixel = _bytesPerData;
            Span<ulong> sumChannels = stackalloc ulong[bytesPerPixel];

            unsafe
            {
                var stride = _stride;
                var rowHead = (byte*)_pointer + (y * stride);
                var rowTail = rowHead + (height * stride);
                var columnLength = width * bytesPerPixel;

                for (byte* rowPtr = rowHead; rowPtr < rowTail; rowPtr += stride)
                {
                    for (byte* ptr = rowPtr; ptr < (rowPtr + columnLength); ptr += bytesPerPixel)
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
        public ColorBgr GetChannelsAverageOfEntire() => GetChannelsAverage(0, 0, _width, _height);
        #endregion

        #region FillAllPixels
        /// <summary>指定の画素値で画像全体を埋めます</summary>
        public void FillAllPixels(in Pixel3ch pixels)
        {
            unsafe
            {
                var pixelsHead = (byte*)_pointer;
                var stride = _stride;
                var pixelsTail = pixelsHead + _height * stride;
                var widthOffset = _width * _bytesPerData;

                for (var line = (byte*)_pointer; line < pixelsTail; line += stride)
                {
                    var lineTail = line + widthOffset;
                    for (var p = (Pixel3ch*)line; p < lineTail; ++p)
                    {
                        *p = pixels;
                    }
                }
            }
        }
        #endregion

        #region FillRectangle
        /// <summary>指定領域の画素を塗りつぶします</summary>
        public void FillRectangle(in Pixel3ch pixel, int x, int y, int width, int height)
        {
            if (_width < x + width) throw new ArgumentException("vertical direction");
            if (_height < y + height) throw new ArgumentException("horizontal direction");

            unsafe
            {
                var lineHeadPtr = (byte*)GetPixelPtr(x, y);
                var lineTailPtr = lineHeadPtr + (height * _stride);
                var widthOffset = width * _bytesPerData;

                for (var linePtr = lineHeadPtr; linePtr < lineTailPtr; linePtr += _stride)
                {
                    for (var p = (Pixel3ch*)linePtr; p < linePtr + widthOffset; p++)
                        *p = pixel;
                }
            }
        }
        #endregion

        #region DrawRectangle
        /// <summary>指定枠を描画します</summary>
        public void DrawRectangle(in Pixel3ch pixel, int x, int y, int width, int height)
        {
            if (_width < x + width) throw new ArgumentException("vertical direction");
            if (_height < y + height) throw new ArgumentException("horizontal direction");

            unsafe
            {
                var stride = _stride;
                var bytesPerPixel = _bytesPerData;
                var widthOffset = (width - 1) * bytesPerPixel;
                var rectHeadPtr = (byte*)GetPixelPtr(x, y);

                // 上ライン
                for (var ptr = rectHeadPtr; ptr < rectHeadPtr + widthOffset; ptr += bytesPerPixel)
                    *((Pixel3ch*)ptr) = pixel;

                // 下ライン
                var bottomHeadPtr = rectHeadPtr + ((height - 1) * stride);
                for (var ptr = bottomHeadPtr; ptr < bottomHeadPtr + widthOffset; ptr += bytesPerPixel)
                    *((Pixel3ch*)ptr) = pixel;

                // 左ライン
                var leftTailPtr = rectHeadPtr + (height * stride);
                for (var ptr = rectHeadPtr; ptr < leftTailPtr; ptr += stride)
                    *((Pixel3ch*)ptr) = pixel;

                // 右ライン
                var rightHeadPtr = rectHeadPtr + widthOffset;
                var rightTailPtr = rightHeadPtr + (height * stride);
                for (var ptr = rightHeadPtr; ptr < rightTailPtr; ptr += stride)
                    *((Pixel3ch*)ptr) = pixel;
            }
        }
        #endregion

        #region WritePixel
        /// <summary>指定画素の IntPtr を取得します</summary>
        public IntPtr GetPixelPtr(int x, int y)
        {
            if (x > _width - 1 || y > _height - 1) throw new ArgumentException("Out of image.");
            return _pointer + (y * _stride) + (x * _bytesPerData);
        }

        /// <summary>指定位置の画素を更新します</summary>
        public void WritePixel(in Pixel3ch pixels, int x, int y)
        {
            if (x > _width - 1 || y > _height - 1) return;
            var ptr = GetPixelPtr(x, y);
            UnsafeHelper.WriteStructureToPtr(ptr, pixels);
        }
        #endregion

        #region CutOut
        /// <summary>画像の一部を切り出した子画像を取得します</summary>
        public Pixel3Matrix CutOutPixelMatrix(int x, int y, int width, int height)
        {
            if (_width < x + width) throw new ArgumentException("vertical direction");
            if (_height < y + height) throw new ArgumentException("horizontal direction");
            return new Pixel3Matrix(width, height, _bytesPerData, _stride, GetPixelPtr(x, y));
        }
        #endregion

        #region CopyTo
        /// <summary>画素値をコピーします</summary>
        public void CopyTo(in Pixel3Matrix destPixels)
        {
            if (_width != destPixels._width || _height != destPixels._height) throw new ArgumentException("size is different.");
            if (_pointer == destPixels._pointer) throw new ArgumentException("same pointer.");

            CopyToInternal(this, destPixels);

            // 画素値のコピー（サイズチェックなし）
            static void CopyToInternal(in Pixel3Matrix srcPixels, in Pixel3Matrix destPixels)
            {
                // メモリが連続していれば memcopy
                if (srcPixels.IsContinuous && destPixels.IsContinuous)
                {
                    UnsafeHelper.MemCopy(destPixels._pointer, srcPixels._pointer, srcPixels.AllocatedSize);
                    return;
                }

                unsafe
                {
                    var (width, height, bytesPerPixel) = (srcPixels._width, srcPixels._height, srcPixels._bytesPerData);
                    var srcHeadPtr = (byte*)srcPixels._pointer;
                    var srcStride = srcPixels._stride;
                    var dstHeadPtr = (byte*)destPixels._pointer;
                    var dstStride = destPixels._stride;

                    for (int y = 0; y < height; y++)
                    {
                        var src = srcHeadPtr + (y * srcStride);
                        var dst = dstHeadPtr + (y * dstStride);

                        for (int x = 0; x < width * bytesPerPixel; x += bytesPerPixel)
                        {
                            *(Pixel3ch*)(dst + x) = *(Pixel3ch*)(src + x);
                        }
                    }
                }
            }
        }

        /// <summary>画素値を拡大コピーします</summary>
        public void CopyToWithScaleUp(in Pixel3Matrix destination)
        {
            if (_bytesPerData != 3 || destination._bytesPerData != 3)
                throw new ArgumentException("bytes/pixel error.");

            if (destination._width % _width != 0 || destination._height % _height != 0)
                throw new ArgumentException("must be an integral multiple.");

            var widthRatio = destination._width / _width;
            var heightRatio = destination._height / _height;
            if (widthRatio != heightRatio) throw new ArgumentException("magnifications are different.");

            var magnification = widthRatio;
            if (magnification <= 1) throw new ArgumentException("ratio must be greater than 1.");

            ScaleUp(this, destination, magnification);

            static unsafe void ScaleUp(in Pixel3Matrix source, in Pixel3Matrix destination, int magnification)
            {
                var bytesPerPixel = source._bytesPerData;
                var srcPixelHead = (byte*)source._pointer;
                var srcStride = source._stride;
                var srcWidth = source._width;
                var srcHeight = source._height;

                var destPixelHead = (byte*)destination._pointer;
                var destStride = destination._stride;

                for (int y = 0; y < srcHeight; y++)
                {
                    var src = srcPixelHead + (srcStride * y);
                    var dest0 = destPixelHead + (destStride * y * magnification);

                    for (int x = 0; x < srcWidth * bytesPerPixel; x += bytesPerPixel)
                    {
                        var pixel = (Pixel3ch*)(src + x);
                        var dest1 = dest0 + (x * magnification);

                        for (byte* dest2 = dest1; dest2 < dest1 + (destStride * magnification); dest2 += destStride)
                        {
                            for (byte* dest3 = dest2; dest3 < dest2 + (bytesPerPixel * magnification); dest3 += bytesPerPixel)
                                *((Pixel3ch*)dest3) = *pixel;
                        }
                    }
                }
            }
        }
        #endregion

        #region ToBmpFile
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
            await Task.Yield();

            using var ms = ToBitmapMemoryStream(savePath);
            using var fs = new FileStream(savePath, FileMode.Create);
            fs.Seek(0, SeekOrigin.Begin);

            await ms.CopyToAsync(fs, token);
        }

        /// <summary>画像をbmpファイルに保存します</summary>
        private MemoryStream ToBitmapMemoryStream(string savePath)
        {
            if (IsInvalid) throw new ArgumentException("Invalid image.");
            if (File.Exists(savePath)) throw new SystemException("File is exists.");

            var bitmapBytes = GetBitmapBinary(this);
            var ms = new MemoryStream(bitmapBytes);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;

            // Bitmapのバイナリ配列を取得します
            static byte[] GetBitmapBinary(in Pixel3Matrix pixel)
            {
                var height = pixel._height;
                var srcStride = pixel._stride;
                var destHeader = new BitmapHeader(pixel._width, height, pixel.BitsPerPixel);
                var destBuffer = new byte[destHeader.FileSize];     // さずがにデカすぎるのでbyte[]

                // bufferにheaderを書き込む
                UnsafeHelper.CopyStructToArray(destHeader, destBuffer);

                // 画素は左下から右上に向かって記録する
                unsafe
                {
                    var srcHead = (byte*)pixel._pointer;
                    fixed (byte* pointer = destBuffer)
                    {
                        var destHead = pointer + destHeader.OffsetBytes;
                        var destStride = destHeader.ImageStride;
                        Debug.Assert(srcStride <= destStride);

                        for (var y = 0; y < height; ++y)
                        {
                            var src = srcHead + (height - 1 - y) * srcStride;
                            var dest = destHead + (y * destStride);
                            UnsafeHelper.MemCopy(dest, src, srcStride);
                        }
                    }
                }
                return destBuffer;
            }
        }
        #endregion

    }
}
