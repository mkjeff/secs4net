namespace SecsDevice {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.GroupBox groupBox4;
            System.Windows.Forms.GroupBox groupBox2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.GroupBox groupBox5;
            System.Windows.Forms.GroupBox groupBox3;
            System.Windows.Forms.Button btnSendPrimary;
            System.Windows.Forms.Button btnReplySecondary;
            System.Windows.Forms.Button btnReplyS9F1;
            this.label4 = new System.Windows.Forms.Label();
            this.numBufferSize = new System.Windows.Forms.NumericUpDown();
            this.numDeviceId = new System.Windows.Forms.NumericUpDown();
            this.lbStatus = new System.Windows.Forms.Label();
            this.btnDisable = new System.Windows.Forms.Button();
            this.btnEnable = new System.Windows.Forms.Button();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.radioPassiveMode = new System.Windows.Forms.RadioButton();
            this.radioActiveMode = new System.Windows.Forms.RadioButton();
            this.txtRecvSecondary = new System.Windows.Forms.TextBox();
            this.txtSendPrimary = new System.Windows.Forms.TextBox();
            this.txtReplySeconary = new System.Windows.Forms.TextBox();
            this.txtRecvPrimary = new System.Windows.Forms.TextBox();
            this.lstUnreplyMsg = new System.Windows.Forms.ListBox();
            this.recvMessageBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            groupBox4 = new System.Windows.Forms.GroupBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            groupBox5 = new System.Windows.Forms.GroupBox();
            groupBox3 = new System.Windows.Forms.GroupBox();
            btnSendPrimary = new System.Windows.Forms.Button();
            btnReplySecondary = new System.Windows.Forms.Button();
            btnReplyS9F1 = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBufferSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            groupBox4.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recvMessageBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this.label4);
            groupBox1.Controls.Add(this.numBufferSize);
            groupBox1.Controls.Add(this.numDeviceId);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(this.lbStatus);
            groupBox1.Controls.Add(this.btnDisable);
            groupBox1.Controls.Add(this.btnEnable);
            groupBox1.Controls.Add(this.numPort);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(this.txtAddress);
            groupBox1.Controls.Add(this.radioPassiveMode);
            groupBox1.Controls.Add(this.radioActiveMode);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBox1.Size = new System.Drawing.Size(1821, 111);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Config";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(706, 55);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 19);
            this.label4.TabIndex = 12;
            this.label4.Text = "Buffer Size";
            // 
            // numBufferSize
            // 
            this.numBufferSize.Location = new System.Drawing.Point(802, 48);
            this.numBufferSize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numBufferSize.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numBufferSize.Name = "numBufferSize";
            this.numBufferSize.Size = new System.Drawing.Size(116, 27);
            this.numBufferSize.TabIndex = 11;
            this.numBufferSize.Value = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            // 
            // numDeviceId
            // 
            this.numDeviceId.Location = new System.Drawing.Point(608, 48);
            this.numDeviceId.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numDeviceId.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numDeviceId.Name = "numDeviceId";
            this.numDeviceId.Size = new System.Drawing.Size(64, 27);
            this.numDeviceId.TabIndex = 10;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(524, 55);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(73, 19);
            label3.TabIndex = 9;
            label3.Text = "Device Id";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbStatus.Location = new System.Drawing.Point(1286, 40);
            this.lbStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(115, 40);
            this.lbStatus.TabIndex = 8;
            this.lbStatus.Text = "Status";
            // 
            // btnDisable
            // 
            this.btnDisable.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDisable.Enabled = false;
            this.btnDisable.Location = new System.Drawing.Point(1113, 48);
            this.btnDisable.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDisable.Name = "btnDisable";
            this.btnDisable.Size = new System.Drawing.Size(112, 36);
            this.btnDisable.TabIndex = 7;
            this.btnDisable.Text = "Disable";
            this.btnDisable.UseVisualStyleBackColor = true;
            this.btnDisable.Click += new System.EventHandler(this.btnDisable_Click);
            // 
            // btnEnable
            // 
            this.btnEnable.Location = new System.Drawing.Point(992, 48);
            this.btnEnable.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(112, 36);
            this.btnEnable.TabIndex = 6;
            this.btnEnable.Text = "Enable";
            this.btnEnable.UseVisualStyleBackColor = true;
            this.btnEnable.Click += new System.EventHandler(this.btnEnable_Click);
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(420, 48);
            this.numPort.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numPort.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numPort.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(78, 27);
            this.numPort.TabIndex = 5;
            this.numPort.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(375, 55);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(38, 19);
            label2.TabIndex = 4;
            label2.Text = "Port";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(120, 55);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(22, 19);
            label1.TabIndex = 3;
            label1.Text = "IP";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(152, 48);
            this.txtAddress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(212, 27);
            this.txtAddress.TabIndex = 2;
            this.txtAddress.Text = "127.0.0.1";
            // 
            // radioPassiveMode
            // 
            this.radioPassiveMode.AutoSize = true;
            this.radioPassiveMode.Location = new System.Drawing.Point(18, 66);
            this.radioPassiveMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioPassiveMode.Name = "radioPassiveMode";
            this.radioPassiveMode.Size = new System.Drawing.Size(81, 23);
            this.radioPassiveMode.TabIndex = 1;
            this.radioPassiveMode.Text = "Passive";
            this.radioPassiveMode.UseVisualStyleBackColor = true;
            // 
            // radioActiveMode
            // 
            this.radioActiveMode.AutoSize = true;
            this.radioActiveMode.Checked = true;
            this.radioActiveMode.Location = new System.Drawing.Point(18, 32);
            this.radioActiveMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioActiveMode.Name = "radioActiveMode";
            this.radioActiveMode.Size = new System.Drawing.Size(72, 23);
            this.radioActiveMode.TabIndex = 0;
            this.radioActiveMode.TabStop = true;
            this.radioActiveMode.Text = "Active";
            this.radioActiveMode.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(this.txtRecvSecondary);
            groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox4.Location = new System.Drawing.Point(0, 475);
            groupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBox4.Size = new System.Drawing.Size(673, 438);
            groupBox4.TabIndex = 5;
            groupBox4.TabStop = false;
            groupBox4.Text = "Received Secondary Message";
            // 
            // txtRecvSecondary
            // 
            this.txtRecvSecondary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRecvSecondary.Location = new System.Drawing.Point(4, 25);
            this.txtRecvSecondary.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtRecvSecondary.Multiline = true;
            this.txtRecvSecondary.Name = "txtRecvSecondary";
            this.txtRecvSecondary.ReadOnly = true;
            this.txtRecvSecondary.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRecvSecondary.Size = new System.Drawing.Size(665, 408);
            this.txtRecvSecondary.TabIndex = 0;
            this.txtRecvSecondary.WordWrap = false;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(this.txtSendPrimary);
            groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox2.Location = new System.Drawing.Point(0, 0);
            groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBox2.Size = new System.Drawing.Size(673, 439);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "Send Primary Message";
            // 
            // txtSendPrimary
            // 
            this.txtSendPrimary.AcceptsReturn = true;
            this.txtSendPrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSendPrimary.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtSendPrimary.Location = new System.Drawing.Point(4, 25);
            this.txtSendPrimary.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSendPrimary.Multiline = true;
            this.txtSendPrimary.Name = "txtSendPrimary";
            this.txtSendPrimary.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSendPrimary.Size = new System.Drawing.Size(665, 409);
            this.txtSendPrimary.TabIndex = 1;
            this.txtSendPrimary.Text = resources.GetString("txtSendPrimary.Text");
            this.txtSendPrimary.WordWrap = false;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(this.txtReplySeconary);
            groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox5.Location = new System.Drawing.Point(0, 475);
            groupBox5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBox5.Size = new System.Drawing.Size(738, 402);
            groupBox5.TabIndex = 2;
            groupBox5.TabStop = false;
            groupBox5.Text = "Reply Secondary Message";
            // 
            // txtReplySeconary
            // 
            this.txtReplySeconary.AcceptsReturn = true;
            this.txtReplySeconary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReplySeconary.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtReplySeconary.Location = new System.Drawing.Point(4, 25);
            this.txtReplySeconary.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtReplySeconary.Multiline = true;
            this.txtReplySeconary.Name = "txtReplySeconary";
            this.txtReplySeconary.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtReplySeconary.Size = new System.Drawing.Size(730, 372);
            this.txtReplySeconary.TabIndex = 0;
            this.txtReplySeconary.WordWrap = false;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(this.txtRecvPrimary);
            groupBox3.Controls.Add(this.lstUnreplyMsg);
            groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox3.Location = new System.Drawing.Point(0, 0);
            groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBox3.Size = new System.Drawing.Size(738, 475);
            groupBox3.TabIndex = 0;
            groupBox3.TabStop = false;
            groupBox3.Text = "Received Primary Message";
            // 
            // txtRecvPrimary
            // 
            this.txtRecvPrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRecvPrimary.Location = new System.Drawing.Point(310, 25);
            this.txtRecvPrimary.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtRecvPrimary.Multiline = true;
            this.txtRecvPrimary.Name = "txtRecvPrimary";
            this.txtRecvPrimary.ReadOnly = true;
            this.txtRecvPrimary.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRecvPrimary.Size = new System.Drawing.Size(424, 445);
            this.txtRecvPrimary.TabIndex = 1;
            this.txtRecvPrimary.WordWrap = false;
            // 
            // lstUnreplyMsg
            // 
            this.lstUnreplyMsg.DataSource = this.recvMessageBindingSource;
            this.lstUnreplyMsg.DisplayMember = "Message";
            this.lstUnreplyMsg.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstUnreplyMsg.FormattingEnabled = true;
            this.lstUnreplyMsg.ItemHeight = 19;
            this.lstUnreplyMsg.Location = new System.Drawing.Point(4, 25);
            this.lstUnreplyMsg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lstUnreplyMsg.Name = "lstUnreplyMsg";
            this.lstUnreplyMsg.Size = new System.Drawing.Size(306, 445);
            this.lstUnreplyMsg.TabIndex = 0;
            this.lstUnreplyMsg.SelectedIndexChanged += new System.EventHandler(this.lstUnreplyMsg_SelectedIndexChanged);
            // 
            // recvMessageBindingSource
            // 
            this.recvMessageBindingSource.DataSource = typeof(Secs4Net.PrimaryMessageWrapper);
            // 
            // btnSendPrimary
            // 
            btnSendPrimary.Dock = System.Windows.Forms.DockStyle.Top;
            btnSendPrimary.Location = new System.Drawing.Point(0, 439);
            btnSendPrimary.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnSendPrimary.Name = "btnSendPrimary";
            btnSendPrimary.Size = new System.Drawing.Size(673, 36);
            btnSendPrimary.TabIndex = 4;
            btnSendPrimary.Text = "Send";
            btnSendPrimary.UseVisualStyleBackColor = true;
            btnSendPrimary.Click += new System.EventHandler(this.btnSendPrimary_Click);
            // 
            // btnReplySecondary
            // 
            btnReplySecondary.Dock = System.Windows.Forms.DockStyle.Bottom;
            btnReplySecondary.Location = new System.Drawing.Point(0, 877);
            btnReplySecondary.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnReplySecondary.Name = "btnReplySecondary";
            btnReplySecondary.Size = new System.Drawing.Size(738, 36);
            btnReplySecondary.TabIndex = 1;
            btnReplySecondary.Text = "Reply";
            btnReplySecondary.UseVisualStyleBackColor = true;
            btnReplySecondary.Click += new System.EventHandler(this.btnReplySecondary_Click);
            // 
            // btnReplyS9F1
            // 
            btnReplyS9F1.Dock = System.Windows.Forms.DockStyle.Bottom;
            btnReplyS9F1.Location = new System.Drawing.Point(0, 841);
            btnReplyS9F1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnReplyS9F1.Name = "btnReplyS9F1";
            btnReplyS9F1.Size = new System.Drawing.Size(738, 36);
            btnReplyS9F1.TabIndex = 3;
            btnReplyS9F1.Text = "Reply S9F7";
            btnReplyS9F1.UseVisualStyleBackColor = true;
            btnReplyS9F1.Click += new System.EventHandler(this.btnReplyS9F7_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(groupBox4);
            this.splitContainer1.Panel1.Controls.Add(btnSendPrimary);
            this.splitContainer1.Panel1.Controls.Add(groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(btnReplyS9F1);
            this.splitContainer1.Panel2.Controls.Add(groupBox5);
            this.splitContainer1.Panel2.Controls.Add(btnReplySecondary);
            this.splitContainer1.Panel2.Controls.Add(groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(1417, 913);
            this.splitContainer1.SplitterDistance = 673;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 111);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer2.Size = new System.Drawing.Size(1821, 913);
            this.splitContainer2.SplitterDistance = 1417;
            this.splitContainer2.SplitterWidth = 6;
            this.splitContainer2.TabIndex = 11;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(398, 913);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            // 
            // Form1
            // 
            this.AcceptButton = this.btnEnable;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnDisable;
            this.ClientSize = new System.Drawing.Size(1821, 1024);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "SECS Device";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBufferSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recvMessageBindingSource)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioPassiveMode;
        private System.Windows.Forms.RadioButton radioActiveMode;
        private System.Windows.Forms.Button btnDisable;
        private System.Windows.Forms.Button btnEnable;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtSendPrimary;
        private System.Windows.Forms.TextBox txtRecvSecondary;
        private System.Windows.Forms.TextBox txtRecvPrimary;
        private System.Windows.Forms.ListBox lstUnreplyMsg;
        private System.Windows.Forms.TextBox txtReplySeconary;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.NumericUpDown numDeviceId;
        private System.Windows.Forms.BindingSource recvMessageBindingSource;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numBufferSize;
    }
}

