using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Secs4Net.Tests
{
	/// <summary>
	/// <para xml:lang="en">This is a test class for <see cref="ItemHeader"/> and is intended to contain all <see cref="ItemHeader"/> Unit Tests.</para>
	/// </summary>
	[TestClass]
	public sealed class ItemHeaderTests
	{
		/// <summary>
		/// <para xml:lang="en">A test for <see cref="ItemHeader.ItemHeader(int)"/>.</para>
		/// </summary>
		[DataTestMethod]
		[DataRow(SecsFormat.U1, 0xffffff, (byte)3, (byte)4)]
		[DataRow(SecsFormat.U1, 0x876543, (byte)3, (byte)4)]
		[DataRow(SecsFormat.U1, 0x8765, (byte)2, (byte)3)]
		[DataRow(SecsFormat.U1, 0x87, (byte)1, (byte)2)]
		[DataRow(SecsFormat.U1, 0, (byte)1, (byte)2)]
		public void CanItemHeaderBeConstructed(SecsFormat expectedSecsFormat, int expectedItemLength, byte expectedNumberOfLengthBytes, byte expectedRawHeaderDataSize)
		{
			ItemHeader itemHeader = new ItemHeader(expectedSecsFormat, expectedItemLength);

			Assert.IsTrue(itemHeader.IsValid, nameof(itemHeader.IsValid));
			Assert.AreEqual(expectedSecsFormat, itemHeader.Format, nameof(itemHeader.Format));
			Assert.AreEqual(expectedItemLength, itemHeader.ItemLength, nameof(itemHeader.ItemLength));
			Assert.AreEqual(expectedNumberOfLengthBytes, itemHeader.NumberOfLengthBytes, nameof(itemHeader.NumberOfLengthBytes));
			Assert.AreEqual(expectedRawHeaderDataSize, itemHeader.RawHeaderDataSize, nameof(itemHeader.RawHeaderDataSize));

			byte[] expectedRawHeaderData = new byte[expectedRawHeaderDataSize];
			expectedRawHeaderData[0] = (byte)((int)expectedSecsFormat | expectedNumberOfLengthBytes);

			switch (expectedNumberOfLengthBytes)
			{
				case 1:
					expectedRawHeaderData[1] = (byte)expectedItemLength;
					break;

				case 2:
					expectedRawHeaderData[1] = (byte)(expectedItemLength >> 8);
					expectedRawHeaderData[2] = (byte)expectedItemLength;
					break;

				case 3:
					expectedRawHeaderData[1] = (byte)(expectedItemLength >> 16);
					expectedRawHeaderData[2] = (byte)(expectedItemLength >> 8);
					expectedRawHeaderData[3] = (byte)expectedItemLength;
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(expectedNumberOfLengthBytes), expectedNumberOfLengthBytes, "The argument must be 1, 2 or 3.");
			}

			byte[] actualRawHeaderData = itemHeader.GetRawHeaderData();
			CollectionAssert.AreEqual(expectedRawHeaderData, actualRawHeaderData);

			byte[] expectedItemBufferWithRawHeaderData = new byte[expectedRawHeaderDataSize + expectedItemLength];
			Array.Copy(expectedRawHeaderData, 0, expectedItemBufferWithRawHeaderData, 0, expectedRawHeaderData.Length);

			var (actualItemBufferWithRawHeaderData, actualOffset) = itemHeader.GetItemBufferWithRawHeaderData();
			Assert.AreEqual(expectedRawHeaderDataSize, actualOffset, nameof(actualOffset));
			CollectionAssert.AreEqual(expectedItemBufferWithRawHeaderData, actualItemBufferWithRawHeaderData);
		}

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="ItemHeader.GetItemBufferWithRawHeaderData"/>.</para>
		/// </summary>
		[DataTestMethod]
		[DataRow(SecsFormat.U1, 0xffffff, (byte)3, (byte)4)]
		[DataRow(SecsFormat.U1, 0x876543, (byte)3, (byte)4)]
		[DataRow(SecsFormat.U1, 0x8765, (byte)2, (byte)3)]
		[DataRow(SecsFormat.U1, 0x87, (byte)1, (byte)2)]
		[DataRow(SecsFormat.U1, 0, (byte)1, (byte)2)]
		public void CanItemHeaderGetItemBufferWithRawHeaderData(SecsFormat expectedSecsFormat, int expectedItemLength, byte expectedNumberOfLengthBytes, byte expectedRawHeaderDataSize)
		{
			ItemHeader itemHeader = new ItemHeader(expectedSecsFormat, expectedItemLength);

			byte[] expectedItemBufferWithRawHeaderData = new byte[expectedRawHeaderDataSize + expectedItemLength];
			expectedItemBufferWithRawHeaderData[0] = (byte)((int)expectedSecsFormat | expectedNumberOfLengthBytes);

			switch (expectedNumberOfLengthBytes)
			{
				case 1:
					expectedItemBufferWithRawHeaderData[1] = (byte)expectedItemLength;
					break;

				case 2:
					expectedItemBufferWithRawHeaderData[1] = (byte)(expectedItemLength >> 8);
					expectedItemBufferWithRawHeaderData[2] = (byte)expectedItemLength;
					break;

				case 3:
					expectedItemBufferWithRawHeaderData[1] = (byte)(expectedItemLength >> 16);
					expectedItemBufferWithRawHeaderData[2] = (byte)(expectedItemLength >> 8);
					expectedItemBufferWithRawHeaderData[3] = (byte)expectedItemLength;
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(expectedNumberOfLengthBytes), expectedNumberOfLengthBytes, "The argument must be 1, 2 or 3.");
			}

			var (actualItemBufferWithRawHeaderData, actualOffset) = itemHeader.GetItemBufferWithRawHeaderData();

			Assert.AreEqual(expectedRawHeaderDataSize, actualOffset, nameof(actualOffset));

			CollectionAssert.AreEqual(expectedItemBufferWithRawHeaderData, actualItemBufferWithRawHeaderData);
		}

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="ItemHeader.GetRawHeaderData"/>.</para>
		/// </summary>
		[DataTestMethod]
		[DataRow(SecsFormat.U1, 0xffffff, (byte)3, (byte)4)]
		[DataRow(SecsFormat.U1, 0x876543, (byte)3, (byte)4)]
		[DataRow(SecsFormat.U1, 0x8765, (byte)2, (byte)3)]
		[DataRow(SecsFormat.U1, 0x87, (byte)1, (byte)2)]
		[DataRow(SecsFormat.U1, 0, (byte)1, (byte)2)]
		public void CanItemHeaderGetRawHeaderData(SecsFormat expectedSecsFormat, int expectedItemLength, byte expectedNumberOfLengthBytes, byte expectedRawHeaderDataSize)
		{
			ItemHeader itemHeader = new ItemHeader(expectedSecsFormat, expectedItemLength);

			byte[] expectedRawHeaderData = new byte[expectedRawHeaderDataSize];
			expectedRawHeaderData[0] = (byte)((int)expectedSecsFormat | expectedNumberOfLengthBytes);

			switch (expectedNumberOfLengthBytes)
			{
				case 1:
					expectedRawHeaderData[1] = (byte)expectedItemLength;
					break;

				case 2:
					expectedRawHeaderData[1] = (byte)(expectedItemLength >> 8);
					expectedRawHeaderData[2] = (byte)expectedItemLength;
					break;

				case 3:
					expectedRawHeaderData[1] = (byte)(expectedItemLength >> 16);
					expectedRawHeaderData[2] = (byte)(expectedItemLength >> 8);
					expectedRawHeaderData[3] = (byte)expectedItemLength;
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(expectedNumberOfLengthBytes), expectedNumberOfLengthBytes, "The argument must be 1, 2 or 3.");
			}

			byte[] actualRawHeaderData = itemHeader.GetRawHeaderData();
			CollectionAssert.AreEqual(expectedRawHeaderData, actualRawHeaderData);
		}

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="ItemHeader.WriteRawHeaderData"/>.</para>
		/// </summary>
		[DataTestMethod]
		[DataRow(SecsFormat.U1, 0xffffff, (byte)3, (byte)4)]
		[DataRow(SecsFormat.U1, 0x876543, (byte)3, (byte)4)]
		[DataRow(SecsFormat.U1, 0x8765, (byte)2, (byte)3)]
		[DataRow(SecsFormat.U1, 0x87, (byte)1, (byte)2)]
		[DataRow(SecsFormat.U1, 0, (byte)1, (byte)2)]
		public void CanItemHeaderWriteRawHeaderData(SecsFormat expectedSecsFormat, int expectedItemLength, byte expectedNumberOfLengthBytes, byte expectedRawHeaderDataSize)
		{
			ItemHeader itemHeader = new ItemHeader(expectedSecsFormat, expectedItemLength);

			byte[] expectedRawHeaderData = new byte[expectedRawHeaderDataSize];
			expectedRawHeaderData[0] = (byte)((int)expectedSecsFormat | expectedNumberOfLengthBytes);

			switch (expectedNumberOfLengthBytes)
			{
				case 1:
					expectedRawHeaderData[1] = (byte)expectedItemLength;
					break;

				case 2:
					expectedRawHeaderData[1] = (byte)(expectedItemLength >> 8);
					expectedRawHeaderData[2] = (byte)expectedItemLength;
					break;

				case 3:
					expectedRawHeaderData[1] = (byte)(expectedItemLength >> 16);
					expectedRawHeaderData[2] = (byte)(expectedItemLength >> 8);
					expectedRawHeaderData[3] = (byte)expectedItemLength;
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(expectedNumberOfLengthBytes), expectedNumberOfLengthBytes, "The argument must be 1, 2 or 3.");
			}

			byte[] actualRawHeaderData = new byte[expectedRawHeaderData.Length];

			itemHeader.WriteRawHeaderData(actualRawHeaderData, 0);

			CollectionAssert.AreEqual(expectedRawHeaderData, actualRawHeaderData);
		}
	}
}