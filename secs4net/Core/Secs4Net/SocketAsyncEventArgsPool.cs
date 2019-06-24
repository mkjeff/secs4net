using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;

namespace Secs4Net
{
	[DebuggerDisplay(nameof(SocketAsyncEventArgsPool) + ": " + nameof(SocketAsyncEventArgsPool.CountAvailable) + ": {" + nameof(SocketAsyncEventArgsPool.CountAvailable) + "} " + nameof(SocketAsyncEventArgsPool.CountAll) + ": {" + nameof(SocketAsyncEventArgsPool.CountAll) + "}")]
	internal sealed class SocketAsyncEventArgsPool :
		IDisposable
	{
		private readonly List<SocketAsyncEventArgs> all;

		private readonly Stack<SocketAsyncEventArgs> available;

		private readonly object lockObject = new object();

		public SocketAsyncEventArgsPool()
		{
			this.all = new List<SocketAsyncEventArgs>();
			this.available = new Stack<SocketAsyncEventArgs>();
		}

		public SocketAsyncEventArgsPool(int capacity)
		{
			this.all = new List<SocketAsyncEventArgs>(capacity);
			this.available = new Stack<SocketAsyncEventArgs>(capacity);
		}

		/// <summary>
		/// <para xml:lang="en">Finalizes an instance of the <see cref="SocketAsyncEventArgsPool"/> class. Allows the <see cref="SocketAsyncEventArgsPool"/> object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</para>
		/// </summary>
		/// <remarks>
		/// <para xml:lang="en">Implement the deconstructor only if this class has unmanaged-resources and only use it to clean those up!</para>
		/// </remarks>
		~SocketAsyncEventArgsPool()
		{
			this.Dispose(false);
		}

		public int CountAll => this.all.Count;

		public int CountAvailable => this.available.Count;

		/// <summary>
		/// <para xml:lang="en">Gets a value indicating whether this instance is disposed or not.</para>
		/// </summary>
		/// <return>
		/// <para xml:lang="en"><see langword="true"/> if this instance is disposed; otherwise <see langword="false"/>.</para>
		/// </return>
		public bool IsDisposed { get; private set; }

		/// <summary>
		/// <para xml:lang="en">Performs class-defined tasks associated with freeing, releasing, or resetting managed resources.</para>
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public SocketAsyncEventArgs Lend()
		{
			this.AssertObjectIsNotDisposed();

			lock (this.lockObject)
			{
				this.AssertObjectIsNotDisposed();

				if (this.available.Count > 0)
				{
					return this.available.Pop();
				}

				var args = new SocketAsyncEventArgs();
				this.all.Add(args);

				return args;
			}
		}

		public void Return(SocketAsyncEventArgs args)
		{
			if (args == null)
			{
				throw new ArgumentNullException(nameof(args));
			}

			this.ClearSocketAsyncEventArgs(args);

			if (this.IsDisposed)
			{
				return;
			}

			lock (this.lockObject)
			{
				if (this.IsDisposed)
				{
					return;
				}

				this.available.Push(args);
			}
		}

		/// <summary>
		/// <para xml:lang="en">Throws <see cref="ObjectDisposedException"/>s if this instance is disposed. (<see cref="SocketAsyncEventArgsPool.IsDisposed"/>)</para>
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		/// <para xml:lang="en">This instance has been disposed.</para>
		/// </exception>
		private void AssertObjectIsNotDisposed()
		{
			if (this.IsDisposed)
			{
				throw new ObjectDisposedException(this.GetType().ToString());
			}
		}

		private void ClearSocketAsyncEventArgs(SocketAsyncEventArgs args)
		{
			args.AcceptSocket = null;
			args.UserToken = null;

			if (args.BufferList != null)
			{
				args.BufferList = null;
			}
			if (args.Buffer != null)
			{
				args.SetBuffer(null, 0, 0);
			}
			if (args.SendPacketsElements != null)
			{
				args.SendPacketsElements = null;
			}
		}

		/// <summary>
		/// <para xml:lang="en">Performs class-defined tasks associated with freeing, releasing, or resetting managed or unmanaged resources.</para>
		/// </summary>
		/// <remarks>
		/// <para xml:lang="en">Should never throw any <see cref="Exception"/>! Even if it is run multiple times.</para>
		/// </remarks>
		/// <param name="disposing">
		/// <para xml:lang="en"><see langword="true"/> to cleanup managed- and unmanaged resources; otherwise <see langword="false"/> to cleanup only unmanaged resources.</para>
		/// </param>
		private void Dispose(bool disposing)
		{
			lock (this.lockObject)
			{
				this.IsDisposed = true;
				foreach (var item in this.all)
				{
					item.Dispose();
				}
				this.all.Clear();
				this.available.Clear();
			}
		}
	}
}