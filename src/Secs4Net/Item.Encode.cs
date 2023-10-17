using CommunityToolkit.HighPerformance;
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
        var lengthSpan = Unsafe.AsRef(count).AsBytes();

        var formatByte = (int)format << 2;
        if (count <= 0xff)
        {//	1 byte
            var span = buffer.GetSpan(sizeHint: 2);
            span.DangerousGetReferenceAt(0) = (byte)(formatByte | 1);
            span.DangerousGetReferenceAt(1) = lengthSpan.DangerousGetReferenceAt(0);
            buffer.Advance(2);
            return;
        }
        if (count <= 0xffff)
        {//	2 byte
            var span = buffer.GetSpan(sizeHint: 3);
            span.DangerousGetReferenceAt(0) = (byte)(formatByte | 2);
            span.DangerousGetReferenceAt(1) = lengthSpan.DangerousGetReferenceAt(1);
            span.DangerousGetReferenceAt(2) = lengthSpan.DangerousGetReferenceAt(0);
            buffer.Advance(3);
            return;
        }
        if (count <= 0xffffff)
        {//	3 byte
            var span = buffer.GetSpan(sizeHint: 4);
            span.DangerousGetReferenceAt(0) = (byte)(formatByte | 3);
            span.DangerousGetReferenceAt(1) = lengthSpan.DangerousGetReferenceAt(2);
            span.DangerousGetReferenceAt(2) = lengthSpan.DangerousGetReferenceAt(1);
            span.DangerousGetReferenceAt(3) = lengthSpan.DangerousGetReferenceAt(0);
            buffer.Advance(4);
            return;
        }

        ThrowHelper(count);

        [DoesNotReturn]
        static void ThrowHelper(int count) => throw new ArgumentOutOfRangeException(nameof(count), count, $@"Item length:{count} is overflow");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    private protected static void EncodeEmptyItem(SecsFormat format, IBufferWriter<byte> buffer)
    {
        var span = buffer.GetSpan(sizeHint: 2);
        span.DangerousGetReferenceAt(0) = (byte)(((int)format << 2) | 1);
        span.DangerousGetReferenceAt(1) = 0;
        buffer.Advance(2);
    }
}
