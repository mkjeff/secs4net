using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Secs4Net
{
    public sealed class Item
    {
        /// <summary>
        /// if Format is List RawData is only header bytes.
        /// otherwise include header and value bytes.
        /// </summary>
        private readonly Lazy<byte[]> _rawData;

        private readonly IEnumerable _values;

        /// <summary>
        /// List
        /// </summary>
        private Item(IReadOnlyList<Item> items)
        {
            if (items.Count > byte.MaxValue)
                throw new ArgumentOutOfRangeException("List length out of range, max length: 255");

            Format = SecsFormat.List;
            _values = items;
            _rawData = new Lazy<byte[]>(()
                => new byte[]{
                    (byte)SecsFormat.List | 1,
                    unchecked((byte)(Unsafe.As<IReadOnlyList<Item>>(_values).Count))
                });
        }

        /// <summary>
        /// U1, U2, U4, U8
        /// I1, I2, I4, I8
        /// F4, F8
        /// Boolean,
        /// Binary
        /// </summary>
        private Item(SecsFormat format, Array value)
        {
            Format = format;
            _values = value;
            _rawData = new Lazy<byte[]>(() =>
            {
                var arr = Unsafe.As<Array>(_values);
                int bytelength = Buffer.ByteLength(arr);
                int headerLength;
                byte[] result = EncodeItem(bytelength, out headerLength);
                Buffer.BlockCopy(arr, 0, result, headerLength, bytelength);
                result.Reverse(headerLength, headerLength + bytelength, bytelength / arr.Length);
                return result;
            });
        }

        /// <summary>
        /// A,J
        /// </summary>
        private Item(SecsFormat format, string value)
        {
            Format = format;
            _values = value;
            _rawData = new Lazy<byte[]>(() =>
            {
                var str = Unsafe.As<string>(_values);
                int bytelength = str.Length;
                int headerLength;
                byte[] result = EncodeItem(bytelength, out headerLength);
                var encoder = Format == SecsFormat.ASCII ? Encoding.ASCII : JIS8Encoding;
                encoder.GetBytes(str, 0, str.Length, result, headerLength);
                return result;
            });
        }

        /// <summary>
        /// Empty Item(none List)
        /// </summary>
        /// <param name="format"></param>
        /// <param name="value"></param>
        private Item(SecsFormat format, IEnumerable value)
        {
            Format = format;
            _values = value;
            _rawData = new Lazy<byte[]>(() => new byte[] { (byte)((byte)Format | 1), 0 });
        }

        public SecsFormat Format { get; }

        public int Count =>
            Format == SecsFormat.List
            ? Unsafe.As<IReadOnlyList<Item>>(_values).Count
            : Unsafe.As<Array>(_values).Length;

        public IReadOnlyList<byte> RawBytes => _rawData.Value;

        /// <summary>
        /// Non-list item values
        /// </summary>
        public IEnumerable Values => Format == SecsFormat.List
            ? throw new InvalidOperationException("Item is a list")
            : _values;

        /// <summary>
        /// List items
        /// </summary>
        public IReadOnlyList<Item> Items => Format != SecsFormat.List
            ? throw new InvalidOperationException("Item is not a list")
            : Unsafe.As<IReadOnlyList<Item>>(_values);

        /// <summary>
        /// get value by specific type
        /// </summary>
        /// <typeparam name="T">return value type</typeparam>
        /// <returns></returns>
        public T GetValue<T>()
        {
            if (Format == SecsFormat.List)
                throw new InvalidOperationException("Item is list");

            if (_values is T v)
                return v;

            if (_values is IEnumerable<T> arr)
                return arr.First();

            throw new InvalidOperationException("Item value type is incompatible");
        }

        public bool IsMatch(Item target)
        {
            if (Format != target.Format) return false;
            if (target.Count == 0) return true;
            if (Count != target.Count) return false;

            switch (target.Format)
            {
                case SecsFormat.List:
                    return IsMatch(
                        Unsafe.As<IReadOnlyList<Item>>(_values),
                        Unsafe.As<IReadOnlyList<Item>>(target._values));
                case SecsFormat.ASCII:
                case SecsFormat.JIS8:
                    return Unsafe.As<string>(_values) == Unsafe.As<string>(target._values);
                default:
                    //return memcmp(Unsafe.As<byte[]>(_values), Unsafe.As<byte[]>(target._values), Buffer.ByteLength((Array)_values)) == 0;
                    return UnsafeCompare(Unsafe.As<Array>(_values), Unsafe.As<Array>(target._values));
            }

            //[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
            //static extern int memcmp(byte[] b1, byte[] b2, long count);
            // http://stackoverflow.com/questions/43289/comparing-two-byte-arrays-in-net/8808245#8808245
            unsafe bool UnsafeCompare(Array a1, Array a2)
            {
                int length = Buffer.ByteLength(a2);
                fixed (byte* p1 = Unsafe.As<byte[]>(a1), p2 = Unsafe.As<byte[]>(a2))
                {
                    byte* x1 = p1, x2 = p2;
                    int l = length;
                    for (int i = 0; i < l / 8; i++, x1 += 8, x2 += 8)
                        if (*((long*)x1) != *((long*)x2)) return false;
                    if ((l & 4) != 0) { if (*((int*)x1) != *((int*)x2)) return false; x1 += 4; x2 += 4; }
                    if ((l & 2) != 0) { if (*((short*)x1) != *((short*)x2)) return false; x1 += 2; x2 += 2; }
                    if ((l & 1) != 0) if (*((byte*)x1) != *((byte*)x2)) return false;
                    return true;
                }
            }

            bool IsMatch(IReadOnlyList<Item> a, IReadOnlyList<Item> b)
            {
                for (var i = 0; i < a.Count; i++)
                    if (!a[i].IsMatch(b[i]))
                        return false;
                return true;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder(Format.GetName()).Append("[");
            switch (Format)
            {
                case SecsFormat.List:
                    sb.Append(Unsafe.As<IReadOnlyList<Item>>(_values).Count).Append("]");
                    break;
                case SecsFormat.ASCII:
                case SecsFormat.JIS8:
                    sb.Append(Unsafe.As<string>(_values).Length).Append("]: ").Append(Unsafe.As<string>(_values));
                    break;
                case SecsFormat.Binary:
                    sb.Append(Unsafe.As<byte[]>(_values).Length).Append("]: ").Append(Unsafe.As<byte[]>(_values).ToHexString());
                    break;
                default:
                    sb.Append(Unsafe.As<Array>(_values).Length).Append("]: ");
                    switch (Format)
                    {
                        case SecsFormat.Boolean: sb.Append(JoinAsString<bool>(_values)); break;
                        case SecsFormat.I1: sb.Append(JoinAsString<sbyte>(_values)); break;
                        case SecsFormat.I2: sb.Append(JoinAsString<short>(_values)); break;
                        case SecsFormat.I4: sb.Append(JoinAsString<int>(_values)); break;
                        case SecsFormat.I8: sb.Append(JoinAsString<long>(_values)); break;
                        case SecsFormat.U1: sb.Append(JoinAsString<byte>(_values)); break;
                        case SecsFormat.U2: sb.Append(JoinAsString<ushort>(_values)); break;
                        case SecsFormat.U4: sb.Append(JoinAsString<uint>(_values)); break;
                        case SecsFormat.U8: sb.Append(JoinAsString<ulong>(_values)); break;
                        case SecsFormat.F4: sb.Append(JoinAsString<float>(_values)); break;
                        case SecsFormat.F8: sb.Append(JoinAsString<double>(_values)); break;
                    }
                    sb.Append(" ...");
                    break;
            }
            return sb.ToString();

            string JoinAsString<T>(IEnumerable src)
                where T : struct => string.Join(" ", Unsafe.As<T[]>(src).Take(10));
        }

        #region Type Casting Operator
        public static implicit operator string(Item item) => item.GetValue<string>();
        public static implicit operator byte(Item item) => item.GetValue<byte>();
        public static implicit operator sbyte(Item item) => item.GetValue<sbyte>();
        public static implicit operator ushort(Item item) => item.GetValue<ushort>();
        public static implicit operator short(Item item) => item.GetValue<short>();
        public static implicit operator uint(Item item) => item.GetValue<uint>();
        public static implicit operator int(Item item) => item.GetValue<int>();
        public static implicit operator ulong(Item item) => item.GetValue<ulong>();
        public static implicit operator long(Item item) => item.GetValue<long>();
        public static implicit operator float(Item item) => item.GetValue<float>();
        public static implicit operator double(Item item) => item.GetValue<double>();
        public static implicit operator bool(Item item) => item.GetValue<bool>();

        #endregion

        #region Factory Methods
        internal static Item L(IList<Item> items) => new Item(new ReadOnlyCollection<Item>(items));
        /// <summary>
        /// Create list
        /// </summary>
        /// <param name="items">sub item</param>
        /// <returns></returns>
        public static Item L(IEnumerable<Item> items) => items.Any() ? L(items.ToList()) : L();

        /// <summary>
        /// Create list
        /// </summary>
        /// <param name="items">sub item</param>
        /// <returns></returns>
        public static Item L(params Item[] items) => L((IList<Item>)items);

        /// <summary>
        /// Create binary item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item B(params byte[] value) => new Item(SecsFormat.Binary, value);

        /// <summary>
        /// Create binary item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item B(IEnumerable<byte> value) => value.Any() ? B(value.ToArray()) : B();

        /// <summary>
        /// Create unsigned integer item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item U1(params byte[] value) => new Item(SecsFormat.U1, value);

        /// <summary>
        /// Create unsigned integer item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item U1(IEnumerable<byte> value) => value.Any() ? U1(value.ToArray()) : U1();

        /// <summary>
        /// Create unsigned integer item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item U2(params ushort[] value) => new Item(SecsFormat.U2, value);

        /// <summary>
        /// Create unsigned integer item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item U2(IEnumerable<ushort> value) => value.Any() ? U2(value.ToArray()) : U2();

        /// <summary>
        /// Create unsigned integer item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item U4(params uint[] value) => new Item(SecsFormat.U4, value);

        /// <summary>
        /// Create unsigned integer item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item U4(IEnumerable<uint> value) => value.Any() ? U4(value.ToArray()) : U4();

        /// <summary>
        /// Create unsigned integer item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item U8(params ulong[] value) => new Item(SecsFormat.U8, value);

        /// <summary>
        /// Create unsigned integer item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item U8(IEnumerable<ulong> value) => value.Any() ? U8(value.ToArray()) : U8();

        /// <summary>
        /// Create signed integer item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item I1(params sbyte[] value) => new Item(SecsFormat.I1, value);

        /// <summary>
        /// Create signed integer item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item I1(IEnumerable<sbyte> value) => value.Any() ? I1(value.ToArray()) : I1();

        /// <summary>
        /// Create signed integer item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item I2(params short[] value) => new Item(SecsFormat.I2, value);

        /// <summary>
        /// Create signed integer item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item I2(IEnumerable<short> value) => value.Any() ? I2(value.ToArray()) : I2();

        /// <summary>
        /// Create signed integer item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item I4(params int[] value) => new Item(SecsFormat.I4, value);

        /// <summary>
        /// Create signed integer item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item I4(IEnumerable<int> value) => value.Any() ? I4(value.ToArray()) : I4();

        /// <summary>
        /// Create signed integer item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item I8(params long[] value) => new Item(SecsFormat.I8, value);

        /// <summary>
        /// Create signed integer item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item I8(IEnumerable<long> value) => value.Any() ? I8(value.ToArray()) : I8();

        /// <summary>
        /// Create floating point number item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item F4(params float[] value) => new Item(SecsFormat.F4, value);

        /// <summary>
        /// Create floating point number item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item F4(IEnumerable<float> value) => value.Any() ? F4(value.ToArray()) : F4();

        /// <summary>
        /// Create floating point number item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item F8(params double[] value) => new Item(SecsFormat.F8, value);

        /// <summary>
        /// Create floating point number item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item F8(IEnumerable<double> value) => value.Any() ? F8(value.ToArray()) : F8();

        /// <summary>
        /// Create boolean item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item Boolean(params bool[] value) => new Item(SecsFormat.Boolean, value);

        /// <summary>
        /// Create boolean item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item Boolean(IEnumerable<bool> value) => value.Any() ? Boolean(value.ToArray()) : Boolean();

        /// <summary>
        /// Create string item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item A(string value) => value != string.Empty ? new Item(SecsFormat.ASCII, value) : A();

        /// <summary>
        /// Create JIS encoded string item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Item J(string value) => value != string.Empty ? new Item(SecsFormat.JIS8, value) : J();
        #endregion

        #region Share Object

        public static Item L() => EmptyL;
        public static Item B() => EmptyBinary;
        public static Item U1() => EmptyU1;
        public static Item U2() => EmptyU2;
        public static Item U4() => EmptyU4;
        public static Item U8() => EmptyU8;
        public static Item I1() => EmptyI1;
        public static Item I2() => EmptyI2;
        public static Item I4() => EmptyI4;
        public static Item I8() => EmptyI8;
        public static Item F4() => EmptyF4;
        public static Item F8() => EmptyF8;
        public static Item Boolean() => EmptyBoolean;
        public static Item A() => EmptyA;
        public static Item J() => EmptyJ;

        static readonly Item EmptyL = new Item(SecsFormat.List, Enumerable.Empty<Item>());
        static readonly Item EmptyA = new Item(SecsFormat.ASCII, string.Empty);
        static readonly Item EmptyJ = new Item(SecsFormat.JIS8, string.Empty);
        static readonly Item EmptyBoolean = new Item(SecsFormat.Boolean, Enumerable.Empty<bool>());
        static readonly Item EmptyBinary = new Item(SecsFormat.Binary, Enumerable.Empty<byte>());
        static readonly Item EmptyU1 = new Item(SecsFormat.U1, Enumerable.Empty<byte>());
        static readonly Item EmptyU2 = new Item(SecsFormat.U2, Enumerable.Empty<ushort>());
        static readonly Item EmptyU4 = new Item(SecsFormat.U4, Enumerable.Empty<uint>());
        static readonly Item EmptyU8 = new Item(SecsFormat.U8, Enumerable.Empty<ulong>());
        static readonly Item EmptyI1 = new Item(SecsFormat.I1, Enumerable.Empty<sbyte>());
        static readonly Item EmptyI2 = new Item(SecsFormat.I2, Enumerable.Empty<short>());
        static readonly Item EmptyI4 = new Item(SecsFormat.I4, Enumerable.Empty<int>());
        static readonly Item EmptyI8 = new Item(SecsFormat.I8, Enumerable.Empty<long>());
        static readonly Item EmptyF4 = new Item(SecsFormat.F4, Enumerable.Empty<float>());
        static readonly Item EmptyF8 = new Item(SecsFormat.F8, Enumerable.Empty<double>());

        internal static readonly Encoding JIS8Encoding = Encoding.GetEncoding(50222);
        #endregion

        /// <summary>
        /// Encode item to raw data buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal uint EncodeTo(List<ArraySegment<byte>> buffer)
        {
            var bytes = _rawData.Value;
            uint length = unchecked((uint)bytes.Length);
            buffer.Add(new ArraySegment<byte>(bytes));
            if (Format == SecsFormat.List)
                foreach (var subItem in Items)
                    length += subItem.EncodeTo(buffer);
            return length;
        }

        /// <summary>
        /// Encode Item header + value (initial array only)
        /// </summary>
        /// <param name="valueCount">Item value bytes length</param>
        /// <param name="headerlength">return header bytes length</param>
        /// <returns>header bytes + initial bytes of value </returns>
        private unsafe byte[] EncodeItem(int valueCount, out int headerlength)
        {
            var ptr = (byte*)Unsafe.AsPointer(ref valueCount);
            if (valueCount <= 0xff)
            {//	1 byte
                headerlength = 2;
                var result = new byte[valueCount + 2];
                result[0] = (byte)((byte)Format | 1);
                result[1] = ptr[0];
                return result;
            }
            if (valueCount <= 0xffff)
            {//	2 byte
                headerlength = 3;
                var result = new byte[valueCount + 3];
                result[0] = (byte)((byte)Format | 2);
                result[1] = ptr[1];
                result[2] = ptr[0];
                return result;
            }
            if (valueCount <= 0xffffff)
            {//	3 byte
                headerlength = 4;
                var result = new byte[valueCount + 4];
                result[0] = (byte)((byte)Format | 3);
                result[1] = ptr[2];
                result[2] = ptr[1];
                result[3] = ptr[0];
                return result;
            }
            throw new ArgumentOutOfRangeException(nameof(valueCount), valueCount, $"Item data length({valueCount}) is overflow");
        }

        internal static Item BytesDecode(ref SecsFormat format, byte[] data, ref int index, ref int length)
        {
            switch (format)
            {
                case SecsFormat.ASCII: return length == 0 ? A() : A(Encoding.ASCII.GetString(data, index, length));
                case SecsFormat.JIS8: return length == 0 ? J() : J(JIS8Encoding.GetString(data, index, length));
                case SecsFormat.Boolean: return length == 0 ? Boolean() : Boolean(Decode<bool>(data, ref index, ref length));
                case SecsFormat.Binary: return length == 0 ? B() : B(Decode<byte>(data, ref index, ref length));
                case SecsFormat.U1: return length == 0 ? U1() : U1(Decode<byte>(data, ref index, ref length));
                case SecsFormat.U2: return length == 0 ? U2() : U2(Decode<ushort>(data, ref index, ref length));
                case SecsFormat.U4: return length == 0 ? U4() : U4(Decode<uint>(data, ref index, ref length));
                case SecsFormat.U8: return length == 0 ? U8() : U8(Decode<ulong>(data, ref index, ref length));
                case SecsFormat.I1: return length == 0 ? I1() : I1(Decode<sbyte>(data, ref index, ref length));
                case SecsFormat.I2: return length == 0 ? I2() : I2(Decode<short>(data, ref index, ref length));
                case SecsFormat.I4: return length == 0 ? I4() : I4(Decode<int>(data, ref index, ref length));
                case SecsFormat.I8: return length == 0 ? I8() : I8(Decode<long>(data, ref index, ref length));
                case SecsFormat.F4: return length == 0 ? F4() : F4(Decode<float>(data, ref index, ref length));
                case SecsFormat.F8: return length == 0 ? F8() : F8(Decode<double>(data, ref index, ref length));
                default: throw new ArgumentException(@"Invalid format", nameof(format));
            }

            T[] Decode<T>(byte[] data2, ref int index2, ref int length2) where T : struct
            {
                int elmSize = Unsafe.SizeOf<T>();
                data2.Reverse(index2, index2 + length2, elmSize);
                var values = new T[length2 / elmSize];
                Buffer.BlockCopy(data2, index2, values, 0, length2);
                return values;
            }
        }
    }
}