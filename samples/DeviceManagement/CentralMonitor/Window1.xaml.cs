using System;
using System.Linq;
using System.Runtime.Remoting;
using System.Windows;
using System.Windows.Data;
using System.Runtime.Remoting.Services;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using System.Windows.Controls;

namespace CentralMonitor {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window {
        readonly ZCentralService service;
        const string TOOLS_XML_FILE = "tools.xml";

        public Window1() {
            InitializeComponent();
            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
            RemotingConfiguration.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, false);

            var tools = Enumerable.Empty<Tool>();
            if (File.Exists(TOOLS_XML_FILE)) {
                tools = from elm in XDocument.Load(TOOLS_XML_FILE).Root.Elements("tool")
                        select new Tool(elm.Attribute("id").Value) {
                            Url = elm.Attribute("url").Value,
                            RegisiterTime = ((DateTime?)elm.Attribute("regtime")) ?? DateTime.Now
                        };
            }
            service = new ZCentralService(this.Dispatcher, tools);
            RemotingServices.Marshal(service, "manager");
            this.DataContext = service.Tools;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            e.Cancel = MessageBox.Show("這是一個重要的服務,請不要隨意關閉. 是否關閉應用程式?", "警告", MessageBoxButton.YesNo, MessageBoxImage.Asterisk, MessageBoxResult.No) == MessageBoxResult.No;
            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e) {
            new XDocument(
                new XElement("tools",
                    from tool in service.Tools
                    select new XElement("tool",
                        new XAttribute("id", tool.Id),
                        new XAttribute("url", tool.Url),
                        new XAttribute("regtime", tool.RegisiterTime))))
            .Save(TOOLS_XML_FILE);
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            GC.Collect();
        }

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            try {
                ((ListBox)sender).SelectedIndex = -1;
            } catch{
            }
        }
    }
}
