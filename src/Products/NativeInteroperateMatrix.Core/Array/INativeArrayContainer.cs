namespace Nima;

public interface INativeArrayContainer
{
    int Length { get; }

    IDisposable GetArrayForRead(out NativeArray array);
    IDisposable GetArrayForWrite(out NativeArray array);
}
