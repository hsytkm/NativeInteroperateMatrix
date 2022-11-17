namespace Nima;

public interface INativeArrayContainer
{
    int Length { get; }

    IDisposable GetArrayForReading(out NativeArray array);
    IDisposable GetArrayForWriting(out NativeArray array);

    NativeArray DangerousGetNativeArray();
}
