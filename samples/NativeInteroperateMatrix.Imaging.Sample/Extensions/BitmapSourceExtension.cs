using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace NativeInteroperateMatrix.Imaging.Sample.Extensions
{
    static class BitmapSourceExtension
    {
        /// <summary>引数PATHのファイルを画像として読み出します</summary>
        public static BitmapImage FromFile(string imagePath)
        {
            if (!File.Exists(imagePath))
                throw new FileNotFoundException(imagePath);

            static BitmapImage ToBitmapImage(Stream stream)
            {
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CreateOptions = BitmapCreateOptions.None;
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = stream;
                bi.EndInit();
                bi.Freeze();

                if (bi.Width == 1 && bi.Height == 1) throw new OutOfMemoryException();
                return bi;
            }

            using var stream = File.Open(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return ToBitmapImage(stream);

            //return new BitmapImage(new Uri(imagePath));  これでも読めるがファイルがロックされる
        }
    }
}
