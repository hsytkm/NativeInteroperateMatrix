using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Nima.Core.Imaging
{
    public partial class PixelBgrMatrixContainer
    {
        /// <summary>Bitmapファイルから PixelBgrMatrixContainer を生成します</summary>
        /// <param name="filePath">File path of Bitmap</param>
        /// <returns></returns>
        public static PixelBgrMatrixContainer Create(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Create(fileStream);
        }

        /// <summary>Bitmap の Stream から PixelBgrMatrixContainer を生成します</summary>
        /// <param name="stream">Stream of Bitmap</param>
        /// <returns></returns>
        public static PixelBgrMatrixContainer Create(Stream stream)
        {
            var header = UnsafeHelper.ReadStruct<BitmapHeader>(stream);
            if (!header.CanRead)
                throw new InvalidDataException("Invalid bitmap format.");

            var rows = header.Height;
            var cols = header.Width;
            var stride = header.ImageStride;
            var bytesPerPixel = header.BytesPerPixel;
            var headerSize = Marshal.SizeOf<BitmapHeader>();
            var colLength = cols * bytesPerPixel;

            var container = new PixelBgrMatrixContainer(rows, cols, false);
            var destPtr = container.Matrix.Pointer;
            var array = new byte[colLength];

            for (var row = 0; row < rows; row++)
            {
                stream.Position = headerSize + ((rows - 1 - row) * stride);
                stream.Read(array);

                unsafe
                {
                    fixed (byte* srcPtr = array)
                    {
                        UnsafeHelper.MemCopy(destPtr + (row * colLength), srcPtr, colLength);
                    }
                }
            }
            return container;
        }

        /// <summary>Bitmapファイルから PixelBgrMatrixContainer を生成します</summary>
        /// <param name="filePath">File path of Bitmap</param>
        /// <returns></returns>
        public static async Task<PixelBgrMatrixContainer> CreateAsync(string filePath, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return await CreateAsync(stream, cancellationToken);
        }

        /// <summary>Bitmap の Stream から PixelBgrMatrixContainer を生成します</summary>
        /// <param name="stream">Stream of Bitmap</param>
        /// <returns></returns>
        public static async Task<PixelBgrMatrixContainer> CreateAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            var header = await UnsafeHelper.ReadStructAsync<BitmapHeader>(stream, cancellationToken);
            if (!header.CanRead)
                throw new InvalidDataException("Invalid bitmap format.");

            var rows = header.Height;
            var cols = header.Width;
            var stride = header.ImageStride;
            var bytesPerPixel = header.BytesPerPixel;
            var headerSize = Marshal.SizeOf<BitmapHeader>();
            var colLength = cols * bytesPerPixel;

            var container = new PixelBgrMatrixContainer(rows, cols, false);
            var destPtr = container.Matrix.Pointer;
            var array = new byte[colLength];

            for (var row = 0; row < rows; row++)
            {
                stream.Position = headerSize + ((rows - 1 - row) * stride);
                await stream.ReadAsync(array, cancellationToken);

                unsafe
                {
                    fixed (byte* srcPtr = array)
                    {
                        UnsafeHelper.MemCopy(destPtr + (row * colLength), srcPtr, colLength);
                    }
                }
            }
            return container;
        }

    }
}
