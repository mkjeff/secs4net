using System;
using System.Text;
using System.Windows.Forms;

namespace SecsDevice;

static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Secs4Net.Item.JIS8Encoding = Encoding.GetEncoding(50222);
#if NET
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
#endif
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());
    }
}
