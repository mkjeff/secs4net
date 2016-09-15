using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using LINQPad.Extensibility.DataContext;
using System.Data;
using System.Xml.Linq;
using System.Xml;
using System.Net;
using System.Threading;

namespace Secs4Net.LinqPad
{
	/// <summary>
	/// Interaction logic for ConnectionDialog.xaml
	/// </summary>
	public partial class ConnectionDialog : Window
	{
        public IConnectionInfo _cxInfo;

		public ConnectionDialog(IConnectionInfo cxInfo)
		{
			_cxInfo = cxInfo;
            _cxInfo.DisplayName = "SECS device";
            DataContext = cxInfo;
			InitializeComponent();
		}

        XElement GetConnectionConfig()
        {
            var xml = this.Resources["deviceConfig"] as XmlDataProvider;
            return XDocument.Parse(xml.Document.OuterXml).Root;
        }

		void btnOK_Click (object sender, RoutedEventArgs e)
		{
            _cxInfo.DriverData = GetConnectionConfig();
            _cxInfo.DriverData.Attribute("file").Value = messageFile.Text;
            DialogResult = true;
		}

		void BrowseAssembly (object sender, RoutedEventArgs e)
		{
			var dialog = new Microsoft.Win32.OpenFileDialog ()
			{
				Title = "Choose message json file",
                Filter= "JSON files (*.json)|*.json|All files (*.*)|*.*",
				DefaultExt = ".json",
			};

			if (dialog.ShowDialog () == true)
				messageFile.Text = dialog.FileName;
		}

		void BrowseAppConfig (object sender, RoutedEventArgs e)
		{
			var dialog = new Microsoft.Win32.OpenFileDialog ()
			{
				Title = "Choose application config file",
				DefaultExt = ".config",
			};

			if (dialog.ShowDialog () == true)
				_cxInfo.AppConfigPath = dialog.FileName;
		}

		private void btnTest_Click(object sender, RoutedEventArgs e)
		{
			try
			{
                var ev = new ManualResetEvent(false);
                var config = GetConnectionConfig();
                var ip = IPAddress.Parse(config.Attribute("ip").Value);
                using (var secsGem = new SecsGem(
                    (bool)config.Attribute("isActive"),
                    IPAddress.Parse(config.Attribute("ip").Value),
                    (int)config.Attribute("port")))
                {
                    secsGem.ConnectionChanged += delegate {
                        if (secsGem.State == ConnectionState.Selected)
                            ev.Set();
                    };
                    secsGem.Start();

                    if (!ev.WaitOne(10000))
                        throw new Exception("timeout");
                }
                MessageBox.Show("Successed");

            }
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Connection Fail");
			}
		}
	}

	[ValueConversion(typeof(bool), typeof(bool))]
	public class InverseBooleanConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture)
		{
			if (targetType != typeof(bool))
				throw new InvalidOperationException("The target must be a boolean");

			return !(bool)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		#endregion
	}
}
