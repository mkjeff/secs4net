using CommunityToolkit.HighPerformance;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Secs4Net.Extensions;

public static class ReverseEndiannessHelper<T> where T : unmanaged
{
    public static readonly unsafe delegate*<Span<T>, void> Reverse;

    static unsafe ReverseEndiannessHelper()
    {
        var t = typeof(T);
        if (t == typeof(ushort))
        {
            Reverse = (delegate*<Span<T>, void>)(delegate*<Span<ushort>, void>)&ReverseHelper.ReverseUInt16;
        }
        else if (t == typeof(uint))
        {
            Reverse = (delegate*<Span<T>, void>)(delegate*<Span<uint>, void>)&ReverseHelper.ReverseUInt32;
        }
        else if (t == typeof(ulong))
        {
            Reverse = (delegate*<Span<T>, void>)(delegate*<Span<ulong>, void>)&ReverseHelper.ReverseUInt64;
        }
        else if (t == typeof(short))
        {
            Reverse = (delegate*<Span<T>, void>)(delegate*<Span<short>, void>)&ReverseHelper.ReverseInt16;
        }
        else if (t == typeof(int))
        {
            Reverse = (delegate*<Span<T>, void>)(delegate*<Span<int>, void>)&ReverseHelper.ReverseInt32;
        }
        else if (t == typeof(long))
        {
            Reverse = (delegate*<Span<T>, void>)(delegate*<Span<long>, void>)&ReverseHelper.ReverseInt64;
        }
        else if (t == typeof(float))
        {
            Reverse = (delegate*<Span<T>, void>)(delegate*<Span<float>, void>)&ReverseHelper.ReverseSingle;
        }
        else if (t == typeof(double))
        {
            Reverse = (delegate*<Span<T>, void>)(delegate*<Span<double>, void>)&ReverseHelper.ReverseDouble;
        }
        else
        {
            Reverse = &ReverseNothing;
        }

        static void ReverseNothing(Span<T> bytes) { }
    }
}

public static class ReverseHelper
{
    private const nuint SingleOffset = 1;

    public static void ReverseUInt16(Span<ushort> span) => span.ReverseEndianness();
    public static void ReverseUInt32(Span<uint> span) => span.ReverseEndianness();
    public static void ReverseUInt64(Span<ulong> span) => span.ReverseEndianness();
    public static void ReverseInt16(Span<short> span) => span.ReverseEndianness();
    public static void ReverseInt32(Span<int> span) => span.ReverseEndianness();
    public static void ReverseInt64(Span<long> span) => span.ReverseEndianness();
    public static void ReverseSingle(Span<float> span) => span.ReverseEndianness();
    public static void ReverseDouble(Span<double> span) => span.ReverseEndianness();


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    public static void ReverseEndianness(this Span<short> span)
    {
        ref var rStart = ref span.DangerousGetReferenceAt(0);
        ref var rEnd = ref span.DangerousGetReferenceAt(span.Length);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
            rStart = BinaryPrimitives.ReverseEndianness(rStart);
#if NET6_0
            rStart = ref Unsafe.Add(ref rStart, SingleOffset);
#else
            rStart = ref Unsafe.Add(ref rStart, 1);
#endif
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    public static void ReverseEndianness(this Span<ushort> span)
    {
        ref var rStart = ref span.DangerousGetReferenceAt(0);
        ref var rEnd = ref span.DangerousGetReferenceAt(span.Length);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
            rStart = BinaryPrimitives.ReverseEndianness(rStart);
#if NET6_0
            rStart = ref Unsafe.Add(ref rStart, SingleOffset);
#else
            rStart = ref Unsafe.Add(ref rStart, 1);
#endif
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    public static void ReverseEndianness(this Span<int> span)
    {
        ref var rStart = ref span.DangerousGetReferenceAt(0);
        ref var rEnd = ref span.DangerousGetReferenceAt(span.Length);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
            rStart = BinaryPrimitives.ReverseEndianness(rStart);
#if NET6_0
            rStart = ref Unsafe.Add(ref rStart, SingleOffset);
#else
            rStart = ref Unsafe.Add(ref rStart, 1);
#endif
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    public static void ReverseEndianness(this Span<uint> span)
    {
        ref var rStart = ref span.DangerousGetReferenceAt(0);
        ref var rEnd = ref span.DangerousGetReferenceAt(span.Length);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
            rStart = BinaryPrimitives.ReverseEndianness(rStart);
#if NET6_0
            rStart = ref Unsafe.Add(ref rStart, SingleOffset);
#else
            rStart = ref Unsafe.Add(ref rStart, 1);
#endif
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    public static void ReverseEndianness(this Span<long> span)
    {
        ref var rStart = ref span.DangerousGetReferenceAt(0);
        ref var rEnd = ref span.DangerousGetReferenceAt(span.Length);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
            rStart = BinaryPrimitives.ReverseEndianness(rStart);
#if NET6_0
            rStart = ref Unsafe.Add(ref rStart, SingleOffset);
#else
            rStart = ref Unsafe.Add(ref rStart, 1);
#endif
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    public static void ReverseEndianness(this Span<ulong> span)
    {
        ref var rStart = ref span.DangerousGetReferenceAt(0);
        ref var rEnd = ref span.DangerousGetReferenceAt(span.Length);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
            rStart = BinaryPrimitives.ReverseEndianness(rStart);
#if NET6_0
            rStart = ref Unsafe.Add(ref rStart, SingleOffset);
#else
            rStart = ref Unsafe.Add(ref rStart, 1);
#endif
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    public static void ReverseEndianness(this Span<float> span)
    {
        ref var rStart = ref span.DangerousGetReferenceAt(0);
        ref var rEnd = ref span.DangerousGetReferenceAt(span.Length);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
#if NET
            rStart = BinaryPrimitives.ReadSingleBigEndian(rStart.AsBytes());
#else
            rStart = ReadSingleBigEndian(rStart.AsBytes());
#endif
            rStart = ref Unsafe.Add(ref rStart, 1);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    public static void ReverseEndianness(this Span<double> span)
    {
        ref var rStart = ref span.DangerousGetReferenceAt(0);
        ref var rEnd = ref span.DangerousGetReferenceAt(span.Length);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
#if NET
            rStart = BinaryPrimitives.ReadDoubleBigEndian(rStart.AsBytes());
#else
            rStart = ReadDoubleBigEndian(rStart.AsBytes());
#endif
#if NET6_0
            rStart = ref Unsafe.Add(ref rStart, SingleOffset);
#else
            rStart = ref Unsafe.Add(ref rStart, 1);
#endif
        }
    }

#if NETSTANDARD
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float ReadSingleBigEndian(ReadOnlySpan<byte> source)
        => Int32BitsToSingle(BinaryPrimitives.ReverseEndianness(MemoryMarshal.Read<int>(source)));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe static float Int32BitsToSingle(int value)
        => *(float*)&value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double ReadDoubleBigEndian(ReadOnlySpan<byte> source)
        => BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(MemoryMarshal.Read<long>(source)));
#endif

}
