using Microsoft.Toolkit.HighPerformance;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Secs4Net
{
    partial class Item
    {
        /// <summary>
        /// Encode Item header
        /// </summary>
        /// <param name="count">List item count or value bytes length</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EncodeItemHeader(SecsFormat format, int count, IBufferWriter<byte> buffer)
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

            static void ThrowHelper(int count) => throw new ArgumentOutOfRangeException(nameof(count), count, $@"Item data length:{count} is overflow");
        }

        private static void EncodeList(Item item, IBufferWriter<byte> buffer)
        {
            var list = Unsafe.As<IReadOnlyList<Item>>(item._values);
            EncodeItemHeader(item.Format, list.Count, buffer);

            for (int i = 0; i < list.Count; i++)
            {
                list[i].EncodeTo(buffer);
            }
        }

        //private static void EncodeArray<T>(Item item, IBufferWriter<byte> buffer) where T : unmanaged
        //{
        //    var arr = Unsafe.As<T[]>(item._values);
        //    var byteLength = Unsafe.SizeOf<T>() * arr.Length;
        //    EncodeItemHeader(item.Format, byteLength, buffer);
        //    var span = buffer.GetSpan(sizeHint: byteLength);
        //    arr.AsSpan().AsBytes().CopyTo(span);
        //    span.Reverse(Unsafe.SizeOf<T>());
        //    buffer.Advance(byteLength);
        //}

        private static void EncodeArray(Item item, IBufferWriter<byte> buffer)
        {
            var arr = Unsafe.As<Array>(item._values);
            var byteLength = Buffer.ByteLength(arr);
            EncodeItemHeader(item.Format, byteLength, buffer);
            var span = buffer.GetSpan(sizeHint: byteLength).Slice(0, byteLength);
            MemoryMarshal.CreateReadOnlySpan(ref MemoryMarshal.GetArrayDataReference(arr), byteLength).CopyTo(span);
            span.Reverse(byteLength / arr.Length);
            buffer.Advance(byteLength);
        }

        private static void EncodeString(Item item, IBufferWriter<byte> buffer)
        {
            var str = Unsafe.As<string>(item._values);
            var encoder = item.Format == SecsFormat.ASCII ? Encoding.ASCII : Jis8Encoding;
            var bytelength = encoder.GetByteCount(str);
            EncodeItemHeader(item.Format, bytelength, buffer);
            var span = buffer.GetSpan(sizeHint: bytelength).Slice(0, bytelength);
            buffer.Advance(encoder.GetBytes(str, span));
        }

        private static void EncodeEmptyItem(Item item, IBufferWriter<byte> buffer)
        {
            var span = buffer.GetSpan(sizeHint: 2);
            span[0] = (byte)((byte)item.Format | 1);
            span[1] = 0;
            buffer.Advance(2);
        }
    }
}
