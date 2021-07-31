using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Windows.Forms;
using Cim.Management;
using Cim.Services;
using LoadPortMonitor.Properties;
using Secs4Net;

namespace LoadPortMonitor
{
    public partial class Form1 : Form
    {
        static readonly ICentralService<ISecsDevice> ServiceManager =
            (ICentralService<ISecsDevice>)RemotingServices.Connect(
                typeof(ICentralService<ISecsDevice>), Settings.Default.CentralServiceUrl);
        readonly BindingList<Loadport> ports = new BindingList<Loadport>{
            new Loadport{ PortId = "L01"},
            new Loadport{ PortId = "L02"},
            new Loadport{ PortId = "L03"}
        };

        public Form1()
        {
            InitializeComponent();

            loadportBindingSource.DataSource = ports;
        }

        readonly SecsMessage s1f3 = new SecsMessage(1, 3, "QueryLoadPortStatus",
                            Item.L(
                                Item.U2(18026),
                                Item.U2(18027),
                                Item.U2(18028)));

        async void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                var serviceManager = (ICentralService<ISecsDevice>)RemotingServices.Connect(typeof(ICentralService<ISecsDevice>), Settings.Default.CentralServiceUrl);
                var device = ServiceManager.GetService(Settings.Default.ToolId);

                var s1f4 = await device.SendAsync(s1f3);
                for (int i = 0; i < ports.Count; i++)
                {
                    string state = null;
                    switch ((byte)s1f4.SecsItem.Items[i])
                    {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Settings.Default.ToolId + " service not available(Active)");
            }
        }

        void btnGC_Click(object sender, EventArgs e)
        {
            GC.Collect();
        }
    }

    class Loadport : INotifyPropertyChanged
    {
        public string PortId { get; set; }
        public string State
        {
            get
            {
                return _State;
            }
            set
            {
                if (value != _State)
                {
                    _State = value;
                    OnPropertyChanged();
                }
            }
        }
        string _State;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
