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
    public abstract class SecsItem : IDisposable
    {
        protected internal abstract ArraySegment<byte> GetEncodedData();
        protected SecsItem(SecsFormat format)
        {
            Format = format;
        }

        public SecsFormat Format { get; }
        public abstract int Count { get; }
        public abstract bool IsMatch(SecsItem target);

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
        public virtual IReadOnlyList<SecsItem> Items
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
        public static explicit operator string(SecsItem secsItem) => secsItem.GetString();
        public static explicit operator byte(SecsItem secsItem) => secsItem.GetValue<byte>();
        public static explicit operator sbyte(SecsItem secsItem) => secsItem.GetValue<sbyte>();
        public static explicit operator ushort(SecsItem secsItem) => secsItem.GetValue<ushort>();
        public static explicit operator short(SecsItem secsItem) => secsItem.GetValue<short>();
        public static explicit operator uint(SecsItem secsItem) => secsItem.GetValue<uint>();
        public static explicit operator int(SecsItem secsItem) => secsItem.GetValue<int>();
        public static explicit operator ulong(SecsItem secsItem) => secsItem.GetValue<ulong>();
        public static explicit operator long(SecsItem secsItem) => secsItem.GetValue<long>();
        public static explicit operator float(SecsItem secsItem) => secsItem.GetValue<float>();
        public static explicit operator double(SecsItem secsItem) => secsItem.GetValue<double>();
        public static explicit operator bool(SecsItem secsItem) => secsItem.GetValue<bool>();

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

    internal abstract class SecsItem<TFormat, TValue> : SecsItem
        where TFormat : IFormat<TValue>
    {
        private static readonly SecsFormat _Format;

        static SecsItem()
        {
            var format = typeof(TFormat)
                .GetFields()
                .First(f => f.IsLiteral && f.Name == "Format");
            _Format = (SecsFormat)format.GetValue(null);
        }


        protected SecsItem()
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

    internal class ValueItem<TFormat, TValue> : SecsItem<TFormat, TValue>
        where TFormat : IFormat<TValue>
        where TValue : struct
    {
        protected internal sealed override ArraySegment<byte> GetEncodedData()
        {
            if (values.Count == 0)
                return EncodEmpty(Format);

            int sizeOf = Unsafe.SizeOf<TValue>();
            int bytelength = values.Count * sizeOf;
            int headerLength;
            byte[] result = EncodeValue(Format, bytelength, out headerLength);
            Buffer.BlockCopy(values.Array, 0, result, headerLength, bytelength);
            result.Reverse(headerLength, headerLength + bytelength, sizeOf);
            return new ArraySegment<byte>(result, 0, headerLength + bytelength);
        }

        protected ArraySegment<TValue> values = new ArraySegment<TValue>(Array.Empty<TValue>());

        internal sealed override void SetValue(ArraySegment<TValue> itemValue)
        {
            values = itemValue;
        }

        public sealed override int Count => values.Count;

        public sealed override IEnumerable Values => values;

        public sealed override unsafe T GetValue<T>() => Unsafe.Read<T>(Unsafe.AsPointer(ref values.Array[0]));

        public sealed override T[] GetValues<T>() => Unsafe.As<T[]>(values.ToArray());

        public sealed override bool IsMatch(SecsItem target)
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
            return UnsafeCompare(values.Array,
                Unsafe.As<SecsItemPooledWrapper<ValueItem<TFormat, TValue>, TFormat, TValue>>(target).Item.values.Array, values.Count);
        }

        public sealed override string ToString()
            => $"<{Format.GetName()} [{Count}] {(Format == SecsFormat.Binary ? Unsafe.As<byte[]>(values).ToHexString() : string.Join(" ", values))} >";

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

    internal sealed class PooledValueItem<TFormat, TValue> : ValueItem<TFormat, TValue>
        where TFormat : IFormat<TValue>
        where TValue : struct
    {
        public override void Dispose()
        {
            ArrayPool<TValue>.Shared.Return(values.Array);
        }
    }

    internal sealed class StringItem<TFormat> : SecsItem<TFormat, string>
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

        public override bool IsMatch(SecsItem target)
        {
            if (ReferenceEquals(this, target))
                return true;

            if (Format != target.Format)
                return false;

            if (target.Count == 0)
                return true;

            if (Count != target.Count)
                return false;

            return _str == Unsafe.As<SecsItemPooledWrapper<StringItem<TFormat>,TFormat,string>>(target).Item._str;
        }

        public override string ToString()
            => $"<{(Format == SecsFormat.ASCII ? "A" : "J")} [{_str.Length}] {_str} >";
    }

    internal class ListItem : SecsItem<ListFormat, SecsItem>
    {
        protected ArraySegment<SecsItem> list = new ArraySegment<SecsItem>(Array.Empty<SecsItem>());

        internal sealed override void SetValue(ArraySegment<SecsItem> items)
        {
            Assert(items.Count <= byte.MaxValue, $"List length out of range, max length: 255");
            list = items;
        }

        protected internal sealed override ArraySegment<byte> GetEncodedData()
        {
            var arr = ArrayPool<byte>.Shared.Rent(2);
            arr[0] = (byte)SecsFormat.List | 1;
            arr[1] = unchecked((byte)list.Count);
            return new ArraySegment<byte>(arr, 0, 2);
        }

        public sealed override int Count => list.Count;
        public sealed override IReadOnlyList<SecsItem> Items => list;
        public sealed override string ToString() => $"<List [{list.Count}] >";

        public sealed override bool IsMatch(SecsItem target)
        {
            if (ReferenceEquals(this, target))
                return true;

            if (Format != target.Format)
                return false;

            if (target.Count == 0)
                return true;

            if (Count != target.Count)
                return false;

            return IsMatch(list.Array,
                           Unsafe.As<SecsItemPooledWrapper<ListItem, ListFormat, SecsItem>>(target).Item
                                 .list.Array,
                           Count);
        }

        private static bool IsMatch(SecsItem[] a, SecsItem[] b, int count)
        {
            for (var i = 0; i < count; i++)
                if (!a[i].IsMatch(b[i]))
                    return false;
            return true;
        }

        public override void Dispose()
        {
            foreach (var item in list)
                item.Dispose();
        }
    }

    internal sealed class PooledListItem : ListItem
    {
        public override void Dispose()
        {
            base.Dispose();
            ArrayPool<SecsItem>.Shared.Return(list.Array);
        }
    }
}