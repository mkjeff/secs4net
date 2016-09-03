using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Net;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Cim.Management;
using Secs4Net;
using System.Xml.Linq;
using Cim.Eap.Tx;
using Cim.Eap.Data;
using System.Threading;
using System.Xml;

namespace Cim.Eap
{
    sealed partial class HostMainForm : Form, IEAP
    {
        class SecsLogger : ISecsGemLogger
        {
            public void TraceMessageIn(SecsMessage msg, int systembyte)
            {
                EapLogger.Info(new SecsMessageLogInfo
                {
                    In = true,
                    Message = msg,
                    SystemByte = systembyte
                });
            }

            public void TraceMessageOut(SecsMessage msg, int systembyte)
            {
                EapLogger.Info(new SecsMessageLogInfo
                {
                    In = false,
                    Message = msg,
                    SystemByte = systembyte
                });
            }

            public void TraceInfo(string msg)
            {
                EapLogger.Info("SECS/GEM Info: " + msg);
            }

            public void TraceWarning(string msg)
            {
                EapLogger.Warn("SECS/GEM Warning: " + msg);
            }

            public void TraceError(string msg, Exception ex = null)
            {
                EapLogger.Error("SECS/GEM Error: " + msg);
            }

            public void TraceDebug(string msg)
            {
                EapLogger.Debug("SECS/GEM Error: " + msg);
            }
        }

        SecsGem _secsGem;
        readonly TextBoxAppender _screenLoger;
        readonly ISecsGemLogger _secsLogger = new SecsLogger();

        public HostMainForm()
        {
            InitializeComponent();

            this.Text = ToolId + " EAP";
            this.eapDriverLabel.Text = EAPConfig.Instance.Driver.GetType().AssemblyQualifiedName;
            this.SecsMessages = new SecsMessageList(EAPConfig.Instance.SmlFile);
            this.EventReportLink = new DefineLinkConfig(EAPConfig.Instance.GemXml);

            listBoxSecsMessages.BeginUpdate();
            foreach (var msg in this.SecsMessages)
                listBoxSecsMessages.Items.Add($"S{msg.S,-3}F{msg.F,3} : {msg.Name}");
            listBoxSecsMessages.EndUpdate();

            var l = (Logger)LogManager.GetLogger("EAP").Logger;
            l.AddAppender(
                _screenLoger = new TextBoxAppender(this.rtxtScreen)
                {
                    Layout = new PatternLayout("%date{MM/dd HH:mm:ss} %message%newline")
                });
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            EAPConfig.Instance.Driver.EAP = this;
            EAPConfig.Instance.Driver.Init();

            this.Subscribe(5, 1, "ToolAlarm", EAPConfig.Instance.Driver.HandleToolAlarm);

            reloadSpecialControlFileToolStripMenuItem_Click(this, EventArgs.Empty);
            menuItemGemEnable_Click(this, EventArgs.Empty);
        }

        protected override void OnClosed(EventArgs e)
        {
            EAPConfig.Instance.Driver.Unload();

            menuItemGemDisable_Click(this, EventArgs.Empty);
            base.OnClosed(e);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        #region UI Action
        void menuItemGemEnable_Click(object sender, EventArgs e)
        {
            EapLogger.Info("SECS/GEM Start");
            gemStatusLabel.Text = "Start";
            _secsGem?.Dispose();
            _secsGem = new SecsGem(
                EAPConfig.Instance.Mode == ConnectionMode.Active,
                IPAddress.Parse(EAPConfig.Instance.IP),
                EAPConfig.Instance.TcpPort, _secsLogger, EAPConfig.Instance.SocketRecvBufferSize, PrimaryMsgHandler)
            {
                DeviceId = EAPConfig.Instance.DeviceId,
                LinkTestInterval = EAPConfig.Instance.LinkTestInterval,
                T3 = EAPConfig.Instance.T3,
                T5 = EAPConfig.Instance.T5,
                T6 = EAPConfig.Instance.T6,
                T7 = EAPConfig.Instance.T7,
                T8 = EAPConfig.Instance.T8,
            };
            _secsGem.ConnectionChanged += delegate
            {
                this.Invoke((MethodInvoker)delegate
                {
                    EapLogger.Info("SECS/GEM " + _secsGem.State);
                    gemStatusLabel.Text = _secsGem.State.ToString();
                    eqpAddressStatusLabel.Text = "Device IP: " + _secsGem.DeviceAddress;
                    if (_secsGem.State == ConnectionState.Selected)
                        _secsGem.SendAsync(new SecsMessage(1, 13, "TestCommunicationsRequest", Item.L()));
                });
            };
            menuItemGemDisable.Enabled = true;
            menuItemGemEnable.Enabled = false;
        }

        private void PrimaryMsgHandler(SecsMessage primaryMsg, Action<SecsMessage> replySecondaryAcync)
        {
            try
            {
                replySecondaryAcync(SecsMessages[primaryMsg.S, (byte)(primaryMsg.F + 1)].FirstOrDefault());
                Action<SecsMessage> handler = null;
                if (_eventHandlers.TryGetValue(primaryMsg.GetKey(), out handler))
                    Parallel.ForEach(handler.GetInvocationList().Cast<Action<SecsMessage>>(), h => h(primaryMsg));
            }
            catch (Exception ex)
            {
                EapLogger.Error("Handle Primary SECS message Error", ex);
            }
        }

        void menuItemGemDisable_Click(object sender, EventArgs e)
        {
            if (_secsGem != null)
            {
                EapLogger.Info("SECS/GEM Stop");
                _secsGem.Dispose();
                _secsGem = null;
            }
            gemStatusLabel.Text = "Disable";
            menuItemGemDisable.Enabled = false;
            menuItemGemEnable.Enabled = true;
        }

        void menuItemSecsMessagestList_Click(object sender, EventArgs e)
        {
            bool showMsgList = ((ToolStripMenuItem)sender).Checked;
            splitContainer1.Panel1Collapsed = !showMsgList;
            _screenLoger.DisplaySecsMesssage = menuItemSecsTrace.Checked = showMsgList;
        }

        void menuItemEapConfig_Click(object sender, EventArgs e)
        {
            Process.Start("Notepad", AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }

        void menuItemGemConfig_Click(object sender, EventArgs e)
        {
            Process.Start("Notepad", @"..\config\Gem.xml");
        }

        void menuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown)
                return;
            e.Cancel = MessageBox.Show("你確定要關閉EAP嗎?", "關閉應用程式", MessageBoxButtons.YesNo) == DialogResult.No;
        }

        async void btnSend_Click(object sender, EventArgs e)
        {
            if (_secsGem == null)
            {
                MessageBox.Show("SECS/GEM not enable!");
                return;
            }
            try
            {
                EapLogger.Notice("Send by operator");
                await _secsGem.SendAsync(txtMsg.Text.ToSecsMessage());
            }
            catch (Exception ex)
            {
                EapLogger.Error(ex);
            }
        }

        void listBoxSecsMessageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SecsMessage secsMsg = SecsMessages[listBoxSecsMessages.SelectedIndex];
            using (var sw = new StringWriter())
            {
                secsMsg.WriteTo(sw);
                txtMsg.Text = sw.ToString();
            }
        }

        void enableTraceLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._secsGem.LinkTestEnable = ((ToolStripMenuItem)sender).Checked;
        }

        void menuItemReloadGemXml_Click(object sender, EventArgs e)
        {
            try
            {
                this.EventReportLink = new DefineLinkConfig(EAPConfig.Instance.GemXml);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Reload gem.xml fail!!");
            }
        }

        void menuItemSecsTrace_Click(object sender, EventArgs e)
        {
            this._screenLoger.DisplaySecsMesssage = ((ToolStripMenuItem)sender).Checked;
        }

        async void defineLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await EAPConfig.Instance.Driver.DefineLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Define report link fail");
            }
        }

        void reloadSpecialControlFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var c = (IConfigurable)EAPConfig.Instance.Driver;
            c?.LoadConfig();
        }

        void menuItemClearScreen_Click(object sender, EventArgs e)
        {
            this.rtxtScreen.Clear();
        }

        void enableLoggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        void publishZServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.PublishZService();
        }
        #endregion

        #region ISecsDevice Members
        public string ToolId => EAPConfig.Instance.ToolId;

        static readonly HashSet<int> InvalidRemoteMessage = new HashSet<int> {
            1<<8 | 15, //s1F15 request offline
            1<<8 | 17, //s1f17 request online
            2<<8 | 33, //s2f33 define link
            2<<8 | 35, //s2f35 define link
            2<<8 | 37  //s2f37 define link
        };

        public Task<SecsMessage> SendAsync(SecsMessage msg)
        {
            if (_secsGem == null)
                throw new Exception("SECS/GEM not enable!");
            if (RemotingServices.IsTransparentProxy(msg) && InvalidRemoteMessage.Contains(msg.GetKey()))
                throw new ArgumentException("This message is not remotable!");
            return _secsGem.SendAsync(msg);
        }

        readonly ConcurrentDictionary<int, Action<SecsMessage>> _eventHandlers = new ConcurrentDictionary<int, Action<SecsMessage>>();
        readonly ConcurrentDictionary<string, Action<SecsMessage>> _recoverEventHandlers = new ConcurrentDictionary<string, Action<SecsMessage>>(StringComparer.Ordinal);

        IDisposable SubscribeLocal(SecsEventSubscription subscription)
        {
            var handler = new Action<SecsMessage>(msg =>
            {
                #region event action
                var filter = subscription.Filter;
                try
                {
                    if (filter.Eval(msg.SecsItem))
                    {
                        EapLogger.Info("event[" + filter.Description + "] >> EAP");
                        msg.Name = filter.Name;
                        subscription.Handle(msg);
                    }
                }
                catch (Exception ex)
                {
                    EapLogger.Error("event[" + filter.Description + "] EAP process Error!", ex);
                }
                #endregion
            });
            _eventHandlers.AddHandler(subscription.GetKey(), handler);
            EapLogger.Notice("EAP subscribe event " + subscription.Filter.Description);
            return new LocalDisposable(() =>
            {
                _eventHandlers.RemoveHandler(subscription.GetKey(), handler);
                EapLogger.Notice("EAP unsubscribe event " + subscription.Filter.Description);
            });
        }

        IDisposable SubscribeRemote(SecsEventSubscription subscription)
        {
            var filter = subscription.Filter;
            var description = filter.Description;
            var handler = new Action<SecsMessage>(msg =>
            {
                #region event action
                try
                {
                    if (filter.Eval(msg.SecsItem))
                    {
                        EapLogger.Info("event[" + description + "] >> Z");
                        msg.Name = filter.Name;
                        subscription.Handle(msg);
                    }
                }
                catch (Exception ex)
                {
                    EapLogger.Error("event[" + description + "] Z process Error!", ex);
                }
                #endregion
            });
            int key = subscription.GetKey();
            string recoverQueuePath = $"FormatName:DIRECT=TCP:{subscription.ClientAddress} \\private$\\{subscription.Id}";
            if (subscription.Recoverable)
            {
                return new SerializableDisposable(subscription,
                    new RemoteDisposable(subscription,
                        () =>
                        {
                            #region recover complete
                            _eventHandlers.AddHandler(key, handler);
                            EapLogger.Notice("Z subscribe event " + description);

                            Action<SecsMessage> recover = null;
                            if (_recoverEventHandlers.TryRemove(recoverQueuePath, out recover))
                            {
                                _eventHandlers.RemoveHandler(key, recover);
                                EapLogger.Notice("Z recover completely event[" + description + "]");
                            }
                            #endregion
                        },
                        () =>
                        {
                            #region Dispose
                            _eventHandlers.RemoveHandler(key, handler);
                            EapLogger.Notice("Z unsubscribe event " + description);

                            var queue = new MessageQueue(recoverQueuePath, QueueAccessMode.Send)
                            {
                                Formatter = new BinaryMessageFormatter()
                            };
                            queue.DefaultPropertiesToSend.Recoverable = true;

                            var recover = new Action<SecsMessage>(msg =>
                            {
                                #region recover action
                                try
                                {
                                    if (filter.Eval(msg.SecsItem))
                                    {
                                        EapLogger.Info("recoverable event[" + description + "]");
                                        msg.Name = filter.Name;
                                        queue.Send(msg);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    EapLogger.Error(ex);
                                }
                                #endregion
                            });
                            if (_recoverEventHandlers.TryAdd(recoverQueuePath, recover))
                            {
                                EapLogger.Notice("Z subscribe event[" + description + "] for recovering.");
                                _eventHandlers.AddHandler(key, recover);
                            }
                            #endregion
                        }));
            }
            _eventHandlers.AddHandler(key, handler);
            EapLogger.Notice("Z subscribe event " + description);
            return new SerializableDisposable(subscription,
                new RemoteDisposable(subscription, null, () =>
                {
                    _eventHandlers.RemoveHandler(key, handler);
                    EapLogger.Notice("Z unsubscribe event " + description);
                }));
        }

        public IDisposable Subscribe(SecsEventSubscription subscription)
        {
            if (RemotingServices.IsTransparentProxy(subscription))
                return SubscribeRemote(subscription);
            return SubscribeLocal(subscription);
        }

        sealed class LocalDisposable : IDisposable
        {
            readonly Action _disposeAction;
            bool _isDisposed;
            internal LocalDisposable(Action disposeAction)
            {
                _disposeAction = disposeAction;
            }
            void IDisposable.Dispose()
            {
                if (!this._isDisposed)
                {
                    this._disposeAction();
                    this._isDisposed = true;
                }
            }
        }

        sealed class RemoteDisposable : MarshalByRefObject, IServerDisposable
        {
            static readonly ISponsor sponsor = new Sponsor();
            readonly SecsEventSubscription _subscription;
            readonly Action _subscribeAction;
            readonly Action _disposeAction;
            internal RemoteDisposable(SecsEventSubscription subscription, Action subscribeAction, Action disposeAction)
            {
                this._subscribeAction = subscribeAction;
                this._disposeAction = disposeAction;
                this._subscription = subscription;
                var lease = (ILease)subscription.GetLifetimeService();
                if (lease != null)
                    lease.Register(sponsor);
            }
#if DEBUG
            public override object InitializeLifetimeService() {
                ILease lease = (ILease)base.InitializeLifetimeService();
                if (lease.CurrentState == LeaseState.Initial) {
                    lease.InitialLeaseTime = TimeSpan.FromMinutes(20);
                    lease.RenewOnCallTime = TimeSpan.FromMinutes(20);
                    lease.SponsorshipTimeout = TimeSpan.FromMinutes(1);
                }
                return lease;
            }
#endif
            ~RemoteDisposable()
            {
                _subscription.Dispose();
                Trace.WriteLine("ServerDisposable destory!");
            }

            #region IDisposable Memebers

            volatile bool _isDisposed;

            [OneWay]
            public void Dispose()
            {
                if (!this._isDisposed)
                {
                    try
                    {
                        this._disposeAction();
                        this._isDisposed = true;
                        var lease = (ILease)_subscription.GetLifetimeService();
                        if (lease != null)
                        {
                            Trace.WriteLine("Unregister Subscrption.");
                            lease.Unregister(sponsor);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            #endregion
            #region IServerDisposable Members
            [OneWay]
            public void RecoverComplete()
            {
                if (_subscribeAction != null)
                    _subscribeAction();
            }
            #endregion
        }
        #endregion

        #region IEAP Members
        readonly IDictionary<string, Func<XElement, Task>> _txHandlers = new Dictionary<string, Func<XElement, Task>>(StringComparer.Ordinal);

        static readonly string AreYouThereAck = new XDocument(
            new XElement("Transaction",
                new XAttribute("TxName", "AreYouThere"),
                new XAttribute("Type", "Ack"),
                new XAttribute("MessageKey", "0000"),
                new XElement("Tool", new XAttribute("ToolID", "EAP." + EAPConfig.Instance.ToolId)))).ToString();

        //void axPostMan_onReceive(object sender, AxPOSTMANCTRLLib._DPostManCtrlEvents_onReceiveEvent e) {
        //    try {
        //        XDocument doc = XDocument.Parse(e.msgContent);
        //        XElement txElm = doc.Root;
        //        if (this.ToolId != txElm.Element("Tool").Attribute("ToolID").Value)
        //            return;

        //        string txName = txElm.Attribute("TxName").Value;
        //        string txType = txElm.Attribute("Type").Value;
        //        if (txName == "AreYouThere") {
        //            AreYouThereQueue.Send(new System.Messaging.Message(AreYouThereAck, new ActiveXMessageFormatter()));
        //            return;
        //        }
        //        string txkey = txName + txType;

        //        Action<XElement> txHandler = null;
        //        if (!_txHandlers.TryGetValue(txkey, out txHandler)) {
        //            EapLogger.Warn("Unsupport TxName='{0}' Type='{1}'", txName, txType);
        //            return;
        //        }
        //        EapLogger.Info("EAP << TCS :\r\n{0}", doc.ToFormatedXml());
        //        txHandler.BeginInvoke(txElm, null, null);
        //    } catch (Exception ex) {
        //        EapLogger.Error("HandleTxMessage Exception: " + ex.Message, ex);
        //    }
        //}

        void Report(XElement txElm)
        {
            txElm.SetAttributeValue("MessageKey", NewMessageKey());
            txElm.Element("Tool").SetAttributeValue("ToolID", this.ToolId);
            Send2TCS(new XDocument(txElm), true);
        }

        public void Report<T>(T report) where T : struct, ITxReport
        {
            Report(report.XML);
        }

        void IEAP.Report(DataCollectionCompleteReport report)
        {
            this.Report<DataCollectionCompleteReport>(report);
            //DummyProcessJob or OCS job
            if (report.ProcessJob == ProcessJob.DummyProcessJob || report.ProcessJob.Id.Length > 10)
                return;
        }

        void IEAP.Report(DataCollectionReport report)
        {
            Report(report.XML);
        }

        static int _MessageKey = 0;
        static string NewMessageKey()
        {
            Interlocked.Increment(ref _MessageKey);
            Interlocked.CompareExchange(ref _MessageKey, 0, 9999);
            return _MessageKey.ToString("0000");
        }

        void Send2TCS(XDocument xmldoc, bool normal)
        {
            try
            {
                string msg = xmldoc.ToFormatedXml();
                //axPostMan.Send(TcsPostmanId, "", msg);
                if (normal)
                    EapLogger.Info("EAP >> TCS : \r\n" + msg);
                else
                    EapLogger.Error("EAP >> TCS : \r\n" + msg);
            }
            catch (Exception ex)
            {
                EapLogger.Error("Send2TCS error:", ex);
            }
        }

        void IEAP.SetTxHandler<T>(Func<T, Task> handler)
        {
            _txHandlers[typeof(T).Name] = async txElm =>
            {
                T tx = default(T);
                bool error = false;
                try
                {
                    tx.Parse(txElm);
                    await handler(tx);
                    txElm.SetAttributeValue("SystemErrCode", 0);
                    txElm.SetAttributeValue("AppErrCode", 0);
                    txElm.SetAttributeValue("AppErrDescription", string.Empty);
                }
                catch (XmlException ex)
                {
                    error = true;
                    txElm.SetAttributeValue("SystemErrCode", 9);
                    txElm.SetAttributeValue("AppErrCode", 9);
                    txElm.SetAttributeValue("AppErrDescription", "XML parsing error:" + ex.Message);
                }
                catch (ScenarioException ex)
                {
                    error = true;
                    txElm.SetAttributeValue("SystemErrCode", 9);
                    txElm.SetAttributeValue("AppErrCode", 0);
                    txElm.SetAttributeValue("AppErrDescription", "Scenario exception:" + ex.Message);
                }
                catch (SecsException ex)
                {
                    error = true;
                    txElm.SetAttributeValue("SystemErrCode", 9);
                    txElm.SetAttributeValue("AppErrCode", 1);
                    txElm.SetAttributeValue("AppErrDescription", "Secs/Gem eror:" + ex.Message);
                }
                catch (Exception ex)
                {
                    error = true;
                    EapLogger.Error(ex);
                    txElm.SetAttributeValue("SystemErrCode", 9);
                    txElm.SetAttributeValue("AppErrCode", 1);
                    txElm.SetAttributeValue("AppErrDescription", string.Format("Exception occurred in {0}: {1}", handler.GetType().Name, ex.Message));
                }
                finally
                {
                    var txtype = txElm.Attribute("Type");
                    if (txtype.Value == "Request")
                    {
                        txtype.Value = "Ack";
                        Send2TCS(txElm.Document, !error);
                    }
                }
            };
        }

        public SecsMessageList SecsMessages { get; private set; }
        public DefineLinkConfig EventReportLink { get; private set; }
        #endregion

    }
}
