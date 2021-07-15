using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Secs4Net
{
    public unsafe sealed partial class Item
    {
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
        private readonly unsafe delegate*<Item, IBufferWriter<byte>, void> _encode;

        public SecsFormat Format { get; }

        public int Count
            => Format == SecsFormat.List
            ? Unsafe.As<IReadOnlyList<Item>>(_values).Count
            : Unsafe.As<Array>(_values).Length;

        /// <summary>
        /// List items
        /// </summary>
        public IReadOnlyList<Item> Items
            => Format != SecsFormat.List
            ? throw new InvalidOperationException("The item is not a list")
            : Unsafe.As<IReadOnlyList<Item>>(_values);

        /// <summary>
        /// Get the first element of item array value
        /// </summary>
        public ref readonly T GetValue<T>() where T : unmanaged
        {
            T[] arr = GetItemArray<T>();

            if (arr.Length == 0)
            {
                throw new IndexOutOfRangeException("The item is empty");
            }

            ref var data = ref MemoryMarshal.GetArrayDataReference(arr);
            return ref Unsafe.Add(ref data, 0);
        }

        /// <summary>
        /// Get the first element of item array value
        /// </summary>
        public ref readonly T GetValueOrDefault<T>(in T defaultValue = default) where T : unmanaged
        {
            T[] arr = GetItemArray<T>();

            if (arr.Length > 0)
            {
                ref var data = ref MemoryMarshal.GetArrayDataReference(arr);
                return ref Unsafe.Add(ref data, 0);
            }
            else
            {
                return ref defaultValue;
            }
        }

        /// <summary>
        /// Get item array value
        /// </summary>
        public IReadOnlyList<T> GetValues<T>() where T : unmanaged
            => GetItemArray<T>();

        private T[] GetItemArray<T>() where T : unmanaged
        {
            if (_values is T[] arr)
            {
                return arr;
            }

            if (Format == SecsFormat.List)
            {
                throw new InvalidOperationException("The item is a list");
            }

            if (Format == SecsFormat.ASCII || Format == SecsFormat.JIS8)
            {
                throw new InvalidOperationException("The item is a string");
            }

            throw new InvalidOperationException("The type is incompatible");
        }

        /// <summary>
        /// Get item string value
        /// </summary>
        /// <returns></returns>
        public string GetString()
            => Format != SecsFormat.ASCII && Format != SecsFormat.JIS8
            ? throw new InvalidOperationException("The type is incompatible")
            : Unsafe.As<string>(_values);

        public bool IsMatch(Item target)
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
                SecsFormat.List => IsMatch(Unsafe.As<IReadOnlyList<Item>>(_values), Unsafe.As<IReadOnlyList<Item>>(target._values)),
                SecsFormat.ASCII or SecsFormat.JIS8 => string.Equals(Unsafe.As<string>(_values), Unsafe.As<string>(target._values), StringComparison.Ordinal),
                _ => CompareArray(Unsafe.As<Array>(_values), Unsafe.As<Array>(target._values)),
            };

            static bool CompareArray(Array a, Array b)
            {
                var spanA = MemoryMarshal.CreateReadOnlySpan(ref MemoryMarshal.GetArrayDataReference(a), Buffer.ByteLength(a));
                var spanB = MemoryMarshal.CreateReadOnlySpan(ref MemoryMarshal.GetArrayDataReference(b), Buffer.ByteLength(b));
                return spanA.SequenceEqual(spanB);
            }

            static bool IsMatch(IReadOnlyList<Item> a, IReadOnlyList<Item> b)
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

        public override string ToString()
        {
            var sb = new StringBuilder(Format.ToString()).Append('[');
            switch (Format)
            {
                case SecsFormat.List:
                    sb.Append(Unsafe.As<IReadOnlyList<Item>>(_values).Count).Append("]: ...");
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
                    break;
            }
            return sb.ToString();

            static string JoinAsString<T>(object src) where T : unmanaged
                => string.Join(" ", Unsafe.As<T[]>(src));
        }

        /// <summary>
        /// Encode item to SECS binary format
        /// </summary>
        public void EncodeTo(IBufferWriter<byte> buffer)
            => _encode(this, buffer);

        internal static Item BytesDecode(SecsFormat format, byte[] data, int index, int length)
        {
            return format switch
            {
                SecsFormat.ASCII => length == 0 ? A() : A(Encoding.ASCII.GetString(data, index, length)),
                SecsFormat.JIS8 => length == 0 ? J() : J(Jis8Encoding.GetString(data, index, length)),
                SecsFormat.Boolean => length == 0 ? Boolean() : Boolean(Decode<bool>(data, index, length)),
                SecsFormat.Binary => length == 0 ? B() : B(Decode<byte>(data, index, length)),
                SecsFormat.U1 => length == 0 ? U1() : U1(Decode<byte>(data, index, length)),
                SecsFormat.U2 => length == 0 ? U2() : U2(Decode<ushort>(data, index, length)),
                SecsFormat.U4 => length == 0 ? U4() : U4(Decode<uint>(data, index, length)),
                SecsFormat.U8 => length == 0 ? U8() : U8(Decode<ulong>(data, index, length)),
                SecsFormat.I1 => length == 0 ? I1() : I1(Decode<sbyte>(data, index, length)),
                SecsFormat.I2 => length == 0 ? I2() : I2(Decode<short>(data, index, length)),
                SecsFormat.I4 => length == 0 ? I4() : I4(Decode<int>(data, index, length)),
                SecsFormat.I8 => length == 0 ? I8() : I8(Decode<long>(data, index, length)),
                SecsFormat.F4 => length == 0 ? F4() : F4(Decode<float>(data, index, length)),
                SecsFormat.F8 => length == 0 ? F8() : F8(Decode<double>(data, index, length)),
                _ => throw new ArgumentException(@"Invalid format", nameof(format)),
            };

            static T[] Decode<T>(byte[] data, int index, int length) where T : unmanaged
            {
                var elmSize = Unsafe.SizeOf<T>();
                data.Reverse(index, index + length, elmSize);
                var values = new T[length / elmSize];
                Buffer.BlockCopy(data, index, values, 0, length);
                return values;
            }
        }
    }
}
