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
		private readonly Lazy<byte[]> rawData;

		private readonly IEnumerable values;

		/// <summary>
		/// <para xml:lang="en">Initializes a <see langword="new"/> instance of the <see cref="Item"/> <see langword="class"/> for <see cref="SecsFormat.List"/>.</para>
		/// </summary>
		/// <param name="items">The items of the <see langword="new"/> instance.</param>
		/// <exception cref="ArgumentOutOfRangeException"><see cref="IReadOnlyCollection{T}.Count"/> of <paramref name="items"/> is greater than <see cref="ItemHeader.MaxLength"/>.</exception>
		private Item(IReadOnlyList<Item> items)
		{
			if (items.Count > ItemHeader.MaxLength)
			{
				throw new ArgumentOutOfRangeException(nameof(items) + "." + nameof(items.Count), items.Count, $"List items length out of range, max length: {ItemHeader.MaxLength}");
			}

			this.Format = SecsFormat.List;
			this.values = items;
			this.rawData = new Lazy<byte[]>(() => new ItemHeader(SecsFormat.List, Unsafe.As<IReadOnlyList<Item>>(this.values).Count).GetRawHeaderData());
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
			switch (format)
			{
				case SecsFormat.Binary:
				case SecsFormat.Boolean:
				case SecsFormat.I8:
				case SecsFormat.I1:
				case SecsFormat.I2:
				case SecsFormat.I4:
				case SecsFormat.F8:
				case SecsFormat.F4:
				case SecsFormat.U8:
				case SecsFormat.U1:
				case SecsFormat.U2:
				case SecsFormat.U4:
					break;

				default:
					throw new NotImplementedException($"The format \"{nameof(SecsFormat)}.{format}\" is not implemented within this constructor.");
			}

			this.Format = format;
			this.values = value;
			this.rawData = new Lazy<byte[]>(() =>
			{
				var array = Unsafe.As<Array>(this.values);

				var bytelength = Buffer.ByteLength(array);

				var itemHeader = new ItemHeader(this.Format, bytelength);

				var (result, headerLength) = itemHeader.GetItemBufferWithRawHeaderData();

				Buffer.BlockCopy(array, 0, result, headerLength, bytelength);

				if (BitConverter.IsLittleEndian)
				{
					result.Reverse(headerLength, headerLength + bytelength, bytelength / array.Length);
				}

				return result;
			});
		}

		/// <summary>
		/// A,J
		/// </summary>
		private Item(SecsFormat format, string value)
		{
			switch (format)
			{
				case SecsFormat.ASCII:
				case SecsFormat.JIS8:
					break;

				default:
					throw new NotImplementedException($"The format \"{nameof(SecsFormat)}.{format}\" is not implemented within this constructor.");
			}

			this.Format = format;
			this.values = value;
			this.rawData = new Lazy<byte[]>(() =>
			{
				var stringValue = Unsafe.As<string>(this.values);

				var itemHeader = new ItemHeader(this.Format, stringValue.Length);

				var (result, headerLength) = itemHeader.GetItemBufferWithRawHeaderData();

				var encoder = this.Format == SecsFormat.ASCII ? Encoding.ASCII : Item.Jis8Encoding;

				encoder.GetBytes(stringValue, 0, stringValue.Length, result, headerLength);

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
			this.Format = format;
			this.values = value;
			this.rawData = new Lazy<byte[]>(() => new ItemHeader(this.Format, 0).GetRawHeaderData());
		}

		public SecsFormat Format { get; }

		public int Count =>
			this.Format == SecsFormat.List
			? Unsafe.As<IReadOnlyList<Item>>(this.values).Count
			: Unsafe.As<Array>(this.values).Length;

		public IReadOnlyList<byte> RawBytes => this.rawData.Value;

		/// <summary>
		/// List items
		/// </summary>
		public IReadOnlyList<Item> Items => this.Format != SecsFormat.List
			? throw new InvalidOperationException("The item is not a list")
			: Unsafe.As<IReadOnlyList<Item>>(this.values);

		/// <summary>
		/// get value by specific type
		/// </summary>
		public T GetValue<T>() where T : unmanaged
		{
			if (this.Format == SecsFormat.List)
			{
				throw new InvalidOperationException("The item is a list");
			}

			if (this.Format == SecsFormat.ASCII || this.Format == SecsFormat.JIS8)
			{
				throw new InvalidOperationException("The item is a string");
			}

			if (this.values is T[] arr)
			{
				return arr[0];
			}

			throw new InvalidOperationException("The type is incompatible");
		}

		public string GetString() => this.Format != SecsFormat.ASCII && this.Format != SecsFormat.JIS8
			? throw new InvalidOperationException("The type is incompatible")
			: Unsafe.As<string>(this.values);

		/// <summary>
		/// get value array by specific type
		/// </summary>
		public T[] GetValues<T>() where T : unmanaged
		{
			if (this.Format == SecsFormat.List)
			{
				throw new InvalidOperationException("The item is list");
			}

			if (this.Format == SecsFormat.ASCII || this.Format == SecsFormat.JIS8)
			{
				throw new InvalidOperationException("The item is a string");
			}

			if (this.values is T[] arr)
			{
				return arr;
			}

			throw new InvalidOperationException("The type is incompatible");
		}

		public bool IsMatch(Item target)
		{
			if (object.ReferenceEquals(this, target))
			{
				return true;
			}

			if (this.Format != target.Format)
			{
				return false;
			}

			if (this.Count != target.Count)
			{
				return target.Count == 0;
			}

			if (this.Count == 0)
			{
				return true;
			}

			switch (target.Format)
			{
				case SecsFormat.List:
					return IsMatch(
						Unsafe.As<IReadOnlyList<Item>>(this.values),
						Unsafe.As<IReadOnlyList<Item>>(target.values));
				case SecsFormat.ASCII:
				case SecsFormat.JIS8:
					return Unsafe.As<string>(this.values) == Unsafe.As<string>(target.values);
				default:
					//return memcmp(Unsafe.As<byte[]>(_values), Unsafe.As<byte[]>(target._values), Buffer.ByteLength((Array)_values)) == 0;
					return UnsafeCompare(Unsafe.As<Array>(this.values), Unsafe.As<Array>(target.values));
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
					{
						if (*((long*)x1) != *((long*)x2))
						{
							return false;
						}
					}

					if ((l & 4) != 0) { if (*((int*)x1) != *((int*)x2)) { return false; } x1 += 4; x2 += 4; }
					if ((l & 2) != 0) { if (*((short*)x1) != *((short*)x2)) { return false; } x1 += 2; x2 += 2; }
					if ((l & 1) != 0)
					{
						if (*x1 != *x2)
						{
							return false;
						}
					}

					return true;
				}
			}

			bool IsMatch(IReadOnlyList<Item> a, IReadOnlyList<Item> b)
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
			var sb = new StringBuilder(this.Format.GetName()).Append("[");

			switch (this.Format)
			{
				case SecsFormat.List:
					sb.Append(Unsafe.As<IReadOnlyList<Item>>(this.values).Count).Append("]: ...");
					break;

				case SecsFormat.ASCII:
				case SecsFormat.JIS8:
					sb.Append(Unsafe.As<string>(this.values).Length).Append("]: ").Append(Unsafe.As<string>(this.values));
					break;

				case SecsFormat.Binary:
					sb.Append(Unsafe.As<byte[]>(this.values).Length).Append("]: ").Append(Unsafe.As<byte[]>(this.values).ToHexString());
					break;

				default:
					sb.Append(Unsafe.As<Array>(this.values).Length).Append("]: ");
					switch (this.Format)
					{
						case SecsFormat.Boolean: AppendAsString<bool>(sb, this.values); break;
						case SecsFormat.I1: AppendAsString<sbyte>(sb, this.values); break;
						case SecsFormat.I2: AppendAsString<short>(sb, this.values); break;
						case SecsFormat.I4: AppendAsString<int>(sb, this.values); break;
						case SecsFormat.I8: AppendAsString<long>(sb, this.values); break;
						case SecsFormat.U1: AppendAsString<byte>(sb, this.values); break;
						case SecsFormat.U2: AppendAsString<ushort>(sb, this.values); break;
						case SecsFormat.U4: AppendAsString<uint>(sb, this.values); break;
						case SecsFormat.U8: AppendAsString<ulong>(sb, this.values); break;
						case SecsFormat.F4: AppendAsString<float>(sb, this.values); break;
						case SecsFormat.F8: AppendAsString<double>(sb, this.values); break;
					}
					break;
			}

			return sb.ToString();

			void AppendAsString<T>(StringBuilder stringBuilder, IEnumerable src)
				where T : unmanaged
			{
				var array = Unsafe.As<T[]>(src);

				if (array.Length == 0)
				{
					return;
				}

				stringBuilder.Append(array[0]);

				for (int i = 1; i < array.Length; i++)
				{
					stringBuilder.Append(' ');
					stringBuilder.Append(array[i]);
				}
			}
		}

		#region Type Casting Operator

		public static implicit operator string(Item item) => item.GetString();
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

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.List"/>.</para>
		/// </summary>
		/// <param name="items">The <see cref="IList{T}"/> of <see cref="Item"/> to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.List"/>.</para></returns>
		internal static Item L(IList<Item> items) => items?.Count > 0 ? new Item(new ReadOnlyCollection<Item>(items)) : Item.L();

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.List"/>.</para>
		/// </summary>
		/// <param name="items">The <see cref="IEnumerable{T}"/> of <see cref="Item"/> to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.List"/>.</para></returns>
		public static Item L(IEnumerable<Item> items) => Item.L(items?.ToList());

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.List"/>.</para>
		/// </summary>
		/// <param name="items">The <see cref="Item"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.List"/>.</para></returns>
		public static Item L(params Item[] items) => Item.L((IList<Item>)items);

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.Binary"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="byte"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.Binary"/>.</para></returns>
		public static Item B(params byte[] value) => value?.Length > 0 ? new Item(SecsFormat.Binary, value) : Item.B();

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.Binary"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="byte"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.Binary"/>.</para></returns>
		public static Item B(IEnumerable<byte> value) => Item.B(value?.ToArray());

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.U1"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="byte"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.U1"/>.</para></returns>
		public static Item U1(params byte[] value) => value?.Length > 0 ? new Item(SecsFormat.U1, value) : Item.U1();

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.U1"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="byte"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.U1"/>.</para></returns>
		public static Item U1(IEnumerable<byte> value) => U1(value?.ToArray());

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.U2"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="ushort"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.U2"/>.</para></returns>
		public static Item U2(params ushort[] value) => value?.Length > 0 ? new Item(SecsFormat.U2, value) : Item.U2();

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.U2"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="ushort"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.U2"/>.</para></returns>
		public static Item U2(IEnumerable<ushort> value) => Item.U2(value?.ToArray());

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.U4"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="uint"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.U4"/>.</para></returns>
		public static Item U4(params uint[] value) => value?.Length > 0 ? new Item(SecsFormat.U4, value) : Item.U4();

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.U4"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="uint"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.U4"/>.</para></returns>
		public static Item U4(IEnumerable<uint> value) => Item.U4(value?.ToArray());

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.U8"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="ulong"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.U8"/>.</para></returns>
		public static Item U8(params ulong[] value) => value?.Length > 0 ? new Item(SecsFormat.U8, value) : Item.U8();

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.U8"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="ulong"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.U8"/>.</para></returns>
		public static Item U8(IEnumerable<ulong> value) => Item.U8(value?.ToArray());

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.I1"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="sbyte"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.I1"/>.</para></returns>
		public static Item I1(params sbyte[] value) => value?.Length > 0 ? new Item(SecsFormat.I1, value) : Item.I1();

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.I1"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="sbyte"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.I1"/>.</para></returns>
		public static Item I1(IEnumerable<sbyte> value) => Item.I1(value?.ToArray());

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.I2"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="short"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.I2"/>.</para></returns>
		public static Item I2(params short[] value) => value?.Length > 0 ? new Item(SecsFormat.I2, value) : Item.I2();

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.I2"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="short"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.I2"/>.</para></returns>
		public static Item I2(IEnumerable<short> value) => Item.I2(value?.ToArray());

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.I4"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="int"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.I4"/>.</para></returns>
		public static Item I4(params int[] value) => value?.Length > 0 ? new Item(SecsFormat.I4, value) : Item.I4();

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.I4"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="int"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.I4"/>.</para></returns>
		public static Item I4(IEnumerable<int> value) => Item.I4(value?.ToArray());

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.I8"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="long"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.I8"/>.</para></returns>
		public static Item I8(params long[] value) => value?.Length > 0 ? new Item(SecsFormat.I8, value) : Item.I8();

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.I8"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="long"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.I8"/>.</para></returns>
		public static Item I8(IEnumerable<long> value) => Item.I8(value?.ToArray());

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.F4"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="float"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.F4"/>.</para></returns>
		public static Item F4(params float[] value) => value?.Length > 0 ? new Item(SecsFormat.F4, value) : Item.F4();

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.F4"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="float"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.F4"/>.</para></returns>
		public static Item F4(IEnumerable<float> value) => Item.F4(value?.ToArray());

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.F8"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="double"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.F8"/>.</para></returns>
		public static Item F8(params double[] value) => value?.Length > 0 ? new Item(SecsFormat.F8, value) : Item.F8();

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.F8"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="double"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.F8"/>.</para></returns>
		public static Item F8(IEnumerable<double> value) => Item.F8(value?.ToArray());

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.Boolean"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="bool"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.Boolean"/>.</para></returns>
		public static Item Boolean(params bool[] value) => value?.Length > 0 ? new Item(SecsFormat.Boolean, value) : Item.Boolean();

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.Boolean"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="bool"/>(s) to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.Boolean"/>.</para></returns>
		public static Item Boolean(IEnumerable<bool> value) => Item.Boolean(value?.ToArray());

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.ASCII"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="string"/> to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.ASCII"/>.</para></returns>
		public static Item A(string value) => !string.IsNullOrEmpty(value) ? new Item(SecsFormat.ASCII, value) : Item.A();

		/// <summary>
		/// <para xml:lang="en">Returns an <see cref="Item"/> of <see cref="SecsFormat.JIS8"/>.</para>
		/// </summary>
		/// <param name="value">The <see cref="string"/> to be used.</param>
		/// <returns><para xml:lang="en">An <see cref="Item"/> of <see cref="SecsFormat.JIS8"/>.</para></returns>
		public static Item J(string value) => !string.IsNullOrEmpty(value) ? new Item(SecsFormat.JIS8, value) : Item.J();

		#endregion

		#region Share Object

		/// <summary>
		/// <para xml:lang="en">Returns an empty <see cref="Item"/> of <see cref="SecsFormat.List"/>.</para>
		/// </summary>
		/// <returns><para xml:lang="en">An empty <see cref="Item"/> of <see cref="SecsFormat.List"/>.</para></returns>
		public static Item L() => Item.EmptyL;

		/// <summary>
		/// <para xml:lang="en">Returns an empty <see cref="Item"/> of <see cref="SecsFormat.Binary"/>.</para>
		/// </summary>
		/// <returns><para xml:lang="en">An empty <see cref="Item"/> of <see cref="SecsFormat.Binary"/>.</para></returns>
		public static Item B() => Item.EmptyBinary;

		/// <summary>
		/// <para xml:lang="en">Returns an empty <see cref="Item"/> of <see cref="SecsFormat.U1"/>.</para>
		/// </summary>
		/// <returns><para xml:lang="en">An empty <see cref="Item"/> of <see cref="SecsFormat.U1"/>.</para></returns>
		public static Item U1() => Item.EmptyU1;

		/// <summary>
		/// <para xml:lang="en">Returns an empty <see cref="Item"/> of <see cref="SecsFormat.U2"/>.</para>
		/// </summary>
		/// <returns><para xml:lang="en">An empty <see cref="Item"/> of <see cref="SecsFormat.U2"/>.</para></returns>
		public static Item U2() => Item.EmptyU2;

		/// <summary>
		/// <para xml:lang="en">Returns an empty <see cref="Item"/> of <see cref="SecsFormat.U4"/>.</para>
		/// </summary>
		/// <returns><para xml:lang="en">An empty <see cref="Item"/> of <see cref="SecsFormat.U4"/>.</para></returns>
		public static Item U4() => Item.EmptyU4;

		/// <summary>
		/// <para xml:lang="en">Returns an empty <see cref="Item"/> of <see cref="SecsFormat.U8"/>.</para>
		/// </summary>
		/// <returns><para xml:lang="en">An empty <see cref="Item"/> of <see cref="SecsFormat.U8"/>.</para></returns>
		public static Item U8() => Item.EmptyU8;

		/// <summary>
		/// <para xml:lang="en">Returns an empty <see cref="Item"/> of <see cref="SecsFormat.I1"/>.</para>
		/// </summary>
		/// <returns><para xml:lang="en">An empty <see cref="Item"/> of <see cref="SecsFormat.I1"/>.</para></returns>
		public static Item I1() => Item.EmptyI1;

		/// <summary>
		/// <para xml:lang="en">Returns an empty <see cref="Item"/> of <see cref="SecsFormat.I2"/>.</para>
		/// </summary>
		/// <returns><para xml:lang="en">An empty <see cref="Item"/> of <see cref="SecsFormat.I2"/>.</para></returns>
		public static Item I2() => Item.EmptyI2;

		/// <summary>
		/// <para xml:lang="en">Returns an empty <see cref="Item"/> of <see cref="SecsFormat.I4"/>.</para>
		/// </summary>
		/// <returns><para xml:lang="en">An empty <see cref="Item"/> of <see cref="SecsFormat.I4"/>.</para></returns>
		public static Item I4() => Item.EmptyI4;

		/// <summary>
		/// <para xml:lang="en">Returns an empty <see cref="Item"/> of <see cref="SecsFormat.I8"/>.</para>
		/// </summary>
		/// <returns><para xml:lang="en">An empty <see cref="Item"/> of <see cref="SecsFormat.I8"/>.</para></returns>
		public static Item I8() => Item.EmptyI8;

		/// <summary>
		/// <para xml:lang="en">Returns an empty <see cref="Item"/> of <see cref="SecsFormat.F4"/>.</para>
		/// </summary>
		/// <returns><para xml:lang="en">An empty <see cref="Item"/> of <see cref="SecsFormat.F4"/>.</para></returns>
		public static Item F4() => Item.EmptyF4;

		/// <summary>
		/// <para xml:lang="en">Returns an empty <see cref="Item"/> of <see cref="SecsFormat.F8"/>.</para>
		/// </summary>
		/// <returns><para xml:lang="en">An empty <see cref="Item"/> of <see cref="SecsFormat.F8"/>.</para></returns>
		public static Item F8() => Item.EmptyF8;

		/// <summary>
		/// <para xml:lang="en">Returns an empty <see cref="Item"/> of <see cref="SecsFormat.Boolean"/>.</para>
		/// </summary>
		/// <returns><para xml:lang="en">An empty <see cref="Item"/> of <see cref="SecsFormat.Boolean"/>.</para></returns>
		public static Item Boolean() => Item.EmptyBoolean;

		/// <summary>
		/// <para xml:lang="en">Returns an empty <see cref="Item"/> of <see cref="SecsFormat.ASCII"/>.</para>
		/// </summary>
		/// <returns><para xml:lang="en">An empty <see cref="Item"/> of <see cref="SecsFormat.ASCII"/>.</para></returns>
		public static Item A() => Item.EmptyA;

		/// <summary>
		/// <para xml:lang="en">Returns an empty <see cref="Item"/> of <see cref="SecsFormat.JIS8"/>.</para>
		/// </summary>
		/// <returns><para xml:lang="en">An empty <see cref="Item"/> of <see cref="SecsFormat.JIS8"/>.</para></returns>
		public static Item J() => Item.EmptyJ;

		private static readonly Item EmptyL = new Item(SecsFormat.List, Enumerable.Empty<Item>());
		private static readonly Item EmptyA = new Item(SecsFormat.ASCII, string.Empty);
		private static readonly Item EmptyJ = new Item(SecsFormat.JIS8, string.Empty);
		private static readonly Item EmptyBoolean = new Item(SecsFormat.Boolean, Enumerable.Empty<bool>());
		private static readonly Item EmptyBinary = new Item(SecsFormat.Binary, Enumerable.Empty<byte>());
		private static readonly Item EmptyU1 = new Item(SecsFormat.U1, Enumerable.Empty<byte>());
		private static readonly Item EmptyU2 = new Item(SecsFormat.U2, Enumerable.Empty<ushort>());
		private static readonly Item EmptyU4 = new Item(SecsFormat.U4, Enumerable.Empty<uint>());
		private static readonly Item EmptyU8 = new Item(SecsFormat.U8, Enumerable.Empty<ulong>());
		private static readonly Item EmptyI1 = new Item(SecsFormat.I1, Enumerable.Empty<sbyte>());
		private static readonly Item EmptyI2 = new Item(SecsFormat.I2, Enumerable.Empty<short>());
		private static readonly Item EmptyI4 = new Item(SecsFormat.I4, Enumerable.Empty<int>());
		private static readonly Item EmptyI8 = new Item(SecsFormat.I8, Enumerable.Empty<long>());
		private static readonly Item EmptyF4 = new Item(SecsFormat.F4, Enumerable.Empty<float>());
		private static readonly Item EmptyF8 = new Item(SecsFormat.F8, Enumerable.Empty<double>());

		private static readonly Encoding Jis8Encoding = Encoding.GetEncoding(50222);
		#endregion

		/// <summary>
		/// Encode item to raw data buffer
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		internal uint EncodeTo(List<ArraySegment<byte>> buffer)
		{
			var bytes = this.rawData.Value;
			uint length = unchecked((uint)bytes.Length);
			buffer.Add(new ArraySegment<byte>(bytes));
			if (this.Format == SecsFormat.List)
			{
				foreach (var subItem in this.Items)
				{
					length += subItem.EncodeTo(buffer);
				}
			}

			return length;
		}

		internal static Item BytesDecode(SecsFormat format, byte[] data, int index, int length)
		{
			switch (format)
			{
				case SecsFormat.ASCII: return length == 0 ? A() : A(Encoding.ASCII.GetString(data, index, length));
				case SecsFormat.JIS8: return length == 0 ? J() : J(Jis8Encoding.GetString(data, index, length));
				case SecsFormat.Boolean: return length == 0 ? Boolean() : Boolean(Decode<bool>(data, index, length));
				case SecsFormat.Binary: return length == 0 ? B() : B(Decode<byte>(data, index, length));
				case SecsFormat.U1: return length == 0 ? U1() : U1(Decode<byte>(data, index, length));
				case SecsFormat.U2: return length == 0 ? U2() : U2(Decode<ushort>(data, index, length));
				case SecsFormat.U4: return length == 0 ? U4() : U4(Decode<uint>(data, index, length));
				case SecsFormat.U8: return length == 0 ? U8() : U8(Decode<ulong>(data, index, length));
				case SecsFormat.I1: return length == 0 ? I1() : I1(Decode<sbyte>(data, index, length));
				case SecsFormat.I2: return length == 0 ? I2() : I2(Decode<short>(data, index, length));
				case SecsFormat.I4: return length == 0 ? I4() : I4(Decode<int>(data, index, length));
				case SecsFormat.I8: return length == 0 ? I8() : I8(Decode<long>(data, index, length));
				case SecsFormat.F4: return length == 0 ? F4() : F4(Decode<float>(data, index, length));
				case SecsFormat.F8: return length == 0 ? F8() : F8(Decode<double>(data, index, length));
				default: throw new ArgumentException("Invalid format", nameof(format));
			}

			T[] Decode<T>(byte[] data2, int index2, int length2) where T : unmanaged
			{
				var elmSize = Unsafe.SizeOf<T>();
				data2.Reverse(index2, index2 + length2, elmSize);
				var values = new T[length2 / elmSize];
				Buffer.BlockCopy(data2, index2, values, 0, length2);
				return values;
			}
		}
	}
}