namespace Nima;

public interface IMatrixContainer : IDisposable
{
    IDisposable GetMatrixForRead(out NativeMatrix matrix);
    IDisposable GetMatrixForWrite(out NativeMatrix matrix);
}
