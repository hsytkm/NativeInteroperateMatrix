namespace Nima;

public interface INativeMatrixContainer
{
    int Rows { get; }
    int Columns { get; }

    int Width { get; }
    int Height { get; }

    IDisposable GetMatrixForReading(out NativeMatrix matrix);
    IDisposable GetMatrixForWriting(out NativeMatrix matrix);
    NativeMatrix DangerousGetNativeMatrix();
}
