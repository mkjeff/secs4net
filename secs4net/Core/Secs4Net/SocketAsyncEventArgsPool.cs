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

		/// <summary>
		/// <para xml:lang="en">Initializes a <see langword="new"/> instance of the <see cref="SocketAsyncEventArgsPool"/> <see langword="class"/>.</para>
		/// </summary>
		public SocketAsyncEventArgsPool()
		{
			this.all = new List<SocketAsyncEventArgs>();
			this.available = new Stack<SocketAsyncEventArgs>();
		}

		/// <summary>
		/// <para xml:lang="en">Initializes a <see langword="new"/> instance of the <see cref="SocketAsyncEventArgsPool"/> <see langword="class"/>.</para>
		/// </summary>
		/// <param name="capacity">
		/// <para xml:lang="en">The number of <see cref="SocketAsyncEventArgsPool"/> that the new <see cref="SocketAsyncEventArgsPool"/> can initially store.</para>
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <para xml:lang="en"><paramref name="capacity"/> is less than 0.</para>
		/// </exception>
		public SocketAsyncEventArgsPool(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "The argument is less than 0.");
			}

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

		/// <summary>
		/// <para xml:lang="en">Gets the count of all <see cref="SocketAsyncEventArgsPool"/> created by this instance.</para>
		/// </summary>
		public int CountAll => this.all.Count;

		/// <summary>
		/// <para xml:lang="en">Gets the count of all currently for lend available <see cref="SocketAsyncEventArgsPool"/> of this instance.</para>
		/// </summary>
		public int CountAvailable => this.available.Count;

		/// <summary>
		/// <para xml:lang="en">Gets a value indicating whether this instance is disposed or not.</para>
		/// </summary>
		/// <return>
		/// <para xml:lang="en"><see langword="true"/> if this instance is disposed; otherwise <see langword="false"/>.</para>
		/// </return>
		public bool IsDisposed { get; private set; }

		/// <summary>
		/// <para xml:lang="en">Disposed all <see cref="SocketAsyncEventArgsPool"/> created by this instance as well disposing this instance.</para>
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// <para xml:lang="en">Lend a <see cref="SocketAsyncEventArgs"/> from this instance.</para>
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		/// <para xml:lang="en">This instance has been disposed.</para>
		/// </exception>
		/// <returns>
		/// <para xml:lang="en">The lent instance of <see cref="SocketAsyncEventArgs"/> from this instance.</para>
		/// </returns>
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

		/// <summary>
		/// <para xml:lang="en">Resets this instance by disposed and removing all <see cref="SocketAsyncEventArgsPool"/> created by this instance.</para>
		/// </summary>
		public void Reset()
		{
			lock (this.lockObject)
			{
				this.InternalReset();
			}
		}

		/// <summary>
		/// <para xml:lang="en">Returns the specified <paramref name="args"/> to this instance.</para>
		/// </summary>
		/// <param name="args">
		/// <para xml:lang="en">An instance of <see cref="SocketAsyncEventArgs"/> which was previously lent from this instance.</para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para xml:lang="en"><paramref name="args"/> is <see langword="null"/>.</para>
		/// </exception>
		/// <returns>
		/// <para xml:lang="en"><see langword="true"/> if the specified <paramref name="args"/> was returned to this instance; otherwise <see langword="false"/>.</para>
		/// </returns>
		public bool Return(SocketAsyncEventArgs args)
		{
			if (args == null)
			{
				throw new ArgumentNullException(nameof(args));
			}

			bool argsIsDisposed = SocketAsyncEventArgsPool.ClearSocketAsyncEventArgs(args);

			if (this.IsDisposed)
			{
				return false;
			}

			lock (this.lockObject)
			{
				if (this.IsDisposed)
				{
					return false;
				}

				if (argsIsDisposed)
				{
					this.all.Remove(args);
					return false;
				}

				this.available.Push(args);
				return true;
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

		private static bool ClearSocketAsyncEventArgs(SocketAsyncEventArgs args)
		{
			try
			{
				args.UserToken = null;
				args.AcceptSocket = null;

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

				return false;
			}
			catch (ObjectDisposedException)
			{
				return true;
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
				this.InternalReset();
			}
		}

		private void InternalReset()
		{
			foreach (var item in this.all)
			{
				item.Dispose();
			}
			this.all.Clear();
			this.available.Clear();
		}
	}
}