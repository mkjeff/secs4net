using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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

		/// <summary>
		/// <para xml:lang="en">A test for <see cref="SecsGem.SecsGem(bool, IPAddress, ushort, int)"/>.</para>
		/// </summary>
		[TestMethod]
		[Timeout(60000)]
		public void CanSecsGemSendMessageToSecsGem()
		{
			ushort port = 60000;

			Item mdlnAndSoftRev = Item.L(Item.A($"{nameof(SecsGemTests)}Host"), Item.A(SecsGemTests.GetSoftRevValue()));
			Item commackAccepted = Item.B(0);
			SecsMessage establishCommunicationsRequest = new SecsMessage(1, 13, "Establish Communications Request (CR)", mdlnAndSoftRev, true);
			SecsMessage establishCommunicationsRequestAcknowledge = new SecsMessage(1, 14, "Establish Communications Request Acknowledge (CRA)", Item.L(commackAccepted, mdlnAndSoftRev), false);

			bool gotSelected = false;
			bool gotReceived = false;
			bool gotReplyed = false;

			using (SecsGemLocalLoop loop = new SecsGemLocalLoop(port))
			{
				Task<SecsMessage> task = null;

				loop.Host.ConnectionChanged += (sender, e) =>
				{
					if (e == ConnectionState.Selected)
					{
						gotSelected = true;

						task = loop.Host.SendAsync(establishCommunicationsRequest);
					}
				};

				loop.Equipment.PrimaryMessageReceived += (sender, e) =>
				{
					if (e.Message.S == 1 && e.Message.F == 13)
					{
						gotReceived = true;

						e.ReplyAsync(establishCommunicationsRequestAcknowledge);
					}
				};

				loop.Host.PrimaryMessageReceived += (sender, e) =>
				{
					if (e.Message.S == 1 && e.Message.F == 14)
					{
						gotReceived = true;

						e.ReplyAsync(establishCommunicationsRequestAcknowledge);
					}
				};

				loop.Start();

				do
				{
					Thread.Sleep(1000);
				}
				while (task?.IsCompleted == false);

				gotReplyed = true;
				Assert.IsTrue(task.Result.S == 1 && task.Result.F == 14);
			}

			Assert.IsTrue(gotSelected);
			Assert.IsTrue(gotReceived);
			Assert.IsTrue(gotReplyed);
		}

		private static string GetSoftRevValue()
		{
			return Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}
	}
}