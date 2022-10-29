namespace Nima.Imaging;

public interface IPixelBgr24MatrixContainer : IMatrixContainer
{
    /// <summary>
    /// 確保済メモリを変更せずに小さい画像サイズに変更します
    /// </summary>
    PixelBgr24MatrixContainer ShrinkToNewMatrix(int newRows, int newColumns, int newBytesPerPixel, int newStride);

    /// <summary>
    /// 引数コンテナと比較して、コンテナを再利用できるかを判定します
    /// </summary>
    bool CanReuseContainer(IPixelBgr24MatrixContainer sourceContainer);

    /// <summary>
    /// 引数コンテナの内容をコピーします(コンテナを再作成しません)
    /// </summary>
    void CopyFrom(IPixelBgr24MatrixContainer sourceContainer);

}
