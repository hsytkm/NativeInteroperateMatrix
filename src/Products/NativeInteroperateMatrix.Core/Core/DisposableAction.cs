namespace Nima;

internal sealed class DisposableAction : IDisposable
{
    bool _disposed;
    readonly Action _action;
    public DisposableAction(Action action) => _action = action;
    public void Dispose()
    {
        if (_disposed)
            return;

        _action.Invoke();
        _disposed = true;
    }
}
