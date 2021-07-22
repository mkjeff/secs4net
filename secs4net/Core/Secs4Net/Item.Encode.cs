using Microsoft.Toolkit.HighPerformance;
using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Secs4Net
{
    partial class Item
    {
        /// <summary>
        /// Encode Item header
        /// </summary>
        /// <param name="count">List item count or value bytes length</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private protected static void EncodeItemHeader(SecsFormat format, int count, IBufferWriter<byte> buffer)
        {
            var lengthSpan = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<int, byte>(ref count), sizeof(int));
            if (count <= 0xff)
            {//	1 byte
                var span = buffer.GetSpan(sizeHint: 2);
                span.DangerousGetReferenceAt(0) = (byte)((byte)format | 1);
                span.DangerousGetReferenceAt(1) = lengthSpan.DangerousGetReferenceAt(0);
                buffer.Advance(2);
                return;
            }
            if (count <= 0xffff)
            {//	2 byte
                var span = buffer.GetSpan(sizeHint: 3);
                span.DangerousGetReferenceAt(0) = (byte)((byte)format | 2);
                span.DangerousGetReferenceAt(1) = lengthSpan.DangerousGetReferenceAt(1);
                span.DangerousGetReferenceAt(2) = lengthSpan.DangerousGetReferenceAt(0);
                buffer.Advance(3);
                return;
            }
            if (count <= 0xffffff)
            {//	3 byte
                var span = buffer.GetSpan(sizeHint: 4);
                span.DangerousGetReferenceAt(0) = (byte)((byte)format | 3);
                span.DangerousGetReferenceAt(1) = lengthSpan.DangerousGetReferenceAt(2);
                span.DangerousGetReferenceAt(2) = lengthSpan.DangerousGetReferenceAt(1);
                span.DangerousGetReferenceAt(3) = lengthSpan.DangerousGetReferenceAt(0);
                buffer.Advance(4);
                return;
            }

            ThrowHelper(count);

            [DoesNotReturn]
            static void ThrowHelper(int count) => throw new ArgumentOutOfRangeException(nameof(count), count, $@"Item data length:{count} is overflow");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private protected static void EncodeEmptyItem(SecsFormat format, IBufferWriter<byte> buffer)
        {
            var span = buffer.GetSpan(sizeHint: 2);
            span.DangerousGetReferenceAt(0) = (byte)((byte)format | 1);
            span.DangerousGetReferenceAt(1) = 0;
            buffer.Advance(2);
        }
    }
}
