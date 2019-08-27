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
		[TestMethod]
		public void CanItemHeaderBeConstructedWithHighLength()
		{
			const SecsFormat expectedSecsFormat = SecsFormat.U1;
			const int expectedItemLength = 0x876543;
			const int expectedNumberOfLengthBytes = 3;
			const int expectedRawDataSize = 4;
			byte[] expectedRawData = new byte[] { (int)expectedSecsFormat | 0b_000000_11, 0x87, 0x65, 0x43 };

			ItemHeader itemHeader = new ItemHeader(expectedSecsFormat, expectedItemLength);

			Assert.IsTrue(itemHeader.IsValid, nameof(itemHeader.IsValid));
			Assert.AreEqual(expectedSecsFormat, itemHeader.Format, nameof(itemHeader.Format));
			Assert.AreEqual(expectedItemLength, itemHeader.ItemLength, nameof(itemHeader.ItemLength));
			Assert.AreEqual(expectedNumberOfLengthBytes, itemHeader.NumberOfLengthBytes, nameof(itemHeader.NumberOfLengthBytes));
			Assert.AreEqual(expectedRawDataSize, itemHeader.RawDataSize, nameof(itemHeader.RawDataSize));

			byte[] actualRawData = itemHeader.GetRawData();
			CollectionAssert.AreEqual(expectedRawData, actualRawData);
		}

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="ItemHeader.ItemHeader(int)"/>.</para>
		/// </summary>
		[TestMethod]
		public void CanItemHeaderBeConstructedWithLowLength()
		{
			const SecsFormat expectedSecsFormat = SecsFormat.U1;
			const int expectedItemLength = 0x87;
			const int expectedNumberOfLengthBytes = 1;
			const int expectedRawDataSize = 2;
			byte[] expectedRawData = new byte[] { (int)expectedSecsFormat | 0b_000000_01, 0x87 };

			ItemHeader itemHeader = new ItemHeader(expectedSecsFormat, expectedItemLength);

			Assert.IsTrue(itemHeader.IsValid, nameof(itemHeader.IsValid));
			Assert.AreEqual(expectedSecsFormat, itemHeader.Format, nameof(itemHeader.Format));
			Assert.AreEqual(expectedItemLength, itemHeader.ItemLength, nameof(itemHeader.ItemLength));
			Assert.AreEqual(expectedNumberOfLengthBytes, itemHeader.NumberOfLengthBytes, nameof(itemHeader.NumberOfLengthBytes));
			Assert.AreEqual(expectedRawDataSize, itemHeader.RawDataSize, nameof(itemHeader.RawDataSize));

			byte[] actualRawData = itemHeader.GetRawData();
			CollectionAssert.AreEqual(expectedRawData, actualRawData);
		}

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="ItemHeader.ItemHeader(int)"/>.</para>
		/// </summary>
		[TestMethod]
		public void CanItemHeaderBeConstructedWithMaxLength()
		{
			const SecsFormat expectedSecsFormat = SecsFormat.U1;
			const int expectedItemLength = 0xffffff;
			const int expectedNumberOfLengthBytes = 3;
			const int expectedRawDataSize = 4;
			byte[] expectedRawData = new byte[] { (int)expectedSecsFormat | 0b_000000_11, 0xff, 0xff, 0xff };

			ItemHeader itemHeader = new ItemHeader(expectedSecsFormat, expectedItemLength);

			Assert.IsTrue(itemHeader.IsValid, nameof(itemHeader.IsValid));
			Assert.AreEqual(expectedSecsFormat, itemHeader.Format, nameof(itemHeader.Format));
			Assert.AreEqual(expectedItemLength, itemHeader.ItemLength, nameof(itemHeader.ItemLength));
			Assert.AreEqual(expectedNumberOfLengthBytes, itemHeader.NumberOfLengthBytes, nameof(itemHeader.NumberOfLengthBytes));
			Assert.AreEqual(expectedRawDataSize, itemHeader.RawDataSize, nameof(itemHeader.RawDataSize));

			byte[] actualRawData = itemHeader.GetRawData();
			CollectionAssert.AreEqual(expectedRawData, actualRawData);
		}

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="ItemHeader.ItemHeader(int)"/>.</para>
		/// </summary>
		[TestMethod]
		public void CanItemHeaderBeConstructedWithMidLength()
		{
			const SecsFormat expectedSecsFormat = SecsFormat.U1;
			const int expectedItemLength = 0x8765;
			const int expectedNumberOfLengthBytes = 2;
			const int expectedRawDataSize = 3;
			byte[] expectedRawData = new byte[] { (int)expectedSecsFormat | 0b_000000_10, 0x87, 0x65 };

			ItemHeader itemHeader = new ItemHeader(expectedSecsFormat, expectedItemLength);

			Assert.IsTrue(itemHeader.IsValid, nameof(itemHeader.IsValid));
			Assert.AreEqual(expectedSecsFormat, itemHeader.Format, nameof(itemHeader.Format));
			Assert.AreEqual(expectedItemLength, itemHeader.ItemLength, nameof(itemHeader.ItemLength));
			Assert.AreEqual(expectedNumberOfLengthBytes, itemHeader.NumberOfLengthBytes, nameof(itemHeader.NumberOfLengthBytes));
			Assert.AreEqual(expectedRawDataSize, itemHeader.RawDataSize, nameof(itemHeader.RawDataSize));

			byte[] actualRawData = itemHeader.GetRawData();
			CollectionAssert.AreEqual(expectedRawData, actualRawData);
		}

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="ItemHeader.ItemHeader(int)"/>.</para>
		/// </summary>
		[TestMethod]
		public void CanItemHeaderBeConstructedWithMinLength()
		{
			const SecsFormat expectedSecsFormat = SecsFormat.U1;
			const int expectedItemLength = 0;
			const int expectedNumberOfLengthBytes = 1;
			const int expectedRawDataSize = 2;
			byte[] expectedRawData = new byte[] { (int)expectedSecsFormat | 0b_000000_01, 0x00 };

			ItemHeader itemHeader = new ItemHeader(expectedSecsFormat, expectedItemLength);

			Assert.IsTrue(itemHeader.IsValid, nameof(itemHeader.IsValid));
			Assert.AreEqual(expectedSecsFormat, itemHeader.Format, nameof(itemHeader.Format));
			Assert.AreEqual(expectedItemLength, itemHeader.ItemLength, nameof(itemHeader.ItemLength));
			Assert.AreEqual(expectedNumberOfLengthBytes, itemHeader.NumberOfLengthBytes, nameof(itemHeader.NumberOfLengthBytes));
			Assert.AreEqual(expectedRawDataSize, itemHeader.RawDataSize, nameof(itemHeader.RawDataSize));

			byte[] actualRawData = itemHeader.GetRawData();
			CollectionAssert.AreEqual(expectedRawData, actualRawData);
		}
	}
}