﻿namespace SecsDevice {
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
            System.Windows.Forms.GroupBox groupBox5;
            System.Windows.Forms.GroupBox groupBox3;
            System.Windows.Forms.Button btnSendPrimary;
            System.Windows.Forms.Button btnReplySecondary;
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
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            groupBox4.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recvMessageBindingSource)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
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
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(891, 70);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Config";
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
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(349, 31);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(50, 12);
            label3.TabIndex = 9;
            label3.Text = "Device Id";
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
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(250, 31);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(24, 12);
            label2.TabIndex = 4;
            label2.Text = "Port";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(80, 31);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(15, 12);
            label1.TabIndex = 3;
            label1.Text = "IP";
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
            // groupBox4
            // 
            groupBox4.Controls.Add(this.txtRecvSecondary);
            groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox4.Location = new System.Drawing.Point(0, 300);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new System.Drawing.Size(424, 277);
            groupBox4.TabIndex = 5;
            groupBox4.TabStop = false;
            groupBox4.Text = "Received Secondary Message";
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
            // groupBox2
            // 
            groupBox2.Controls.Add(this.txtSendPrimary);
            groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox2.Location = new System.Drawing.Point(0, 0);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(424, 277);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "Send Primary Message";
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
            groupBox5.Controls.Add(this.txtReplySeconary);
            groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox5.Location = new System.Drawing.Point(0, 300);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new System.Drawing.Size(463, 254);
            groupBox5.TabIndex = 2;
            groupBox5.TabStop = false;
            groupBox5.Text = "Reply Secondary Message";
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
            // groupBox3
            // 
            groupBox3.Controls.Add(this.txtRecvPrimary);
            groupBox3.Controls.Add(this.lstUnreplyMsg);
            groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox3.Location = new System.Drawing.Point(0, 0);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(463, 300);
            groupBox3.TabIndex = 0;
            groupBox3.TabStop = false;
            groupBox3.Text = "Received Primary Message";
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
            this.lstUnreplyMsg.Size = new System.Drawing.Size(205, 279);
            this.lstUnreplyMsg.TabIndex = 0;
            this.lstUnreplyMsg.SelectedIndexChanged += new System.EventHandler(this.lstUnreplyMsg_SelectedIndexChanged);
            // 
            // recvMessageBindingSource
            // 
            this.recvMessageBindingSource.DataSource = typeof(SecsDevice.RecvMessage);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 70);
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
            this.splitContainer1.Panel2.Controls.Add(groupBox5);
            this.splitContainer1.Panel2.Controls.Add(btnReplySecondary);
            this.splitContainer1.Panel2.Controls.Add(groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(891, 577);
            this.splitContainer1.SplitterDistance = 424;
            this.splitContainer1.TabIndex = 3;
            // 
            // btnSendPrimary
            // 
            btnSendPrimary.Dock = System.Windows.Forms.DockStyle.Top;
            btnSendPrimary.Location = new System.Drawing.Point(0, 277);
            btnSendPrimary.Name = "btnSendPrimary";
            btnSendPrimary.Size = new System.Drawing.Size(424, 23);
            btnSendPrimary.TabIndex = 4;
            btnSendPrimary.Text = "Send";
            btnSendPrimary.UseVisualStyleBackColor = true;
            btnSendPrimary.Click += new System.EventHandler(this.btnSendPrimary_Click);
            // 
            // btnReplySecondary
            // 
            btnReplySecondary.Dock = System.Windows.Forms.DockStyle.Bottom;
            btnReplySecondary.Location = new System.Drawing.Point(0, 554);
            btnReplySecondary.Name = "btnReplySecondary";
            btnReplySecondary.Size = new System.Drawing.Size(463, 23);
            btnReplySecondary.TabIndex = 1;
            btnReplySecondary.Text = "Reply";
            btnReplySecondary.UseVisualStyleBackColor = true;
            btnReplySecondary.Click += new System.EventHandler(this.btnReplySecondary_Click);
            // 
            // Form1
            // 
            this.AcceptButton = this.btnEnable;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnDisable;
            this.ClientSize = new System.Drawing.Size(891, 647);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(groupBox1);
            this.Name = "Form1";
            this.Text = "SECS Device";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
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
            this.splitContainer1.ResumeLayout(false);
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
    }
}

