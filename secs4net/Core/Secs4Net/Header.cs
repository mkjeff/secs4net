using System;
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

        internal unsafe byte[] EncodeTo(byte[] buffer)
        {
            EncodeTo(buffer.AsSpan());
            return buffer;
        }

        internal unsafe void EncodeTo(Span<byte> buffer)
        {
            // DeviceId
            BinaryPrimitives.WriteUInt16BigEndian(buffer, DeviceId);

            ref var head = ref MemoryMarshal.GetReference(buffer);
            // S, ReplyExpected
            Unsafe.WriteUnaligned(ref Unsafe.Add(ref head, 2), (byte)(S | (ReplyExpected ? 0b1000_0000 : 0)));

            // F
            Unsafe.WriteUnaligned(ref Unsafe.Add(ref head, 3), F);

            Unsafe.WriteUnaligned(ref Unsafe.Add(ref head, 4), 0);

            // MessageType
            Unsafe.WriteUnaligned(ref Unsafe.Add(ref head, 5), (byte)MessageType);

            // SystemBytes
            BinaryPrimitives.WriteInt32BigEndian(buffer.Slice(6), SystemBytes);
        }

        internal static unsafe MessageHeader Decode(in ReadOnlySpan<byte> buffer)
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