namespace Nima;

public interface IMatrixContainer : IDisposable
{
    int Rows { get; }
    int Columns { get; }

    int Width { get; }
    int Height { get; }

    IDisposable GetMatrixForRead(out NativeMatrix matrix);
    IDisposable GetMatrixForWrite(out NativeMatrix matrix);
}
