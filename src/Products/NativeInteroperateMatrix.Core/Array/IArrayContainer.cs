namespace Nima;

public interface IArrayContainer
{
    IDisposable GetArrayForRead(out NativeArray array);
    IDisposable GetArrayForWrite(out NativeArray array);
}
