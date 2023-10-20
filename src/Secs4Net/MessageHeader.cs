using CommunityToolkit.HighPerformance;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace Secs4Net;

public readonly record struct MessageHeader
{
    public int Id { get; init; }
    public ushort DeviceId { get; init; }
    public MessageType MessageType { get; init; }
    public byte S { get; init; }
    public byte F { get; init; }
    public bool ReplyExpected { get; init; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    internal void EncodeTo(IBufferWriter<byte> buffer)
    {
        var span = buffer.GetSpan(sizeHint: 10);
        BinaryPrimitives.WriteUInt16BigEndian(span, DeviceId);
        span.DangerousGetReferenceAt(2) = (byte)(S | (ReplyExpected ? 0b1000_0000 : 0));
        span.DangerousGetReferenceAt(3) = F;
        span.DangerousGetReferenceAt(4) = 0;
        span.DangerousGetReferenceAt(5) = (byte)MessageType;
        BinaryPrimitives.WriteInt32BigEndian(span[6..], Id);
        buffer.Advance(10);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    internal static void Decode(ReadOnlySpan<byte> buffer, out MessageHeader header)
    {
        var s = buffer[2];
        header = new MessageHeader
        {
            DeviceId = (ushort)(BinaryPrimitives.ReadUInt16BigEndian(buffer) & 0b01111111_11111111),
            ReplyExpected = (s & 0b1000_0000) != 0,
            S = (byte)(s & 0b0111_1111),
            F = buffer[3],
            MessageType = (MessageType)buffer[5],
            Id = BinaryPrimitives.ReadInt32BigEndian(buffer[6..]),
        };
    }
}
