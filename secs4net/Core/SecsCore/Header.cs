using System.Runtime.CompilerServices;

namespace Secs4Net
{
    public struct MessageHeader
    {
        public ushort DeviceId;
        public bool ReplyExpected;
        public byte S;
        public byte F;
        internal MessageType MessageType;
        public int SystemBytes;

        internal unsafe byte[] EncodeTo(byte[] buffer)
        {
            byte* target = (byte*)Unsafe.AsPointer(ref buffer[0]);

            // DeviceId
            var values = (byte*)Unsafe.AsPointer(ref DeviceId);

            var
            tmp = Unsafe.Read<byte>(values + 0);
            Unsafe.Copy(target + 1, ref tmp);
            tmp = Unsafe.Read<byte>(values + 1);
            Unsafe.Copy(target + 0, ref tmp);

            // ReplyExpected
            buffer[2] = (byte)(S | (ReplyExpected ? 0x80 : 0));

            // S
            buffer[2] = (byte)(S | (ReplyExpected ? 0x80 : 0));

            // F
            buffer[3] = F;

            buffer[4] = 0;

            // MessageType
            buffer[5] = (byte)MessageType;

            // SystemBytes
            values = (byte*)Unsafe.AsPointer(ref SystemBytes);

            
            tmp = Unsafe.Read<byte>(values + 0);
            Unsafe.Copy(target + 9, ref tmp);
            tmp = Unsafe.Read<byte>(values + 1);
            Unsafe.Copy(target + 8, ref tmp);
            tmp = Unsafe.Read<byte>(values + 2);
            Unsafe.Copy(target + 7, ref tmp);
            tmp = Unsafe.Read<byte>(values + 3);
            Unsafe.Copy(target + 6, ref tmp);

            return buffer;
        }

        internal static unsafe MessageHeader Decode(byte[] buffer, int startIndex)
        {
            byte* src = (byte*) Unsafe.AsPointer(ref buffer[startIndex]);

            // DeviceId
            ushort deviceId = 0;

            var ptr = (byte*) Unsafe.AsPointer(ref deviceId);
            // tmp variable is redundant if use C# 7.0 ref return + Unsafe.AsRef
            var
                tmp = Unsafe.Read<byte>(src + 1);
            Unsafe.Copy(ptr + 0, ref tmp);
            tmp = Unsafe.Read<byte>(src + 0);
            Unsafe.Copy(ptr + 1, ref tmp);

            // SystemBytes
            int systemBytes = 0;
            ptr = (byte*) Unsafe.AsPointer(ref systemBytes);
            // tmp variable is redundant if use C# 7.0 ref return + Unsafe.AsRef
            // https://github.com/mkjeff/CS7Sample/blob/25f7d8719eb082e92055574e27d7dbf1e6731f27/Test/Program.cs#L77-L104

            tmp = Unsafe.Read<byte>(src + 9);
            Unsafe.Copy(ptr + 0, ref tmp);
            tmp = Unsafe.Read<byte>(src + 8);
            Unsafe.Copy(ptr + 1, ref tmp);
            tmp = Unsafe.Read<byte>(src + 7);
            Unsafe.Copy(ptr + 2, ref tmp);
            tmp = Unsafe.Read<byte>(src + 6);
            Unsafe.Copy(ptr + 3, ref tmp);

            return new MessageHeader
                   {
                       DeviceId = deviceId,
                       ReplyExpected = (buffer[startIndex + 2] & 0x80) == 0x80,
                       S = (byte) (buffer[startIndex + 2] & 0x7F),
                       F = buffer[startIndex + 3],
                       MessageType = (MessageType) buffer[startIndex + 5],
                       SystemBytes = systemBytes
                   };
        }
    }
}
