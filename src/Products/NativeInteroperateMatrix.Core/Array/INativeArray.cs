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
    Span<T> AsReadOnlySpan<T>() where T : struct;

    NativeArray GetRearrangedArray(int bytesPerItem);
}
