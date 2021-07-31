using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Cim.Eap {
    sealed class TextBoxAppender : AppenderSkeleton {
        static readonly Color[] _logColors = { Color.Black, Color.Green, Color.Blue, Color.Red };
        int _screentextLength = 0;
        readonly RichTextBox _textBox;
        readonly StringBuilder _buffer = new StringBuilder();
        public bool DisplaySecsMesssage { get; set; }

        delegate void WriteUILog(string msg, Level level);
        readonly WriteUILog WriteLog;

        public TextBoxAppender(RichTextBox textbox) {
            this._textBox = textbox;
            WriteLog = (msg, level) => {
                try {
                    if (_screentextLength > 0xFFFF) {
                        _textBox.Clear();
                        _screentextLength = 0;
                    }
                    _textBox.SelectionColor = _logColors[level.Value / 10000 - 4];
                    _textBox.AppendText(msg);
                    _screentextLength += msg.Length;
                } catch { }
            };
        }

        protected override void Append(LoggingEvent loggingEvent) {
            if (loggingEvent.MessageObject is SecsMessageLogInfo && !this.DisplaySecsMesssage)
                return;

            try {
                using (var sw = new StringWriter(_buffer)) {
                    Layout.Format(sw, loggingEvent);
                    string msg = sw.ToString();
                    if (_textBox.InvokeRequired)
                        _textBox.BeginInvoke(WriteLog, new object[] { msg, loggingEvent.Level });
                    else
                        WriteLog(msg, loggingEvent.Level);
                }
            } finally {
                _buffer.Length = 0; //clear StringBuffer
            }
        }

        protected override bool PreAppendCheck() {
            return !_textBox.IsDisposed;
        }
    }
}