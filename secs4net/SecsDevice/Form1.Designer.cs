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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numDeviceId = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.btnDisable = new System.Windows.Forms.Button();
            this.btnEnable = new System.Windows.Forms.Button();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.radioPassiveMode = new System.Windows.Forms.RadioButton();
            this.radioActiveMode = new System.Windows.Forms.RadioButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtRecvSecondary = new System.Windows.Forms.TextBox();
            this.btnSendPrimary = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtSendPrimary = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtReplySeconary = new System.Windows.Forms.TextBox();
            this.btnReplySecondary = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtRecvPrimary = new System.Windows.Forms.TextBox();
            this.lstUnreplyMsg = new System.Windows.Forms.ListBox();
            this.recvMessageBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recvMessageBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numDeviceId);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lbStatus);
            this.groupBox1.Controls.Add(this.btnDisable);
            this.groupBox1.Controls.Add(this.btnEnable);
            this.groupBox1.Controls.Add(this.numPort);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtAddress);
            this.groupBox1.Controls.Add(this.radioPassiveMode);
            this.groupBox1.Controls.Add(this.radioActiveMode);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(891, 70);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Config";
            // 
            // numDeviceId
            // 
            this.numDeviceId.Location = new System.Drawing.Point(405, 27);
            this.numDeviceId.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numDeviceId.Name = "numDeviceId";
            this.numDeviceId.Size = new System.Drawing.Size(43, 22);
            this.numDeviceId.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(349, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "Device Id";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbStatus.Location = new System.Drawing.Point(660, 25);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(94, 32);
            this.lbStatus.TabIndex = 8;
            this.lbStatus.Text = "Status";
            // 
            // btnDisable
            // 
            this.btnDisable.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDisable.Enabled = false;
            this.btnDisable.Location = new System.Drawing.Point(545, 25);
            this.btnDisable.Name = "btnDisable";
            this.btnDisable.Size = new System.Drawing.Size(75, 23);
            this.btnDisable.TabIndex = 7;
            this.btnDisable.Text = "Disable";
            this.btnDisable.UseVisualStyleBackColor = true;
            this.btnDisable.Click += new System.EventHandler(this.btnDisable_Click);
            // 
            // btnEnable
            // 
            this.btnEnable.Location = new System.Drawing.Point(464, 26);
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(75, 23);
            this.btnEnable.TabIndex = 6;
            this.btnEnable.Text = "Enable";
            this.btnEnable.UseVisualStyleBackColor = true;
            this.btnEnable.Click += new System.EventHandler(this.btnEnable_Click);
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(280, 27);
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
            this.numPort.Size = new System.Drawing.Size(52, 22);
            this.numPort.TabIndex = 5;
            this.numPort.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(250, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Port";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "IP";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(101, 27);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(143, 22);
            this.txtAddress.TabIndex = 2;
            // 
            // radioPassiveMode
            // 
            this.radioPassiveMode.AutoSize = true;
            this.radioPassiveMode.Location = new System.Drawing.Point(12, 42);
            this.radioPassiveMode.Name = "radioPassiveMode";
            this.radioPassiveMode.Size = new System.Drawing.Size(56, 16);
            this.radioPassiveMode.TabIndex = 1;
            this.radioPassiveMode.Text = "Passive";
            this.radioPassiveMode.UseVisualStyleBackColor = true;
            // 
            // radioActiveMode
            // 
            this.radioActiveMode.AutoSize = true;
            this.radioActiveMode.Checked = true;
            this.radioActiveMode.Location = new System.Drawing.Point(12, 20);
            this.radioActiveMode.Name = "radioActiveMode";
            this.radioActiveMode.Size = new System.Drawing.Size(53, 16);
            this.radioActiveMode.TabIndex = 0;
            this.radioActiveMode.TabStop = true;
            this.radioActiveMode.Text = "Active";
            this.radioActiveMode.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 70);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox4);
            this.splitContainer1.Panel1.Controls.Add(this.btnSendPrimary);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox5);
            this.splitContainer1.Panel2.Controls.Add(this.btnReplySecondary);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(891, 577);
            this.splitContainer1.SplitterDistance = 424;
            this.splitContainer1.TabIndex = 3;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtRecvSecondary);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 300);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(424, 277);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Received Secondary Message";
            // 
            // txtRecvSecondary
            // 
            this.txtRecvSecondary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRecvSecondary.Location = new System.Drawing.Point(3, 18);
            this.txtRecvSecondary.Multiline = true;
            this.txtRecvSecondary.Name = "txtRecvSecondary";
            this.txtRecvSecondary.ReadOnly = true;
            this.txtRecvSecondary.Size = new System.Drawing.Size(418, 256);
            this.txtRecvSecondary.TabIndex = 0;
            this.txtRecvSecondary.WordWrap = false;
            // 
            // btnSendPrimary
            // 
            this.btnSendPrimary.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSendPrimary.Location = new System.Drawing.Point(0, 277);
            this.btnSendPrimary.Name = "btnSendPrimary";
            this.btnSendPrimary.Size = new System.Drawing.Size(424, 23);
            this.btnSendPrimary.TabIndex = 4;
            this.btnSendPrimary.Text = "Send";
            this.btnSendPrimary.UseVisualStyleBackColor = true;
            this.btnSendPrimary.Click += new System.EventHandler(this.btnSendPrimary_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtSendPrimary);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(424, 277);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Send Primary Message";
            // 
            // txtSendPrimary
            // 
            this.txtSendPrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSendPrimary.Location = new System.Drawing.Point(3, 18);
            this.txtSendPrimary.Multiline = true;
            this.txtSendPrimary.Name = "txtSendPrimary";
            this.txtSendPrimary.Size = new System.Drawing.Size(418, 256);
            this.txtSendPrimary.TabIndex = 1;
            this.txtSendPrimary.WordWrap = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtReplySeconary);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 300);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(463, 254);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Reply Secondary Message";
            // 
            // txtReplySeconary
            // 
            this.txtReplySeconary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReplySeconary.Location = new System.Drawing.Point(3, 18);
            this.txtReplySeconary.Multiline = true;
            this.txtReplySeconary.Name = "txtReplySeconary";
            this.txtReplySeconary.Size = new System.Drawing.Size(457, 233);
            this.txtReplySeconary.TabIndex = 0;
            this.txtReplySeconary.WordWrap = false;
            // 
            // btnReplySecondary
            // 
            this.btnReplySecondary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnReplySecondary.Location = new System.Drawing.Point(0, 554);
            this.btnReplySecondary.Name = "btnReplySecondary";
            this.btnReplySecondary.Size = new System.Drawing.Size(463, 23);
            this.btnReplySecondary.TabIndex = 1;
            this.btnReplySecondary.Text = "Reply";
            this.btnReplySecondary.UseVisualStyleBackColor = true;
            this.btnReplySecondary.Click += new System.EventHandler(this.btnReplySecondary_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtRecvPrimary);
            this.groupBox3.Controls.Add(this.lstUnreplyMsg);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(463, 300);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Received Primary Message";
            // 
            // txtRecvPrimary
            // 
            this.txtRecvPrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRecvPrimary.Location = new System.Drawing.Point(208, 18);
            this.txtRecvPrimary.Multiline = true;
            this.txtRecvPrimary.Name = "txtRecvPrimary";
            this.txtRecvPrimary.ReadOnly = true;
            this.txtRecvPrimary.Size = new System.Drawing.Size(252, 279);
            this.txtRecvPrimary.TabIndex = 1;
            this.txtRecvPrimary.WordWrap = false;
            // 
            // lstUnreplyMsg
            // 
            this.lstUnreplyMsg.DataSource = this.recvMessageBindingSource;
            this.lstUnreplyMsg.DisplayMember = "Msg";
            this.lstUnreplyMsg.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstUnreplyMsg.FormattingEnabled = true;
            this.lstUnreplyMsg.ItemHeight = 12;
            this.lstUnreplyMsg.Location = new System.Drawing.Point(3, 18);
            this.lstUnreplyMsg.Name = "lstUnreplyMsg";
            this.lstUnreplyMsg.Size = new System.Drawing.Size(205, 268);
            this.lstUnreplyMsg.TabIndex = 0;
            this.lstUnreplyMsg.SelectedIndexChanged += new System.EventHandler(this.lstUnreplyMsg_SelectedIndexChanged);
            // 
            // recvMessageBindingSource
            // 
            this.recvMessageBindingSource.DataSource = typeof(SecsDevice.RecvMessage);
            // 
            // Form1
            // 
            this.AcceptButton = this.btnEnable;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnDisable;
            this.ClientSize = new System.Drawing.Size(891, 647);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "SECS Device";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recvMessageBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioPassiveMode;
        private System.Windows.Forms.RadioButton radioActiveMode;
        private System.Windows.Forms.Button btnDisable;
        private System.Windows.Forms.Button btnEnable;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtSendPrimary;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtRecvSecondary;
        private System.Windows.Forms.Button btnSendPrimary;
        private System.Windows.Forms.TextBox txtRecvPrimary;
        private System.Windows.Forms.ListBox lstUnreplyMsg;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtReplySeconary;
        private System.Windows.Forms.Button btnReplySecondary;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.NumericUpDown numDeviceId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource recvMessageBindingSource;
    }
}

