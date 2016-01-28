using System;
using System.ComponentModel;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Services;
using System.Threading;
using System.Windows.Forms;
using LoadPortMonitor.Properties;
using Cim.Services;
using Cim.Management;
using Secs4Net;
using System.Collections.Generic;
using System.Diagnostics;
using System.Configuration;

namespace LoadPortMonitor {
    public partial class Form1 : Form {
        readonly ICentralService<ISecsDevice> _serviceManager;

        readonly BindingList<Loadport> ports = new BindingList<Loadport>{
             new Loadport{ PortId = "L01"},
             new Loadport{ PortId = "L02"},
             new Loadport{ PortId = "L03"}
        };

        public Form1() {
            InitializeComponent();
            loadportBindingSource.DataSource = ports;
            this.Text = Settings.Default.ToolId + " service not available(Passive)";
            this.FormClosed += btnDisable_Click;

            _serviceManager = (ICentralService<ISecsDevice>)Activator.GetObject(typeof(ICentralService<ISecsDevice>),
                ConfigurationManager.AppSettings["zmanagerUrl"]);
        }

        readonly SecsMessage s1f3 = new SecsMessage(1, 3, "QueryLoadPortStatus",
                            Item.L(
                                Item.U2(18026),
                                Item.U2(18027),
                                Item.U2(18028)));

        void GetLoadPortStatus(ISecsDevice device) {
            try {
                var s1f4 = device.Send(s1f3);

                #region Update UI
                for (int i = 0; i < 3; i++) {
                    string state = null;
                    switch ((byte)s1f4.SecsItem.Items[i]) {
                        case 0:
                            state = "OutOfService";
                            break;
                        case 1:
                            state = "TransferBlock";
                            break;
                        case 2:
                            state = "ReadyToLoad";
                            break;
                        case 3:
                            state = "ReadyToUnload";
                            break;
                        case 4:
                            state = "LoadComplete";
                            break;
                        default:
                            state = "UnloadComplete";
                            break;
                    }
                    ports[i].State = state;
                }
                #endregion
            } catch {

            }
        }

        void btnGCCollect_Click(object sender, EventArgs e) {
            GC.Collect();
        }

        void Form1_Load(object sender, EventArgs e) {
            ThreadPool.QueueUserWorkItem(SubscribeService);
        }

        IDisposable[] _events;
        void SubscribeService(object _) {
            while (true) {
                try {
                    var device = _serviceManager.GetService(Settings.Default.ToolId);
                    this.Invoke((MethodInvoker)delegate {
                        GetLoadPortStatus(device);
                        this.Text = Settings.Default.ToolId + " Connected(Passive)";
                    });
                    var readytoload = device.SubscribeS6F11("114", "ReadyToLoad",
                        msg => {
                            byte portId = msg.SecsItem.Items[2].Items[0].Items[1].Items[0].GetValue<byte>();
                            ports[portId - 1].State = msg.Name;
                        });

                    var readytounload = device.SubscribeS6F11("115", "ReadyToUnload",
                        msg => {
                            byte portId = msg.SecsItem.Items[2].Items[0].Items[1].Items[0].GetValue<byte>();
                            ports[portId - 1].State = msg.Name;
                        });

                    var loadcomplete = device.SubscribeS6F11("101", "LoadComplete",
                        msg => {
                            byte portId = msg.SecsItem.Items[2].Items[0].Items[1].Items[0].GetValue<byte>();
                            ports[portId - 1].State = msg.Name;
                        });

                    var unloadcomplete = device.Subscribe(
                        new S6F11Filter { CEID = "102", Name = "UnLoadComplete" }, true,
                        msg => {
                            byte portId = msg.SecsItem.Items[2].Items[0].Items[1].Items[0].GetValue<byte>();
                            ports[portId - 1].State = msg.Name;
                        });
                    _events = new IDisposable[]{
                        readytoload,
                        readytounload,
                        loadcomplete,
                        unloadcomplete
                    };
                    break;
                } catch (Exception) {
                    //Console.WriteLine(ex.Message);
                }
                Thread.Sleep(5000);
            }
        }

        private void btnDisable_Click(object sender, EventArgs e) {
            this.btnDisable.Enabled = false;
            this.btnEnable.Enabled = true;
            if (_events != null) {
                foreach (var disposable in _events)
                    disposable.Dispose();
                _events = null;
            }
        }

        private void btnEnable_Click(object sender, EventArgs e) {
            this.btnEnable.Enabled = false;
            this.btnDisable.Enabled = true;
            ThreadPool.QueueUserWorkItem(SubscribeService);
        }
    }

    class Loadport : INotifyPropertyChanged {
        public string PortId { get; set; }
        public string State {
            get {
                return _State;
            }
            set {
                if (value != _State) {
                    _State = value;
                    OnPropertyChanged("State");
                }
            }
        }
        string _State;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}
