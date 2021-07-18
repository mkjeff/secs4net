using Microsoft.Toolkit.HighPerformance;
using Microsoft.Toolkit.HighPerformance.Buffers;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Secs4Net
{
    [DebuggerDisplay("{GetDebugString()}")]
    [DebuggerTypeProxy(typeof(EncodedItemDebugView))]
    public unsafe sealed partial class Item : IEquatable<Item>, IEnumerable<Item>
    {
        private const int DebuggerDisplayMaxCount = 20;
        private static readonly Encoding Jis8Encoding = Encoding.GetEncoding(50222);
        private static readonly Item EmptyL = new(SecsFormat.List, Array.Empty<Item>(), &EncodeEmptyItem);
        private static readonly Item EmptyA = new(SecsFormat.ASCII, string.Empty, &EncodeEmptyItem);
        private static readonly Item EmptyJ = new(SecsFormat.JIS8, string.Empty, &EncodeEmptyItem);
        private static readonly Item EmptyBoolean = new(SecsFormat.Boolean, Array.Empty<bool>(), &EncodeEmptyItem);
        private static readonly Item EmptyBinary = new(SecsFormat.Binary, Array.Empty<byte>(), &EncodeEmptyItem);
        private static readonly Item EmptyU1 = new(SecsFormat.U1, Array.Empty<byte>(), &EncodeEmptyItem);
        private static readonly Item EmptyU2 = new(SecsFormat.U2, Array.Empty<ushort>(), &EncodeEmptyItem);
        private static readonly Item EmptyU4 = new(SecsFormat.U4, Array.Empty<uint>(), &EncodeEmptyItem);
        private static readonly Item EmptyU8 = new(SecsFormat.U8, Array.Empty<ulong>(), &EncodeEmptyItem);
        private static readonly Item EmptyI1 = new(SecsFormat.I1, Array.Empty<sbyte>(), &EncodeEmptyItem);
        private static readonly Item EmptyI2 = new(SecsFormat.I2, Array.Empty<short>(), &EncodeEmptyItem);
        private static readonly Item EmptyI4 = new(SecsFormat.I4, Array.Empty<int>(), &EncodeEmptyItem);
        private static readonly Item EmptyI8 = new(SecsFormat.I8, Array.Empty<long>(), &EncodeEmptyItem);
        private static readonly Item EmptyF4 = new(SecsFormat.F4, Array.Empty<float>(), &EncodeEmptyItem);
        private static readonly Item EmptyF8 = new(SecsFormat.F8, Array.Empty<double>(), &EncodeEmptyItem);

        private readonly object _values;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly unsafe delegate*<Item, IBufferWriter<byte>, void> _encode;

        private unsafe Item(SecsFormat format, IEnumerable values, delegate*<Item, IBufferWriter<byte>, void> encoder)
        {
            Format = format;
            _values = values;
            _encode = encoder;
        }

        public SecsFormat Format { get; }

        public int Count
            => Format == SecsFormat.List
            ? Unsafe.As<IList<Item>>(_values).Count
            : Unsafe.As<Array>(_values).Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal IList<Item> GetListItems()
        {
            return Format == SecsFormat.List ? Unsafe.As<IList<Item>>(_values) : ThrowHelper();
            static IList<Item> ThrowHelper() => throw new InvalidOperationException("The item is not a list");
        }

        /// <summary>
        /// Indexer for List items
        /// </summary>
        /// <exception cref="InvalidOperationException">if item is not a list</exception>
        public Item this[int index]
        {
            get => GetListItems()[index];
            set => GetListItems()[index] = value;
        }

        /// <summary>
        /// Get the first element of item array value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ref T FirstValue<T>() where T : unmanaged
        {
            var arr = GetArray();
            if (arr.Length == 0 || Buffer.ByteLength(arr) / Unsafe.SizeOf<T>() == 0)
            {
                ThrowHelper();
            }

            ref var data = ref MemoryMarshal.GetArrayDataReference(arr);
            return ref Unsafe.As<byte, T>(ref data);

            static void ThrowHelper() => throw new IndexOutOfRangeException("The item is empty or data length less than sizeof(T)");
        }

        /// <summary>
        /// Get the first element of item array value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ref readonly T GetFirstValueOrDefault<T>(in T defaultValue = default) where T : unmanaged
        {
            var arr = GetArray();
            if (arr.Length == 0 || Buffer.ByteLength(arr) / Unsafe.SizeOf<T>() == 0)
            {
                return ref defaultValue;
            }
            else
            {
                ref var data = ref MemoryMarshal.GetArrayDataReference(arr);
                return ref Unsafe.As<byte, T>(ref data);
            }
        }

        /// <summary>
        /// Get item value array wrapper
        /// </summary>
        public ValueArray<T> GetValues<T>() where T : unmanaged
            => new(GetArray());

        private Array GetArray()
        {
            if (Format == SecsFormat.List)
            {
                ThrowHelper(new InvalidOperationException("The item is a list"));
            }

            if (Format == SecsFormat.ASCII || Format == SecsFormat.JIS8)
            {
                ThrowHelper(new InvalidOperationException("The item is a string"));
            }

            if (_values is not Array arr)
            {
                ThrowHelper(new InvalidOperationException("The type is incompatible"));
            }

            return arr;

            static void ThrowHelper(Exception ex) => throw ex;
        }

        /// <summary>
        /// Get item string value
        /// </summary>
        /// <returns></returns>
        public string GetString()
            => Format != SecsFormat.ASCII && Format != SecsFormat.JIS8
            ? throw new InvalidOperationException("The type is incompatible")
            : Unsafe.As<string>(_values);

        public override bool Equals(object? obj) => Equals(obj as Item);
        public bool Equals(Item? other) => other is not null && IsMatch(other);
        private bool IsMatch(Item target)
        {
            if (Format != target.Format)
            {
                return false;
            }

            if (Count != target.Count)
            {
                return target.Count == 0;
            }

            if (Count == 0)
            {
                return true;
            }

            return target.Format switch
            {
                SecsFormat.List => IsMatch(Unsafe.As<IList<Item>>(_values), Unsafe.As<IList<Item>>(target._values)),
                SecsFormat.ASCII or SecsFormat.JIS8 => string.Equals(Unsafe.As<string>(_values), Unsafe.As<string>(target._values), StringComparison.Ordinal),
                _ => CompareArray(Unsafe.As<Array>(_values), Unsafe.As<Array>(target._values)),
            };

            static bool CompareArray(Array a, Array b)
            {
                var spanA = MemoryMarshal.CreateReadOnlySpan(ref MemoryMarshal.GetArrayDataReference(a), Buffer.ByteLength(a));
                var spanB = MemoryMarshal.CreateReadOnlySpan(ref MemoryMarshal.GetArrayDataReference(b), Buffer.ByteLength(b));
                return spanA.SequenceEqual(spanB);
            }

            static bool IsMatch(IList<Item> a, IList<Item> b)
            {
                for (int i = 0, count = a.Count; i < count; i++)
                {
                    if (!a[i].IsMatch(b[i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public override string ToString() => $"{Format}[{Count}]";

        private string GetDebugString()
        {
            var sb = new StringBuilder(Format.ToString()).Append('[').Append(Count).Append("]: ");
            return Format switch
            {
                SecsFormat.List => sb.Append("...").ToString(),
                SecsFormat.ASCII or SecsFormat.JIS8 => sb.Append(Unsafe.As<string>(_values)).ToString(),
                SecsFormat.Binary => AppendBinary(sb, Unsafe.As<byte[]>(_values)).ToString(),
                SecsFormat.Boolean => AppendArray<bool>(sb, _values).ToString(),
                SecsFormat.I1 => AppendArray<sbyte>(sb, _values).ToString(),
                SecsFormat.I2 => AppendArray<short>(sb, _values).ToString(),
                SecsFormat.I4 => AppendArray<int>(sb, _values).ToString(),
                SecsFormat.I8 => AppendArray<long>(sb, _values).ToString(),
                SecsFormat.U1 => AppendArray<byte>(sb, _values).ToString(),
                SecsFormat.U2 => AppendArray<ushort>(sb, _values).ToString(),
                SecsFormat.U4 => AppendArray<uint>(sb, _values).ToString(),
                SecsFormat.U8 => AppendArray<ulong>(sb, _values).ToString(),
                SecsFormat.F4 => AppendArray<float>(sb, _values).ToString(),
                SecsFormat.F8 => AppendArray<double>(sb, _values).ToString(),
                _ => sb.ToString(),
            };

            static StringBuilder AppendArray<T>(StringBuilder sb, object src) where T : unmanaged
            {
                ReadOnlySpan<T> arrary = Unsafe.As<T[]>(src);
                if (arrary.IsEmpty)
                {
                    return sb;
                }

                var len = Math.Min(arrary.Length, DebuggerDisplayMaxCount);
                for (int i = 0; i < len - 1; i++)
                {
                    sb.Append(arrary.DangerousGetReferenceAt(i).ToString()).Append(' ');
                }

                sb.Append(arrary.DangerousGetReferenceAt(len - 1));
                if (len < arrary.Length)
                {
                    sb.Append(" ...");
                }

                return sb;
            }

            static StringBuilder AppendBinary(StringBuilder sb, ReadOnlySpan<byte> arrary)
            {
                if (arrary.IsEmpty)
                {
                    return sb;
                }

                var len = Math.Min(arrary.Length, DebuggerDisplayMaxCount);
                for (int i = 0; i < len - 1; i++)
                {
                    AppendHexChars(sb, arrary.DangerousGetReferenceAt(i));
                    sb.Append(' ');
                }

                AppendHexChars(sb, arrary.DangerousGetReferenceAt(len - 1));
                if (len < arrary.Length)
                {
                    sb.Append(" ...");
                }

                return sb;

                static void AppendHexChars(StringBuilder sb, byte num)
                {
                    var hex1 = Math.DivRem(num, 0x10, out var hex0);
                    sb.Append(GetHexChar(hex1)).Append(GetHexChar(hex0));
                }

                static char GetHexChar(int i) => (i < 10) ? (char)(i + 0x30) : (char)(i - 10 + 0x41);
            }
        }

        /// <summary>
        /// Encode item to SECS binary format
        /// </summary>
        public void EncodeTo(IBufferWriter<byte> buffer)
            => _encode(this, buffer);

        public byte[] GetEncodedBytes()
        {
            using var buffer = new ArrayPoolBufferWriter<byte>();
            EncodeTo(buffer);
            return buffer.WrittenSpan.ToArray();
        }

        private sealed class EncodedItemDebugView
        {
            private readonly Item _item;
            public EncodedItemDebugView(Item item) => _item = item;
            public byte[] EncodedBytes => _item.GetEncodedBytes();
        }

        public override int GetHashCode()
            => throw new NotImplementedException("Secs4Net.Item is possible a large value object. You should implement a custom IEqualityComparer<Item> for your hash logic.");

        public IEnumerator<Item> GetEnumerator() => GetListItems().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
