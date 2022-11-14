using System.Runtime.InteropServices;
using Nima;

#if NET7_0_OR_GREATER
[assembly: System.Runtime.CompilerServices.DisableRuntimeMarshalling]
#endif

namespace NativeInteroperateMatrix.Core.Tests.Natives;

#if NET7_0_OR_GREATER
// required unsafe option
internal static partial class NativeMatrixMethods
{
    const string DLL_NAME = "NativeInteroperateMatrix.Tests.Natives.Core.dll";

    // Byte
    [LibraryImport(DLL_NAME, EntryPoint = "SumByteMatrix")]
    public static partial long SumByte(in NativeMatrix matrix);
    [LibraryImport(DLL_NAME, EntryPoint = "ClearByteMatrix")]
    public static partial void ClearByte(in NativeMatrix matrix);

    // Int16
    [LibraryImport(DLL_NAME, EntryPoint = "SumInt16Matrix")]
    public static partial long SumInt16(in NativeMatrix matrix);
    [LibraryImport(DLL_NAME, EntryPoint = "ClearInt16Matrix")]
    public static partial void ClearInt16(in NativeMatrix matrix);

    // Int32
    [LibraryImport(DLL_NAME, EntryPoint = "SumInt32Matrix")]
    public static partial long SumInt32(in NativeMatrix matrix);
    [LibraryImport(DLL_NAME, EntryPoint = "ClearInt32Matrix")]
    public static partial void ClearInt32(in NativeMatrix matrix);

    // Int64
    [LibraryImport(DLL_NAME, EntryPoint = "SumInt64Matrix")]
    public static partial long SumInt64(in NativeMatrix matrix);
    [LibraryImport(DLL_NAME, EntryPoint = "ClearInt64Matrix")]
    public static partial void ClearInt64(in NativeMatrix matrix);

    // Single
    [LibraryImport(DLL_NAME, EntryPoint = "SumSingleMatrix")]
    public static partial double SumSingle(in NativeMatrix matrix);
    [LibraryImport(DLL_NAME, EntryPoint = "ClearSingleMatrix")]
    public static partial void ClearSingle(in NativeMatrix matrix);

    // Double
    [LibraryImport(DLL_NAME, EntryPoint = "SumDoubleMatrix")]
    public static partial double SumDouble(in NativeMatrix matrix);
    [LibraryImport(DLL_NAME, EntryPoint = "ClearDoubleMatrix")]
    public static partial void ClearDouble(in NativeMatrix matrix);
}
#else
internal static class NativeMatrixMethods
{
    const string DLL_NAME = "NativeInteroperateMatrix.Tests.Natives.Core.dll";

    // Byte
    [DllImport(DLL_NAME, EntryPoint = "SumByteMatrix")]
    public static extern long SumByte(in NativeMatrix matrix);
    [DllImport(DLL_NAME, EntryPoint = "ClearByteMatrix")]
    public static extern void ClearByte(in NativeMatrix matrix);

    // Int16
    [DllImport(DLL_NAME, EntryPoint = "SumInt16Matrix")]
    public static extern long SumInt16(in NativeMatrix matrix);
    [DllImport(DLL_NAME, EntryPoint = "ClearInt16Matrix")]
    public static extern void ClearInt16(in NativeMatrix matrix);

    // Int32
    [DllImport(DLL_NAME, EntryPoint = "SumInt32Matrix")]
    public static extern long SumInt32(in NativeMatrix matrix);
    [DllImport(DLL_NAME, EntryPoint = "ClearInt32Matrix")]
    public static extern void ClearInt32(in NativeMatrix matrix);

    // Int64
    [DllImport(DLL_NAME, EntryPoint = "SumInt64Matrix")]
    public static extern long SumInt64(in NativeMatrix matrix);
    [DllImport(DLL_NAME, EntryPoint = "ClearInt64Matrix")]
    public static extern void ClearInt64(in NativeMatrix matrix);

    // Single
    [DllImport(DLL_NAME, EntryPoint = "SumSingleMatrix")]
    public static extern double SumSingle(in NativeMatrix matrix);
    [DllImport(DLL_NAME, EntryPoint = "ClearSingleMatrix")]
    public static extern void ClearSingle(in NativeMatrix matrix);

    // Double
    [DllImport(DLL_NAME, EntryPoint = "SumDoubleMatrix")]
    public static extern double SumDouble(in NativeMatrix matrix);
    [DllImport(DLL_NAME, EntryPoint = "ClearDoubleMatrix")]
    public static extern void ClearDouble(in NativeMatrix matrix);
}
#endif
