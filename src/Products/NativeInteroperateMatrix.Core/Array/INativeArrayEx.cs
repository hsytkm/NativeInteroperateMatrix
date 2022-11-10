namespace Nima;

internal static class INativeArrayEx
{
    internal static bool IsValid<TArray>(TArray array)
        where TArray : struct, INativeArray
    {
        if (array.Pointer == IntPtr.Zero) return false;
        if (array.Length <= 0) return false;
        if (array.BytesPerItem <= 0) return false;
        if (array.AllocateSize < array.Length * array.BytesPerItem) return false;
        return true;    //valid

    }

    internal static unsafe Span<TType> AsSpan<TArray, TType>(TArray array)
        where TArray : struct, INativeArray
        where TType : struct
    {
        return new(array.Pointer.ToPointer(), array.Length);
    }

    public static TType GetValue<TArray, TType>(TArray array, int index)
        where TArray : struct, INativeArray
        where TType : struct
    {
        IntPtr ptr = array.Pointer + index * array.BytesPerItem;
        return Marshal.PtrToStructure<TType>(ptr);
    }

}
