using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Secs4Net.Tests
{
	/// <summary>
	/// <para xml:lang="en">This is a test class for <see cref="SecsExtension"/> and is intended to contain all <see cref="SecsExtension"/> Unit Tests.</para>
	/// </summary>
	[TestClass]
	public sealed class SecsExtensionTests
	{
		/// <summary>
		/// <para xml:lang="en">A test for <see cref="SecsExtension.ToHexString(byte[])"/>.</para>
		/// </summary>
		[TestMethod]
		public void CanByteArrayToHexString()
		{
			byte[] data = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

			const string expected = "00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15 16";

			string actual = SecsExtension.ToHexString(data);

			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="SecsExtension.ToHexString(byte[])"/> with an empty array.</para>
		/// </summary>
		[TestMethod]
		public void CanByteArrayToHexStringEmpty()
		{
			byte[] data = Array.Empty<byte>();

			string expected = string.Empty;

			string actual = SecsExtension.ToHexString(data);

			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="SecsExtension.ToHexString(byte[])"/> with an <see langword="null"/> array.</para>
		/// </summary>
		[TestMethod]
		public void CanByteArrayToHexStringNull()
		{
			byte[] data = null;

			string expected = string.Empty;

			string actual = SecsExtension.ToHexString(data);

			Assert.AreEqual(expected, actual);
		}
	}
}