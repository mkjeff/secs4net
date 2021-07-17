using Secs4Net;
using Secs4Net.Sml;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace SecsDevice
{
    public partial class Form1 : Form
    {
        SecsGem? _secsGem;
        readonly ISecsGemLogger _logger;
        readonly BindingList<PrimaryMessageWrapper> recvBuffer = new BindingList<PrimaryMessageWrapper>();
        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public Form1()
        {
            InitializeComponent();

            radioActiveMode.DataBindings.Add("Enabled", btnEnable, "Enabled");
            radioPassiveMode.DataBindings.Add("Enabled", btnEnable, "Enabled");
            txtAddress.DataBindings.Add("Enabled", btnEnable, "Enabled");
            numPort.DataBindings.Add("Enabled", btnEnable, "Enabled");
            numDeviceId.DataBindings.Add("Enabled", btnEnable, "Enabled");
            numBufferSize.DataBindings.Add("Enabled", btnEnable, "Enabled");
            recvMessageBindingSource.DataSource = recvBuffer;
            Application.ThreadException += (sender, e) => MessageBox.Show(e.Exception.ToString());
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => MessageBox.Show(e.ExceptionObject.ToString());
            _logger = new SecsLogger(this);
        }

        private async void btnEnable_Click(object sender, EventArgs e)
        {
            if (_secsGem is not null)
            {
                await _secsGem.DisposeAsync();
            }

            _secsGem = new SecsGem(
                radioActiveMode.Checked,
                IPAddress.Parse(txtAddress.Text),
                (int)numPort.Value,
                (int)numBufferSize.Value)
            {
                Logger = _logger,
                DeviceId = (ushort)numDeviceId.Value,
            };

            _secsGem.ConnectionChanged += delegate
            {
                base.Invoke((MethodInvoker)delegate
                {
                    lbStatus.Text = _secsGem.State.ToString();
                });
            };

            btnEnable.Enabled = false;
            _secsGem.Start();
            btnDisable.Enabled = true;

            try
            {
                await foreach (var primaryMessage in _secsGem.GetPrimaryMessageAsync(_cancellationTokenSource.Token))
                {
                    recvBuffer.Add(primaryMessage);
                }
            }
            catch (OperationCanceledException)
            {

            }
        }

        private async void btnDisable_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            if (_secsGem is not null)
            {
                await _secsGem.DisposeAsync();
            }
            _cancellationTokenSource = new CancellationTokenSource();

            _secsGem = null;
            btnEnable.Enabled = true;
            btnDisable.Enabled = false;
            lbStatus.Text = "Disable";
            recvBuffer.Clear();
            richTextBox1.Clear();
        }

        private async void btnSendPrimary_Click(object sender, EventArgs e)
        {
            if (_secsGem?.State != ConnectionState.Selected)
                return;
            if (string.IsNullOrWhiteSpace(txtSendPrimary.Text))
                return;

            try
            {
                var reply = await _secsGem.SendAsync(txtSendPrimary.Text.ToSecsMessage());
                txtRecvSecondary.Text = reply.ToSml();
            }
            catch (SecsException ex)
            {
                txtRecvSecondary.Text = ex.Message;
            }
        }

        private void lstUnreplyMsg_SelectedIndexChanged(object sender, EventArgs e)
        {
            var receivedMessage = lstUnreplyMsg.SelectedItem as PrimaryMessageWrapper;
            txtRecvPrimary.Text = receivedMessage?.PrimaryMessage.ToSml();
        }

        private async void btnReplySecondary_Click(object sender, EventArgs e)
        {
            if (lstUnreplyMsg.SelectedItem is not PrimaryMessageWrapper recv)
                return;

            if (string.IsNullOrWhiteSpace(txtReplySeconary.Text))
                return;

            await recv.TryReplyAsync(txtReplySeconary.Text.ToSecsMessage());
            recvBuffer.Remove(recv);
            txtRecvPrimary.Clear();
        }

        private async void btnReplyS9F7_Click(object sender, EventArgs e)
        {
            if (lstUnreplyMsg.SelectedItem is not PrimaryMessageWrapper recv)
                return;

            await recv.TryReplyAsync(null);

            recvBuffer.Remove(recv);
            txtRecvPrimary.Clear();
        }

        class SecsLogger : ISecsGemLogger
        {
            readonly Form1 _form;
            internal SecsLogger(Form1 form)
            {
                _form = form;
            }
            public void MessageIn(SecsMessage msg, int systembyte)
            {
                _form.Invoke((MethodInvoker)delegate
                {
                    _form.richTextBox1.SelectionColor = Color.Black;
                    _form.richTextBox1.AppendText($"<-- [0x{systembyte:X8}] {msg.ToSml()}\n");
                });
            }

            public void MessageOut(SecsMessage msg, int systembyte)
            {
                _form.Invoke((MethodInvoker)delegate
                {
                    _form.richTextBox1.SelectionColor = Color.Black;
                    _form.richTextBox1.AppendText($"--> [0x{systembyte:X8}] {msg.ToSml()}\n");
                });
            }

            public void Info(string msg)
            {
                _form.Invoke((MethodInvoker)delegate
                {
                    _form.richTextBox1.SelectionColor = Color.Blue;
                    _form.richTextBox1.AppendText($"{msg}\n");
                });
            }

            public void Warning(string msg)
            {
                _form.Invoke((MethodInvoker)delegate
                {
                    _form.richTextBox1.SelectionColor = Color.Green;
                    _form.richTextBox1.AppendText($"{msg}\n");
                });
            }

            public void Error(string msg, Exception? ex = null)
            {
                _form.Invoke((MethodInvoker)delegate
                {
                    _form.richTextBox1.SelectionColor = Color.Red;
                    _form.richTextBox1.AppendText($"{msg}\n");
                    _form.richTextBox1.SelectionColor = Color.Gray;
                    _form.richTextBox1.AppendText($"{ex}\n");
                });
            }

            public void Debug(string msg)
            {
                _form.Invoke((MethodInvoker)delegate
                {
                    _form.richTextBox1.SelectionColor = Color.Yellow;
                    _form.richTextBox1.AppendText($"{msg}\n");
                });
            }
        }
    }
}
