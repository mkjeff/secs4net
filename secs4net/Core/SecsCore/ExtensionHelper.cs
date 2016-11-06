using System;
using System.Text;
using System.Runtime.CompilerServices;
using System.Buffers;
using static Secs4Net.Item;

namespace Secs4Net
{
    public static class SecsExtension
    {
        internal static string GetName(this MessageType msgType)
        {
            switch (msgType)
            {
                case MessageType.DataMessage:
                    return nameof(MessageType.DataMessage);
                case MessageType.LinkTestRequest:
                    return nameof(MessageType.LinkTestRequest);
                case MessageType.LinkTestResponse:
                    return nameof(MessageType.LinkTestResponse);
                case MessageType.SelectRequest:
                    return nameof(MessageType.SelectRequest);
                case MessageType.SelectResponse:
                    return nameof(MessageType.SelectResponse);
                case MessageType.SeperateRequest:
                    return nameof(MessageType.SeperateRequest);
                default:
                    return string.Empty;
            }
        }

        public static string GetName(this SecsFormat format)
        {
            switch (format)
            {
                case SecsFormat.List:
                    return nameof(SecsFormat.List);
                case SecsFormat.ASCII:
                    return nameof(SecsFormat.ASCII);
                case SecsFormat.JIS8:
                    return nameof(SecsFormat.JIS8);
                case SecsFormat.Boolean:
                    return nameof(SecsFormat.Boolean);
                case SecsFormat.Binary:
                    return nameof(SecsFormat.Binary);
                case SecsFormat.U1:
                    return nameof(SecsFormat.U1);
                case SecsFormat.U2:
                    return nameof(SecsFormat.U2);
                case SecsFormat.U4:
                    return nameof(SecsFormat.U4);
                case SecsFormat.U8:
                    return nameof(SecsFormat.U8);
                case SecsFormat.I1:
                    return nameof(SecsFormat.I1);
                case SecsFormat.I2:
                    return nameof(SecsFormat.I2);
                case SecsFormat.I4:
                    return nameof(SecsFormat.I4);
                case SecsFormat.I8:
                    return nameof(SecsFormat.I8);
                case SecsFormat.F4:
                    return nameof(SecsFormat.F4);
                case SecsFormat.F8:
                    return nameof(SecsFormat.F8);
                default:
                    return string.Empty;
            }
        }

        public static string ToHexString(this byte[] value)
        {
            if (value.Length == 0)
                return string.Empty;
            int length = value.Length*3;
            char[] chs = ArrayPool<char>.Shared.Rent(length);
            for (int ci = 0, i = 0; ci < length; ci += 3)
            {
                byte num = value[i++];
                chs[ci] = GetHexValue(num/0x10);
                chs[ci + 1] = GetHexValue(num%0x10);
                chs[ci + 2] = ' ';
            }
            var result = new string(chs, 0, length - 1);
            ArrayPool<char>.Shared.Return(chs);
            return result;
        }

        static char GetHexValue(int i) => (i < 10) ? (char) (i + 0x30) : (char) ((i - 10) + 0x41);

        public static bool IsMatch(this SecsMessage src, SecsMessage target)
        {
            if (src.S != target.S)
                return false;
            if (src.F != target.F)
                return false;
            if (target.SecsItem == null)
                return true;
            return src.SecsItem.IsMatch(target.SecsItem);
        }

        internal static SecsItem BytesDecode(this SecsFormat format)
        {
            switch (format)
            {
                case SecsFormat.ASCII:
                    return A();
                case SecsFormat.JIS8:
                    return J();
                case SecsFormat.Boolean:
                    return Boolean();
                case SecsFormat.Binary:
                    return B();
                case SecsFormat.U1:
                    return U1();
                case SecsFormat.U2:
                    return U2();
                case SecsFormat.U4:
                    return U4();
                case SecsFormat.U8:
                    return U8();
                case SecsFormat.I1:
                    return I1();
                case SecsFormat.I2:
                    return I2();
                case SecsFormat.I4:
                    return I4();
                case SecsFormat.I8:
                    return I8();
                case SecsFormat.F4:
                    return F4();
                case SecsFormat.F8:
                    return F8();
            }
            throw new ArgumentException($"Invalid format: {format.GetName()}", nameof(format));
        }

        internal static readonly Encoding JIS8Encoding = Encoding.GetEncoding(50222);

        internal static SecsItem BytesDecode(this SecsFormat format, byte[] data, ref int index, ref int length)
        {
            switch (format)
            {
                case SecsFormat.ASCII:
                    return A(Encoding.ASCII.GetString(data, index, length));
                case SecsFormat.JIS8:
                    return J(JIS8Encoding.GetString(data, index, length));
                case SecsFormat.Boolean:
                    return Boolean(Decode<bool>(data, ref index, ref length));
                case SecsFormat.Binary:
                    return B(Decode<byte>(data, ref index, ref length));
                case SecsFormat.U1:
                    return U1(Decode<byte>(data, ref index, ref length));
                case SecsFormat.U2:
                    return U2(Decode<ushort>(data, ref index, ref length));
                case SecsFormat.U4:
                    return U4(Decode<uint>(data, ref index, ref length));
                case SecsFormat.U8:
                    return U8(Decode<ulong>(data, ref index, ref length));
                case SecsFormat.I1:
                    return I1(Decode<sbyte>(data, ref index, ref length));
                case SecsFormat.I2:
                    return I2(Decode<short>(data, ref index, ref length));
                case SecsFormat.I4:
                    return I4(Decode<int>(data, ref index, ref length));
                case SecsFormat.I8:
                    return I8(Decode<long>(data, ref index, ref length));
                case SecsFormat.F4:
                    return F4(Decode<float>(data, ref index, ref length));
                case SecsFormat.F8:
                    return F8(Decode<double>(data, ref index, ref length));
            }
            throw new ArgumentException("Invalid format", nameof(format));
        }

        private static ArraySegment<T> Decode<T>(byte[] data, ref int index, ref int length) where T : struct
        {
            int elmSize = Unsafe.SizeOf<T>();
            data.Reverse(index, index + length, elmSize);
            var arrLength = length/elmSize;
            var values = ArrayPool<T>.Shared.Rent(arrLength);
            Buffer.BlockCopy(data, index, values, 0, length);
            //unsafe
            //{
            //    Unsafe.CopyBlock(
            //        Unsafe.AsPointer(ref values[0]),
            //        Unsafe.AsPointer(ref data[0]),
            //        (uint) length
            //    );
            //}
            return new ArraySegment<T>(values, 0, arrLength);
        }

        internal static void Reverse(this byte[] bytes, int begin, int end, int offSet)
        {
            if (offSet <= 1)
                return;

            for (var i = begin; i < end; i += offSet)
                Array.Reverse(bytes, i, offSet);
        }
    }
}