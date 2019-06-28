using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Secs4Net.Exceptions;

namespace Secs4Net
{
	public sealed class SecsGem :
		IDisposable
	{
		private const int disposalComplete = 1;

		private const int disposalNotStarted = 0;

		private static readonly SecsMessage controlMessage = new SecsMessage(0, 0, string.Empty);

		private static readonly ArraySegment<byte> controlMessageLengthBytes = new ArraySegment<byte>(new byte[] { 0, 0, 0, 10 });

		private static readonly DefaultSecsGemLogger defaultLogger = new DefaultSecsGemLogger();

		private readonly TokenCache replyExpectedMessages = new TokenCache();

		private readonly StreamDecoder secsDecoder;

		private readonly SocketAsyncEventArgsPool socketAsyncEventArgsPool = new SocketAsyncEventArgsPool();

		private readonly Func<Task> startImplementationFunction;

		private readonly SystemByteGenerator systemByteGenerator = new SystemByteGenerator();

		/// <summary>
		/// between socket connected and received Select.req timer
		/// </summary>
		private readonly Timer timer7;

		private readonly Timer timer8;

		private readonly Timer timerLinkTest;

		private int disposeStage;

		private bool linkTestEnable;

		private int linkTestInterval = 60000;

		private ISecsGemLogger logger = defaultLogger;

		private Socket serverSocket;

		private Socket socket;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="isActive">passive or active mode</param>
		/// <param name="ip">if active mode it should be remote device address, otherwise local listener address</param>
		/// <param name="port">if active mode it should be remote device listener's port</param>
		/// <param name="receiveBufferSize">Socket receive buffer size</param>
		public SecsGem(bool isActive, IPAddress ip, int port, int receiveBufferSize = 0x4000)
		{
			if (port <= 0)
			{
				port = 0;
			}

			this.secsDecoder = new StreamDecoder(receiveBufferSize, this.HandleControlMessage, this.HandleDataMessage);

			this.IpAddress = ip;
			this.Port = port;
			this.IsActive = isActive;
			this.DecoderBufferSize = receiveBufferSize;

			#region Timer Action
			this.timer7 = new Timer(delegate
			{
				this.logger.Error($"T7 Timeout: {this.T7 / 1000} sec.");
				this.CommunicationStateChanging(ConnectionState.Retry);
			}, null, Timeout.Infinite, Timeout.Infinite);

			this.timer8 = new Timer(delegate
			{
				this.logger.Error($"T8 Timeout: {this.T8 / 1000} sec.");
				this.CommunicationStateChanging(ConnectionState.Retry);
			}, null, Timeout.Infinite, Timeout.Infinite);

			this.timerLinkTest = new Timer(delegate
			{
				if (this.State == ConnectionState.Selected)
				{
					this.SendControlMessage(MessageType.LinkTestRequest, this.GetNewSystemId());
				}
			}, null, Timeout.Infinite, Timeout.Infinite);
			#endregion

			if (this.IsActive)
			{
				this.startImplementationFunction = async () =>
				{
					var connected = false;
					do
					{
						if (this.IsDisposed)
						{
							return;
						}

						this.CommunicationStateChanging(ConnectionState.Connecting);
						try
						{
							if (this.IsDisposed)
							{
								return;
							}

							this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
							await this.socket.ConnectAsync(this.IpAddress, this.Port).ConfigureAwait(false);
							connected = true;
						}
						catch (Exception ex)
						{
							if (this.IsDisposed)
							{
								return;
							}

							this.logger.Error(ex.Message, ex);
							this.logger.Info($"Start T5 Timer: {this.T5 / 1000} sec.");
							await Task.Delay(this.T5).ConfigureAwait(false);
						}
					} while (!connected);

					// hook receive event first, because no message will received before 'SelectRequest' send to device
					this.StartSocketReceive();
					this.SendControlMessage(MessageType.SelectRequest, this.GetNewSystemId());
				};
			}
			else
			{
				this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				this.serverSocket.Bind(new IPEndPoint(this.IpAddress, this.Port));
				this.serverSocket.Listen(0);

				this.startImplementationFunction = async () =>
				{
					var connected = false;
					do
					{
						if (this.IsDisposed)
						{
							return;
						}

						this.CommunicationStateChanging(ConnectionState.Connecting);
						try
						{
							if (this.IsDisposed)
							{
								return;
							}

							this.socket = await this.serverSocket.AcceptAsync().ConfigureAwait(false);
							connected = true;
						}
						catch (Exception ex)
						{
							if (this.IsDisposed)
							{
								return;
							}

							this.logger.Error(ex.Message, ex);
							await Task.Delay(2000).ConfigureAwait(false);
						}
					} while (!connected);

					this.StartSocketReceive();
				};
			}
		}

		/// <summary>
		/// HSMS connection state changed event
		/// </summary>
		public event EventHandler<ConnectionState> ConnectionChanged;

		/// <summary>
		/// Primary message received event
		/// </summary>
		public event EventHandler<PrimaryMessageWrapper> PrimaryMessageReceived;

		public int DecoderBufferSize { get; private set; }

		/// <summary>
		/// Device Id.
		/// </summary>
		public ushort DeviceId { get; set; } = 0;

		/// <summary>
		/// remote device endpoint address
		/// </summary>
		public string DeviceIpAddress => this.IsActive
			? this.IpAddress.ToString()
			: ((IPEndPoint)this.socket?.RemoteEndPoint)?.Address?.ToString() ?? "NA";

		public IPAddress IpAddress { get; }

		public bool IsActive { get; }

		public bool IsDisposed => Interlocked.CompareExchange(ref this.disposeStage, SecsGem.disposalComplete, SecsGem.disposalComplete) == SecsGem.disposalComplete;

		/// <summary>
		/// Gets or sets linking test timer enable or not.
		/// </summary>
		public bool LinkTestEnable
		{
			get => this.linkTestEnable;
			set
			{
				if (this.linkTestEnable == value)
				{
					return;
				}

				this.linkTestEnable = value;
				if (this.linkTestEnable)
				{
					this.timerLinkTest.Change(0, this.LinkTestInterval);
				}
				else
				{
					this.timerLinkTest.Change(Timeout.Infinite, Timeout.Infinite);
				}
			}
		}

		/// <summary>
		/// Linking test timer interval in milliseconds. (Default: 60000)
		/// </summary>
		public int LinkTestInterval
		{
			get => this.linkTestInterval;
			set
			{
				if (this.linkTestEnable)
				{
					this.linkTestInterval = value;
					this.timerLinkTest.Change(0, this.linkTestInterval);
				}
			}
		}

		public ISecsGemLogger Logger
		{
			get => this.logger;
			set => this.logger = value ?? SecsGem.defaultLogger;
		}

		public int Port { get; }

		/// <summary>
		/// Connection state
		/// </summary>
		public ConnectionState State { get; private set; }

		/// <summary>
		/// T3 timer interval in milliseconds. (Default: 45000)
		/// </summary>
		public int T3 { get; set; } = 45000;

		/// <summary>
		/// T5 timer interval in milliseconds. (Default: 10000)
		/// </summary>
		public int T5 { get; set; } = 10000;

		/// <summary>
		/// T6 timer interval in milliseconds. (Default: 5000)
		/// </summary>
		public int T6 { get; set; } = 5000;

		/// <summary>
		/// T7 timer interval in milliseconds. (Default: 10000)
		/// </summary>
		public int T7 { get; set; } = 10000;

		/// <summary>
		/// T8 timer interval in milliseconds. (Default: 5000)
		/// </summary>
		public int T8 { get; set; } = 5000;

		public void Dispose()
		{
			if (Interlocked.Exchange(ref this.disposeStage, SecsGem.disposalComplete) != SecsGem.disposalNotStarted)
			{
				return;
			}

			this.ConnectionChanged = null;
			if (this.State == ConnectionState.Selected)
			{
				this.SendControlMessage(MessageType.SeperateRequest, this.GetNewSystemId());
			}

			this.Reset();
			this.socketAsyncEventArgsPool.Dispose();
			this.timer7.Dispose();
			this.timer8.Dispose();
			this.timerLinkTest.Dispose();
			this.PrimaryMessageReceived = null;
		}

		/// <summary>
		/// Asynchronously send message to device.
		/// </summary>
		/// <param name="secsMessage">primary message</param>
		/// <returns>secondary message</returns>
		public Task<SecsMessage> SendAsync(SecsMessage secsMessage) => this.SendDataMessageAsync(secsMessage, this.GetNewSystemId());

		public void Start() => Task.Run(this.startImplementationFunction);

		internal int GetNewSystemId() => this.systemByteGenerator.New();

		internal Task<SecsMessage> SendDataMessageAsync(SecsMessage secsMessage, int systemBytes)
		{
			if (this.State != ConnectionState.Selected)
			{
				throw new SecsException("Device is not selected.");
			}

			var token = new TaskCompletionSourceToken(secsMessage, systemBytes, MessageType.DataMessage);
			if (secsMessage.ReplyExpected)
			{
				this.replyExpectedMessages.Add(token);
			}

			var header = new MessageHeader
			(
				s: secsMessage.S,
				f: secsMessage.F,
				replyExpected: secsMessage.ReplyExpected,
				deviceId: this.DeviceId,
				systemBytes: systemBytes
			);

			var bufferList = secsMessage.RawDatas.Value;
			bufferList[1] = new ArraySegment<byte>(header.EncodeTo(new byte[10]));

			var socketAsyncEventArgs = this.socketAsyncEventArgsPool.Lend();

			socketAsyncEventArgs.BufferList = bufferList;
			socketAsyncEventArgs.UserToken = token;
			socketAsyncEventArgs.Completed += this.SendDataMessageCompleteHandler;

			if (!this.socket.SendAsync(socketAsyncEventArgs))
			{
				this.SendDataMessageCompleteHandler(this.socket, socketAsyncEventArgs);
			}

			return token.Task;
		}

		private void CommunicationStateChanging(ConnectionState newState)
		{
			this.State = newState;
			this.ConnectionChanged?.Invoke(this, this.State);

			switch (this.State)
			{
				case ConnectionState.Selected:
					this.timer7.Change(Timeout.Infinite, Timeout.Infinite);
					this.logger.Info("Stop T7 Timer");
					break;

				case ConnectionState.Connected:
#if !DISABLE_TIMER
					this.logger.Info($"Start T7 Timer: {this.T7 / 1000} sec.");
					this.timer7.Change(this.T7, Timeout.Infinite);
#endif
					break;

				case ConnectionState.Retry:
					if (this.IsDisposed)
					{
						return;
					}

					this.Reset();
					Task.Run(this.startImplementationFunction);
					break;
			}
		}

		private void HandleControlMessage(MessageHeader header)
		{
			int systemBytes = header.SystemBytes;
			if (((byte)header.MessageType & 1) == 0)
			{
				if (this.replyExpectedMessages.TryGetValue(systemBytes, out var token))
				{
					token.SetResult(controlMessage);
				}
				else
				{
					this.logger.Warning($"Received Unexpected Control Message: {header.MessageType}");
					return;
				}
			}

			this.logger.Info($"Receive Control message: {header.MessageType}");

			switch (header.MessageType)
			{
				case MessageType.SelectRequest:
					this.SendControlMessage(MessageType.SelectResponse, systemBytes);
					this.CommunicationStateChanging(ConnectionState.Selected);
					break;

				case MessageType.SelectResponse:
					switch (header.F)
					{
						case 0:
							this.CommunicationStateChanging(ConnectionState.Selected);
							break;

						case 1:
							this.logger.Error("Communication Already Active.");
							break;

						case 2:
							this.logger.Error("Connection Not Ready.");
							break;

						case 3:
							this.logger.Error("Connection Exhaust.");
							break;

						default:
							this.logger.Error("Connection Status Is Unknown.");
							break;
					}
					break;

				case MessageType.LinkTestRequest:
					this.SendControlMessage(MessageType.LinkTestResponse, systemBytes);
					break;

				case MessageType.SeperateRequest:
					this.CommunicationStateChanging(ConnectionState.Retry);
					break;
			}
		}

		private void HandleDataMessage(MessageHeader header, SecsMessage secsMessage)
		{
			int systemBytes = header.SystemBytes;

			if (header.DeviceId != this.DeviceId && secsMessage.S != 9 && secsMessage.F != 1)
			{
				this.logger.MessageIn(secsMessage, systemBytes);
				this.logger.Warning("Received Unrecognized Device Id Message");
				this.SendDataMessageAsync(new SecsMessage(9, 1, "Unrecognized Device Id", Item.B(header.EncodeTo(new byte[10])), replyExpected: false), this.GetNewSystemId());
				return;
			}

			if ((secsMessage.F & 1) == 1)
			{
				if (secsMessage.S != 9)
				{
					//Primary message
					this.logger.MessageIn(secsMessage, systemBytes);

					Task.Factory.StartNew(
						wrapper => this.InvokePrimaryMessageReceived(Unsafe.As<PrimaryMessageWrapper>(wrapper)),
						new PrimaryMessageWrapper(this, header, secsMessage));

					return;
				}
				// Error message
				var headerBytes = secsMessage.SecsItem.GetValues<byte>();
				systemBytes = BitConverter.ToInt32(new[] { headerBytes[9], headerBytes[8], headerBytes[7], headerBytes[6] }, 0);
			}

			// Secondary message
			this.logger.MessageIn(secsMessage, systemBytes);
			if (this.replyExpectedMessages.TryGetValue(systemBytes, out var token))
			{
				token.HandleReplyMessage(secsMessage);
			}
		}

		private void InvokePrimaryMessageReceived(PrimaryMessageWrapper wrapper)
		{
			this.PrimaryMessageReceived?.Invoke(this, wrapper);
		}

		private void Reset()
		{
			this.timer7.Change(Timeout.Infinite, Timeout.Infinite);
			this.timer8.Change(Timeout.Infinite, Timeout.Infinite);
			this.timerLinkTest.Change(Timeout.Infinite, Timeout.Infinite);
			this.secsDecoder.Reset();
			this.replyExpectedMessages.Clear();

			if (this.socket != null)
			{
				if (this.socket.Connected)
				{
					this.socket.Shutdown(SocketShutdown.Both);
				}

				this.socket.Dispose();
				this.socket = null;
			}

			if (this.serverSocket != null)
			{
				if (this.serverSocket.Connected)
				{
					this.serverSocket.Shutdown(SocketShutdown.Both);
				}

				this.serverSocket.Dispose();
				this.serverSocket = null;
			}

			this.socketAsyncEventArgsPool.Reset();
		}

		private void SendControlMessage(MessageType messageType, int systemBytes)
		{
			var token = new TaskCompletionSourceToken(controlMessage, systemBytes, messageType);
			if (((byte)messageType & 1) == 1 && messageType != MessageType.SeperateRequest)
			{
				this.replyExpectedMessages.Add(token);
			}

			var socketAsyncEventArgs = this.socketAsyncEventArgsPool.Lend();

			socketAsyncEventArgs.BufferList = new List<ArraySegment<byte>>(2)
			{
				SecsGem.controlMessageLengthBytes,
				new ArraySegment<byte>(new MessageHeader(
					deviceId: 0xFFFF,
					messageType: messageType,
					systemBytes: systemBytes
				).EncodeTo(new byte[10]))
			};

			socketAsyncEventArgs.UserToken = token;

			socketAsyncEventArgs.Completed += this.SendControlMessageCompleteHandler;

			if (!this.socket.SendAsync(socketAsyncEventArgs))
			{
				this.SendControlMessageCompleteHandler(this.socket, socketAsyncEventArgs);
			}
		}

		private void SendControlMessageCompleteHandler(object sender, SocketAsyncEventArgs e)
		{
			e.Completed -= this.SendControlMessageCompleteHandler;

			try
			{
				var completeToken = Unsafe.As<TaskCompletionSourceToken>(e.UserToken);

				if (e.SocketError != SocketError.Success)
				{
					completeToken.SetException(new SocketException((int)e.SocketError));
					return;
				}

				this.logger.Info($"Sent Control Message: {completeToken.MessageType}");
				if (this.replyExpectedMessages.Contains(completeToken))
				{
					if (!completeToken.Task.Wait(this.T6))
					{
						this.logger.Error($"T6 Timeout: {this.T6 / 1000} sec.");
						this.CommunicationStateChanging(ConnectionState.Retry);
					}
					this.replyExpectedMessages.Remove(completeToken);
				}
			}
			finally
			{
				this.socketAsyncEventArgsPool.Return(e);
			}
		}

		private void SendDataMessageCompleteHandler(object sender, SocketAsyncEventArgs e)
		{
			e.Completed -= this.SendDataMessageCompleteHandler;

			try
			{
				var completeToken = Unsafe.As<TaskCompletionSourceToken>(e.UserToken);

				if (e.SocketError != SocketError.Success)
				{
					completeToken.SetException(new SocketException((int)e.SocketError));
					this.CommunicationStateChanging(ConnectionState.Retry);
					return;
				}

				this.logger.MessageOut(completeToken.SentSecsMessage, completeToken.SystemBytes);

				if (!this.replyExpectedMessages.Contains(completeToken))
				{
					completeToken.SetResult(null);
					return;
				}

				try
				{
					if (!completeToken.Task.Wait(this.T3))
					{
						this.logger.Error($"T3 Timeout[id=0x{completeToken.SystemBytes:X8}]: {this.T3 / 1000} sec.");
						completeToken.SetException(new SecsSentMessageException(completeToken.SentSecsMessage, Resources.T3Timeout));
					}
				}
				catch (AggregateException) { }
				finally
				{
					this.replyExpectedMessages.Remove(completeToken);
				}
			}
			finally
			{
				this.socketAsyncEventArgsPool.Return(e);
			}
		}

		private void SocketReceiveEventCompleted(object sender, SocketAsyncEventArgs e)
		{
			e.Completed -= this.SocketReceiveEventCompleted;

			try
			{
				if (e.SocketError != SocketError.Success)
				{
					var ex = new SocketException((int)e.SocketError);
					this.logger.Error($"RecieveComplete socket error:{ex.Message}, ErrorCode:{ex.SocketErrorCode}", ex);
					this.CommunicationStateChanging(ConnectionState.Retry);
					return;
				}

				try
				{
					this.timer8.Change(Timeout.Infinite, Timeout.Infinite);
					var receivedCount = e.BytesTransferred;
					if (receivedCount == 0)
					{
						this.logger.Error("Received 0 byte.");
						this.CommunicationStateChanging(ConnectionState.Retry);
						return;
					}

					if (this.secsDecoder.Decode(receivedCount))
					{
#if !DISABLE_T8
						this.logger.Debug($"Start T8 Timer: {this.T8 / 1000} sec.");
						this.timer8.Change(this.T8, Timeout.Infinite);
#endif
					}

					if (this.socket is null || this.IsDisposed)
					{
						return;
					}

					this.DecoderBufferSize = this.secsDecoder.Buffer.Length;

					var receiveCompleteEvent = this.socketAsyncEventArgsPool.Lend();
					receiveCompleteEvent.SetBuffer(this.secsDecoder.Buffer, this.secsDecoder.BufferOffset, this.secsDecoder.BufferCount);

					receiveCompleteEvent.Completed += this.SocketReceiveEventCompleted;
					if (!this.socket.ReceiveAsync(receiveCompleteEvent))
					{
						this.SocketReceiveEventCompleted(sender, receiveCompleteEvent);
					}
				}
				catch (Exception ex)
				{
					this.logger.Error("Unexpected exception", ex);
					this.CommunicationStateChanging(ConnectionState.Retry);
				}
			}
			finally
			{
				this.socketAsyncEventArgsPool.Return(e);
			}
		}

		private void StartSocketReceive()
		{
			this.CommunicationStateChanging(ConnectionState.Connected);

			var receiveCompleteEvent = this.socketAsyncEventArgsPool.Lend();

			receiveCompleteEvent.SetBuffer(this.secsDecoder.Buffer, this.secsDecoder.BufferOffset, this.secsDecoder.BufferCount);
			receiveCompleteEvent.Completed += this.SocketReceiveEventCompleted;

			if (!this.socket.ReceiveAsync(receiveCompleteEvent))
			{
				this.SocketReceiveEventCompleted(this.socket, receiveCompleteEvent);
			}
		}

		private sealed class TaskCompletionSourceToken :
			TaskCompletionSource<SecsMessage>
		{
			internal TaskCompletionSourceToken(SecsMessage primarySecsMessage, int systemBytes, MessageType messageType)
			{
				this.SentSecsMessage = primarySecsMessage;
				this.SystemBytes = systemBytes;
				this.MessageType = messageType;
			}

			internal MessageType MessageType { get; }

			internal SecsMessage SentSecsMessage { get; }

			internal int SystemBytes { get; }

			internal void HandleReplyMessage(SecsMessage replySecsMessage)
			{
				replySecsMessage.Name = this.SentSecsMessage.Name;
				if (replySecsMessage.F == 0)
				{
					this.SetException(new SecsDialogMessagesException(this.SentSecsMessage, replySecsMessage, Resources.SxF0));
					return;
				}

				if (replySecsMessage.S == 9)
				{
					switch (replySecsMessage.F)
					{
						case 1:
							this.SetException(new SecsDialogMessagesException(this.SentSecsMessage, replySecsMessage, Resources.S9F1));
							break;

						case 3:
							this.SetException(new SecsDialogMessagesException(this.SentSecsMessage, replySecsMessage, Resources.S9F3));
							break;

						case 5:
							this.SetException(new SecsDialogMessagesException(this.SentSecsMessage, replySecsMessage, Resources.S9F5));
							break;

						case 7:
							this.SetException(new SecsDialogMessagesException(this.SentSecsMessage, replySecsMessage, Resources.S9F7));
							break;

						case 9:
							this.SetException(new SecsDialogMessagesException(this.SentSecsMessage, replySecsMessage, Resources.S9F9));
							break;

						case 11:
							this.SetException(new SecsDialogMessagesException(this.SentSecsMessage, replySecsMessage, Resources.S9F11));
							break;

						case 13:
							this.SetException(new SecsDialogMessagesException(this.SentSecsMessage, replySecsMessage, Resources.S9F13));
							break;

						default:
							this.SetException(new SecsDialogMessagesException(this.SentSecsMessage, replySecsMessage, Resources.S9Fy));
							break;
					}
					return;
				}

				this.SetResult(replySecsMessage);
			}
		}

		private sealed class TokenCache
		{
			private readonly Dictionary<int, TaskCompletionSourceToken> dictionary = new Dictionary<int, TaskCompletionSourceToken>();

			public void Add(TaskCompletionSourceToken token)
			{
				lock (this.dictionary)
				{
					this.dictionary.Add(token.SystemBytes, token);
				}
			}

			public void Clear()
			{
				lock (this.dictionary)
				{
					this.dictionary.Clear();
				}
			}

			public bool Contains(TaskCompletionSourceToken token)
			{
				lock (this.dictionary)
				{
					return this.dictionary.ContainsKey(token.SystemBytes);
				}
			}
			public bool Remove(TaskCompletionSourceToken token)
			{
				lock (this.dictionary)
				{
					return this.dictionary.Remove(token.SystemBytes);
				}
			}

			public bool TryGetValue(int id, out TaskCompletionSourceToken token)
			{
				lock (this.dictionary)
				{
					return this.dictionary.TryGetValue(id, out token);
				}
			}
		}
	}
}