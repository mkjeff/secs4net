using System;
using System.Diagnostics;
using System.Net;

namespace Secs4Net.Tests
{
	internal sealed class SecsGemLocalLoop :
		IDisposable
	{
		public SecsGemLocalLoop(ushort port)
		{
			try
			{
				this.Equipment = new SecsGem(false, IPAddress.Loopback, port);
				this.Equipment.ConnectionChanged += this.Equipment_ConnectionChanged;
				this.Equipment.PrimaryMessageReceived += this.Equipment_PrimaryMessageReceived;


				this.Host = new SecsGem(true, IPAddress.Loopback, port);
				this.Host.ConnectionChanged += this.Host_ConnectionChanged;
				this.Host.PrimaryMessageReceived += this.Host_PrimaryMessageReceived;
			}
			catch
			{
				this.Dispose();
				throw;
			}
		}

		~SecsGemLocalLoop()
		{
			this.Dispose(false);
		}

		public SecsGem Equipment { get; private set; }

		public SecsGem Host { get; private set; }

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Start()
		{
			this.Equipment.Start();
			this.Host.Start();
		}

		private void Dispose(bool disposing)
		{
			if (this.Host != null)
			{
				this.Host.Dispose();
				this.Host = null;
			}

			if (this.Equipment != null)
			{
				this.Equipment.Dispose();
				this.Equipment = null;
			}
		}

		private void Equipment_ConnectionChanged(object sender, ConnectionState e)
		{
			Trace.WriteLine($"{nameof(SecsGem.ConnectionChanged)}: {e}", nameof(this.Equipment));
		}

		private void Equipment_PrimaryMessageReceived(object sender, PrimaryMessageWrapper e)
		{
			Trace.WriteLine($"{nameof(SecsGem.PrimaryMessageReceived)}: {e.Message}", nameof(this.Equipment));
		}

		private void Host_ConnectionChanged(object sender, ConnectionState e)
		{
			Trace.WriteLine($"{nameof(SecsGem.ConnectionChanged)}: {e}", nameof(this.Host));
		}

		private void Host_PrimaryMessageReceived(object sender, PrimaryMessageWrapper e)
		{
			Trace.WriteLine($"{nameof(SecsGem.PrimaryMessageReceived)}: {e.Message}", nameof(this.Host));
		}
	}
}