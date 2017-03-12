using System.Runtime.CompilerServices;

namespace Secs4Net
{
    internal struct MessageHeader
    {
        public ushort DeviceId;
        public bool ReplyExpected;
        public byte S;
        public byte F;
        public MessageType MessageType;
        public int SystemBytes;

        internal unsafe byte[] EncodeTo(byte[] buffer)
        {
            byte* target = (byte*)Unsafe.AsPointer(ref buffer[0]);

            // DeviceId
            var values = (byte*)Unsafe.AsPointer(ref DeviceId);
            Unsafe.Copy(target + 1, ref Unsafe.AsRef<byte>(values));
            Unsafe.Copy(target + 0, ref Unsafe.AsRef<byte>(values + 1));

            // S, ReplyExpected
            Unsafe.Write(target + 2, (byte)(S | (ReplyExpected ? 0b1000_0000 : 0)));

            // F
            Unsafe.Write(target + 3, F);

            Unsafe.Write(target + 4, 0);

            // MessageType
            Unsafe.Write(target + 5, (byte)MessageType);

            // SystemBytes
            values = (byte*)Unsafe.AsPointer(ref SystemBytes);
            Unsafe.Copy(target + 9, ref Unsafe.AsRef<byte>(values));
            Unsafe.Copy(target + 8, ref Unsafe.AsRef<byte>(values + 1));
            Unsafe.Copy(target + 7, ref Unsafe.AsRef<byte>(values + 2));
            Unsafe.Copy(target + 6, ref Unsafe.AsRef<byte>(values + 3));

            return buffer;
        }

        internal static unsafe MessageHeader Decode(byte[] buffer, int startIndex)
        {
            byte* src = (byte*) Unsafe.AsPointer(ref buffer[startIndex]);

            // DeviceId
            ushort deviceId = 0;

            var ptr = (byte*) Unsafe.AsPointer(ref deviceId);
            Unsafe.Copy(ptr + 0, ref Unsafe.AsRef<byte>(src + 1));
            Unsafe.Copy(ptr + 1, ref Unsafe.AsRef<byte>(src));

            // SystemBytes
            int systemBytes = 0;
            ptr = (byte*) Unsafe.AsPointer(ref systemBytes);
            Unsafe.Copy(ptr + 0, ref Unsafe.AsRef<byte>(src + 9));
            Unsafe.Copy(ptr + 1, ref Unsafe.AsRef<byte>(src + 8));
            Unsafe.Copy(ptr + 2, ref Unsafe.AsRef<byte>(src + 7));
            Unsafe.Copy(ptr + 3, ref Unsafe.AsRef<byte>(src + 6));

            return new MessageHeader
            {
                DeviceId = deviceId,
                ReplyExpected = (buffer[startIndex + 2] & 0b1000_0000) != 0,
                S = (byte)(buffer[startIndex + 2] & 0b0111_111),
                F = buffer[startIndex + 3],
                MessageType = (MessageType)buffer[startIndex + 5],
                SystemBytes = systemBytes
            };
        }
    }
}
