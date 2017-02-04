using Secs4Net.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Secs4Net
{
    public abstract class SecsItem
    {
        public abstract SecsFormat Format { get; }
        public abstract int Count { get; }
        public abstract bool IsMatch(SecsItem target);

        [Browsable(false), Obsolete("This property only for debugging. Don't use in production.")]
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
        /// Get sub-list item
        /// </summary>
        /// <exception cref="NotSupportedException">this item is not a list</exception>
        public virtual IReadOnlyList<SecsItem> Items => throw new NotSupportedException("This is not a list Item");

        /// <summary>
        /// get first value by specific type
        /// </summary>
        /// <typeparam name="T">value type</typeparam>
        /// <returns>first value of this item</returns>
        /// <exception cref="NotSupportedException">this item is not a value</exception>
        public virtual T GetValue<T>() where T : struct => throw new NotSupportedException("This is not a value Item");

        /// <summary>
        /// get value array by specific type
        /// </summary>
        /// <typeparam name="T">value type</typeparam>
        /// <returns>value array</returns>
        /// <exception cref="NotSupportedException">this item is not a value</exception>
        public virtual T[] GetValues<T>() where T : struct => throw new NotSupportedException("This is not a value Item");

        /// <summary>
        /// get string value
        /// </summary>
        /// <returns>string</returns>
        /// <exception cref="NotSupportedException">this item is not a string item</exception>
        public virtual string GetString() => throw new NotSupportedException("This is not a string value Item");

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
        /// <returns>total bytes length of buffer</returns>
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
        /// Get encoded array for header and value(reserved) 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="valueCount">Item value0 bytes length</param>
        /// <returns>(header bytes and reserved bytes of value, length of header bytes) </returns>
        protected static unsafe (byte[] buffer,int headerlength) EncodeValue(SecsFormat format, int valueCount)
        {
            var result = SecsGem.EncodedBytePool.Rent(valueCount + 4);
            var target = (byte*)Unsafe.AsPointer(ref result[0]);
            var ptr = (byte*)Unsafe.AsPointer(ref valueCount);
            if (valueCount <= 0xff)
            {//	1 byte
                Unsafe.Write(target, (byte)((byte)format | 1));
                Unsafe.Copy(target + 1, ref Unsafe.AsRef<byte>(ptr));
                return (result,2);
            }
            if (valueCount <= 0xffff)
            {//	2 byte
                Unsafe.Write(target, (byte)((byte)format | 2));
                Unsafe.Copy(target + 1, ref Unsafe.AsRef<byte>(ptr + 1));
                Unsafe.Copy(target + 2, ref Unsafe.AsRef<byte>(ptr));
                return (result,3);
            }
            if (valueCount <= 0xffffff)
            {//	3 byte
                Unsafe.Write(target, (byte)((byte)format | 3));
                Unsafe.Copy(target + 1, ref Unsafe.AsRef<byte>(ptr + 2));
                Unsafe.Copy(target + 2, ref Unsafe.AsRef<byte>(ptr + 1));
                Unsafe.Copy(target + 3, ref Unsafe.AsRef<byte>(ptr));
                return (result,4);
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

        internal abstract void Release();
    }

    internal abstract class SecsItem<TFormat, TValue> : SecsItem
        where TFormat : IFormat<TValue>
    {
        private static readonly SecsFormat SecsFormat;

        static SecsItem()
        {
            var format = typeof(TFormat)
                .GetFields()
                .First(IsFormatField);
            SecsFormat = (SecsFormat)format.GetValue(null);
        }
        private static bool IsFormatField(FieldInfo f) => f.IsLiteral && f.Name == "Format";

        public sealed override SecsFormat Format => SecsFormat;
    }
}