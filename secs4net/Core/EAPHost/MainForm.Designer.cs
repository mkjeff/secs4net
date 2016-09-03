namespace Cim.Eap {
    partial class HostMainForm {
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
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Button btnSend;
            System.Windows.Forms.MenuStrip menu;
            System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem menuItemClearScreen;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
            System.Windows.Forms.ToolStripMenuItem reloadSpecialControlFileToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem menuItemReloadGemXml;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
            System.Windows.Forms.ToolStripMenuItem publishZServiceToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem menuItemExit;
            System.Windows.Forms.ToolStripMenuItem sECSToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem menuItemSECSMessageList;
            System.Windows.Forms.ToolStripMenuItem defineLinkToolStripMenuItem;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
            System.Windows.Forms.ToolStripMenuItem menuItemEnableLinkTest;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
            System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem menuItemEapConfig;
            System.Windows.Forms.ToolStripMenuItem menuItemGemConfig;
            System.Windows.Forms.StatusStrip statusbar;
            System.Windows.Forms.SplitContainer splitContainer2;
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.menuItemGemEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemGemDisable = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSecsTrace = new System.Windows.Forms.ToolStripMenuItem();
            this.eqpAddressStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.eapDriverLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.gemStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.listBoxSecsMessages = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.rtxtScreen = new System.Windows.Forms.RichTextBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            btnSend = new System.Windows.Forms.Button();
            menu = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuItemClearScreen = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            reloadSpecialControlFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuItemReloadGemXml = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            publishZServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            sECSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuItemSECSMessageList = new System.Windows.Forms.ToolStripMenuItem();
            defineLinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            menuItemEnableLinkTest = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuItemEapConfig = new System.Windows.Forms.ToolStripMenuItem();
            menuItemGemConfig = new System.Windows.Forms.ToolStripMenuItem();
            statusbar = new System.Windows.Forms.StatusStrip();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            groupBox1.SuspendLayout();
            menu.SuspendLayout();
            statusbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer2)).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this.txtMsg);
            groupBox1.Controls.Add(btnSend);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(96, 207);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Message";
            // 
            // txtMsg
            // 
            this.txtMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMsg.Location = new System.Drawing.Point(3, 18);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMsg.Size = new System.Drawing.Size(90, 162);
            this.txtMsg.TabIndex = 2;
            this.txtMsg.WordWrap = false;
            // 
            // btnSend
            // 
            btnSend.Dock = System.Windows.Forms.DockStyle.Bottom;
            btnSend.Location = new System.Drawing.Point(3, 180);
            btnSend.Name = "btnSend";
            btnSend.Size = new System.Drawing.Size(90, 24);
            btnSend.TabIndex = 1;
            btnSend.Text = "Send";
            btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // menu
            // 
            menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            fileToolStripMenuItem,
            sECSToolStripMenuItem,
            configToolStripMenuItem});
            menu.Location = new System.Drawing.Point(0, 0);
            menu.Name = "menu";
            menu.Size = new System.Drawing.Size(642, 24);
            menu.TabIndex = 0;
            menu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            menuItemClearScreen,
            toolStripSeparator5,
            reloadSpecialControlFileToolStripMenuItem,
            menuItemReloadGemXml,
            toolStripSeparator1,
            publishZServiceToolStripMenuItem,
            menuItemExit});
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // menuItemClearScreen
            // 
            menuItemClearScreen.Name = "menuItemClearScreen";
            menuItemClearScreen.Size = new System.Drawing.Size(226, 22);
            menuItemClearScreen.Text = "Clear Screen";
            menuItemClearScreen.Click += new System.EventHandler(this.menuItemClearScreen_Click);
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(223, 6);
            // 
            // reloadSpecialControlFileToolStripMenuItem
            // 
            reloadSpecialControlFileToolStripMenuItem.Name = "reloadSpecialControlFileToolStripMenuItem";
            reloadSpecialControlFileToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            reloadSpecialControlFileToolStripMenuItem.Text = "Reload Special Control File";
            reloadSpecialControlFileToolStripMenuItem.Click += new System.EventHandler(this.reloadSpecialControlFileToolStripMenuItem_Click);
            // 
            // menuItemReloadGemXml
            // 
            menuItemReloadGemXml.Name = "menuItemReloadGemXml";
            menuItemReloadGemXml.Size = new System.Drawing.Size(226, 22);
            menuItemReloadGemXml.Text = "Reload Gem.xml";
            menuItemReloadGemXml.Click += new System.EventHandler(this.menuItemReloadGemXml_Click);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(223, 6);
            // 
            // publishZServiceToolStripMenuItem
            // 
            publishZServiceToolStripMenuItem.Name = "publishZServiceToolStripMenuItem";
            publishZServiceToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            publishZServiceToolStripMenuItem.Text = "Publish Z Service";
            publishZServiceToolStripMenuItem.Click += new System.EventHandler(this.publishZServiceToolStripMenuItem_Click);
            // 
            // menuItemExit
            // 
            menuItemExit.Name = "menuItemExit";
            menuItemExit.Size = new System.Drawing.Size(226, 22);
            menuItemExit.Text = "Exit";
            menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // sECSToolStripMenuItem
            // 
            sECSToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            menuItemSECSMessageList,
            defineLinkToolStripMenuItem,
            toolStripSeparator2,
            this.menuItemGemEnable,
            this.menuItemGemDisable,
            toolStripSeparator3,
            menuItemEnableLinkTest,
            toolStripSeparator4,
            this.menuItemSecsTrace});
            sECSToolStripMenuItem.Name = "sECSToolStripMenuItem";
            sECSToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            sECSToolStripMenuItem.Text = "SECS";
            // 
            // menuItemSECSMessageList
            // 
            menuItemSECSMessageList.CheckOnClick = true;
            menuItemSECSMessageList.Name = "menuItemSECSMessageList";
            menuItemSECSMessageList.Size = new System.Drawing.Size(178, 22);
            menuItemSECSMessageList.Text = "SECS Message List";
            menuItemSECSMessageList.Click += new System.EventHandler(this.menuItemSecsMessagestList_Click);
            // 
            // defineLinkToolStripMenuItem
            // 
            defineLinkToolStripMenuItem.Name = "defineLinkToolStripMenuItem";
            defineLinkToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            defineLinkToolStripMenuItem.Text = "Define Link";
            defineLinkToolStripMenuItem.Click += new System.EventHandler(this.defineLinkToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(175, 6);
            // 
            // menuItemGemEnable
            // 
            this.menuItemGemEnable.Name = "menuItemGemEnable";
            this.menuItemGemEnable.Size = new System.Drawing.Size(178, 22);
            this.menuItemGemEnable.Text = "Enable";
            this.menuItemGemEnable.Click += new System.EventHandler(this.menuItemGemEnable_Click);
            // 
            // menuItemGemDisable
            // 
            this.menuItemGemDisable.Name = "menuItemGemDisable";
            this.menuItemGemDisable.Size = new System.Drawing.Size(178, 22);
            this.menuItemGemDisable.Text = "Disable";
            this.menuItemGemDisable.Click += new System.EventHandler(this.menuItemGemDisable_Click);
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(175, 6);
            // 
            // menuItemEnableLinkTest
            // 
            menuItemEnableLinkTest.CheckOnClick = true;
            menuItemEnableLinkTest.Name = "menuItemEnableLinkTest";
            menuItemEnableLinkTest.Size = new System.Drawing.Size(178, 22);
            menuItemEnableLinkTest.Text = "Link Test";
            menuItemEnableLinkTest.Click += new System.EventHandler(this.enableTraceLogToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(175, 6);
            // 
            // menuItemSecsTrace
            // 
            this.menuItemSecsTrace.CheckOnClick = true;
            this.menuItemSecsTrace.Name = "menuItemSecsTrace";
            this.menuItemSecsTrace.Size = new System.Drawing.Size(178, 22);
            this.menuItemSecsTrace.Text = "Trace On Screen";
            this.menuItemSecsTrace.Click += new System.EventHandler(this.menuItemSecsTrace_Click);
            // 
            // configToolStripMenuItem
            // 
            configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            menuItemEapConfig,
            menuItemGemConfig});
            configToolStripMenuItem.Name = "configToolStripMenuItem";
            configToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            configToolStripMenuItem.Text = "Config";
            // 
            // menuItemEapConfig
            // 
            menuItemEapConfig.Name = "menuItemEapConfig";
            menuItemEapConfig.Size = new System.Drawing.Size(102, 22);
            menuItemEapConfig.Text = "EAP";
            menuItemEapConfig.Click += new System.EventHandler(this.menuItemEapConfig_Click);
            // 
            // menuItemGemConfig
            // 
            menuItemGemConfig.Name = "menuItemGemConfig";
            menuItemGemConfig.Size = new System.Drawing.Size(102, 22);
            menuItemGemConfig.Text = "GEM";
            menuItemGemConfig.Click += new System.EventHandler(this.menuItemGemConfig_Click);
            // 
            // statusbar
            // 
            statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eqpAddressStatusLabel,
            this.eapDriverLabel,
            this.gemStatusLabel});
            statusbar.Location = new System.Drawing.Point(0, 441);
            statusbar.Name = "statusbar";
            statusbar.Size = new System.Drawing.Size(642, 22);
            statusbar.TabIndex = 1;
            // 
            // eqpAddressStatusLabel
            // 
            this.eqpAddressStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.eqpAddressStatusLabel.Name = "eqpAddressStatusLabel";
            this.eqpAddressStatusLabel.Size = new System.Drawing.Size(47, 17);
            this.eqpAddressStatusLabel.Text = "EQP IP:";
            // 
            // eapDriverLabel
            // 
            this.eapDriverLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.eapDriverLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.eapDriverLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.eapDriverLabel.Name = "eapDriverLabel";
            this.eapDriverLabel.Size = new System.Drawing.Size(476, 17);
            this.eapDriverLabel.Spring = true;
            this.eapDriverLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gemStatusLabel
            // 
            this.gemStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.gemStatusLabel.Name = "gemStatusLabel";
            this.gemStatusLabel.Size = new System.Drawing.Size(104, 17);
            this.gemStatusLabel.Text = "SECS GEM Status";
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(this.listBoxSecsMessages);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(groupBox1);
            splitContainer2.Size = new System.Drawing.Size(150, 207);
            splitContainer2.TabIndex = 0;
            splitContainer2.TabStop = false;
            // 
            // listBoxSecsMessages
            // 
            this.listBoxSecsMessages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxSecsMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxSecsMessages.ItemHeight = 12;
            this.listBoxSecsMessages.Location = new System.Drawing.Point(0, 0);
            this.listBoxSecsMessages.Name = "listBoxSecsMessages";
            this.listBoxSecsMessages.Size = new System.Drawing.Size(50, 207);
            this.listBoxSecsMessages.TabIndex = 1;
            this.listBoxSecsMessages.SelectedIndexChanged += new System.EventHandler(this.listBoxSecsMessageList_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(splitContainer2);
            this.splitContainer1.Panel1Collapsed = true;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rtxtScreen);
            this.splitContainer1.Size = new System.Drawing.Size(642, 417);
            this.splitContainer1.SplitterDistance = 207;
            this.splitContainer1.TabIndex = 10;
            // 
            // rtxtScreen
            // 
            this.rtxtScreen.BackColor = System.Drawing.Color.White;
            this.rtxtScreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtxtScreen.DetectUrls = false;
            this.rtxtScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtScreen.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxtScreen.Location = new System.Drawing.Point(0, 0);
            this.rtxtScreen.MaxLength = 8192;
            this.rtxtScreen.Name = "rtxtScreen";
            this.rtxtScreen.ReadOnly = true;
            this.rtxtScreen.ShortcutsEnabled = false;
            this.rtxtScreen.Size = new System.Drawing.Size(642, 417);
            this.rtxtScreen.TabIndex = 8;
            this.rtxtScreen.TabStop = false;
            this.rtxtScreen.Text = "";
            this.rtxtScreen.WordWrap = false;
            // 
            // HostMainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(642, 463);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(statusbar);
            this.Controls.Add(menu);
            this.MainMenuStrip = menu;
            this.Name = "HostMainForm";
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            menu.ResumeLayout(false);
            menu.PerformLayout();
            statusbar.ResumeLayout(false);
            statusbar.PerformLayout();
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainer2)).EndInit();
            splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem menuItemGemEnable;
        private System.Windows.Forms.ToolStripMenuItem menuItemGemDisable;
        private System.Windows.Forms.ToolStripStatusLabel gemStatusLabel;
        private System.Windows.Forms.ListBox listBoxSecsMessages;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.RichTextBox rtxtScreen;
        private System.Windows.Forms.ToolStripStatusLabel eapDriverLabel;
        private System.Windows.Forms.ToolStripMenuItem menuItemSecsTrace;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripStatusLabel eqpAddressStatusLabel;
    }
}