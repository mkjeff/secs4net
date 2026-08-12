using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace Secs4Net;

[InlineArray(length: 10)]
public struct MessageHeader
{
    private byte firstByte;
    
    public ushort DeviceId
    {
        get => (ushort)(BinaryPrimitives.ReadUInt16BigEndian(this[..2]) & 0b01111111_11111111);
        init => BinaryPrimitives.WriteUInt16BigEndian(this[..2], value);
    }

    public byte S
    {
        get => (byte)(this[2] & 0b0111_1111);
        init => this[2] = (byte)(value | (ReplyExpected ? 0b1000_0000 : 0));
    }
    
    public bool ReplyExpected
    {
        get => (this[2] & 0b1000_0000) != 0; 
        init => this[2] |= (byte)(value? 0b1000_0000 :0);
    }

    public byte F
    {
        get => this[3];
        init => this[3] = value;
    }
    
    public MessageType MessageType
    {
        get => (MessageType)this[5];
        init => this[5] = (byte)value;
    }
    
    public int Id
    {
        get => BinaryPrimitives.ReadInt32BigEndian(this[6..]);
        init => BinaryPrimitives.WriteInt32BigEndian(this[6..], value);
    }
}
