using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

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
            var items = Read(stream, rows, cols, stride, bytesPerPixel, headerSize);

            return new PixelBgrMatrixContainer(rows: header.Height, columns: header.Width, items);

            static IEnumerable<PixelBgr> Read(Stream stream, int rows, int cols, int stride, int bytesPerPixel, int headerOffset)
            {
                var rowLast = headerOffset + ((rows - 1) * stride);
                var rowFirst = headerOffset;
                var colLength = cols * bytesPerPixel;
                var array = new byte[colLength];

                for (var rowOffset = rowLast; rowOffset >= rowFirst; rowOffset -= stride)
                {
                    stream.Position = rowOffset;
                    stream.Read(array, 0, array.Length);

                    for (var c = 0; c < colLength; c += bytesPerPixel)
                    {
                        yield return new PixelBgr(array[c + 0], array[c + 1], array[c + 2]);
                    }
                }
            }
        }

    }
}
