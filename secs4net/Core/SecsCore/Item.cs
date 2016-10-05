using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Secs4Net
{
    public abstract class Item
    {
        protected abstract ArraySegment<byte> EncodedData { get; }

        protected readonly IEnumerable _values;

        protected Item(SecsFormat format, IEnumerable value)
        {
            Format = format;
            _values = value;
        }

        public SecsFormat Format { get; }

        public int Count =>
            Format == SecsFormat.List
            ? Unsafe.As<IReadOnlyList<Item>>(_values).Count
            : Unsafe.As<Array>(_values).Length;

        public IReadOnlyList<byte> RawBytes => EncodedData.ToArray();

        /// <summary>
        /// Non-list item values
        /// </summary>
        public IEnumerable Values
        {
            get
            {
                if (Format == SecsFormat.List) throw new InvalidOperationException("Item is a list");
                return _values;
            }
        }

        /// <summary>
        /// List items
        /// </summary>
        public IReadOnlyList<Item> Items
        {
            get
            {
                if (Format != SecsFormat.List) throw new InvalidOperationException("Item is not a list");
                return Unsafe.As<IReadOnlyList<Item>>(_values);
            }
        }

        /// <summary>
        /// get value by specific type
        /// </summary>
        /// <typeparam name="T">return value type</typeparam>
        /// <returns></returns>
        public T GetValue<T>()
        {
            if (Format == SecsFormat.List)
                throw new InvalidOperationException("Item is list");

            if (_values is T)
                return (T)_values;

            if (_values is IEnumerable<T>)
                return ((IEnumerable<T>)_values).First();

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
                    return IsMatch(Items, target.Items);
                case SecsFormat.ASCII:
                case SecsFormat.JIS8:
                    return (string)_values == (string)target._values;
                default:
                    //return memcmp(Unsafe.As<byte[]>(_values), Unsafe.As<byte[]>(target._values), Buffer.ByteLength((Array)_values)) == 0;
                    return UnsafeCompare((Array)_values, (Array)target._values);
            }
        }

        static bool IsMatch(IReadOnlyList<Item> a, IReadOnlyList<Item> b)
        {
            for (int i = 0; i < a.Count; i++)
                if (!a[i].IsMatch(b[i]))
                    return false;
            return true;
        }

        protected static string JoinAsString<T>(IEnumerable src) where T : struct => string.Join(" ", Unsafe.As<T[]>(src));

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
        internal static Item L(IList<Item> items) => new ListItem(new ReadOnlyCollection<Item>(items));
        public static Item L(IEnumerable<Item> items) => items.Any() ? L(items.ToList()) : L();
        public static Item L(params Item[] items) => L((IList<Item>)items);

        //public static Item B(byte value) => new Item<byte>(SecsFormat.Binary, ToArray(value));
        public static Item B(params byte[] value) => new Item<byte>(SecsFormat.Binary, value);
        public static Item B(IEnumerable<byte> value) => value.Any() ? B(value.ToArray()) : B();

        //public static Item U1(byte value) => new Item<byte>(SecsFormat.U1, ToArray(value));
        public static Item U1(params byte[] value) => new Item<byte>(SecsFormat.U1, value);
        public static Item U1(IEnumerable<byte> value) => value.Any() ? U1(value.ToArray()) : U1();

        //public static Item U2(ushort value) => new Item<ushort>(SecsFormat.U2, ToArray(value));
        public static Item U2(params ushort[] value) => new Item<ushort>(SecsFormat.U2, value);
        public static Item U2(IEnumerable<ushort> value) => value.Any() ? U2(value.ToArray()) : U2();

        //public static Item U4(uint value) => new Item<uint>(SecsFormat.U4, ToArray(value));
        public static Item U4(params uint[] value) => new Item<uint>(SecsFormat.U4, value);
        public static Item U4(IEnumerable<uint> value) => value.Any() ? U4(value.ToArray()) : U4();

        //public static Item U8(ulong value) => new Item<ulong>(SecsFormat.U8, ToArray(value));
        public static Item U8(params ulong[] value) => new Item<ulong>(SecsFormat.U8, value);
        public static Item U8(IEnumerable<ulong> value) => value.Any() ? U8(value.ToArray()) : U8();

        //public static Item I1(sbyte value) => new Item<sbyte>(SecsFormat.I1, ToArray(value));
        public static Item I1(params sbyte[] value) => new Item<sbyte>(SecsFormat.I1, value);
        public static Item I1(IEnumerable<sbyte> value) => value.Any() ? I1(value.ToArray()) : I1();

        //public static Item I2(short value) => new Item<short>(SecsFormat.I2, ToArray(value));
        public static Item I2(params short[] value) => new Item<short>(SecsFormat.I2, value);
        public static Item I2(IEnumerable<short> value) => value.Any() ? I2(value.ToArray()) : I2();

        //public static Item I4(int value) => new Item<int>(SecsFormat.I4, ToArray(value));
        public static Item I4(params int[] value) => new Item<int>(SecsFormat.I4, value);
        public static Item I4(IEnumerable<int> value) => value.Any() ? I4(value.ToArray()) : I4();

        //public static Item I8(long value) => new Item<long>(SecsFormat.I8, ToArray(value));
        public static Item I8(params long[] value) => new Item<long>(SecsFormat.I8, value);
        public static Item I8(IEnumerable<long> value) => value.Any() ? I8(value.ToArray()) : I8();

        //public static Item F4(float value) => new Item<float>(SecsFormat.F4, ToArray(value));
        public static Item F4(params float[] value) => new Item<float>(SecsFormat.F4, value);
        public static Item F4(IEnumerable<float> value) => value.Any() ? F4(value.ToArray()) : F4();

        //public static Item F8(double value) => new Item<double>(SecsFormat.F8, ToArray(value));
        public static Item F8(params double[] value) => new Item<double>(SecsFormat.F8, value);
        public static Item F8(IEnumerable<double> value) => value.Any() ? F8(value.ToArray()) : F8();

        //public static Item Boolean(bool value) => new Item<bool>(SecsFormat.Boolean, ToArray(value));
        public static Item Boolean(params bool[] value) => new Item<bool>(SecsFormat.Boolean, value);
        public static Item Boolean(IEnumerable<bool> value) => value.Any() ? Boolean(value.ToArray()) : Boolean();

        public static Item A(string value) => value != string.Empty ? new StringItem(SecsFormat.ASCII, value) : A();

        public static Item J(string value) => value != string.Empty ? new StringItem(SecsFormat.JIS8, value) : J();
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

        static readonly Item EmptyL = new ListItem(Array.Empty<Item>());
        static readonly Item EmptyA = new StringItem(SecsFormat.ASCII, string.Empty);
        static readonly Item EmptyJ = new StringItem(SecsFormat.JIS8, string.Empty);
        static readonly Item EmptyBoolean = new Item<bool>(SecsFormat.Boolean, Array.Empty<bool>());
        static readonly Item EmptyBinary = new Item<byte>(SecsFormat.Binary, Array.Empty<byte>());
        static readonly Item EmptyU1 = new Item<byte>(SecsFormat.U1, Array.Empty<byte>());
        static readonly Item EmptyU2 = new Item<ushort>(SecsFormat.U2, Array.Empty<ushort>());
        static readonly Item EmptyU4 = new Item<uint>(SecsFormat.U4, Array.Empty<uint>());
        static readonly Item EmptyU8 = new Item<ulong>(SecsFormat.U8, Array.Empty<ulong>());
        static readonly Item EmptyI1 = new Item<sbyte>(SecsFormat.I1, Array.Empty<sbyte>());
        static readonly Item EmptyI2 = new Item<short>(SecsFormat.I2, Array.Empty<short>());
        static readonly Item EmptyI4 = new Item<int>(SecsFormat.I4, Array.Empty<int>());
        static readonly Item EmptyI8 = new Item<long>(SecsFormat.I8, Array.Empty<long>());
        static readonly Item EmptyF4 = new Item<float>(SecsFormat.F4, Array.Empty<float>());
        static readonly Item EmptyF8 = new Item<double>(SecsFormat.F8, Array.Empty<double>());

        internal static readonly Encoding JIS8Encoding = Encoding.GetEncoding(50222);
        #endregion

        public static T[] ToArray<T>(T value) where T : struct
        {
            var arr = ArrayPool<T>.Shared.Rent(1);
            arr[0] = value;
            return arr;
        }

        public static T[] ToArray<T>(T value0, T value1) where T : struct
        {
            var arr = ArrayPool<T>.Shared.Rent(2);
            arr[0] = value0;
            arr[1] = value1;
            return arr;
        }

        public static T[] ToArray<T>(T value0, T value1, T value2) where T : struct
        {
            var arr = ArrayPool<T>.Shared.Rent(3);
            arr[0] = value0;
            arr[1] = value1;
            arr[2] = value2;
            return arr;
        }

        //[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        //static extern int memcmp(byte[] b1, byte[] b2, long count);

        /// <summary>
        /// http://stackoverflow.com/questions/43289/comparing-two-byte-arrays-in-net/8808245#8808245
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        static unsafe bool UnsafeCompare(Array a1, Array a2)
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

        /// <summary>
        /// Encode item to raw data buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal uint EncodeTo(List<ArraySegment<byte>> buffer)
        {
            var bytes = EncodedData;
            uint length = unchecked((uint)bytes.Count);
            buffer.Add(bytes);
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
        protected static unsafe byte[] EncodeItem(SecsFormat format, int valueCount, out int headerlength)
        {
            var ptr = (byte*)Unsafe.AsPointer(ref valueCount);
            if (valueCount <= 0xff)
            {//	1 byte
                headerlength = 2;
                var result = ArrayPool<byte>.Shared.Rent(valueCount + 2);
                result[0] = (byte)((byte)format | 1);
                result[1] = ptr[0];
                return result;
            }
            if (valueCount <= 0xffff)
            {//	2 byte
                headerlength = 3;
                var result = ArrayPool<byte>.Shared.Rent(valueCount + 3);
                result[0] = (byte)((byte)format | 2);
                result[1] = ptr[1];
                result[2] = ptr[0];
                return result;
            }
            if (valueCount <= 0xffffff)
            {//	3 byte
                headerlength = 4;
                var result = ArrayPool<byte>.Shared.Rent(valueCount + 4);
                result[0] = (byte)((byte)format | 3);
                result[1] = ptr[2];
                result[2] = ptr[1];
                result[3] = ptr[0];
                return result;
            }
            throw new ArgumentOutOfRangeException(nameof(valueCount), valueCount, $"Item data length({valueCount}) is overflow");
        }

        protected static ArraySegment< byte> EmptyEncodedData(SecsFormat format)
        {
            var arr = ArrayPool<byte>.Shared.Rent(2);
            arr[0] = (byte)((byte)format | 1);
            arr[1] = 0;
            return new ArraySegment<byte>(arr, 0, 2);
        }
    }

    sealed class Item<T> : Item where T : struct
    {
        protected override ArraySegment< byte> EncodedData => _encodedData.Value;
        readonly Lazy<ArraySegment<byte>> _encodedData;
        internal Item(SecsFormat format, T[] value) : base(format, value)
        {
            _encodedData = new Lazy<ArraySegment<byte>>(() =>
            {
                var arr = (Array)_values;
                if (arr.Length == 0)
                    return EmptyEncodedData(Format);

                int bytelength = Buffer.ByteLength(arr);
                int headerLength;
                byte[] result = EncodeItem(Format, bytelength, out headerLength);
                Buffer.BlockCopy(arr, 0, result, headerLength, bytelength);
                result.Reverse(headerLength, headerLength + bytelength, bytelength / arr.Length);
                return new ArraySegment<byte>( result,0, headerLength+ bytelength);
            });
        }

        ~Item()
        {
            //ArrayPool<T>.Shared.Return((T[])_values);
            if (_encodedData.IsValueCreated)
                ArrayPool<byte>.Shared.Return(_encodedData.Value.Array);
        }

        public override string ToString()
            => new StringBuilder("<").Append(Format.GetName()).Append(" [").Append(((T[])_values).Length).Append("] ")
                .Append(Format == SecsFormat.Binary
                    ? Unsafe.As<byte[]>(_values).ToHexString()
                    : JoinAsString<T>(_values))
                .Append('>')
                .ToString();
    }

    sealed class StringItem : Item
    {
        readonly Lazy<ArraySegment<byte>> _encodedData;

        protected override ArraySegment<byte> EncodedData => _encodedData.Value;

        internal StringItem(SecsFormat format, string value) : base(format, value)
        {
            _encodedData = new Lazy<ArraySegment< byte>>(() =>
            {
                var str = (string)_values;
                if (str.Length == 0)
                    return EmptyEncodedData(Format);

                int bytelength = str.Length;
                int headerLength;
                byte[] result = EncodeItem(Format, bytelength, out headerLength);
                var encoder = Format == SecsFormat.ASCII ? Encoding.ASCII : JIS8Encoding;
                encoder.GetBytes(str, 0, str.Length, result, headerLength);
                return new ArraySegment<byte>(result, 0, headerLength + bytelength);
            });
        }

        ~StringItem()
        {
            if (_encodedData.IsValueCreated)
                ArrayPool<byte>.Shared.Return(_encodedData.Value.Array);
        }

        public override string ToString()
            => new StringBuilder("<").Append(Format == SecsFormat.ASCII ? "A" : "J").Append(" [")
                .Append(Unsafe.As<string>(_values).Length).Append("] ").Append(Unsafe.As<string>(_values)).Append('>')
                .ToString();
    }

    sealed class ListItem : Item
    {
        protected override ArraySegment< byte> EncodedData => _encodedData.Value;
        readonly Lazy< ArraySegment<byte>> _encodedData;
        internal ListItem(IReadOnlyList<Item> items) : base(SecsFormat.List, items)
        {
            Debug.Assert(items.Count <= byte.MaxValue, $"List length out of range, max length: 255");

            _encodedData = new Lazy<ArraySegment<byte>>(() =>
            {
                var arr = ArrayPool<byte>.Shared.Rent(2);
                arr[0] = (byte)SecsFormat.List | 1;
                arr[1] = unchecked((byte)items.Count);
                return new ArraySegment<byte>(arr, 0, 2);
            });
        }

        ~ListItem()
        {
            if (_encodedData.IsValueCreated)
                ArrayPool<byte>.Shared.Return(_encodedData.Value.Array);
        }

        public override string ToString() =>
            new StringBuilder("<").Append(nameof(SecsFormat.List)).Append(" [")
                .Append(Unsafe.As<IReadOnlyList<Item>>(_values).Count).Append("] >")
                .ToString();
    }
}