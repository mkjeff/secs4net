using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Net
{
	public sealed class SecsGem : IDisposable
	{
		/// <summary>
		/// HSMS connection state changed event
		/// </summary>
		public event EventHandler<ConnectionState> ConnectionChanged;

		/// <summary>
		/// Primary message received event
		/// </summary>
		public event EventHandler<PrimaryMessageWrapper> PrimaryMessageReceived = DefaultPrimaryMessageReceived;
		private static void DefaultPrimaryMessageReceived(object sender, PrimaryMessageWrapper _) { }

		private ISecsGemLogger _logger = DefaultLogger;
		public ISecsGemLogger Logger
		{
			get => this._logger;
			set => this._logger = value ?? DefaultLogger;
		}

		/// <summary>
		/// Connection state
		/// </summary>
		public ConnectionState State { get; private set; }

		/// <summary>
		/// Device Id.
		/// </summary>
		public ushort DeviceId { get; set; } = 0;

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

		private int _linkTestInterval = 60000;
		private bool _linkTestEnable;

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

		public bool IsActive { get; }
		public IPAddress IpAddress { get; }
		public int Port { get; }
		public int DecoderBufferSize { get; private set; }

		private const int DisposalNotStarted = 0;
		private const int DisposalComplete = 1;
		private int _disposeStage;

		public bool IsDisposed => Interlocked.CompareExchange(ref this._disposeStage, DisposalComplete, DisposalComplete) == DisposalComplete;
		/// <summary>
		/// remote device endpoint address
		/// </summary>
		public string DeviceIpAddress => this.IsActive
			? this.IpAddress.ToString()
			: ((IPEndPoint)this._socket?.RemoteEndPoint)?.Address?.ToString() ?? "NA";

		private Socket _socket;

		private readonly StreamDecoder _secsDecoder;
		private readonly ConcurrentDictionary<int, TaskCompletionSourceToken> _replyExpectedMsgs = new ConcurrentDictionary<int, TaskCompletionSourceToken>();
		private readonly Timer _timer7; // between socket connected and received Select.req timer
		private readonly Timer _timer8;
		private readonly Timer _timerLinkTest;

		private readonly Func<Task> _startImpl;
		private readonly Action _stopImpl;

		private static readonly SecsMessage ControlMessage = new SecsMessage(0, 0, string.Empty);
		private static readonly ArraySegment<byte> ControlMessageLengthBytes = new ArraySegment<byte>(new byte[] { 0, 0, 0, 10 });
		private static readonly DefaultSecsGemLogger DefaultLogger = new DefaultSecsGemLogger();
		private readonly SystemByteGenerator _systemByte = new SystemByteGenerator();

		private readonly EventHandler<SocketAsyncEventArgs> _sendControlMessageCompleteHandler;
		private readonly EventHandler<SocketAsyncEventArgs> _sendDataMessageCompleteHandler;

		internal int NewSystemId => this._systemByte.New();

		private readonly TaskFactory _taskFactory = new TaskFactory(TaskScheduler.Default);

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

			this._sendControlMessageCompleteHandler = this.SendControlMessageCompleteHandler;
			this._sendDataMessageCompleteHandler = this.SendDataMessageCompleteHandler;
			this._secsDecoder = new StreamDecoder(receiveBufferSize, HandleControlMessage, HandleDataMessage);

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
							await Task.Delay(this.T5);
						}
					} while (!connected);

					// hook receive event first, because no message will received before 'SelectRequest' send to device
					StartSocketReceive();
					this.SendControlMessage(MessageType.SelectRequest, this.NewSystemId);
				};

				//_stopImpl = delegate { };
			}
			else
			{
				var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				server.Bind(new IPEndPoint(this.IpAddress, this.Port));
				server.Listen(0);

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

							this._socket = await server.AcceptAsync().ConfigureAwait(false);
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

					StartSocketReceive();
				};

				this._stopImpl = delegate
				{
					if (this.IsDisposed)
					{
						server.Dispose();
					}
				};
			}

			void StartSocketReceive()
			{
				this.CommunicationStateChanging(ConnectionState.Connected);

				var receiveCompleteEvent = new SocketAsyncEventArgs();
				receiveCompleteEvent.SetBuffer(this._secsDecoder.Buffer, this._secsDecoder.BufferOffset, this._secsDecoder.BufferCount);
				receiveCompleteEvent.Completed += SocketReceiveEventCompleted;

				if (!this._socket.ReceiveAsync(receiveCompleteEvent))
				{
					SocketReceiveEventCompleted(this._socket, receiveCompleteEvent);
				}

				void SocketReceiveEventCompleted(object sender, SocketAsyncEventArgs e)
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

						if (this._secsDecoder.Buffer.Length != this.DecoderBufferSize)
						{
							// buffer size changed
							e.SetBuffer(this._secsDecoder.Buffer, this._secsDecoder.BufferOffset, this._secsDecoder.BufferCount);
							this.DecoderBufferSize = this._secsDecoder.Buffer.Length;
						}
						else
						{
							e.SetBuffer(this._secsDecoder.BufferOffset, this._secsDecoder.BufferCount);
						}

						if (this._socket is null || this.IsDisposed)
						{
							return;
						}

						if (!this._socket.ReceiveAsync(e))
						{
							SocketReceiveEventCompleted(sender, e);
						}
					}
					catch (Exception ex)
					{
						this._logger.Error("Unexpected exception", ex);
						this.CommunicationStateChanging(ConnectionState.Retry);
					}
				}
			}

			void HandleControlMessage(MessageHeader header)
			{
				var systembyte = header.SystemBytes;
				if ((byte)header.MessageType % 2 == 0)
				{
					if (this._replyExpectedMsgs.TryGetValue(systembyte, out var ar))
					{
						ar.SetResult(ControlMessage);
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
						this.SendControlMessage(MessageType.SelectResponse, systembyte);
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
						this.SendControlMessage(MessageType.LinkTestResponse, systembyte);
						break;
					case MessageType.SeperateRequest:
						this.CommunicationStateChanging(ConnectionState.Retry);
						break;
				}
			}

			void HandleDataMessage(MessageHeader header, SecsMessage msg)
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
						this._taskFactory.StartNew(
							wrapper => PrimaryMessageReceived(this, Unsafe.As<PrimaryMessageWrapper>(wrapper)),
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
		}

		private void SendControlMessage(MessageType msgType, int systembyte)
		{
			var token = new TaskCompletionSourceToken(ControlMessage, systembyte, msgType);
			if ((byte)msgType % 2 == 1 && msgType != MessageType.SeperateRequest)
			{
				this._replyExpectedMsgs[systembyte] = token;
			}

			var eap = new SocketAsyncEventArgs
			{
				BufferList = new List<ArraySegment<byte>>(2) {
					ControlMessageLengthBytes,
					new ArraySegment<byte>(new MessageHeader(
						deviceId: 0xFFFF,
						messageType: msgType,
						systemBytes: systembyte
					).EncodeTo(new byte[10]))
				},
				UserToken = token,
			};
			eap.Completed += this._sendControlMessageCompleteHandler;
			if (!this._socket.SendAsync(eap))
			{
				this.SendControlMessageCompleteHandler(this._socket, eap);
			}
		}

		private void SendControlMessageCompleteHandler(object o, SocketAsyncEventArgs e)
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
			var eap = new SocketAsyncEventArgs
			{
				BufferList = bufferList,
				UserToken = token,
			};
			eap.Completed += this._sendDataMessageCompleteHandler;
			if (!this._socket.SendAsync(eap))
			{
				this.SendDataMessageCompleteHandler(this._socket, eap);
			}

			return token.Task;
		}

		private void SendDataMessageCompleteHandler(object socket, SocketAsyncEventArgs e)
		{
			var completeToken = Unsafe.As<TaskCompletionSourceToken>(e.UserToken);

			if (e.SocketError != SocketError.Success)
			{
				completeToken.SetException(new SocketException((int)e.SocketError));
				this.CommunicationStateChanging(ConnectionState.Retry);
				return;
			}

			this._logger.MessageOut(completeToken.MessageSent, completeToken.Id);

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
					completeToken.SetException(new SecsException(completeToken.MessageSent, Resources.T3Timeout));
				}
			}
			catch (AggregateException) { }
			finally
			{
				this._replyExpectedMsgs.TryRemove(completeToken.Id, out _);
			}
		}

		private void CommunicationStateChanging(ConnectionState newState)
		{
			this.State = newState;
			ConnectionChanged?.Invoke(this, this.State);

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

		private void Reset()
		{
			this._timer7.Change(Timeout.Infinite, Timeout.Infinite);
			this._timer8.Change(Timeout.Infinite, Timeout.Infinite);
			this._timerLinkTest.Change(Timeout.Infinite, Timeout.Infinite);
			this._secsDecoder.Reset();
			this._replyExpectedMsgs.Clear();
			this._stopImpl?.Invoke();

			if (this._socket is null)
			{
				return;
			}

			if (this._socket.Connected)
			{
				this._socket.Shutdown(SocketShutdown.Both);
			}

			this._socket.Dispose();
			this._socket = null;
		}

		public void Start() => new TaskFactory(TaskScheduler.Default).StartNew(this._startImpl);

		/// <summary>
		/// Asynchronously send message to device .
		/// </summary>
		/// <param name="msg">primary message</param>
		/// <returns>secondary message</returns>
		public Task<SecsMessage> SendAsync(SecsMessage msg) => this.SendDataMessageAsync(msg, this.NewSystemId);

		public void Dispose()
		{
			if (Interlocked.Exchange(ref this._disposeStage, DisposalComplete) != DisposalNotStarted)
			{
				return;
			}

			ConnectionChanged = null;
			if (this.State == ConnectionState.Selected)
			{
				this.SendControlMessage(MessageType.SeperateRequest, this.NewSystemId);
			}

			this.Reset();
			this._timer7.Dispose();
			this._timer8.Dispose();
			this._timerLinkTest.Dispose();
		}



		private sealed class TaskCompletionSourceToken : TaskCompletionSource<SecsMessage>
		{
			internal readonly SecsMessage MessageSent;
			internal readonly int Id;
			internal readonly MessageType MsgType;

			internal TaskCompletionSourceToken(SecsMessage primaryMessageMsg, int id, MessageType msgType)
			{
				this.MessageSent = primaryMessageMsg;
				this.Id = id;
				this.MsgType = msgType;
			}

			internal void HandleReplyMessage(SecsMessage replyMsg)
			{
				replyMsg.Name = this.MessageSent.Name;
				if (replyMsg.F == 0)
				{
					this.SetException(new SecsException(this.MessageSent, Resources.SxF0));
					return;
				}

				if (replyMsg.S == 9)
				{
					switch (replyMsg.F)
					{
						case 1:
							this.SetException(new SecsException(this.MessageSent, Resources.S9F1));
							break;
						case 3:
							this.SetException(new SecsException(this.MessageSent, Resources.S9F3));
							break;
						case 5:
							this.SetException(new SecsException(this.MessageSent, Resources.S9F5));
							break;
						case 7:
							this.SetException(new SecsException(this.MessageSent, Resources.S9F7));
							break;
						case 9:
							this.SetException(new SecsException(this.MessageSent, Resources.S9F9));
							break;
						case 11:
							this.SetException(new SecsException(this.MessageSent, Resources.S9F11));
							break;
						case 13:
							this.SetException(new SecsException(this.MessageSent, Resources.S9F13));
							break;
						default:
							this.SetException(new SecsException(this.MessageSent, Resources.S9Fy));
							break;
					}
					return;
				}

				this.SetResult(replyMsg);
			}
		}
	}
}