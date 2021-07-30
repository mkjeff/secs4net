﻿using Microsoft.Toolkit.HighPerformance;
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
        public readonly int Id;

        internal MessageHeader(
            ushort deviceId,
            bool replyExpected = default,
            byte s = default,
            byte f = default,
            MessageType messageType = default,
            int id = default)
        {
            DeviceId = deviceId;
            ReplyExpected = replyExpected;
            S = s;
            F = f;
            MessageType = messageType;
            Id = id;
        }

        public override string ToString()
        {
            return $"DeviceId={DeviceId}, S={S}, ReplyExpected={ReplyExpected}, F={F}, MessageType={MessageType}, Id={Id:X8}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void EncodeTo(IBufferWriter<byte> buffer)
        {
            var span = buffer.GetSpan(sizeHint: 10);
            BinaryPrimitives.WriteUInt16BigEndian(span, DeviceId);
            span.DangerousGetReferenceAt(2) = (byte)(S | (ReplyExpected ? 0b1000_0000 : 0));
            span.DangerousGetReferenceAt(3) = F;
            span.DangerousGetReferenceAt(4) = (byte)0;
            span.DangerousGetReferenceAt(5) = (byte)MessageType;
            BinaryPrimitives.WriteInt32BigEndian(span.Slice(6), Id);
            buffer.Advance(10);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
                id: BinaryPrimitives.ReadInt32BigEndian(buffer.Slice(6))
            );
        }
    }
}
