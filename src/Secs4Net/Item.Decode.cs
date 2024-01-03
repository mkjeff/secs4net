using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Secs4Net;

public partial class Item
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void DecodeFormatAndLengthByteCount(in ReadOnlySequence<byte> sourceBytes, out SecsFormat format, out byte lengthByteCount)
    {
#if NET
        var formatSeqFirstSpan = sourceBytes.FirstSpan;
#else
        var formatSeqFirstSpan = sourceBytes.First.Span;
#endif

#if DEBUG
        byte formatAndLengthByte = formatSeqFirstSpan[0];
#else
        byte formatAndLengthByte = formatSeqFirstSpan.DangerousGetReferenceAt(0);
#endif
        format = (SecsFormat)(formatAndLengthByte >> 2);
        lengthByteCount = (byte)(formatAndLengthByte & 0b00000011);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    internal static int DecodeDataLength(in ReadOnlySequence<byte> sourceBytes)
    {
        var dataLength = 0;
        var lengthBytes = dataLength.AsBytes();
        sourceBytes.CopyTo(lengthBytes);
        lengthBytes[..(int)sourceBytes.Length].Reverse();
        return dataLength;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    [SkipLocalsInit]
    public static Item DecodeFromFullBuffer(ref ReadOnlySequence<byte> bytes)
    {
        var formatSeq = bytes.Slice(0, 1);
        DecodeFormatAndLengthByteCount(formatSeq, out var format, out var lengthByteCount);

        var dataLengthSeq = bytes.Slice(formatSeq.End, lengthByteCount);
        var dataLength = DecodeDataLength(dataLengthSeq);
        bytes = bytes.Slice(dataLengthSeq.End);

        if (format == SecsFormat.List)
        {
            if (dataLength == 0)
            {
                return L();
            }

            var items = new Item[dataLength];
            foreach (ref var subItem in items.AsSpan())
            {
                subItem = DecodeFromFullBuffer(ref bytes);
            }

            return L(items);
        }

        var dataItemBytes = bytes.Slice(0, dataLength);
        var item = DecodeDataItem(format, dataItemBytes);
        bytes = bytes.Slice(dataItemBytes.End);
        return item;
    }

    internal static Item DecodeDataItem(SecsFormat format, in ReadOnlySequence<byte> bytes)
    {
        var length = (int)bytes.Length;
        return (format, length) switch
        {
            (SecsFormat.ASCII, 0) => A(),
            (SecsFormat.ASCII, >= 512) => DecodeLazyStringItem(format, length, bytes),
            (SecsFormat.ASCII, _) => DecodeStringItem(format, length, bytes, Encoding.ASCII),

            (SecsFormat.JIS8, 0) => J(),
            (SecsFormat.JIS8, >= 512) => DecodeLazyStringItem(format, length, bytes),
            (SecsFormat.JIS8, _) => DecodeStringItem(format, length, bytes, JIS8Encoding),

            (SecsFormat.Binary, 0) => B(),
            (SecsFormat.Binary, >= 1024) => DecodeMemoryOwnerItem<byte>(SecsFormat.Binary, length, bytes),
            (SecsFormat.Binary, _) => DecodeMemoryItem<byte>(SecsFormat.Binary, length, bytes),

            (SecsFormat.Boolean, 0) => Boolean(),
            (SecsFormat.Boolean, >= 1024) => DecodeMemoryOwnerItem<bool>(SecsFormat.Boolean, length, bytes),
            (SecsFormat.Boolean, _) => DecodeMemoryItem<bool>(SecsFormat.Boolean, length, bytes),

            (SecsFormat.I8, 0) => I8(),
            (SecsFormat.I8, >= 1024) => DecodeMemoryOwnerItem<long>(SecsFormat.I8, length, bytes),
            (SecsFormat.I8, _) => DecodeMemoryItem<long>(SecsFormat.I8, length, bytes),

            (SecsFormat.I1, 0) => I1(),
            (SecsFormat.I1, >= 1024) => DecodeMemoryOwnerItem<sbyte>(SecsFormat.I1, length, bytes),
            (SecsFormat.I1, _) => DecodeMemoryItem<sbyte>(SecsFormat.I1, length, bytes),

            (SecsFormat.I2, 0) => I2(),
            (SecsFormat.I2, >= 1024) => DecodeMemoryOwnerItem<short>(SecsFormat.I2, length, bytes),
            (SecsFormat.I2, _) => DecodeMemoryItem<short>(SecsFormat.I2, length, bytes),

            (SecsFormat.I4, 0) => I4(),
            (SecsFormat.I4, >= 1024) => DecodeMemoryOwnerItem<int>(SecsFormat.I4, length, bytes),
            (SecsFormat.I4, _) => DecodeMemoryItem<int>(SecsFormat.I4, length, bytes),

            (SecsFormat.F8, 0) => F8(),
            (SecsFormat.F8, >= 1024) => DecodeMemoryOwnerItem<double>(SecsFormat.F8, length, bytes),
            (SecsFormat.F8, _) => DecodeMemoryItem<double>(SecsFormat.F8, length, bytes),

            (SecsFormat.F4, 0) => F4(),
            (SecsFormat.F4, >= 1024) => DecodeMemoryOwnerItem<float>(SecsFormat.F4, length, bytes),
            (SecsFormat.F4, _) => DecodeMemoryItem<float>(SecsFormat.F4, length, bytes),

            (SecsFormat.U8, 0) => U8(),
            (SecsFormat.U8, >= 1024) => DecodeMemoryOwnerItem<ulong>(SecsFormat.U8, length, bytes),
            (SecsFormat.U8, _) => DecodeMemoryItem<ulong>(SecsFormat.U8, length, bytes),

            (SecsFormat.U1, 0) => U1(),
            (SecsFormat.U1, > 1024) => DecodeMemoryOwnerItem<byte>(SecsFormat.U1, length, bytes),
            (SecsFormat.U1, _) => DecodeMemoryItem<byte>(SecsFormat.U1, length, bytes),

            (SecsFormat.U2, 0) => U2(),
            (SecsFormat.U2, >= 1024) => DecodeMemoryOwnerItem<ushort>(SecsFormat.U2, length, bytes),
            (SecsFormat.U2, _) => DecodeMemoryItem<ushort>(SecsFormat.U2, length, bytes),

            (SecsFormat.U4, 0) => U4(),
            (SecsFormat.U4, >= 1024) => DecodeMemoryOwnerItem<uint>(SecsFormat.U4, length, bytes),
            (SecsFormat.U4, _) => DecodeMemoryItem<uint>(SecsFormat.U4, length, bytes),
            _ => ThrowHelper(),
        };

        [SkipLocalsInit]
        static Item DecodeStringItem(SecsFormat format, int length, in ReadOnlySequence<byte> bytes, Encoding encoding)
        {
            using var spanOwner = SpanOwner<byte>.Allocate(length);
            bytes.CopyTo(spanOwner.Span);
            return new StringItem(format, StringPool.Shared.GetOrAdd(spanOwner.Span, encoding));
        }

        [SkipLocalsInit]
        static Item DecodeLazyStringItem(SecsFormat format, int length, in ReadOnlySequence<byte> bytes)
        {
            var owner = MemoryOwner<byte>.Allocate(length);
            bytes.CopyTo(owner.Memory.Span);
            return new LazyStringItem(format, owner);
        }

        [SkipLocalsInit]
        static unsafe Item DecodeMemoryItem<T>(SecsFormat format, int length, in ReadOnlySequence<byte> bytes) where T : unmanaged, IEquatable<T>
        {
            var memory = new T[length / sizeof(T)];
            var span = memory.AsSpan();
            bytes.CopyTo(span.AsBytes());
            ReverseEndiannessHelper<T>.Reverse(span);
            return new MemoryItem<T>(format, memory);
        }

        [SkipLocalsInit]
        static unsafe Item DecodeMemoryOwnerItem<T>(SecsFormat format, int length, in ReadOnlySequence<byte> bytes) where T : unmanaged, IEquatable<T>
        {
            var owner = MemoryOwner<T>.Allocate(length / sizeof(T));
            var span = owner.Span;
            bytes.CopyTo(span.AsBytes());
            ReverseEndiannessHelper<T>.Reverse(span);
            return new MemoryOwnerItem<T>(format, owner);
        }

        [DoesNotReturn]
        static Item ThrowHelper() => throw new ArgumentOutOfRangeException();
    }
}
