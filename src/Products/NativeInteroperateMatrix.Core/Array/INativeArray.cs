namespace Nima;

public interface INativeArray : INativeMemory
{
    /// <summary>
    /// Maximum number of length
    /// </summary>
    int Length { get; }

    /// <summary>
    /// Return the raw Span
    /// </summary>
    Span<T> AsSpan<T>() where T : struct;
    ReadOnlySpan<T> AsReadOnlySpan<T>() where T : struct;

    T GetValue<T>(int index) where T : struct;

    NativeArray GetRearrangedArray(int bytesPerItem);
}
