using System;
using System.ComponentModel;
using System.Windows.Forms;
using Secs4Net;
using System.Net;

namespace SecsDevice {
    public partial class Form1 : Form {
        SecsGem _secsGem;
        readonly FormLog _logform = new FormLog();
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
        }

        void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
            MessageBox.Show(e.Exception.ToString());
        }

        private void btnEnable_Click(object sender, EventArgs e) {
            if (_secsGem != null)
                _secsGem.Dispose();


            _secsGem = new SecsGem(IPAddress.Parse(txtAddress.Text), (int)numPort.Value,
                radioActiveMode.Checked,
                (primaryMsg, reply) => {
                    this.Invoke((MethodInvoker)delegate {
                        recvBuffer.Add(new RecvMessage {
                            Msg = primaryMsg,
                            ReplyAction = reply
                        });
                    });
                },
                _logform.Logger);

            _secsGem.ConnectionChanged += delegate {
                this.Invoke((MethodInvoker)delegate {
                    lbStatus.Text = _secsGem.State.ToString();
                });
            };

            btnEnable.Enabled = false;
            btnDisable.Enabled = true;
        }

        private void btnDisable_Click(object sender, EventArgs e) {
            if (_secsGem != null) {
                _secsGem.Dispose();
                _secsGem = null;
            }
            btnEnable.Enabled = true;
            btnDisable.Enabled = false;
            lbStatus.Text = "Disable";
            recvBuffer.Clear();
        }

        private void Form1_Load(object sender, EventArgs e) {
            _logform.Show(this);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            _logform.Close();
        }

        private void btnSendPrimary_Click(object sender, EventArgs e) {
            if (_secsGem.State != ConnectionState.Selected)
                return;

            var msg = txtSendPrimary.Text.ToSecsMessage();
            _secsGem.BeginSend(msg, ar => {
                try {
                    var reply = _secsGem.EndSend(ar);
                    this.Invoke((MethodInvoker)delegate {
                        txtRecvSecondary.Text = reply.ToSML();
                    });
                } catch (SecsException ex) {
                    this.Invoke((MethodInvoker)delegate {
                        txtRecvSecondary.Text = ex.Message;
                    });
                }
            }, null);
        }

        private void lstUnreplyMsg_SelectedIndexChanged(object sender, EventArgs e) {
            RecvMessage recv = lstUnreplyMsg.SelectedItem as RecvMessage;
            if (recv == null)
                return;
            txtRecvPrimary.Text = recv.Msg.ToSML();
        }

        private void btnReplySecondary_Click(object sender, EventArgs e) {
            RecvMessage recv = lstUnreplyMsg.SelectedItem as RecvMessage;
            if (recv == null)
                return;

            if (string.IsNullOrEmpty(txtReplySeconary.Text))
                return;

            recv.ReplyAction(txtReplySeconary.Text.ToSecsMessage());
            recvBuffer.Remove(recv);
            txtRecvPrimary.Clear();
        }
    }

    public sealed class RecvMessage {
        public SecsMessage Msg { get; set; }
        public Action<SecsMessage> ReplyAction { get; set; }
    }
}
