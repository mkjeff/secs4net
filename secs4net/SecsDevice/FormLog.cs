using System;
using System.Drawing;
using System.Windows.Forms;
using Secs4Net;

namespace SecsDevice {
    public partial class FormLog : Form,ISecsTracer {
        public FormLog() {
            InitializeComponent();
        }

        public void TraceMessageIn(SecsMessage msg, int systembyte) {
            this.Invoke((MethodInvoker)delegate {
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.AppendText(string.Format("<-- [0x{0:X8}] {1}\n", systembyte , msg.ToSML()));
            });
        }

        public void TraceMessageOut(SecsMessage msg, int systembyte) {
            this.Invoke((MethodInvoker)delegate {
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.AppendText(string.Format("--> [0x{0:X8}] {1}\n", systembyte, msg.ToSML()));
            });
        }

        public void TraceInfo(string msg) {
            this.Invoke((MethodInvoker)delegate {
                richTextBox1.SelectionColor = Color.Blue;
                richTextBox1.AppendText(msg + Environment.NewLine);
            });
        }

        public void TraceWarning(string msg) {
            this.Invoke((MethodInvoker)delegate {
                richTextBox1.SelectionColor = Color.Green;
                richTextBox1.AppendText(msg + Environment.NewLine);
            });
        }

        public void TraceError(string msg) {
            this.Invoke((MethodInvoker)delegate {
                richTextBox1.SelectionColor = Color.Red;
                richTextBox1.AppendText(msg + Environment.NewLine);
            });
        }
    }
}
