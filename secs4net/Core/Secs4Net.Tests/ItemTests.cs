using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Secs4Net.Tests
{
	/// <summary>
	/// <para xml:lang="en">This is a test class for <see cref="Item"/> and is intended to contain all <see cref="Item"/> Unit Tests.</para>
	/// </summary>
	[TestClass]
	public sealed class ItemTests
	{
		/// <summary>
		/// <para xml:lang="en">A test for <see cref="Item.A(string)"/>.</para>
		/// </summary>
		[TestMethod]
		public void CanItemConstructA()
		{
			var chars = new List<char>();

			for (byte i = 32; i < 127; i++)
			{
				chars.Add((char)i);
			}

			chars.AddRange(chars);
			chars.AddRange(chars);

			Assert.IsTrue(chars.Count > byte.MaxValue);

			string targetValue = new string(chars.ToArray());

			Item item = Item.A(targetValue);

			Assert.AreEqual(SecsFormat.ASCII, item.Format, nameof(item.Count));
			Assert.AreEqual(chars.Count, item.Count, nameof(item.Count));

			byte[] rawBytes = item.RawBytes.ToArray();

			Assert.AreEqual(new ItemHeader(SecsFormat.ASCII, chars.Count).RawHeaderDataSize + chars.Count, rawBytes.Length, nameof(rawBytes));
		}

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="Item.U4(uint)"/>.</para>
		/// </summary>
		[TestMethod]
		public void CanItemConstructList()
		{
			Item[] targetValue = new Item[300];

			Random random = new Random();

			for (int i = 0; i < targetValue.Length; i++)
			{
				targetValue[i] = Item.L();
			}

			Item item = Item.L(targetValue);

			Assert.AreEqual(SecsFormat.List, item.Format, nameof(item.Count));
			Assert.AreEqual(targetValue.Length, item.Count, nameof(item.Count));

			SecsMessage secsMessage = new SecsMessage(1, 1, "Test", item, false);

			byte[] rawBytes = secsMessage.RawBytes.SelectMany(x => x).ToArray();

			int targetByteCount = targetValue.Length * new ItemHeader(SecsFormat.List, 0).RawHeaderDataSize;

			int itemByteSize = new ItemHeader(SecsFormat.List, targetByteCount).RawHeaderDataSize + targetByteCount;

			int messageByteSize = 4 + itemByteSize;

			Assert.AreEqual(messageByteSize, rawBytes.Length, nameof(rawBytes));
		}

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="Item.U4(uint)"/>.</para>
		/// </summary>
		[TestMethod]
		public void CanItemConstructU4()
		{
			uint[] targetValue = new uint[300];

			Random random = new Random();

			for (int i = 0; i < targetValue.Length; i++)
			{
				targetValue[i] = (uint)random.Next();
			}

			Item item = Item.U4(targetValue);

			Assert.AreEqual(SecsFormat.U4, item.Format, nameof(item.Count));
			Assert.AreEqual(targetValue.Length, item.Count, nameof(item.Count));

			byte[] rawBytes = item.RawBytes.ToArray();

			int byteCount = targetValue.Length * sizeof(uint);

			Assert.AreEqual(new ItemHeader(SecsFormat.ASCII, byteCount).RawHeaderDataSize + byteCount, rawBytes.Length, nameof(rawBytes));
		}
	}
}