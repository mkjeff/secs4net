using System;
using System.Collections.Concurrent;
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

		private readonly ConcurrentDictionary<int, TaskCompletionSourceToken> _replyExpectedMsgs = new ConcurrentDictionary<int, TaskCompletionSourceToken>();

		private readonly StreamDecoder _secsDecoder;

		private readonly Func<Task> _startImpl;

		private readonly SystemByteGenerator systemByteGenerator = new SystemByteGenerator();

		/// <summary>
		/// between socket connected and received Select.req timer
		/// </summary>
		private readonly Timer _timer7;

		private readonly Timer _timer8;

		private readonly Timer _timerLinkTest;

		private readonly SocketAsyncEventArgsPool socketAsyncEventArgsPool = new SocketAsyncEventArgsPool();

		private int _disposeStage;

		private bool _linkTestEnable;

		private int _linkTestInterval = 60000;

		private ISecsGemLogger _logger = defaultLogger;

		private Socket _socket;

		private Socket serverSocket;

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

			this._secsDecoder = new StreamDecoder(receiveBufferSize, this.HandleControlMessage, this.HandleDataMessage);

			this.IpAddress = ip;
			this.Port = port;
			this.IsActive = isActive;
			this.DecoderBufferSize = receiveBufferSize;

			#region Timer Action
			this._timer7 = new Timer(delegate
			{
				this._logger.Error($"T7 Timeout: {this.T7 / 1000} sec.");
				this.CommunicationStateChanging(ConnectionState.Retry);
			}, null, Timeout.Infinite, Timeout.Infinite);

			this._timer8 = new Timer(delegate
			{
				this._logger.Error($"T8 Timeout: {this.T8 / 1000} sec.");
				this.CommunicationStateChanging(ConnectionState.Retry);
			}, null, Timeout.Infinite, Timeout.Infinite);

			this._timerLinkTest = new Timer(delegate
			{
				if (this.State == ConnectionState.Selected)
				{
					this.SendControlMessage(MessageType.LinkTestRequest, this.NewSystemId);
				}
			}, null, Timeout.Infinite, Timeout.Infinite);
			#endregion

			if (this.IsActive)
			{
				this._startImpl = async () =>
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

							this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
							await this._socket.ConnectAsync(this.IpAddress, this.Port).ConfigureAwait(false);
							connected = true;
						}
						catch (Exception ex)
						{
							if (this.IsDisposed)
							{
								return;
							}

							this._logger.Error(ex.Message);
							this._logger.Info($"Start T5 Timer: {this.T5 / 1000} sec.");
							await Task.Delay(this.T5).ConfigureAwait(false);
						}
					} while (!connected);

					// hook receive event first, because no message will received before 'SelectRequest' send to device
					this.StartSocketReceive();
					this.SendControlMessage(MessageType.SelectRequest, this.NewSystemId);
				};
			}
			else
			{
				this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				this.serverSocket.Bind(new IPEndPoint(this.IpAddress, this.Port));
				this.serverSocket.Listen(0);

				this._startImpl = async () =>
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

							this._socket = await this.serverSocket.AcceptAsync().ConfigureAwait(false);
							connected = true;
						}
						catch (Exception ex)
						{
							if (this.IsDisposed)
							{
								return;
							}

							this._logger.Error(ex.Message);
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
			: ((IPEndPoint)this._socket?.RemoteEndPoint)?.Address?.ToString() ?? "NA";

		public IPAddress IpAddress { get; }

		public bool IsActive { get; }

		public bool IsDisposed => Interlocked.CompareExchange(ref this._disposeStage, SecsGem.disposalComplete, SecsGem.disposalComplete) == SecsGem.disposalComplete;

		/// <summary>
		/// get or set linking test timer enable or not 
		/// </summary>
		public bool LinkTestEnable
		{
			get => this._linkTestEnable;
			set
			{
				if (this._linkTestEnable == value)
				{
					return;
				}

				this._linkTestEnable = value;
				if (this._linkTestEnable)
				{
					this._timerLinkTest.Change(0, this.LinkTestInterval);
				}
				else
				{
					this._timerLinkTest.Change(Timeout.Infinite, Timeout.Infinite);
				}
			}
		}

		/// <summary>
		/// Linking test timer interval
		/// </summary>
		public int LinkTestInterval
		{
			get => this._linkTestInterval;
			set
			{
				if (this._linkTestEnable)
				{
					this._linkTestInterval = value;
					this._timerLinkTest.Change(0, this._linkTestInterval);
				}
			}
		}

		public ISecsGemLogger Logger
		{
			get => this._logger;
			set => this._logger = value ?? SecsGem.defaultLogger;
		}

		public int Port { get; }

		/// <summary>
		/// Connection state
		/// </summary>
		public ConnectionState State { get; private set; }

		/// <summary>
		/// T3 timer interval 
		/// </summary>
		public int T3 { get; set; } = 45000;

		/// <summary>
		/// T5 timer interval
		/// </summary>
		public int T5 { get; set; } = 10000;

		/// <summary>
		/// T6 timer interval
		/// </summary>
		public int T6 { get; set; } = 5000;

		/// <summary>
		/// T7 timer interval
		/// </summary>
		public int T7 { get; set; } = 10000;

		/// <summary>
		/// T8 timer interval
		/// </summary>
		public int T8 { get; set; } = 5000;

		internal int NewSystemId => this.systemByteGenerator.New();

		public void Dispose()
		{
			if (Interlocked.Exchange(ref this._disposeStage, SecsGem.disposalComplete) != SecsGem.disposalNotStarted)
			{
				return;
			}

			this.ConnectionChanged = null;
			if (this.State == ConnectionState.Selected)
			{
				this.SendControlMessage(MessageType.SeperateRequest, this.NewSystemId);
			}

			this.Reset();
			this._timer7.Dispose();
			this._timer8.Dispose();
			this._timerLinkTest.Dispose();
			this.PrimaryMessageReceived = null;
		}

		/// <summary>
		/// Asynchronously send message to device .
		/// </summary>
		/// <param name="msg">primary message</param>
		/// <returns>secondary message</returns>
		public Task<SecsMessage> SendAsync(SecsMessage msg) => this.SendDataMessageAsync(msg, this.NewSystemId);

		public void Start() => new TaskFactory(TaskScheduler.Default).StartNew(this._startImpl);

		internal Task<SecsMessage> SendDataMessageAsync(SecsMessage msg, int systembyte)
		{
			if (this.State != ConnectionState.Selected)
			{
				throw new SecsException("Device is not selected");
			}

			var token = new TaskCompletionSourceToken(msg, systembyte, MessageType.DataMessage);
			if (msg.ReplyExpected)
			{
				this._replyExpectedMsgs[systembyte] = token;
			}

			var header = new MessageHeader
			(
				s: msg.S,
				f: msg.F,
				replyExpected: msg.ReplyExpected,
				deviceId: this.DeviceId,
				systemBytes: systembyte
			);

			var bufferList = msg.RawDatas.Value;
			bufferList[1] = new ArraySegment<byte>(header.EncodeTo(new byte[10]));

			var socketAsyncEventArgs = this.socketAsyncEventArgsPool.Lend();

			socketAsyncEventArgs.BufferList = bufferList;
			socketAsyncEventArgs.UserToken = token;
			socketAsyncEventArgs.Completed += this.SendDataMessageCompleteHandler;

			if (!this._socket.SendAsync(socketAsyncEventArgs))
			{
				this.SendDataMessageCompleteHandler(this._socket, socketAsyncEventArgs);
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
					this._timer7.Change(Timeout.Infinite, Timeout.Infinite);
					this._logger.Info("Stop T7 Timer");
					break;

				case ConnectionState.Connected:
#if !DISABLE_TIMER
					this._logger.Info($"Start T7 Timer: {this.T7 / 1000} sec.");
					this._timer7.Change(this.T7, Timeout.Infinite);
#endif
					break;

				case ConnectionState.Retry:
					if (this.IsDisposed)
					{
						return;
					}

					this.Reset();
					Task.Factory.StartNew(this._startImpl);
					break;
			}
		}

		private void HandleControlMessage(MessageHeader header)
		{
			var systemBytes = header.SystemBytes;
			if ((byte)header.MessageType % 2 == 0)
			{
				if (this._replyExpectedMsgs.TryGetValue(systemBytes, out var ar))
				{
					ar.SetResult(controlMessage);
				}
				else
				{
					this._logger.Warning("Received Unexpected Control Message: " + header.MessageType);
					return;
				}
			}

			this._logger.Info("Receive Control message: " + header.MessageType);

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
							this._logger.Error("Communication Already Active.");
							break;

						case 2:
							this._logger.Error("Connection Not Ready.");
							break;

						case 3:
							this._logger.Error("Connection Exhaust.");
							break;

						default:
							this._logger.Error("Connection Status Is Unknown.");
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

		private void HandleDataMessage(MessageHeader header, SecsMessage msg)
		{
			var systembyte = header.SystemBytes;

			if (header.DeviceId != this.DeviceId && msg.S != 9 && msg.F != 1)
			{
				this._logger.MessageIn(msg, systembyte);
				this._logger.Warning("Received Unrecognized Device Id Message");
				this.SendDataMessageAsync(new SecsMessage(9, 1, "Unrecognized Device Id", Item.B(header.EncodeTo(new byte[10])), replyExpected: false), this.NewSystemId);
				return;
			}

			if (msg.F % 2 != 0)
			{
				if (msg.S != 9)
				{
					//Primary message
					this._logger.MessageIn(msg, systembyte);

					Task.Factory.StartNew(
						wrapper => this.InvokePrimaryMessageReceived(Unsafe.As<PrimaryMessageWrapper>(wrapper)),
						new PrimaryMessageWrapper(this, header, msg));

					return;
				}
				// Error message
				var headerBytes = msg.SecsItem.GetValues<byte>();
				systembyte = BitConverter.ToInt32(new[] { headerBytes[9], headerBytes[8], headerBytes[7], headerBytes[6] }, 0);
			}

			// Secondary message
			this._logger.MessageIn(msg, systembyte);
			if (this._replyExpectedMsgs.TryGetValue(systembyte, out var ar))
			{
				ar.HandleReplyMessage(msg);
			}
		}

		private void InvokePrimaryMessageReceived(PrimaryMessageWrapper wrapper)
		{
			this.PrimaryMessageReceived?.Invoke(this, wrapper);
		}

		private void Reset()
		{
			this._timer7.Change(Timeout.Infinite, Timeout.Infinite);
			this._timer8.Change(Timeout.Infinite, Timeout.Infinite);
			this._timerLinkTest.Change(Timeout.Infinite, Timeout.Infinite);
			this._secsDecoder.Reset();
			this._replyExpectedMsgs.Clear();

			if (this._socket != null)
			{
				if (this._socket.Connected)
				{
					this._socket.Shutdown(SocketShutdown.Both);
				}

				this._socket.Dispose();
				this._socket = null;
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

			this.socketAsyncEventArgsPool.Dispose();
		}

		private void SendControlMessage(MessageType msgType, int systembyte)
		{
			var token = new TaskCompletionSourceToken(controlMessage, systembyte, msgType);
			if ((byte)msgType % 2 == 1 && msgType != MessageType.SeperateRequest)
			{
				this._replyExpectedMsgs[systembyte] = token;
			}

			var socketAsyncEventArgs = this.socketAsyncEventArgsPool.Lend();

			socketAsyncEventArgs.BufferList = new List<ArraySegment<byte>>(2)
			{
				SecsGem.controlMessageLengthBytes,
				new ArraySegment<byte>(new MessageHeader(
					deviceId: 0xFFFF,
					messageType: msgType,
					systemBytes: systembyte
				).EncodeTo(new byte[10]))
			};

			socketAsyncEventArgs.UserToken = token;

			socketAsyncEventArgs.Completed += this.SendControlMessageCompleteHandler;

			if (!this._socket.SendAsync(socketAsyncEventArgs))
			{
				this.SendControlMessageCompleteHandler(this._socket, socketAsyncEventArgs);
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

				this._logger.Info("Sent Control Message: " + completeToken.MsgType);
				if (this._replyExpectedMsgs.ContainsKey(completeToken.Id))
				{
					if (!completeToken.Task.Wait(this.T6))
					{
						this._logger.Error($"T6 Timeout: {this.T6 / 1000} sec.");
						this.CommunicationStateChanging(ConnectionState.Retry);
					}
					this._replyExpectedMsgs.TryRemove(completeToken.Id, out _);
				}
			}
			finally
			{
				this.socketAsyncEventArgsPool.Return(e);
			}
		}

		private void SendDataMessageCompleteHandler(object socket, SocketAsyncEventArgs e)
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

				this._logger.MessageOut(completeToken.SentSecsMessage, completeToken.Id);

				if (!this._replyExpectedMsgs.ContainsKey(completeToken.Id))
				{
					completeToken.SetResult(null);
					return;
				}

				try
				{
					if (!completeToken.Task.Wait(this.T3))
					{
						this._logger.Error($"T3 Timeout[id=0x{completeToken.Id:X8}]: {this.T3 / 1000} sec.");
						completeToken.SetException(new SecsSentMessageException(completeToken.SentSecsMessage, Resources.T3Timeout));
					}
				}
				catch (AggregateException) { }
				finally
				{
					this._replyExpectedMsgs.TryRemove(completeToken.Id, out _);
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

			receiveCompleteEvent.SetBuffer(this._secsDecoder.Buffer, this._secsDecoder.BufferOffset, this._secsDecoder.BufferCount);
			receiveCompleteEvent.Completed += this.SocketReceiveEventCompleted;

			if (!this._socket.ReceiveAsync(receiveCompleteEvent))
			{
				this.SocketReceiveEventCompleted(this._socket, receiveCompleteEvent);
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
					this._logger.Error($"RecieveComplete socket error:{ex.Message}, ErrorCode:{ex.SocketErrorCode}", ex);
					this.CommunicationStateChanging(ConnectionState.Retry);
					return;
				}

				try
				{
					this._timer8.Change(Timeout.Infinite, Timeout.Infinite);
					var receivedCount = e.BytesTransferred;
					if (receivedCount == 0)
					{
						this._logger.Error("Received 0 byte.");
						this.CommunicationStateChanging(ConnectionState.Retry);
						return;
					}

					if (this._secsDecoder.Decode(receivedCount))
					{
#if !DISABLE_T8
						this._logger.Debug($"Start T8 Timer: {this.T8 / 1000} sec.");
						this._timer8.Change(this.T8, Timeout.Infinite);
#endif
					}

					if (this._socket is null || this.IsDisposed)
					{
						return;
					}

					this.DecoderBufferSize = this._secsDecoder.Buffer.Length;

					var receiveCompleteEvent = this.socketAsyncEventArgsPool.Lend();
					receiveCompleteEvent.SetBuffer(this._secsDecoder.Buffer, this._secsDecoder.BufferOffset, this._secsDecoder.BufferCount);

					receiveCompleteEvent.Completed += this.SocketReceiveEventCompleted;
					if (!this._socket.ReceiveAsync(receiveCompleteEvent))
					{
						this.SocketReceiveEventCompleted(sender, receiveCompleteEvent);
					}
				}
				catch (Exception ex)
				{
					this._logger.Error("Unexpected exception", ex);
					this.CommunicationStateChanging(ConnectionState.Retry);
				}
			}
			finally
			{
				this.socketAsyncEventArgsPool.Return(e);
			}
		}

		private sealed class TaskCompletionSourceToken :
			TaskCompletionSource<SecsMessage>
		{
			internal TaskCompletionSourceToken(SecsMessage primarySecsMessage, int id, MessageType msgType)
			{
				this.SentSecsMessage = primarySecsMessage;
				this.Id = id;
				this.MsgType = msgType;
			}

			internal int Id { get; }

			internal SecsMessage SentSecsMessage { get; }

			internal MessageType MsgType { get; }

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
	}
}