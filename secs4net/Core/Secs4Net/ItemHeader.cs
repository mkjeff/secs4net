using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Secs4Net
{
	/// <summary>
	/// <para xml:lang="en">Represents the header of an <see cref="Item"/>.</para>
	/// </summary>
	[DebuggerDisplay(nameof(ItemHeader) + ": " + nameof(IsValid) + ": {" + nameof(IsValid) + "}")]
	[StructLayout(LayoutKind.Explicit)]
	internal struct ItemHeader
	{
		/// <summary>
		/// <para xml:lang="en">The maximum item length.</para>
		/// </summary>
		public const int MaxLength = 0x00FFFFFF;

		[FieldOffset(0)]
		private readonly int itemLength;

		[FieldOffset(0)]
		private readonly byte lengthByteThree;

		[FieldOffset(1)]
		private readonly byte lengthByteTwo;

		[FieldOffset(2)]
		private readonly byte lengthByteOne;

		[FieldOffset(3)]
		private readonly byte itemFormatCodeAndNumberOfLengthBytes;

		/// <summary>
		/// <para xml:lang="en">Initializes a <see langword="new"/> instance of the <see cref="ItemHeader"/> <see langword="struct"/> .</para>
		/// </summary>
		/// <param name="secsFormat"><para xml:lang="en">The <see cref="SecsFormat"/> of this instance.</para></param>
		/// <param name="itemLength"><para xml:lang="en">The length of the item of this instance.</para></param>
		/// <exception cref="ArgumentOutOfRangeException"><para xml:lang="en"><paramref name="itemLength"/> is less than zero (0).</para></exception>
		/// <exception cref="ArgumentOutOfRangeException"><para xml:lang="en"><paramref name="itemLength"/> greater than <see cref="ItemHeader.MaxLength"/>.</para></exception>
		public ItemHeader(SecsFormat secsFormat, int itemLength)
		{
			if (itemLength < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(itemLength), itemLength, "The argument is less than zero (0).");
			}
			if (itemLength > ItemHeader.MaxLength)
			{
				throw new ArgumentOutOfRangeException(nameof(itemLength), itemLength, $"The argument is greater than {ItemHeader.MaxLength}.");
			}

			this.itemFormatCodeAndNumberOfLengthBytes = 0;
			this.lengthByteOne = 0;
			this.lengthByteTwo = 0;
			this.lengthByteThree = 0;

			this.itemLength = itemLength;

			if (!BitConverter.IsLittleEndian)
			{
				byte temp = this.lengthByteThree;
				this.lengthByteThree = this.itemFormatCodeAndNumberOfLengthBytes;
				this.itemFormatCodeAndNumberOfLengthBytes = temp;

				temp = this.lengthByteTwo;
				this.lengthByteTwo = this.lengthByteOne;
				this.lengthByteOne = temp;
			}

			if (itemLength <= byte.MaxValue)
			{
				this.itemFormatCodeAndNumberOfLengthBytes = (byte)((int)secsFormat | 1);
			}
			else if (itemLength <= ushort.MaxValue)
			{
				this.itemFormatCodeAndNumberOfLengthBytes = (byte)((int)secsFormat | 2);
			}
			else
			{
				this.itemFormatCodeAndNumberOfLengthBytes = (byte)((int)secsFormat | 3);
			}
		}

		/// <summary>
		/// <para xml:lang="en">Gets the format of this instance.</para>
		/// </summary>
		public SecsFormat Format => (SecsFormat)(this.itemFormatCodeAndNumberOfLengthBytes & 0b_111111_00);

		/// <summary>
		/// <para xml:lang="en">Gets a value indicating whether this instance is valid or not.</para>
		/// </summary>
		public bool IsValid
		{
			get
			{
				return (this.NumberOfLengthBytes == 1
					|| this.NumberOfLengthBytes == 2
					|| this.NumberOfLengthBytes == 3)
					&& Enum.IsDefined(typeof(SecsFormat), this.Format);
			}
		}

		/// <summary>
		/// <para xml:lang="en">Gets the item length of this instance.</para>
		/// </summary>
		public int ItemLength
		{
			get
			{
				if (BitConverter.IsLittleEndian)
				{
					return this.itemLength & 0x00FFFFFF;
				}
				else
				{
					return this.lengthByteOne | (this.lengthByteTwo << 8) | (this.lengthByteThree << 16);
				}
			}
		}

		/// <summary>
		/// <para xml:lang="en">Gets the number of length bytes of this instance.</para>
		/// </summary>
		public int NumberOfLengthBytes => this.itemFormatCodeAndNumberOfLengthBytes & 0b_000000_11;

		/// <summary>
		/// <para xml:lang="en">Gets the RAW header data size of this instance.</para>
		/// </summary>
		public int RawHeaderDataSize => this.NumberOfLengthBytes + 1;

		/// <summary>
		/// <para xml:lang="en">Gets a <see langword="new"/> item buffer with the RAW header data of this instance and all the space needed for the item values.</para>
		/// </summary>
		/// <exception cref="InvalidOperationException"><para xml:lang="en">This instance does not have valid number of length bytes. Valid are 1, 2 or 3.</para></exception>
		/// <returns><para xml:lang="en">The <see langword="new"/> item buffer including RAW header data of this instance and an offset of where to begin writing the item values.</para></returns>
		public (byte[], int) GetItemBufferWithRawHeaderData()
		{
			var headerSize = this.RawHeaderDataSize;
			var buffer = new byte[headerSize + this.ItemLength];

			this.InternalWriteRawHeaderData(buffer, 0);

			return (buffer, headerSize);
		}

		/// <summary>
		/// <para xml:lang="en">Gets the RAW header data of this instance.</para>
		/// </summary>
		/// <exception cref="InvalidOperationException"><para xml:lang="en">This instance does not have valid number of length bytes. Valid are 1, 2 or 3.</para></exception>
		/// <returns><para xml:lang="en">The RAW header data of this instance.</para></returns>
		public byte[] GetRawHeaderData()
		{
			switch (this.NumberOfLengthBytes)
			{
				case 1:
					return new byte[] { this.itemFormatCodeAndNumberOfLengthBytes, this.lengthByteThree };

				case 2:
					return new byte[] { this.itemFormatCodeAndNumberOfLengthBytes, this.lengthByteTwo, this.lengthByteThree };

				case 3:
					return new byte[] { this.itemFormatCodeAndNumberOfLengthBytes, this.lengthByteOne, this.lengthByteTwo, this.lengthByteThree };

				default:
					throw new InvalidOperationException($"This instance does not have valid number of length bytes. Valid are 1, 2 or 3, however the current value is {this.NumberOfLengthBytes}.");
			}
		}

		/// <summary>
		/// <para xml:lang="en">Writes the RAW header data of this instance into the specified <paramref name="buffer"/> at the specified <paramref name="offset"/>.</para>
		/// </summary>
		/// <exception cref="ArgumentNullException"><para xml:lang="en"><paramref name="buffer"/> is <see langword="null"/>.</para></exception>
		/// <exception cref="ArgumentOutOfRangeException"><para xml:lang="en"><paramref name="offset"/> is less than zero (0).</para></exception>
		/// <exception cref="ArgumentOutOfRangeException"><para xml:lang="en"><see cref="Array.Length"/> of <paramref name="buffer"/> is less than <paramref name="offset"/> + <see cref="ItemHeader.RawHeaderDataSize"/>.</para></exception>
		/// <exception cref="InvalidOperationException"><para xml:lang="en">This instance does not have valid number of length bytes. Valid are 1, 2 or 3.</para></exception>
		public void WriteRawHeaderData(byte[] buffer, int offset)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException(nameof(buffer));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(offset), offset, "The argument is less than 0.");
			}
			if (buffer.Length < (offset + this.RawHeaderDataSize))
			{
				throw new ArgumentOutOfRangeException(nameof(buffer), buffer.Length, $"The length of the specified {nameof(buffer)} is less than {nameof(offset)} + {this.RawHeaderDataSize}.");
			}

			this.InternalWriteRawHeaderData(buffer, offset);
		}

		private void InternalWriteRawHeaderData(byte[] buffer, int offset)
		{
			switch (this.NumberOfLengthBytes)
			{
				case 1:
					buffer[offset] = this.itemFormatCodeAndNumberOfLengthBytes;
					buffer[offset + 1] = this.lengthByteThree;
					break;

				case 2:
					buffer[offset] = this.itemFormatCodeAndNumberOfLengthBytes;
					buffer[offset + 1] = this.lengthByteTwo;
					buffer[offset + 2] = this.lengthByteThree;
					break;

				case 3:
					buffer[offset] = this.itemFormatCodeAndNumberOfLengthBytes;
					buffer[offset + 1] = this.lengthByteOne;
					buffer[offset + 2] = this.lengthByteTwo;
					buffer[offset + 3] = this.lengthByteThree;
					break;

				default:
					throw new InvalidOperationException($"This instance does not have valid number of length bytes. Valid are 1, 2 or 3, however the current value is {this.NumberOfLengthBytes}.");
			}
		}
	}
}