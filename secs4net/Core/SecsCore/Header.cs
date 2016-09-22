using System.Runtime.CompilerServices;

namespace Secs4Net
{
    unsafe struct MessageHeader
    {
        fixed byte _bytes[10];

        internal MessageHeader(byte[] data, int startIndex)
        {
            fixed (byte* ptr = _bytes)
                Unsafe.CopyBlock(ptr, Unsafe.AsPointer(ref data[startIndex]), 10);
        }

        internal byte[] Bytes
        {
            get
            {
                var result = new byte[10];
                fixed(byte* ptr = _bytes)
                    Unsafe.CopyBlock(Unsafe.AsPointer(ref result[0]), ptr, 10);
                return result;
            }
        }

        public unsafe ushort DeviceId
        {
            get
            {
                ushort result = 0;
                fixed (byte* src = _bytes)
                {
                    var ptr = (byte*)Unsafe.AsPointer(ref result);
                    // tmp variable is redundant if use C# 7.0 ref return + Unsafe.AsRef
                    var
                    tmp = Unsafe.Read<byte>(src + 1);
                    Unsafe.Copy(ptr + 0, ref tmp);
                    tmp = Unsafe.Read<byte>(src + 0);
                    Unsafe.Copy(ptr + 1, ref tmp);
                }
                return result;
            }
            set
            {
                var values = (byte*)Unsafe.AsPointer(ref value);

                fixed (byte* target = _bytes)
                {
                    var
                    tmp = Unsafe.Read<byte>(values + 0);
                    Unsafe.Copy(target + 1, ref tmp);
                    tmp = Unsafe.Read<byte>(values + 1);
                    Unsafe.Copy(target + 0, ref tmp);
                }
            }
        }

        public bool ReplyExpected
        {
            get {
                fixed (byte* ptr = _bytes)
                    return (Unsafe.Read<byte>(ptr+2) & 0x80) == 0x80;
            }
            set {
                fixed (byte* ptr = _bytes)
                    Unsafe.Write(ptr + 2, (byte)(S | (value ? 0x80 : 0)));
            }
        }

        public byte S
        {
            get {
                fixed (byte* ptr = _bytes)
                    return (byte)(Unsafe.Read<byte>(ptr+2) & 0x7F);
            }
            set {
                fixed (byte* ptr = _bytes)
                    Unsafe.Write(ptr+2, (byte)(value | (ReplyExpected ? 0x80 : 0)));
            }
        }

        public byte F
        {
            get {
                fixed (byte* ptr = _bytes)
                    return Unsafe.Read<byte>(ptr+3);
            }
            set {
                fixed (byte* ptr = _bytes)
                    Unsafe.Write(ptr + 3, value);
            }
        }

        public MessageType MessageType
        {
            get {
                fixed (byte* ptr = _bytes)
                    return (MessageType)Unsafe.Read<MessageType>(ptr + 5);
            }
            set {
                fixed (byte* ptr = _bytes)
                    Unsafe.Write(ptr + 5, value);
            }
        }

        public unsafe int SystemBytes
        {
            get
            {
                int result = 0;
                fixed (byte* src = _bytes)
                {
                    var ptr = (byte*)Unsafe.AsPointer(ref result);
                    // tmp variable is redundant if use C# 7.0 ref return + Unsafe.AsRef
                    // https://github.com/mkjeff/CS7Sample/blob/25f7d8719eb082e92055574e27d7dbf1e6731f27/Test/Program.cs#L77-L104
                    var
                    tmp = Unsafe.Read<byte>(src + 9);
                    Unsafe.Copy(ptr + 0, ref tmp);
                    tmp = Unsafe.Read<byte>(src + 8);
                    Unsafe.Copy(ptr + 1, ref tmp);
                    tmp = Unsafe.Read<byte>(src + 7);
                    Unsafe.Copy(ptr + 2, ref tmp);
                    tmp = Unsafe.Read<byte>(src + 6);
                    Unsafe.Copy(ptr + 3, ref tmp);
                }
                return result;
            }
            set
            {
                var values = (byte*)Unsafe.AsPointer(ref value);

                fixed (byte* target = _bytes)
                {
                    var
                    tmp = Unsafe.Read<byte>(values + 0);
                    Unsafe.Copy(target + 9, ref tmp);
                    tmp = Unsafe.Read<byte>(values + 1);
                    Unsafe.Copy(target + 8, ref tmp);
                    tmp = Unsafe.Read<byte>(values + 2);
                    Unsafe.Copy(target + 7, ref tmp);
                    tmp = Unsafe.Read<byte>(values + 3);
                    Unsafe.Copy(target + 6, ref tmp);
                }
            }
        }
    }
}
