using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
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
		/// <para xml:lang="en">A test for <see cref="SecsGem(bool, IPAddress, ushort, int)"/>.</para>
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
		/// <para xml:lang="en">A test for <see cref="SecsGem(bool, IPAddress, ushort, int)"/>.</para>
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
		/// <para xml:lang="en">A test for <see cref="SecsGem(bool, IPAddress, ushort, int)"/>.</para>
		/// </summary>
		[TestMethod]
		[Timeout(60000)]
		public void CanSecsGemConnectToSecsGem()
		{
			ushort port = 60000;

			bool keepRunning = true;
			bool gotSelected = false;

			using (SecsGemLocalLoop loop = new SecsGemLocalLoop(port))
			{
				loop.Host.ConnectionChanged += (sender, e) =>
				{
					if (e == ConnectionState.Selected)
					{
						gotSelected = true;
						keepRunning = false;
					}
				};

				loop.Start();

				do
				{
					Thread.Sleep(1000);
				}
				while (keepRunning);
			}

			Assert.IsTrue(gotSelected);
		}

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="SecsGem(bool, IPAddress, ushort, int)"/>.</para>
		/// </summary>
		[TestMethod]
		public void CanSecsGemNotBeConstructedWithNegativInitialReceiveBufferSize()
		{
			bool isActive = false;
			IPAddress ipAddress = IPAddress.Loopback;
			ushort port = 60000;
			int initialReceiveBufferSize = -1;

			Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
			{
				using (SecsGem secsGem = new SecsGem(isActive, ipAddress, port, initialReceiveBufferSize))
				{
				}
			});
		}

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="SecsGem(bool, IPAddress, ushort, int)"/>.</para>
		/// </summary>
		[TestMethod]
		public void CanSecsGemNotBeConstructedWithToSmallInitialReceiveBufferSize()
		{
			bool isActive = false;
			IPAddress ipAddress = IPAddress.Loopback;
			ushort port = 60000;
			int initialReceiveBufferSize = 63;

			Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
			{
				using (SecsGem secsGem = new SecsGem(isActive, ipAddress, port, initialReceiveBufferSize))
				{
				}
			});
		}

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="SecsGem(bool, IPAddress, ushort, int)"/>.</para>
		/// </summary>
		[TestMethod]
		[Timeout(60000)]
		public void CanSecsGemSendMessageToSecsGem()
		{
			const ushort port = 60000;

			Item mdlnAndSoftRev = Item.L(Item.A($"{nameof(SecsGemTests)}Host"), Item.A(SecsGemTests.GetSoftRevValue()));
			Item commackAccepted = Item.B(0);
			SecsMessage establishCommunicationsRequest = new SecsMessage(1, 13, "Establish Communications Request (CR)", mdlnAndSoftRev, true);
			SecsMessage establishCommunicationsRequestAcknowledge = new SecsMessage(1, 14, "Establish Communications Request Acknowledge (CRA)", Item.L(commackAccepted, mdlnAndSoftRev), false);

			using (SecsGemLocalLoop loop = new SecsGemLocalLoop(port))
			{
				SecsMessage message = null;

				loop.Host.ConnectionChanged += async (sender, e) =>
				{
					if (e == ConnectionState.Selected)
					{
						message = await loop.Host.SendAsync(establishCommunicationsRequest).ConfigureAwait(false);
					}
				};

				loop.Equipment.PrimaryMessageReceived += async (sender, e) =>
				{
					if (e.Message.S == 1 && e.Message.F == 13)
					{
						await e.ReplyAsync(establishCommunicationsRequestAcknowledge).ConfigureAwait(false);
					}
				};

				loop.Start();

				while (message == null)
				{
					Thread.Sleep(1000);
				}

				Assert.IsTrue(message.S == 1 && message.F == 14);
			}
		}

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="SecsGem(bool, IPAddress, ushort, int)"/>.</para>
		/// </summary>
		[TestMethod]
		[Timeout(60000)]
		public void CanSecsGemSendLargeMessageToSecsGem()
		{
			const ushort port = 60001;

			Item mdlnAndSoftRev = Item.L(Item.A($"{nameof(SecsGemTests)}Host"), Item.A(SecsGemTests.GetSoftRevValue()));
			Item commackAccepted = Item.B(0);
			SecsMessage establishCommunicationsRequest = new SecsMessage(1, 13, "Establish Communications Request (CR)", mdlnAndSoftRev, true);
			SecsMessage establishCommunicationsRequestAcknowledge = new SecsMessage(1, 14, "Establish Communications Request Acknowledge (CRA)", Item.L(commackAccepted, mdlnAndSoftRev), false);

			SecsMessage selectedEquipmentStatusRequest = new SecsMessage(1, 3, "Selected Equipment Status Request (SSR)", Item.L(), true);

			const int statusValuesLength = ushort.MaxValue + 1;
			const int maxValueCount = byte.MaxValue + 1;

			List<uint> values = new List<uint>(maxValueCount);
			Item[] statusValues = new Item[statusValuesLength];
			for (uint i = 0; i < statusValues.Length; i++)
			{
				values.Add(i);
				statusValues[i] = Item.U4(values);

				if (values.Count >= maxValueCount)
				{
					values.Clear();
				}
			}
			Item listItemStatusValues = Item.L(statusValues);

			SecsMessage selectedEquipmentStatusData = new SecsMessage(1, 4, "Selected Equipment Status Data (SSD)", listItemStatusValues, true);

			int expectedTotalRawBytesCount = selectedEquipmentStatusData.RawBytes.SelectMany(x => x).Count();
			Trace.WriteLine($"{nameof(selectedEquipmentStatusData)}.{nameof(selectedEquipmentStatusData.RawBytes)} Count: {expectedTotalRawBytesCount}");

			using (SecsGemLocalLoop loop = new SecsGemLocalLoop(port))
			{
				SecsMessage message = null;

				loop.Host.ConnectionChanged += async (sender, e) =>
				{
					if (e == ConnectionState.Selected)
					{
						await loop.Host.SendAsync(establishCommunicationsRequest).ConfigureAwait(false);

						message = await loop.Host.SendAsync(selectedEquipmentStatusRequest).ConfigureAwait(false);
					}
				};

				loop.Equipment.PrimaryMessageReceived += async (sender, e) =>
				{
					if (e.Message.S == 1)
					{
						switch (e.Message.F)
						{
							case 3:
								await e.ReplyAsync(selectedEquipmentStatusData).ConfigureAwait(false);
								break;

							case 13:
								await e.ReplyAsync(establishCommunicationsRequestAcknowledge).ConfigureAwait(false);
								break;
						}
					}
				};

				loop.Start();

				while (message == null)
				{
					Thread.Sleep(1000);
				}

				Assert.IsTrue(message.S == 1 && message.F == 4);
				Assert.IsTrue(message.SecsItem.Count == statusValuesLength);
				Assert.AreEqual(expectedTotalRawBytesCount, message.RawBytes.SelectMany(x => x).Count(), nameof(expectedTotalRawBytesCount));
			}
		}

		private static string GetSoftRevValue()
		{
			return Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}
	}
}