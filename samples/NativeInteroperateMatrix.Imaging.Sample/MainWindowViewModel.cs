using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Nima.Core.Imaging;
using Nima.Imaging.Sample.Extensions;
using Nima.Imaging.Wpf;
using Prism.Mvvm;
using Reactive.Bindings;

namespace Nima.Imaging.Sample;

class MainWindowViewModel : BindableBase
{
    public IReadOnlyReactiveProperty<BitmapSource> SourceImage { get; }
    public IReactiveProperty<WriteableBitmap> WriteableImage { get; }

    public MainWindowViewModel()
    {
        var bitmapImage = BitmapSourceExtension.FromFile(@"Asserts\image1.bmp");
        SourceImage = new ReactivePropertySlim<BitmapSource>(initialValue: bitmapImage);

        using var pixelContainer = bitmapImage.ToPixelBgrMatrixContainer();
        var fullPixelMatrix = pixelContainer.Matrix;

        // 元画像の画素値平均
        var channelAverage1 = fullPixelMatrix.GetChannelsAverageOfEntire();

        Debug.WriteLine($"{channelAverage1:f1}");

        // 1. 三角領域を指定色で指定塗り
        FillTriangle(fullPixelMatrix);

        // 2. 四角形（塗りつぶしなし）を描画
        fullPixelMatrix.DrawRectangle(Colors.Cyan.ToPixelBgr(), 200, 200, 100, 200);

        // 3. 上部を切り出して指定塗り
        var headerPixelMatrix = fullPixelMatrix.CutOutPixelMatrix(0, 0, fullPixelMatrix.Columns, 30);
        headerPixelMatrix.FillAllPixels(PixelBgrs.Gray);
        var headerChannelAverage2 = headerPixelMatrix.GetChannelsAverageOfEntire();

        // 4. 上部を除いた左部を切り出してグレスケ塗り
        var leftPixelMatrix = fullPixelMatrix.CutOutPixelMatrix(0, headerPixelMatrix.Rows, 50, fullPixelMatrix.Rows - headerPixelMatrix.Rows);
        FillGrayScaleVertical(leftPixelMatrix);

        // BitmapSourceに変換してView表示
        var writableBitmap = fullPixelMatrix.ToWriteableBitmap();
        WriteableImage = new ReactivePropertySlim<WriteableBitmap>(initialValue: writableBitmap);
    }

    // 三角領域を単色で塗り(WriteValueのテスト)
    static void FillTriangle(in PixelBgrMatrix pixelMatrix)
    {
        int baseX = 100, baseY = 200, height = 100;
        var color = new PixelBgr(0, 0xff, 0);

        for (int y = 0; y < height; y++)
        {
            for (int x = baseX; x < baseX + y; x++)
                pixelMatrix.WriteValue(x, baseY + y, color);    // ホントは FillRectangle() を使うべきだけど、WriteValue() のテストなので。
        }
    }

    // 垂直方向で階調が変化するグレー塗り
    static void FillGrayScaleVertical(in PixelBgrMatrix pixelMatrix)
    {
        const int range = 256;
        var length = pixelMatrix.Rows / range;

        if (length > 0)
        {
            for (int lv = 0; lv < range; ++lv)
            {
                var color = new PixelBgr((byte)(lv & 0xff));
                pixelMatrix.FillRectangle(color, 0, lv * length, pixelMatrix.Columns, length);
            }
        }

        var filledHeight = length * range;
        pixelMatrix.FillRectangle(PixelBgrs.Black, 0, filledHeight, pixelMatrix.Columns, pixelMatrix.Rows - filledHeight);
    }

}
