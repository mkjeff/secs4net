using System;
using System.ComponentModel;
using System.Windows.Forms;
using Secs4Net;
using System.Net;
using System.Drawing;

namespace SecsDevice {
    public partial class Form1 : Form {
        SecsGem _secsGem;
        readonly SecsTracer Logger;
        readonly BindingList<RecvMessage> recvBuffer = new BindingList<RecvMessage>();
        public Form1() {
            InitializeComponent();

            radioActiveMode.DataBindings.Add("Enabled", btnEnable, "Enabled");
            radioPassiveMode.DataBindings.Add("Enabled", btnEnable, "Enabled");
            txtAddress.DataBindings.Add("Enabled", btnEnable, "Enabled");
            numPort.DataBindings.Add("Enabled", btnEnable, "Enabled");
            numDeviceId.DataBindings.Add("Enabled", btnEnable, "Enabled");
            recvMessageBindingSource.DataSource = recvBuffer;
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            Logger = new SecsLogger(this);
        }

        void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
            MessageBox.Show(e.Exception.ToString());
        }

        private async void btnEnable_Click(object sender, EventArgs e)
        {
            _secsGem?.Dispose();
            _secsGem = new SecsGem(
                ip: IPAddress.Parse(txtAddress.Text),
                port: (int)numPort.Value,
                isActive: radioActiveMode.Checked,
                tracer: Logger,
                primaryMsgHandler: (primaryMsg, replyAction) =>
                    this.Invoke(new MethodInvoker(() =>
                        recvBuffer.Add(new RecvMessage
                        {
                            Msg = primaryMsg,
                            ReplyAction = replyAction
                        }))));

            _secsGem.ConnectionChanged += delegate
            {
                this.Invoke((MethodInvoker)delegate
                {
                    lbStatus.Text = _secsGem.State.ToString();
                });
            };

            btnEnable.Enabled = false;
            await _secsGem.Start();
            btnDisable.Enabled = true;
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            _secsGem?.Dispose();
            _secsGem = null;
            btnEnable.Enabled = true;
            btnDisable.Enabled = false;
            lbStatus.Text = "Disable";
            recvBuffer.Clear();
        }

        private async void btnSendPrimary_Click(object sender, EventArgs e)
        {
            if (_secsGem.State != ConnectionState.Selected)
                return;
            if (string.IsNullOrWhiteSpace(txtSendPrimary.Text))
                return;

            try
            {
                var reply = await _secsGem.SendAsync(txtSendPrimary.Text.ToSecsMessage());
                txtRecvSecondary.Text = reply.ToSML();
            }
            catch (SecsException ex)
            {
                txtRecvSecondary.Text = ex.Message;
            }
        }

        private void lstUnreplyMsg_SelectedIndexChanged(object sender, EventArgs e) {
            var recv = lstUnreplyMsg.SelectedItem as RecvMessage;
            txtRecvPrimary.Text = recv?.Msg.ToSML();
        }

        private void btnReplySecondary_Click(object sender, EventArgs e) {
            var recv = lstUnreplyMsg.SelectedItem as RecvMessage;
            if (recv == null)
                return;

            if (string.IsNullOrWhiteSpace(txtReplySeconary.Text))
                return;

            recv.ReplyAction(txtReplySeconary.Text.ToSecsMessage());
            recvBuffer.Remove(recv);
            txtRecvPrimary.Clear();
        }

        class SecsLogger : SecsTracer
        {
            readonly Form1 _form;
            internal SecsLogger(Form1 form)
            {
                _form = form;
            }
            public override void TraceMessageIn(SecsMessage msg, int systembyte)
            {
                _form.Invoke((MethodInvoker)delegate {
                    _form.richTextBox1.SelectionColor = Color.Black;
                    _form.richTextBox1.AppendText($"<-- [0x{systembyte:X8}] {msg.ToSML()}\n");
                });
            }

            public override void TraceMessageOut(SecsMessage msg, int systembyte)
            {
                _form.Invoke((MethodInvoker)delegate {
                    _form.richTextBox1.SelectionColor = Color.Black;
                    _form.richTextBox1.AppendText($"--> [0x{systembyte:X8}] {msg.ToSML()}\n");
                });
            }

            public override void TraceInfo(string msg)
            {
                _form.Invoke((MethodInvoker)delegate {
                    _form.richTextBox1.SelectionColor = Color.Blue;
                    _form.richTextBox1.AppendText($"{msg}\n");
                });
            }

            public override void TraceWarning(string msg)
            {
                _form.Invoke((MethodInvoker)delegate {
                    _form.richTextBox1.SelectionColor = Color.Green;
                    _form.richTextBox1.AppendText($"{msg}\n");
                });
            }

            public override void TraceError(string msg, Exception ex = null)
            {
                _form.Invoke((MethodInvoker)delegate {
                    _form.richTextBox1.SelectionColor = Color.Red;
                    _form.richTextBox1.AppendText($"{msg}\n");
                    _form.richTextBox1.SelectionColor = Color.Gray;
                    _form.richTextBox1.AppendText($"{ex?.ToString()}\n");
                });
            }
        }
    }

    public sealed class RecvMessage {
        public SecsMessage Msg { get; set; }
        public Action<SecsMessage> ReplyAction { get; set; }
    }
}
