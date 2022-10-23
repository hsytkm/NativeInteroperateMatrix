namespace Nima;

public interface INativeArray : INativeMemory
{
    /// <summary>
    /// Maximum number of length
    /// </summary>
    int Length { get; }

    Span<T> AsSpan<T>() where T : struct;
    Span<T> AsReadOnlySpan<T>() where T : struct;

    NativeArray GetRearrangedArray(int bytesPerItem);
}
