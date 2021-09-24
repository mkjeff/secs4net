﻿using Microsoft.Extensions.Options;
using Secs4Net;
using Secs4Net.Sml;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SecsDevice
{
    public partial class Form1 : Form
    {
        SecsGem? _secsGem;
        HsmsConnection? _connector;
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
            _secsGem?.Dispose();

            if (_connector is not null)
            {
                await _connector.DisposeAsync();
            }

            var options = Options.Create(new SecsGemOptions
            {
                IsActive = radioActiveMode.Checked,
                IpAddress = txtAddress.Text,
                Port = (int)numPort.Value,
                SocketReceiveBufferSize = (int)numBufferSize.Value,
                DeviceId = (ushort)numDeviceId.Value,
            });

            _connector = new HsmsConnection(options, _logger);
            _secsGem = new SecsGem(options, _connector, _logger);

            _connector.ConnectionChanged += delegate
            {
                base.Invoke((MethodInvoker)delegate
                {
                    lbStatus.Text = _connector.State.ToString();
                });
            };

            btnEnable.Enabled = false;
            _ = _connector.StartAsync(_cancellationTokenSource.Token);
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
            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
            }
            if (_connector is not null)
            {
                await _connector.DisposeAsync();
            }
            _secsGem?.Dispose();
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
            if (_secsGem is null || _connector?.State != ConnectionState.Selected)
            {
                return;
            }
            string str = txtSendPrimary.Text;
            if (string.IsNullOrWhiteSpace(str))
            {
                str = new TestClass().ToSml();
                str = $"TEST:'S6F1' W\r\n{str}";
            }
            try
            {
                var message = str.ToSecsMessage();
                var reply = await _secsGem.SendAsync(message);
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
            if (lstUnreplyMsg.SelectedItem is not PrimaryMessageWrapper recv
                || string.IsNullOrWhiteSpace(txtReplySeconary.Text))
            {
                return;
            }

            await recv.TryReplyAsync(txtReplySeconary.Text.ToSecsMessage());
            recvBuffer.Remove(recv);
            txtRecvPrimary.Clear();
        }

        private async void btnReplyS9F7_Click(object sender, EventArgs e)
        {
            if (lstUnreplyMsg.SelectedItem is not PrimaryMessageWrapper recv)
            {
                return;
            }

            await recv.TryReplyAsync();

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
            public void MessageIn(SecsMessage msg, int id)
            {
                _form.Invoke((MethodInvoker)delegate
                {
                    _form.richTextBox1.SelectionColor = Color.Black;
                    _form.richTextBox1.AppendText($"<-- [0x{id:X8}] {msg.ToSml()}\n");
                });
            }

            public void MessageOut(SecsMessage msg, int id)
            {
                _form.Invoke((MethodInvoker)delegate
                {
                    _form.richTextBox1.SelectionColor = Color.Black;
                    _form.richTextBox1.AppendText($"--> [0x{id:X8}] {msg.ToSml()}\n");
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

            public void Error(string msg, SecsMessage? message, Exception? ex)
            {
                _form.Invoke((MethodInvoker)delegate
                {
                    _form.richTextBox1.SelectionColor = Color.Red;
                    _form.richTextBox1.AppendText($"{msg}\n");
                    _form.richTextBox1.AppendText($"{message?.ToSml()}\n");
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

#if NET472
            public void Error(string msg)
            {
                Error(msg, null, null);
            }

            public void Error(string msg, Exception ex)
            {
                Error(msg, null, ex);
            }
#endif
        }

        private class TestClass
        {
            public byte ALCD = 1;
            public uint ALID = 2;
            public TestSubClass testSubClass;
            public TestClass()
            {
                testSubClass = new TestSubClass();
            }
            public class TestSubClass
            {
                public string DeviceID = "DeviceID";
                public ushort UnitID = 666;
                public float[] FloatInfo = new float[2] { 1, 2 };
                public int[] IntInfo = new int[2] { 4, 5 };
            }
        }
    }
}
