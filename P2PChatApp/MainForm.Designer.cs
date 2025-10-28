namespace P2PChatApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabChat = new System.Windows.Forms.TabPage();
            this.rtbChat = new System.Windows.Forms.RichTextBox();
            this.pnlMessage = new System.Windows.Forms.Panel();
            this.btnAttachFile = new System.Windows.Forms.Button();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.tabConnection = new System.Windows.Forms.TabPage();
            this.grpServer = new System.Windows.Forms.GroupBox();
            this.btnStopServer = new System.Windows.Forms.Button();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.txtLocalPort = new System.Windows.Forms.TextBox();
            this.lblLocalPort = new System.Windows.Forms.Label();
            this.txtLocalIP = new System.Windows.Forms.TextBox();
            this.lblLocalIP = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.grpClient = new System.Windows.Forms.GroupBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtRemotePort = new System.Windows.Forms.TextBox();
            this.lblRemotePort = new System.Windows.Forms.Label();
            this.txtRemoteIP = new System.Windows.Forms.TextBox();
            this.lblRemoteIP = new System.Windows.Forms.Label();
            this.grpDiscovery = new System.Windows.Forms.GroupBox();
            this.lstPeers = new System.Windows.Forms.ListBox();
            this.btnDiscoverPeers = new System.Windows.Forms.Button();
            this.lblPeers = new System.Windows.Forms.Label();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1.SuspendLayout();
            this.tabChat.SuspendLayout();
            this.pnlMessage.SuspendLayout();
            this.tabConnection.SuspendLayout();
            this.grpServer.SuspendLayout();
            this.grpClient.SuspendLayout();
            this.grpDiscovery.SuspendLayout();
            this.tabLog.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabConnection);
            this.tabControl1.Controls.Add(this.tabChat);
            this.tabControl1.Controls.Add(this.tabLog);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 450);
            this.tabControl1.TabIndex = 0;
            // 
            // tabChat
            // 
            this.tabChat.Controls.Add(this.rtbChat);
            this.tabChat.Controls.Add(this.pnlMessage);
            this.tabChat.Location = new System.Drawing.Point(4, 24);
            this.tabChat.Name = "tabChat";
            this.tabChat.Padding = new System.Windows.Forms.Padding(3);
            this.tabChat.Size = new System.Drawing.Size(792, 422);
            this.tabChat.TabIndex = 1;
            this.tabChat.Text = "Chat";
            this.tabChat.UseVisualStyleBackColor = true;
            // 
            // rtbChat
            // 
            this.rtbChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbChat.BackColor = System.Drawing.Color.White;
            this.rtbChat.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rtbChat.Location = new System.Drawing.Point(6, 6);
            this.rtbChat.Name = "rtbChat";
            this.rtbChat.ReadOnly = true;
            this.rtbChat.Size = new System.Drawing.Size(780, 350);
            this.rtbChat.TabIndex = 0;
            this.rtbChat.Text = "";
            this.rtbChat.Cursor = System.Windows.Forms.Cursors.Hand;
            // 
            // pnlMessage
            // 
            this.pnlMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMessage.Controls.Add(this.btnAttachFile);
            this.pnlMessage.Controls.Add(this.btnSendMessage);
            this.pnlMessage.Controls.Add(this.txtMessage);
            this.pnlMessage.Controls.Add(this.lblMessage);
            this.pnlMessage.Location = new System.Drawing.Point(6, 362);
            this.pnlMessage.Name = "pnlMessage";
            this.pnlMessage.Size = new System.Drawing.Size(780, 54);
            this.pnlMessage.TabIndex = 1;
            // 
            // btnAttachFile
            // 
            this.btnAttachFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAttachFile.Location = new System.Drawing.Point(584, 25);
            this.btnAttachFile.Name = "btnAttachFile";
            this.btnAttachFile.Size = new System.Drawing.Size(110, 23);
            this.btnAttachFile.TabIndex = 3;
            this.btnAttachFile.Text = "📎 Gửi File";
            this.btnAttachFile.UseVisualStyleBackColor = true;
            this.btnAttachFile.Click += new System.EventHandler(this.btnSelectAndSendFile_Click);
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendMessage.Location = new System.Drawing.Point(700, 25);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(75, 23);
            this.btnSendMessage.TabIndex = 2;
            this.btnSendMessage.Text = "Gửi";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.Location = new System.Drawing.Point(70, 25);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(508, 23);
            this.txtMessage.TabIndex = 1;
            this.txtMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMessage_KeyPress);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(3, 28);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(61, 15);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Tin nhắn:";
            // 
            // tabConnection
            // 
            this.tabConnection.Controls.Add(this.txtUserName);
            this.tabConnection.Controls.Add(this.lblUserName);
            this.tabConnection.Controls.Add(this.grpServer);
            this.tabConnection.Controls.Add(this.grpClient);
            this.tabConnection.Controls.Add(this.grpDiscovery);
            this.tabConnection.Location = new System.Drawing.Point(4, 24);
            this.tabConnection.Name = "tabConnection";
            this.tabConnection.Padding = new System.Windows.Forms.Padding(3);
            this.tabConnection.Size = new System.Drawing.Size(792, 422);
            this.tabConnection.TabIndex = 0;
            this.tabConnection.Text = "Kết nối";
            this.tabConnection.UseVisualStyleBackColor = true;
            // 
            // grpServer
            // 
            this.grpServer.Controls.Add(this.btnStopServer);
            this.grpServer.Controls.Add(this.btnStartServer);
            this.grpServer.Controls.Add(this.txtLocalPort);
            this.grpServer.Controls.Add(this.lblLocalPort);
            this.grpServer.Controls.Add(this.txtLocalIP);
            this.grpServer.Controls.Add(this.lblLocalIP);
            this.grpServer.Location = new System.Drawing.Point(6, 60);
            this.grpServer.Name = "grpServer";
            this.grpServer.Size = new System.Drawing.Size(380, 120);
            this.grpServer.TabIndex = 0;
            this.grpServer.TabStop = false;
            this.grpServer.Text = "Khởi động P2P Node";
            // 
            // btnStopServer
            // 
            this.btnStopServer.Enabled = false;
            this.btnStopServer.Location = new System.Drawing.Point(200, 80);
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(75, 23);
            this.btnStopServer.TabIndex = 5;
            this.btnStopServer.Text = "Dừng";
            this.btnStopServer.UseVisualStyleBackColor = true;
            this.btnStopServer.Click += new System.EventHandler(this.btnStopServer_Click);
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(119, 80);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(75, 23);
            this.btnStartServer.TabIndex = 4;
            this.btnStartServer.Text = "Bắt đầu";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // txtLocalPort
            // 
            this.txtLocalPort.Location = new System.Drawing.Point(119, 50);
            this.txtLocalPort.Name = "txtLocalPort";
            this.txtLocalPort.Size = new System.Drawing.Size(100, 23);
            this.txtLocalPort.TabIndex = 3;
            this.txtLocalPort.Text = "8080";
            // 
            // lblLocalPort
            // 
            this.lblLocalPort.AutoSize = true;
            this.lblLocalPort.Location = new System.Drawing.Point(6, 53);
            this.lblLocalPort.Name = "lblLocalPort";
            this.lblLocalPort.Size = new System.Drawing.Size(35, 15);
            this.lblLocalPort.TabIndex = 2;
            this.lblLocalPort.Text = "Cổng:";
            // 
            // txtLocalIP
            // 
            this.txtLocalIP.Location = new System.Drawing.Point(119, 20);
            this.txtLocalIP.Name = "txtLocalIP";
            this.txtLocalIP.ReadOnly = true;
            this.txtLocalIP.Size = new System.Drawing.Size(200, 23);
            this.txtLocalIP.TabIndex = 1;
            // 
            // lblLocalIP
            // 
            this.lblLocalIP.AutoSize = true;
            this.lblLocalIP.Location = new System.Drawing.Point(6, 23);
            this.lblLocalIP.Name = "lblLocalIP";
            this.lblLocalIP.Size = new System.Drawing.Size(20, 15);
            this.lblLocalIP.TabIndex = 0;
            this.lblLocalIP.Text = "IP:";
            // 
            // grpClient
            // 
            this.grpClient.Controls.Add(this.btnDisconnect);
            this.grpClient.Controls.Add(this.btnConnect);
            this.grpClient.Controls.Add(this.txtRemotePort);
            this.grpClient.Controls.Add(this.lblRemotePort);
            this.grpClient.Controls.Add(this.txtRemoteIP);
            this.grpClient.Controls.Add(this.lblRemoteIP);
            this.grpClient.Location = new System.Drawing.Point(406, 60);
            this.grpClient.Name = "grpClient";
            this.grpClient.Size = new System.Drawing.Size(380, 120);
            this.grpClient.TabIndex = 1;
            this.grpClient.TabStop = false;
            this.grpClient.Text = "Kết nối đến Peer";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Location = new System.Drawing.Point(200, 80);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 5;
            this.btnDisconnect.Text = "Ngắt kết nối";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(119, 80);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "Kết nối";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtRemotePort
            // 
            this.txtRemotePort.Location = new System.Drawing.Point(119, 50);
            this.txtRemotePort.Name = "txtRemotePort";
            this.txtRemotePort.Size = new System.Drawing.Size(100, 23);
            this.txtRemotePort.TabIndex = 3;
            this.txtRemotePort.Text = "8080";
            // 
            // lblRemotePort
            // 
            this.lblRemotePort.AutoSize = true;
            this.lblRemotePort.Location = new System.Drawing.Point(6, 53);
            this.lblRemotePort.Name = "lblRemotePort";
            this.lblRemotePort.Size = new System.Drawing.Size(35, 15);
            this.lblRemotePort.TabIndex = 2;
            this.lblRemotePort.Text = "Cổng:";
            // 
            // txtRemoteIP
            // 
            this.txtRemoteIP.Location = new System.Drawing.Point(119, 20);
            this.txtRemoteIP.Name = "txtRemoteIP";
            this.txtRemoteIP.Size = new System.Drawing.Size(200, 23);
            this.txtRemoteIP.TabIndex = 1;
            // 
            // lblRemoteIP
            // 
            this.lblRemoteIP.AutoSize = true;
            this.lblRemoteIP.Location = new System.Drawing.Point(6, 23);
            this.lblRemoteIP.Name = "lblRemoteIP";
            this.lblRemoteIP.Size = new System.Drawing.Size(20, 15);
            this.lblRemoteIP.TabIndex = 0;
            this.lblRemoteIP.Text = "IP:";
            // 
            // grpDiscovery
            // 
            this.grpDiscovery.Controls.Add(this.lstPeers);
            this.grpDiscovery.Controls.Add(this.btnDiscoverPeers);
            this.grpDiscovery.Controls.Add(this.lblPeers);
            this.grpDiscovery.Location = new System.Drawing.Point(6, 194);
            this.grpDiscovery.Name = "grpDiscovery";
            this.grpDiscovery.Size = new System.Drawing.Size(780, 276);
            this.grpDiscovery.TabIndex = 2;
            this.grpDiscovery.TabStop = false;
            this.grpDiscovery.Text = "Tìm kiếm Peer";
            // 
            // lstPeers
            // 
            this.lstPeers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstPeers.FormattingEnabled = true;
            this.lstPeers.ItemHeight = 15;
            this.lstPeers.Location = new System.Drawing.Point(6, 50);
            this.lstPeers.Name = "lstPeers";
            this.lstPeers.Size = new System.Drawing.Size(768, 214);
            this.lstPeers.TabIndex = 2;
            this.lstPeers.SelectedIndexChanged += new System.EventHandler(this.lstPeers_SelectedIndexChanged);
            // 
            // btnDiscoverPeers
            // 
            this.btnDiscoverPeers.Location = new System.Drawing.Point(6, 20);
            this.btnDiscoverPeers.Name = "btnDiscoverPeers";
            this.btnDiscoverPeers.Size = new System.Drawing.Size(120, 23);
            this.btnDiscoverPeers.TabIndex = 1;
            this.btnDiscoverPeers.Text = "Tìm kiếm Peers";
            this.btnDiscoverPeers.UseVisualStyleBackColor = true;
            this.btnDiscoverPeers.Click += new System.EventHandler(this.btnDiscoverPeers_Click);
            // 
            // lblPeers
            // 
            this.lblPeers.AutoSize = true;
            this.lblPeers.Location = new System.Drawing.Point(140, 23);
            this.lblPeers.Name = "lblPeers";
            this.lblPeers.Size = new System.Drawing.Size(200, 15);
            this.lblPeers.TabIndex = 0;
            this.lblPeers.Text = "Danh sách các peer đã tìm thấy:";
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.rtbLog);
            this.tabLog.Location = new System.Drawing.Point(4, 24);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Size = new System.Drawing.Size(792, 422);
            this.tabLog.TabIndex = 2;
            this.tabLog.Text = "Log";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // rtbLog
            // 
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rtbLog.Location = new System.Drawing.Point(3, 3);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Size = new System.Drawing.Size(786, 416);
            this.rtbLog.TabIndex = 0;
            this.rtbLog.Text = "";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 450);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(58, 17);
            this.lblStatus.Text = "Sẵn sàng";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(13, 28);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(66, 15);
            this.lblUserName.TabIndex = 0;
            this.lblUserName.Text = "Tên của bạn:";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(85, 25);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(200, 23);
            this.txtUserName.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 472);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.Text = "P2P Chat Application";
            this.tabControl1.ResumeLayout(false);
            this.tabChat.ResumeLayout(false);
            this.pnlMessage.ResumeLayout(false);
            this.pnlMessage.PerformLayout();
            this.tabConnection.ResumeLayout(false);
            this.tabConnection.PerformLayout();
            this.grpServer.ResumeLayout(false);
            this.grpServer.PerformLayout();
            this.grpClient.ResumeLayout(false);
            this.grpClient.PerformLayout();
            this.grpDiscovery.ResumeLayout(false);
            this.grpDiscovery.PerformLayout();
            this.tabLog.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabChat;
        private System.Windows.Forms.RichTextBox rtbChat;
        private System.Windows.Forms.Panel pnlMessage;
        private System.Windows.Forms.Button btnAttachFile;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TabPage tabConnection;
        private System.Windows.Forms.GroupBox grpServer;
        private System.Windows.Forms.Button btnStopServer;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.TextBox txtLocalPort;
        private System.Windows.Forms.Label lblLocalPort;
        private System.Windows.Forms.TextBox txtLocalIP;
        private System.Windows.Forms.Label lblLocalIP;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.GroupBox grpClient;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtRemotePort;
        private System.Windows.Forms.Label lblRemotePort;
        private System.Windows.Forms.TextBox txtRemoteIP;
        private System.Windows.Forms.Label lblRemoteIP;
        private System.Windows.Forms.GroupBox grpDiscovery;
        private System.Windows.Forms.ListBox lstPeers;
        private System.Windows.Forms.Button btnDiscoverPeers;
        private System.Windows.Forms.Label lblPeers;
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}