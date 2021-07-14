using Nima.Imaging.Drawing;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Nima.Core.Tests.Imaging.Drawing
{
    public class ImagingDrawingTest
    {
        private const string _tempPath = "_temp.bmp";
        public ImagingDrawingTest()
        {
            if (File.Exists(_tempPath))
                File.Delete(_tempPath);
        }

        [Theory]
        [ClassData(typeof(ImagePathTestData))]
        public async Task ToContainer(string sourcePath)
        {
            using var drawing = new Bitmap(sourcePath);
            using var container = drawing.ToPixelBgrMatrixContainer();

            await container.Matrix.ToBmpFileAsync(_tempPath);
            var isMatch = await FileComparator.IsMatchAsync(sourcePath, _tempPath);
            isMatch.IsTrue();

            File.Delete(_tempPath);
        }

    }
}
