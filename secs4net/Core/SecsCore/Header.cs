using System;
using System.Buffers;

namespace Secs4Net
{
    internal struct MessageHeader
    {
        readonly ArraySegment<byte> _bytes;

        internal MessageHeader(ArraySegment<byte> bytes)
        {
            _bytes = bytes;
        }

        public static implicit operator ArraySegment<byte>(MessageHeader header) => header._bytes;

        public ushort DeviceId
        {
            get
            {
                return BitConverter.ToUInt16(new[] { _bytes.Array[1], _bytes.Array[0] }, 0);
            }
            set
            {
                byte[] values = BitConverter.GetBytes(value);
                _bytes.Array[0] = values[1];
                _bytes.Array[1] = values[0];
            }
        }

        public bool ReplyExpected
        {
            get { return (_bytes.Array[2] & 0x80) == 0x80; }
            set { _bytes.Array[2] = (byte)(S | (value ? 0x80 : 0)); }
        }

        public byte S
        {
            get { return (byte)(_bytes.Array[2] & 0x7F); }
            set { _bytes.Array[2] = (byte)(value | (ReplyExpected ? 0x80 : 0)); }
        }

        public byte F
        {
            get { return _bytes.Array[3]; }
            set { _bytes.Array[3] = value; }
        }

        public MessageType MessageType
        {
            get { return (MessageType)_bytes.Array[5]; }
            set { _bytes.Array[5] = (byte)value; }
        }

        public int SystemBytes
        {
            get
            {
                return BitConverter.ToInt32(new[] {
                        _bytes.Array[9],
                        _bytes.Array[8],
                        _bytes.Array[7],
                        _bytes.Array[6]
                    }, 0);
            }
            set
            {
                byte[] values = BitConverter.GetBytes(value);
                _bytes.Array[6] = values[3];
                _bytes.Array[7] = values[2];
                _bytes.Array[8] = values[1];
                _bytes.Array[9] = values[0];
            }
        }
    }
}
