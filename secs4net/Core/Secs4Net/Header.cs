using Microsoft.Toolkit.HighPerformance;
using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Secs4Net
{
    public readonly struct MessageHeader
    {
        public readonly ushort DeviceId;
        public readonly bool ReplyExpected;
        public readonly byte S;
        public readonly byte F;
        public readonly MessageType MessageType;
        public readonly int SystemBytes;

        internal MessageHeader(
            ushort deviceId = default,
            bool replyExpected = default,
            byte s = default,
            byte f = default,
            MessageType messageType = default,
            int systemBytes = default)
        {
            DeviceId = deviceId;
            ReplyExpected = replyExpected;
            S = s;
            F = f;
            MessageType = messageType;
            SystemBytes = systemBytes;
        }

        internal void EncodeTo(IBufferWriter<byte> buffer)
        {
            var span = buffer.GetSpan(sizeHint: 10);
            BinaryPrimitives.WriteUInt16BigEndian(span, DeviceId);
            Unsafe.WriteUnaligned(ref span.DangerousGetReferenceAt(2), (byte)(S | (ReplyExpected ? 0b1000_0000 : 0)));
            Unsafe.WriteUnaligned(ref span.DangerousGetReferenceAt(3), F);
            Unsafe.WriteUnaligned(ref span.DangerousGetReferenceAt(4), (byte)0);
            Unsafe.WriteUnaligned(ref span.DangerousGetReferenceAt(5), (byte)MessageType);
            BinaryPrimitives.WriteInt32BigEndian(span.Slice(6), SystemBytes);
            buffer.Advance(10);
        }

        internal static MessageHeader Decode(ReadOnlySpan<byte> buffer)
        {
            ref var head = ref MemoryMarshal.GetReference(buffer);
            var s = Unsafe.ReadUnaligned<byte>(ref Unsafe.Add(ref head, 2));
            return new MessageHeader(
                deviceId: BinaryPrimitives.ReadUInt16BigEndian(buffer),
                replyExpected: (s & 0b1000_0000) != 0,
                s: (byte)(s & 0b0111_111),
                f: Unsafe.ReadUnaligned<byte>(ref Unsafe.Add(ref head, 3)),
                messageType: (MessageType)Unsafe.ReadUnaligned<byte>(ref Unsafe.Add(ref head, 5)),
                systemBytes: BinaryPrimitives.ReadInt32BigEndian(buffer.Slice(6))
            );
        }
    }
}
