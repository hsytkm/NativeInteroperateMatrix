namespace Nima.Imaging;

public interface IPixelBgr24MatrixContainer : INativeMatrixContainer
{
    /// <summary>
    /// 指定と同じ画像のコンテナを新規に作成します
    /// </summary>
    PixelBgr24MatrixContainer Clone();

    /// <summary>
    /// 確保済メモリを変更せずに小さい画像サイズに変更します
    /// </summary>
    PixelBgr24MatrixContainer Rearrange(int newRows, int newColumns, int newStride);

    /// <summary>
    /// 引数コンテナと比較して、コンテナを再利用できるかを判定します
    /// </summary>
    bool CanReuseContainer(IPixelBgr24MatrixContainer sourceContainer);

    /// <summary>
    /// 引数コンテナの内容をコピーします(コンテナを再作成しません)
    /// </summary>
    void CopyFrom(IPixelBgr24MatrixContainer sourceContainer);

    /// <summary>
    /// 指定領域における各チャンネルの画素平均値を取得します
    /// </summary>
    ColorBgr GetChannelsAverage(int x, int y, int width, int height);

    /// <summary>
    /// 画面全体における各チャンネルの画素平均値を取得します
    /// </summary>
    ColorBgr GetChannelsAverageOfEntire();

    /// <summary>
    /// 指定領域の画素を塗りつぶします
    /// </summary>
    void FillRectangle(PixelBgr24 pixel, int x, int y, int width, int height);

    /// <summary>
    /// 指定枠を描画します
    /// </summary>
    void DrawRectangle(PixelBgr24 pixel, int x, int y, int width, int height);

    /// <summary>
    /// 指定色で塗り潰します
    /// </summary>
    void FillAllPixels(PixelBgr24 pixel);

    /// <summary>
    /// 画像をbmpファイルに保存します
    /// </summary>
    void ToBmpFile(string savePath);

    /// <summary>
    /// 画像をbmpファイルに保存します
    /// </summary>
    Task ToBmpFileAsync(string savePath, CancellationToken token);
}
