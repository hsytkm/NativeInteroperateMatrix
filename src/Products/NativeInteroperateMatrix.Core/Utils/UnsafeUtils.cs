﻿using System.Buffers;
using System.Runtime.CompilerServices;

namespace Nima;

public static class UnsafeUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void MemCopy(IntPtr dest, IntPtr src, int length)
        => MemCopyInternal(dest, src, length);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void MemCopy(void* dest, void* src, int length)
        => MemCopyInternal(new IntPtr(dest), new IntPtr(src), length);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void MemCopy(IntPtr dest, void* src, int length)
        => MemCopyInternal(dest, new IntPtr(src), length);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void MemCopy(void* dest, IntPtr src, int length)
        => MemCopyInternal(new IntPtr(dest), src, length);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static unsafe void MemCopyInternal(IntPtr dest, IntPtr src, int length)
    {
#if true
        Unsafe.CopyBlockUnaligned(dest.ToPointer(), src.ToPointer(), (uint)length);
#else
        var destPtr = (byte*)dest;
        var srcPtr = (byte*)src;
        var tail = destPtr + length;

        while (destPtr + 7 < tail)
        {
            *(ulong*)destPtr = *(ulong*)srcPtr;
            srcPtr += 8;
            destPtr += 8;
        }

        if (destPtr + 3 < tail)
        {
            *(uint*)destPtr = *(uint*)srcPtr;
            srcPtr += 4;
            destPtr += 4;
        }

        while (destPtr < tail)
        {
            *destPtr = *srcPtr;
            ++srcPtr;
            ++destPtr;
        }
#endif
    }

    /// <summary>構造体を byte[] に書き出します</summary>
    public static unsafe void CopyStructToArray<T>(T srcData, Span<byte> destArray)
        where T : unmanaged
    {
        // unsafe is faster than Marshal.Copy and GCHandle.
        // https://gist.github.com/hsytkm/55b9bdfaa3eae18fcc1b91449cf16998

        var size = Unsafe.SizeOf<T>();
        if (size > destArray.Length)
            throw new ArgumentOutOfRangeException(nameof(destArray));

        fixed (byte* p = destArray)
        {
            *(T*)p = srcData;
        }
    }

    /// <summary>構造体を IntPtr から読み出します</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe T ReadStructureFromPtr<T>(IntPtr src)
        where T : unmanaged
    {
        return ReadStructureFromPtr<T>(src.ToPointer());
    }

    /// <summary>構造体を pointer から読み出します</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe T ReadStructureFromPtr<T>(void* src)
        where T : unmanaged
    {
        return *(T*)src;
    }

    /// <summary>構造体を IntPtr に書き出します</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WriteStructureToPtr<T>(IntPtr dest, in T data)
        where T : unmanaged
    {
        WriteStructureToPtr(dest.ToPointer(), data);
    }

    /// <summary>構造体を pointer に書き出します</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WriteStructureToPtr<T>(void* dest, in T data)
        where T : unmanaged
    {
        *(T*)dest = data;
    }

    /// <summary>構造体を IntPtr に書き出します</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void FillZero(IntPtr dest, int size)
        => FillZero(dest.ToPointer(), size);

    /// <summary>構造体を pointer に書き出します</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void FillZero(void* dest, int size)
        => Unsafe.InitBlock(dest, 0, (uint)size);

    /// <summary>Stream の内容を構造体として読込んで返します</summary>
    public static T ReadStruct<T>(Stream stream)
        where T : struct
    {
        var size = Unsafe.SizeOf<T>();
        Span<byte> span = stackalloc byte[size];

        stream.Position = 0;
        stream.Read(span);

        var data = default(T);
        unsafe
        {
            fixed (byte* p = span)
            {
                Unsafe.Copy(ref data, p);
            }
        }
        return data;
    }

    /// <summary>Stream の内容を構造体として読込んで返します</summary>
    public static async Task<T> ReadStructAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        where T : struct
    {
        var size = Unsafe.SizeOf<T>();
        var array = ArrayPool<byte>.Shared.Rent(size);   // stackalloc cannot be used in Task.
        var memory = array.AsMemory()[0..size];
        try
        {
            stream.Position = 0;
            _ = await stream.ReadAsync(memory, cancellationToken);

            var data = default(T);
            unsafe
            {
                fixed (byte* p = array)
                {
                    Unsafe.Copy(ref data, p);
                }
            }
            return data;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(array);
        }
    }

}