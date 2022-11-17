using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

        using var pixelContainer = bitmapImage.ToPixelBgr24MatrixContainer();
    
        // 元画像の画素値平均
        var channelAverage1 = pixelContainer.GetChannelsAverageOfEntire();

        Debug.WriteLine($"{channelAverage1:f1}");

        // 1. 三角領域を指定色で指定塗り
        FillTriangle(pixelContainer);

        // 2. 四角形（塗りつぶしなし）を描画
        pixelContainer.DrawRectangle(Colors.Cyan.ToPixelBgr(), 200, 200, 100, 200);

#if false   //♪未実装です
        var fullPixelMatrix = pixelContainer.Matrix;

        // 3. 上部を切り出して指定塗り
        var headerPixelMatrix = fullPixelMatrix.CutOutPixelMatrix(0, 0, fullPixelMatrix.Width, 30);
        headerPixelMatrix.FillAllPixels(PixelBgr24Color.Gray);
        var headerChannelAverage2 = headerPixelMatrix.GetChannelsAverageOfEntire();

        // 4. 上部を除いた左部を切り出してグレスケ塗り
        var leftPixelMatrix = fullPixelMatrix.CutOutPixelMatrix(0, headerPixelMatrix.Height, 50, fullPixelMatrix.Height - headerPixelMatrix.Height);
        FillGrayScaleVertical(leftPixelMatrix);
#endif

        // BitmapSourceに変換してView表示
        var writableBitmap = pixelContainer.ToWriteableBitmap();
        WriteableImage = new ReactivePropertySlim<WriteableBitmap>(initialValue: writableBitmap);
    }

    // 三角領域を単色で塗り(WriteValueのテスト)
    static void FillTriangle(PixelBgr24MatrixContainer container)
    {
        using var token = container.GetMatrixForReading(out var pixelMatrix);

        int baseX = 100, baseY = 200, height = 100;
        var color = PixelBgr24.FromBgr(0, 0xff, 0);
        for (int y = 0; y < height; y++)
        {
            var rowSpan = pixelMatrix.AsRowSpan<PixelBgr24>(baseY + y);
            for (int x = baseX; x < baseX + y; x++)
            {
                rowSpan[x] = color;  // ホントは FillRectangle() を使うべきだけど、Span のテストなので。
            }
        }
    }

    // 垂直方向で階調が変化するグレー塗り
    static void FillGrayScaleVertical(PixelBgr24MatrixContainer container)
    {
        using var token = container.GetMatrixForReading(out var pixelMatrix);

        const int range = 256;
        var length = pixelMatrix.Height / range;

        if (length > 0)
        {
            for (int lv = 0; lv < range; ++lv)
            {
                var color = PixelBgr24.FromGray((byte)(lv & 0xff));
                container.FillRectangle(color, 0, lv * length, pixelMatrix.Width, length);
            }
        }

        var filledHeight = length * range;
        container.FillRectangle(PixelBgr24Color.Black, 0, filledHeight, pixelMatrix.Width, pixelMatrix.Height - filledHeight);
    }

}
