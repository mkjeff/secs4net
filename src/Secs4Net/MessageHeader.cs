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
        ref var r0 = ref MemoryMarshal.GetReference(span);
        Unsafe.Add(ref r0, 2u) = (byte)(S | (ReplyExpected ? 0b1000_0000 : 0));
        Unsafe.Add(ref r0, 3u) = F;
        Unsafe.Add(ref r0, 4u) = 0;
        Unsafe.Add(ref r0, 5u) = (byte)MessageType;
        BinaryPrimitives.WriteInt32BigEndian(span[6..], Id);
        buffer.Advance(10);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    internal static void Decode(ReadOnlySpan<byte> span, out MessageHeader header)
    {
        ref var r0 = ref MemoryMarshal.GetReference(span);
        var s = Unsafe.Add(ref r0, 2u);
        header = new MessageHeader
        {
            DeviceId = (ushort)(BinaryPrimitives.ReadUInt16BigEndian(span) & 0b01111111_11111111),
            ReplyExpected = (s & 0b1000_0000) != 0,
            S = (byte)(s & 0b0111_1111),
            F = Unsafe.Add(ref r0, 3u),
            MessageType = (MessageType)Unsafe.Add(ref r0, 5u),
            Id = BinaryPrimitives.ReadInt32BigEndian(span[6..]),
        };
    }
}
