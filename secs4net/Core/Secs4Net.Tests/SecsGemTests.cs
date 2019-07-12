using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Secs4Net.Tests
{
	/// <summary>
	/// <para xml:lang="en">This is a test class for <see cref="SecsGem"/> and is intended to contain all <see cref="SecsGem"/> Unit Tests.</para>
	/// </summary>
	[TestClass]
	public sealed class SecsGemTests
	{
		/// <summary>
		/// <para xml:lang="en">A test for <see cref="SecsGem.SecsGem(bool, IPAddress, ushort, int)"/>.</para>
		/// </summary>
		[TestMethod]
		public void CanSecsGemBeConstructed()
		{
			bool isActive = false;
			IPAddress ipAddress = IPAddress.Loopback;
			ushort port = 60000;

			using (SecsGem secsGem = new SecsGem(isActive, ipAddress, port))
			{
			}
		}

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="SecsGem.SecsGem(bool, IPAddress, ushort, int)"/>.</para>
		/// </summary>
		[TestMethod]
		public void CanSecsGemBeConstructedWithInitialReceiveBufferSize()
		{
			bool isActive = false;
			IPAddress ipAddress = IPAddress.Loopback;
			ushort port = 60000;
			int initialReceiveBufferSize = 0x4000;

			using (SecsGem secsGem = new SecsGem(isActive, ipAddress, port, initialReceiveBufferSize))
			{
			}
		}

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="SecsGem.SecsGem(bool, IPAddress, ushort, int)"/>.</para>
		/// </summary>
		[TestMethod]
		public void CanSecsGemNotBeConstructedWithNegativInitialReceiveBufferSize()
		{
			bool isActive = false;
			IPAddress ipAddress = IPAddress.Loopback;
			ushort port = 60000;
			int initialReceiveBufferSize = -1;

			Assert.ThrowsException<System.ArgumentOutOfRangeException>(() =>
			{
				using (SecsGem secsGem = new SecsGem(isActive, ipAddress, port, initialReceiveBufferSize))
				{
				}
			});
		}

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="SecsGem.SecsGem(bool, IPAddress, ushort, int)"/>.</para>
		/// </summary>
		[TestMethod]
		public void CanSecsGemNotBeConstructedWithToSmallInitialReceiveBufferSize()
		{
			bool isActive = false;
			IPAddress ipAddress = IPAddress.Loopback;
			ushort port = 60000;
			int initialReceiveBufferSize = 63;

			Assert.ThrowsException<System.ArgumentOutOfRangeException>(() =>
			{
				using (SecsGem secsGem = new SecsGem(isActive, ipAddress, port, initialReceiveBufferSize))
				{
				}
			});
		}
	}
}