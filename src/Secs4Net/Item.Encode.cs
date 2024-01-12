using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Secs4Net;

public partial class Item
{
    /// <summary>
    /// Encode Item header
    /// </summary>
    /// <param name="count">List item count or value bytes length</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    private static void EncodeItemHeader(SecsFormat format, int count, IBufferWriter<byte> buffer)
    {
        ref var lengthRef0 = ref Unsafe.As<int, byte>(ref Unsafe.AsRef(in count));
        ref var encodedRef0 = ref MemoryMarshal.GetReference(buffer.GetSpan(sizeHint: 4));
        var formatByte = (int)format << 2;
        if (count <= 0xff)
        {//	1 byte

            encodedRef0 = (byte)(formatByte | 1);
            Unsafe.Add(ref encodedRef0, 1u) = lengthRef0;
            buffer.Advance(2);
            return;
        }
        if (count <= 0xff_ff)
        {//	2 byte
            encodedRef0 = (byte)(formatByte | 2);
            Unsafe.Add(ref encodedRef0, 1u) = Unsafe.Add(ref lengthRef0, 1u);
            Unsafe.Add(ref encodedRef0, 2u) = lengthRef0;
            buffer.Advance(3);
            return;
        }
        if (count <= 0xff_ff_ff)
        {//	3 byte
            encodedRef0 = (byte)(formatByte | 3);
            Unsafe.Add(ref encodedRef0, 1u) = Unsafe.Add(ref lengthRef0, 2u);
            Unsafe.Add(ref encodedRef0, 2u) = Unsafe.Add(ref lengthRef0, 1u);
            Unsafe.Add(ref encodedRef0, 3u) = lengthRef0;
            buffer.Advance(4);
            return;
        }

        ThrowHelper(count);

        [DoesNotReturn]
        static void ThrowHelper(int count) => throw new ArgumentOutOfRangeException(nameof(count), count, $@"Item length:{count} is overflow");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    private static void EncodeEmptyItem(SecsFormat format, IBufferWriter<byte> buffer)
    {
        var span = buffer.GetSpan(sizeHint: 2);
        ref var r0 = ref MemoryMarshal.GetReference(span);
        Unsafe.Add(ref r0, 0u) = (byte)(((int)format << 2) | 1);
        Unsafe.Add(ref r0, 1u) = 0;
        buffer.Advance(2);
    }
}
