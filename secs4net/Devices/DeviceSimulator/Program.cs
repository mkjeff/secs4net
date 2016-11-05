using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Secs4Net;

namespace SecsDevice {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Console.WriteLine(Item.B().Format);
            Console.WriteLine(Item.Boolean().Format);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
