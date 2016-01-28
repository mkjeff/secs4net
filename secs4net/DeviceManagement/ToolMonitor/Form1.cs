using System;
using System.ComponentModel;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Services;
using System.Windows.Forms;
using LoadPortMonitor.Properties;
using Cim.Services;
using Cim.Management;
using Secs4Net;

namespace LoadPortMonitor {
    public partial class Form1 : Form {
        static readonly ICentralService<ISecsDevice> ServiceManager = (ICentralService<ISecsDevice>)RemotingServices.Connect(typeof(ICentralService<ISecsDevice>), Settings.Default.CentralServiceUrl);
        readonly BindingList<Loadport> ports = new BindingList<Loadport>{
            new Loadport{ PortId = "L01"},
            new Loadport{ PortId = "L02"},
            new Loadport{ PortId = "L03"}
        };

        public Form1() {
            InitializeComponent();

            loadportBindingSource.DataSource = ports;
        }

        readonly SecsMessage s1f3 = new SecsMessage(1, 3, "QueryLoadPortStatus",
                            Item.L(
                                Item.U2(18026),
                                Item.U2(18027),
                                Item.U2(18028)));

        void btnRefresh_Click(object sender, EventArgs e) {
            try {
                var serviceManager = (ICentralService<ISecsDevice>)RemotingServices.Connect(typeof(ICentralService<ISecsDevice>), Settings.Default.CentralServiceUrl);
                var device = ServiceManager.GetService(Settings.Default.ToolId);

                var s1f4 = device.Send(s1f3);
                #region Update UI
                for (int i = 0; i < ports.Count; i++) {
                    string state = null;
                    switch ((byte)s1f4.SecsItem.Items[i]) {
                        case 1:
                            state = "ReadyToLoad";
                            break;
                        case 3:
                            state = "ReadyToUnload";
                            break;
                        default:
                            state = "Unkonwn";
                            break;
                    }
                    ports[i].State = state;
                }
                #endregion
            } catch(Exception ex) {
                MessageBox.Show(ex.ToString(),Settings.Default.ToolId + " service not available(Active)");
            }
        }

        void btnGC_Click(object sender, EventArgs e) {
            GC.Collect();
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
