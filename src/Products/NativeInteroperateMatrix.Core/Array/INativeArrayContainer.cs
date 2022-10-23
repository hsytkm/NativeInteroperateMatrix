namespace Nima;

public interface INativeArrayContainer
{
    IDisposable GetArrayForRead(out NativeArray array);
    IDisposable GetArrayForWrite(out NativeArray array);
}
