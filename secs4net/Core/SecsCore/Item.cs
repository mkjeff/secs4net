using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Secs4Net.Properties;
using static System.Diagnostics.Debug;

namespace Secs4Net
{
    public abstract class Item : IDisposable
    {
        protected internal abstract ArraySegment<byte> GetEncodedData();
        protected Item(SecsFormat format)
        {
            Format = format;
        }

        public SecsFormat Format { get; }
        public abstract int Count { get; }
        public abstract bool IsMatch(Item target);

        [Obsolete("This property only for debuging. Don't use in production.")]
        public IReadOnlyList<byte> RawBytes
        {
            get
            {
                var tmp = GetEncodedData();
                var result = tmp.ToArray();
                ArrayPool<byte>.Shared.Return(tmp.Array);
                return result;
            }
        } 

        /// <summary>
        /// Data item
        /// </summary>
        public virtual IEnumerable Values
        {
            get { throw new NotSupportedException("This is not a value item"); }
        }

        /// <summary>
        /// List item
        /// </summary>
        public virtual IReadOnlyList<Item> Items
        {
            get { throw new NotSupportedException("This is not a list Item"); }
        }

        /// <summary>
        /// get value0 by specific type
        /// </summary>
        /// <typeparam name="T">return value0 type</typeparam>
        /// <returns></returns>
        public virtual T GetValue<T>() where T : struct
        {
            throw new NotSupportedException("This is not a value Item");
        }
        public virtual T[] GetValues<T>() where T : struct
        {
            throw new NotSupportedException("This is not a value Item");
        }
        public virtual string GetString()
        {
            throw new NotSupportedException("This is not a string value Item");
        }

        #region conversion operator
        public static explicit operator string(Item item) => item.GetString();
        public static explicit operator byte(Item item) => item.GetValue<byte>();
        public static explicit operator sbyte(Item item) => item.GetValue<sbyte>();
        public static explicit operator ushort(Item item) => item.GetValue<ushort>();
        public static explicit operator short(Item item) => item.GetValue<short>();
        public static explicit operator uint(Item item) => item.GetValue<uint>();
        public static explicit operator int(Item item) => item.GetValue<int>();
        public static explicit operator ulong(Item item) => item.GetValue<ulong>();
        public static explicit operator long(Item item) => item.GetValue<long>();
        public static explicit operator float(Item item) => item.GetValue<float>();
        public static explicit operator double(Item item) => item.GetValue<double>();
        public static explicit operator bool(Item item) => item.GetValue<bool>();

        #endregion

        #region Factory Method
        #region L
        public static Item L() => ListFormat.Empty;
        public static Item L(Item item0) => ListFormat.Create(item0);
        public static Item L(Item item0, Item item1) => ListFormat.Create(item0, item1);
        public static Item L(Item item0, Item item1, Item item2) => ListFormat.Create(item0, item1, item2);
        public static Item L(Item item0, Item item1, Item item2, Item item3) => ListFormat.Create(item0, item1, item2, item3);
        public static Item L(Item item0, Item item1, Item item2, Item item3, Item item4) => ListFormat.Create(item0, item1, item2, item3, item4);
        public static Item L(Item item0, Item item1, Item item2, Item item3, Item item4, Item item5) => ListFormat.Create(item0, item1, item2, item3, item4, item5);
        public static Item L(Item item0, Item item1, Item item2, Item item3, Item item4, Item item5, Item item6) => ListFormat.Create(item0, item1, item2, item3, item4, item5, item6);
        internal static Item L(ArraySegment<Item> items) => ListFormat.Create(items);
        public static Item L(IEnumerable<Item> items) => ListFormat.Create(items);
        public static Item L(params Item[] items) => ListFormat.Create(items);

        #endregion

        #region B
        public static Item B() => BinaryFormat.Empty;
        public static Item B(byte value0) => BinaryFormat.Create(value0);
        public static Item B(byte value0, byte value1) => BinaryFormat.Create(value0, value1);
        public static Item B(byte value0, byte value1, byte value2) => BinaryFormat.Create(value0, value1, value2);
        public static Item B(byte value0, byte value1, byte value2, byte value3) => BinaryFormat.Create(value0, value1, value2, value3);
        public static Item B(byte value0, byte value1, byte value2, byte value3, byte value4) => BinaryFormat.Create(value0, value1, value2, value3, value4);
        public static Item B(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5) => BinaryFormat.Create(value0, value1, value2, value3, value4, value5);
        public static Item B(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6) => BinaryFormat.Create(value0, value1, value2, value3, value4, value5, value6);
        internal static Item B(ArraySegment<byte> value) => BinaryFormat.Create(value);
        public static Item B(IEnumerable<byte> value) => BinaryFormat.Create(value);
        public static Item B(params byte[] value) => BinaryFormat.Create(value);
        #endregion

        #region U1
        public static Item U1() => U1Format.Empty;
        public static Item U1(byte value0) => U1Format.Create(value0);
        public static Item U1(byte value0, byte value1) => U1Format.Create(value0, value1);
        public static Item U1(byte value0, byte value1, byte value2) => U1Format.Create(value0, value1, value2);
        public static Item U1(byte value0, byte value1, byte value2, byte value3) => U1Format.Create(value0, value1, value2, value3);
        public static Item U1(byte value0, byte value1, byte value2, byte value3, byte value4) => U1Format.Create(value0, value1, value2, value3, value4);
        public static Item U1(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5) => U1Format.Create(value0, value1, value2, value3, value4, value5);
        public static Item U1(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6) => U1Format.Create(value0, value1, value2, value3, value4, value5, value6);
        internal static Item U1(ArraySegment<byte> value) => U1Format.Create(value);
        public static Item U1(IEnumerable<byte> value) => U1Format.Create(value);
        public static Item U1(params byte[] value) => U1Format.Create(value);
        #endregion

        #region U2
        public static Item U2() => U2Format.Empty;
        public static Item U2(ushort value0) => U2Format.Create(value0);
        public static Item U2(ushort value0, ushort value1) => U2Format.Create(value0, value1);
        public static Item U2(ushort value0, ushort value1, ushort value2) => U2Format.Create(value0, value1, value2);
        public static Item U2(ushort value0, ushort value1, ushort value2, ushort value3) => U2Format.Create(value0, value1, value2, value3);
        public static Item U2(ushort value0, ushort value1, ushort value2, ushort value3, ushort value4) => U2Format.Create(value0, value1, value2, value3, value4);
        public static Item U2(ushort value0, ushort value1, ushort value2, ushort value3, ushort value4, ushort value5) => U2Format.Create(value0, value1, value2, value3, value4, value5);
        public static Item U2(ushort value0, ushort value1, ushort value2, ushort value3, ushort value4, ushort value5, ushort value6) => U2Format.Create(value0, value1, value2, value3, value4, value5, value6);
        public static Item U2(ArraySegment<ushort> value) => U2Format.Create(value);
        public static Item U2(IEnumerable<ushort> value) => U2Format.Create(value);
        public static Item U2(params ushort[] value) => U2Format.Create(value);
        #endregion

        #region U4
        public static Item U4() => U4Format.Empty;
        public static Item U4(uint value0) =>U4Format.Create(value0);
        public static Item U4(uint value0, uint value1) =>U4Format.Create(value0, value1);
        public static Item U4(uint value0, uint value1, uint value2) =>U4Format.Create(value0, value1, value2);
        public static Item U4(uint value0, uint value1, uint value2, uint value3) =>U4Format.Create(value0, value1, value2, value3);
        public static Item U4(uint value0, uint value1, uint value2, uint value3, uint value4) =>U4Format.Create(value0, value1, value2, value3, value4);
        public static Item U4(uint value0, uint value1, uint value2, uint value3, uint value4, uint value5) =>U4Format.Create(value0, value1, value2, value3, value4, value5);
        public static Item U4(uint value0, uint value1, uint value2, uint value3, uint value4, uint value5, uint value6) =>U4Format.Create(value0, value1, value2, value3, value4, value5, value6);
        public static Item U4(ArraySegment<uint> value) =>U4Format.Create(value);
        public static Item U4(IEnumerable<uint> value) => U4Format.Create(value);
        public static Item U4(params uint[] value) => U4Format.Create(value);
        #endregion

        #region U8
        public static Item U8() => U8Format.Empty;
        public static Item U8(ulong value0) => U8Format.Create(value0);
        public static Item U8(ulong value0, ulong value1) => U8Format.Create(value0, value1);
        public static Item U8(ulong value0, ulong value1, ulong value2) => U8Format.Create(value0, value1, value2);
        public static Item U8(ulong value0, ulong value1, ulong value2, ulong value3) => U8Format.Create(value0, value1, value2, value3);
        public static Item U8(ulong value0, ulong value1, ulong value2, ulong value3, ulong value4) => U8Format.Create(value0, value1, value2, value3, value4);
        public static Item U8(ulong value0, ulong value1, ulong value2, ulong value3, ulong value4, ulong value5) => U8Format.Create(value0, value1, value2, value3, value4, value5);
        public static Item U8(ulong value0, ulong value1, ulong value2, ulong value3, ulong value4, ulong value5, ulong value6) => U8Format.Create(value0, value1, value2, value3, value4, value5, value6);
        public static Item U8(ArraySegment<ulong> value) => U8Format.Create(value);
        public static Item U8(IEnumerable<ulong> value) => U8Format.Create(value);
        public static Item U8(params ulong[] value) => U8Format.Create(value);
        #endregion

        #region I1
        public static Item I1() => I1Format.Empty;
        public static Item I1(sbyte value0) => I1Format.Create(value0);
        public static Item I1(sbyte value0, sbyte value1) => I1Format.Create(value0, value1);
        public static Item I1(sbyte value0, sbyte value1, sbyte value2) => I1Format.Create(value0, value1, value2);
        public static Item I1(sbyte value0, sbyte value1, sbyte value2, sbyte value3) => I1Format.Create(value0, value1, value2, value3);
        public static Item I1(sbyte value0, sbyte value1, sbyte value2, sbyte value3, sbyte value4) => I1Format.Create(value0, value1, value2, value3, value4);
        public static Item I1(sbyte value0, sbyte value1, sbyte value2, sbyte value3, sbyte value4, sbyte value5) => I1Format.Create(value0, value1, value2, value3, value4, value5);
        public static Item I1(sbyte value0, sbyte value1, sbyte value2, sbyte value3, sbyte value4, sbyte value5, sbyte value6) => I1Format.Create(value0, value1, value2, value3, value4, value5, value6);
        internal static Item I1(ArraySegment<sbyte> value) => I1Format.Create(value);
        public static Item I1(IEnumerable<sbyte> value) => I1Format.Create(value);
        public static Item I1(params sbyte[] value) => I1Format.Create(value);
        #endregion

        #region I2
        public static Item I2() => I2Format.Empty;
        public static Item I2(short value0) => I2Format.Create(value0);
        public static Item I2(short value0, short value1) => I2Format.Create(value0, value1);
        public static Item I2(short value0, short value1, short value2) => I2Format.Create(value0, value1, value2);
        public static Item I2(short value0, short value1, short value2, short value3) => I2Format.Create(value0, value1, value2, value3);
        public static Item I2(short value0, short value1, short value2, short value3, short value4) => I2Format.Create(value0, value1, value2, value3, value4);
        public static Item I2(short value0, short value1, short value2, short value3, short value4, short value5) => I2Format.Create(value0, value1, value2, value3, value4, value5);
        public static Item I2(short value0, short value1, short value2, short value3, short value4, short value5, short value6) => I2Format.Create(value0, value1, value2, value3, value4, value5, value6);
        internal static Item I2(ArraySegment<short> value) => I2Format.Create(value);
        public static Item I2(IEnumerable<short> value) => I2Format.Create(value);
        public static Item I2(params short[] value) => I2Format.Create(value);
        #endregion

        #region I4
        public static Item I4() => I4Format.Empty;
        public static Item I4(int value0) => I4Format.Create(value0);
        public static Item I4(int value0, int value1) => I4Format.Create(value0, value1);
        public static Item I4(int value0, int value1, int value2) => I4Format.Create(value0, value1, value2);
        public static Item I4(int value0, int value1, int value2, int value3) => I4Format.Create(value0, value1, value2, value3);
        public static Item I4(int value0, int value1, int value2, int value3, int value4) => I4Format.Create(value0, value1, value2, value3, value4);
        public static Item I4(int value0, int value1, int value2, int value3, int value4, int value5) => I4Format.Create(value0, value1, value2, value3, value4, value5);
        public static Item I4(int value0, int value1, int value2, int value3, int value4, int value5, int value6) => I4Format.Create(value0, value1, value2, value3, value4, value5, value6);
        internal static Item I4(ArraySegment<int> value) => I4Format.Create(value);
        public static Item I4(IEnumerable<int> value) => I4Format.Create(value);
        public static Item I4(params int[] value) => I4Format.Create(value);
        #endregion

        #region I8
        public static Item I8() => I8Format.Empty;
        public static Item I8(long value0) => I8Format.Create(value0);
        public static Item I8(long value0, long value1) => I8Format.Create(value0, value1);
        public static Item I8(long value0, long value1, long value2) => I8Format.Create(value0, value1, value2);
        public static Item I8(long value0, long value1, long value2, long value3) => I8Format.Create(value0, value1, value2, value3);
        public static Item I8(long value0, long value1, long value2, long value3, long value4) => I8Format.Create(value0, value1, value2, value3, value4);
        public static Item I8(long value0, long value1, long value2, long value3, long value4, long value5) => I8Format.Create(value0, value1, value2, value3, value4, value5);
        public static Item I8(long value0, long value1, long value2, long value3, long value4, long value5, long value6) => I8Format.Create(value0, value1, value2, value3, value4, value5, value6);
        internal static Item I8(ArraySegment<long> value) => I8Format.Create(value);
        public static Item I8(IEnumerable<long> value) => I8Format.Create(value);
        public static Item I8(params long[] value) => I8Format.Create(value);
        #endregion

        #region F4
        public static Item F4() => F4Format.Empty;
        public static Item F4(float value0) => F4Format.Create(value0);
        public static Item F4(float value0, float value1) => F4Format.Create(value0, value1);
        public static Item F4(float value0, float value1, float value2) => F4Format.Create(value0, value1, value2);
        public static Item F4(float value0, float value1, float value2, float value3) => F4Format.Create(value0, value1, value2, value3);
        public static Item F4(float value0, float value1, float value2, float value3, float value4) => F4Format.Create(value0, value1, value2, value3, value4);
        public static Item F4(float value0, float value1, float value2, float value3, float value4, float value5) => F4Format.Create(value0, value1, value2, value3, value4, value5);
        public static Item F4(float value0, float value1, float value2, float value3, float value4, float value5, float value6) => F4Format.Create(value0, value1, value2, value3, value4, value5, value6);
        internal static Item F4(ArraySegment<float> value) => F4Format.Create(value);
        public static Item F4(IEnumerable<float> value) => F4Format.Create(value);
        public static Item F4(params float[] value) => F4Format.Create(value);
        #endregion

        #region F8
        public static Item F8() => F8Format.Empty;
        public static Item F8(double value0) => F8Format.Create(value0);
        public static Item F8(double value0, double value1) => F8Format.Create(value0, value1);
        public static Item F8(double value0, double value1, double value2) => F8Format.Create(value0, value1, value2);
        public static Item F8(double value0, double value1, double value2, double value3) => F8Format.Create(value0, value1, value2, value3);
        public static Item F8(double value0, double value1, double value2, double value3, double value4) => F8Format.Create(value0, value1, value2, value3, value4);
        public static Item F8(double value0, double value1, double value2, double value3, double value4, double value5) => F8Format.Create(value0, value1, value2, value3, value4, value5);
        public static Item F8(double value0, double value1, double value2, double value3, double value4, double value5, double value6) => F8Format.Create(value0, value1, value2, value3, value4, value5, value6);
        internal static Item F8(ArraySegment<double> value) => F8Format.Create(value);
        public static Item F8(IEnumerable<double> value) => F8Format.Create(value);
        public static Item F8(params double[] value) => F8Format.Create(value);
        #endregion

        #region Boolean
        public static Item Boolean() => BooleanFormat.Empty;
        public static Item Boolean(bool value0) => BooleanFormat.Create(value0);
        public static Item Boolean(bool value0, bool value1) => BooleanFormat.Create(value0, value1);
        public static Item Boolean(bool value0, bool value1, bool value2) => BooleanFormat.Create(value0, value1, value2);
        public static Item Boolean(bool value0, bool value1, bool value2, bool value3) => BooleanFormat.Create(value0, value1, value2, value3);
        public static Item Boolean(bool value0, bool value1, bool value2, bool value3, bool value4) => BooleanFormat.Create(value0, value1, value2, value3, value4);
        public static Item Boolean(bool value0, bool value1, bool value2, bool value3, bool value4, bool value5) => BooleanFormat.Create(value0, value1, value2, value3, value4, value5);
        public static Item Boolean(bool value0, bool value1, bool value2, bool value3, bool value4, bool value5, bool value6) => BooleanFormat.Create(value0, value1, value2, value3, value4, value5, value6);
        internal static Item Boolean(ArraySegment<bool> value) => BooleanFormat.Create(value);
        public static Item Boolean(IEnumerable<bool> value) => BooleanFormat.Create(value);
        public static Item Boolean(params bool[] value) => BooleanFormat.Create(value);
        #endregion

        #region A
        public static Item A() => ASCIIFormat.Empty;
        public static Item A(string value) => ASCIIFormat.Create(value);
        #endregion

        #region J
        public static Item J() => JIS8Format.Empty;
        public static Item J(string value) => JIS8Format.Create(value);

        #endregion
        #endregion

        /// <summary>
        /// Encode item to raw data buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal uint EncodeTo(IList<ArraySegment<byte>> buffer)
        {
            var bytes = GetEncodedData();
            var length = unchecked((uint)bytes.Count);
            buffer.Add(bytes);
            if (Format != SecsFormat.List)
                return length;
            foreach (var subItem in Items)
                length += subItem.EncodeTo(buffer);
            return length;
        }

        /// <summary>
        /// Encode Item header + value0 (initial array only)
        /// </summary>
        /// <param name="format"></param>
        /// <param name="valueCount">Item value0 bytes length</param>
        /// <param name="headerlength">return header bytes length</param>
        /// <returns>header bytes + initial bytes of value0 </returns>
        protected static unsafe byte[] EncodeValue(SecsFormat format, int valueCount, out int headerlength)
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
            throw new ArgumentOutOfRangeException(nameof(valueCount), valueCount, string.Format(Resources.ValueItemDataLength__0__Overflow, valueCount));
        }

        protected static ArraySegment<byte> EncodEmpty(SecsFormat format)
        {
            var arr = ArrayPool<byte>.Shared.Rent(2);
            arr[0] = (byte)((byte)format | 1);
            arr[1] = 0;
            return new ArraySegment<byte>(arr, 0, 2);
        }

        public virtual void Dispose()
        {
        }
    }

    internal abstract class Item<TFormat, TValue> : Item
        where TFormat : IFormat<TValue>
    {
        private static readonly SecsFormat _Format;

        static Item()
        {
            var format = typeof(TFormat)
                .GetFields()
                .First(f => f.IsLiteral && f.Name == "Format");
            _Format = (SecsFormat)format.GetValue(null);
        }


        protected Item()
            : base(_Format)
        {
        }

        internal virtual void SetValue(ArraySegment<TValue> itemValue)
        {
            throw new NotImplementedException();
        }

        internal virtual void SetValue(string itemValue)
        {
            throw new NotImplementedException();
        }
    }

    internal class ValueTypeItem<TFormat, TValue> : Item<TFormat, TValue>
        where TFormat : IFormat<TValue>
        where TValue : struct
    {
        protected internal sealed override ArraySegment<byte> GetEncodedData()
        {
            if (_values.Count == 0)
                return EncodEmpty(Format);

            int sizeOf = Unsafe.SizeOf<TValue>();
            int bytelength = _values.Count * sizeOf;
            int headerLength;
            byte[] result = EncodeValue(Format, bytelength, out headerLength);
            Buffer.BlockCopy(_values.Array, 0, result, headerLength, bytelength);
            result.Reverse(headerLength, headerLength + bytelength, sizeOf);
            return new ArraySegment<byte>(result, 0, headerLength + bytelength);
        }

        protected ArraySegment<TValue> _values = new ArraySegment<TValue>(Array.Empty<TValue>());

        internal sealed override void SetValue(ArraySegment<TValue> itemValue)
        {
            _values = itemValue;
        }

        public sealed override int Count => _values.Count;

        public sealed override IEnumerable Values => _values;

        public sealed override unsafe T GetValue<T>() => Unsafe.Read<T>(Unsafe.AsPointer(ref _values.Array[0]));

        public sealed override T[] GetValues<T>() => Unsafe.As<T[]>(_values.ToArray());

        public sealed override bool IsMatch(Item target)
        {
            if (ReferenceEquals(this, target))
                return true;

            if (Format != target.Format)
                return false;

            if (target.Count == 0)
                return true;

            if (Count != target.Count)
                return false;

            //return memcmp(Unsafe.As<byte[]>(_values), Unsafe.As<byte[]>(target._values), Buffer.ByteLength((Array)_values)) == 0;
            return UnsafeCompare(_values.Array, Unsafe.As<ValueTypeItem<TFormat, TValue>>(target)._values.Array, _values.Count);
        }

        public sealed override string ToString()
            => $"<{Format.GetName()} [{Count}] {(Format == SecsFormat.Binary ? Unsafe.As<byte[]>(_values).ToHexString() : string.Join(" ", _values))} >";

        //[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        //static extern int memcmp(byte[] b1, byte[] b2, long count);

        /// <summary>
        /// http://stackoverflow.com/questions/43289/comparing-two-byte-arrays-in-net/8808245#8808245
        /// </summary>
        private static unsafe bool UnsafeCompare(TValue[] a1, TValue[] a2, int count)
        {
            int length = count * Unsafe.SizeOf<TValue>();
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
    }

    internal sealed class PooledValueItem<TFormat, TValue> : ValueTypeItem<TFormat, TValue>
        where TFormat : IFormat<TValue>
        where TValue : struct
    {
        public override void Dispose()
        {
            ArrayPool<TValue>.Shared.Return(_values.Array);
        }
    }

    internal sealed class StringItem<TFormat> : Item<TFormat, string>
        where TFormat : IFormat<string>
    {
        private string _str = string.Empty;

        internal override void SetValue(string itemValue)
        {
            _str = itemValue;
        }

        protected internal override ArraySegment<byte> GetEncodedData()
        {
            if (string.IsNullOrEmpty(_str))
                return EncodEmpty(Format);

            int bytelength = _str.Length;
            int headerLength;
            byte[] result = EncodeValue(Format, bytelength, out headerLength);
            var encoder = Format == SecsFormat.ASCII ? Encoding.ASCII : SecsExtension.JIS8Encoding;
            encoder.GetBytes(_str, 0, _str.Length, result, headerLength);
            return new ArraySegment<byte>(result, 0, headerLength + bytelength);
        }
        public override int Count => _str.Length;
        public override IEnumerable Values => _str;
        public override string GetString() => _str;

        public override bool IsMatch(Item target)
        {
            if (ReferenceEquals(this, target))
                return true;

            if (Format != target.Format)
                return false;

            if (target.Count == 0)
                return true;

            if (Count != target.Count)
                return false;

            return _str == Unsafe.As<StringItem<TFormat>>(target)._str;
        }

        public override string ToString()
            => $"<{(Format == SecsFormat.ASCII ? "A" : "J")} [{_str.Length}] {_str} >";
    }

    internal class ListItem : Item<ListFormat, Item>
    {
        protected ArraySegment<Item> _values = new ArraySegment<Item>(Array.Empty<Item>());

        internal sealed override void SetValue(ArraySegment<Item> items)
        {
            Assert(items.Count <= byte.MaxValue, $"List length out of range, max length: 255");
            _values = items;
        }

        protected internal sealed override ArraySegment<byte> GetEncodedData()
        {
            var arr = ArrayPool<byte>.Shared.Rent(2);
            arr[0] = (byte)SecsFormat.List | 1;
            arr[1] = unchecked((byte)_values.Count);
            return new ArraySegment<byte>(arr, 0, 2);
        }

        public sealed override int Count => _values.Count;
        public sealed override IReadOnlyList<Item> Items => _values;
        public sealed override string ToString() => $"<List [{_values.Count}] >";

        public sealed override bool IsMatch(Item target)
        {
            if (ReferenceEquals(this, target))
                return true;

            if (Format != target.Format)
                return false;

            if (target.Count == 0)
                return true;

            if (Count != target.Count)
                return false;

            return IsMatch(_values.Array,
                           Unsafe.As<ListItem>(target)
                                 ._values.Array,
                           Count);
        }

        private static bool IsMatch(Item[] a, Item[] b, int count)
        {
            for (var i = 0; i < count; i++)
                if (!a[i].IsMatch(b[i]))
                    return false;
            return true;
        }

        public override void Dispose()
        {
            foreach (var item in _values)
                item.Dispose();
        }
    }

    internal sealed class PooledListItem : ListItem
    {
        public override void Dispose()
        {
            base.Dispose();
            ArrayPool<Item>.Shared.Return(_values.Array);
        }
    }
}