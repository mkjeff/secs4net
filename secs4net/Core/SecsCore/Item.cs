using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Secs4Net.Properties;

namespace Secs4Net
{
    public abstract class SecsItem
    {
        public abstract SecsFormat Format { get; }
        public abstract int Count { get; }
        public abstract bool IsMatch(SecsItem target);

        [Obsolete("This property only for debugging. Don't use in production.")]
        public IReadOnlyList<byte> RawBytes
        {
            get
            {
                var tmp = GetEncodedData();
                var result = tmp.ToArray();
                SecsGem.EncodedBytePool.Return(tmp.Array);
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
        /// get first value by specific type
        /// </summary>
        /// <typeparam name="T">value type</typeparam>
        /// <returns></returns>
        public virtual T GetValue<T>() where T : struct
        {
            throw new NotSupportedException("This is not a value Item");
        }

        /// <summary>
        /// get value array by specific type
        /// </summary>
        /// <typeparam name="T">value type</typeparam>
        /// <returns></returns>
        public virtual T[] GetValues<T>() where T : struct
        {
            throw new NotSupportedException("This is not a value Item");
        }

        /// <summary>
        /// get string value
        /// </summary>
        /// <returns></returns>
        public virtual string GetString()
        {
            throw new NotSupportedException("This is not a string value Item");
        }

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

        protected abstract ArraySegment<byte> GetEncodedData();

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
                var result = SecsGem.EncodedBytePool.Rent(valueCount + 2);
                result[0] = (byte)((byte)format | 1);
                result[1] = ptr[0];
                return result;
            }
            if (valueCount <= 0xffff)
            {//	2 byte
                headerlength = 3;
                var result = SecsGem.EncodedBytePool.Rent(valueCount + 3);
                result[0] = (byte)((byte)format | 2);
                result[1] = ptr[1];
                result[2] = ptr[0];
                return result;
            }
            if (valueCount <= 0xffffff)
            {//	3 byte
                headerlength = 4;
                var result = SecsGem.EncodedBytePool.Rent(valueCount + 4);
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
            var arr = SecsGem.EncodedBytePool.Rent(2);
            arr[0] = (byte)((byte)format | 1);
            arr[1] = 0;
            return new ArraySegment<byte>(arr, 0, 2);
        }

        internal virtual void Release()
        {
        }
    }

    internal abstract class SecsItem<TFormat, TValue> : SecsItem
        where TFormat : IFormat<TValue>
    {
        private static readonly SecsFormat SecsFormat;

        static SecsItem()
        {
            var format = typeof(TFormat)
                .GetFields()
                .First(f => f.IsLiteral && f.Name == "Format");
            SecsFormat = (SecsFormat)format.GetValue(null);
        }

        public override SecsFormat Format => SecsFormat;
    }
}