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
            tabControl1 = new TabControl();
            tabConnection = new TabPage();
            txtUserName = new TextBox();
            lblUserName = new Label();
            grpServer = new GroupBox();
            btnStopServer = new Button();
            btnStartServer = new Button();
            txtLocalPort = new TextBox();
            lblLocalPort = new Label();
            txtLocalIP = new TextBox();
            lblLocalIP = new Label();
            grpClient = new GroupBox();
            btnDisconnect = new Button();
            btnConnect = new Button();
            txtRemotePort = new TextBox();
            lblRemotePort = new Label();
            txtRemoteIP = new TextBox();
            lblRemoteIP = new Label();
            grpDiscovery = new GroupBox();
            lstPeers = new ListBox();
            btnDiscoverPeers = new Button();
            lblPeers = new Label();
            tabChat = new TabPage();
            rtbChat = new RichTextBox();
            pnlMessage = new Panel();
            btnAttachFile = new Button();
            btnSendMessage = new Button();
            txtMessage = new TextBox();
            lblMessage = new Label();
            statusStrip1 = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            tabControl1.SuspendLayout();
            tabConnection.SuspendLayout();
            grpServer.SuspendLayout();
            grpClient.SuspendLayout();
            grpDiscovery.SuspendLayout();
            tabChat.SuspendLayout();
            pnlMessage.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabConnection);
            tabControl1.Controls.Add(tabChat);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 450);
            tabControl1.TabIndex = 0;
            // 
            // tabConnection
            // 
            tabConnection.Controls.Add(txtUserName);
            tabConnection.Controls.Add(lblUserName);
            tabConnection.Controls.Add(grpServer);
            tabConnection.Controls.Add(grpClient);
            tabConnection.Controls.Add(grpDiscovery);
            tabConnection.Location = new Point(4, 24);
            tabConnection.Name = "tabConnection";
            tabConnection.Padding = new Padding(3);
            tabConnection.Size = new Size(792, 422);
            tabConnection.TabIndex = 0;
            tabConnection.Text = "Kết nối";
            tabConnection.UseVisualStyleBackColor = true;
            // 
            // txtUserName
            // 
            txtUserName.Location = new Point(85, 25);
            txtUserName.Name = "txtUserName";
            txtUserName.Size = new Size(200, 23);
            txtUserName.TabIndex = 1;
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Location = new Point(13, 28);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(73, 15);
            lblUserName.TabIndex = 0;
            lblUserName.Text = "Tên của bạn:";
            // 
            // grpServer
            // 
            grpServer.Controls.Add(btnStopServer);
            grpServer.Controls.Add(btnStartServer);
            grpServer.Controls.Add(txtLocalPort);
            grpServer.Controls.Add(lblLocalPort);
            grpServer.Controls.Add(txtLocalIP);
            grpServer.Controls.Add(lblLocalIP);
            grpServer.Location = new Point(6, 60);
            grpServer.Name = "grpServer";
            grpServer.Size = new Size(380, 120);
            grpServer.TabIndex = 0;
            grpServer.TabStop = false;
            grpServer.Text = "Khởi động P2P Node";
            // 
            // btnStopServer
            // 
            btnStopServer.Enabled = false;
            btnStopServer.Location = new Point(200, 80);
            btnStopServer.Name = "btnStopServer";
            btnStopServer.Size = new Size(75, 23);
            btnStopServer.TabIndex = 5;
            btnStopServer.Text = "Dừng";
            btnStopServer.UseVisualStyleBackColor = true;
            btnStopServer.Click += btnStopServer_Click;
            // 
            // btnStartServer
            // 
            btnStartServer.Location = new Point(119, 80);
            btnStartServer.Name = "btnStartServer";
            btnStartServer.Size = new Size(75, 23);
            btnStartServer.TabIndex = 4;
            btnStartServer.Text = "Bắt đầu";
            btnStartServer.UseVisualStyleBackColor = true;
            btnStartServer.Click += btnStartServer_Click;
            // 
            // txtLocalPort
            // 
            txtLocalPort.Location = new Point(119, 50);
            txtLocalPort.Name = "txtLocalPort";
            txtLocalPort.Size = new Size(100, 23);
            txtLocalPort.TabIndex = 3;
            txtLocalPort.Text = "8080";
            // 
            // lblLocalPort
            // 
            lblLocalPort.AutoSize = true;
            lblLocalPort.Location = new Point(6, 53);
            lblLocalPort.Name = "lblLocalPort";
            lblLocalPort.Size = new Size(39, 15);
            lblLocalPort.TabIndex = 2;
            lblLocalPort.Text = "Cổng:";
            // 
            // txtLocalIP
            // 
            txtLocalIP.Location = new Point(119, 20);
            txtLocalIP.Name = "txtLocalIP";
            txtLocalIP.ReadOnly = true;
            txtLocalIP.Size = new Size(200, 23);
            txtLocalIP.TabIndex = 1;
            // 
            // lblLocalIP
            // 
            lblLocalIP.AutoSize = true;
            lblLocalIP.Location = new Point(6, 23);
            lblLocalIP.Name = "lblLocalIP";
            lblLocalIP.Size = new Size(20, 15);
            lblLocalIP.TabIndex = 0;
            lblLocalIP.Text = "IP:";
            // 
            // grpClient
            // 
            grpClient.Controls.Add(btnDisconnect);
            grpClient.Controls.Add(btnConnect);
            grpClient.Controls.Add(txtRemotePort);
            grpClient.Controls.Add(lblRemotePort);
            grpClient.Controls.Add(txtRemoteIP);
            grpClient.Controls.Add(lblRemoteIP);
            grpClient.Location = new Point(406, 60);
            grpClient.Name = "grpClient";
            grpClient.Size = new Size(380, 120);
            grpClient.TabIndex = 1;
            grpClient.TabStop = false;
            grpClient.Text = "Kết nối đến Peer";
            // 
            // btnDisconnect
            // 
            btnDisconnect.Enabled = false;
            btnDisconnect.Location = new Point(200, 80);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(82, 23);
            btnDisconnect.TabIndex = 5;
            btnDisconnect.Text = "Ngắt kết nối";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(119, 80);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(75, 23);
            btnConnect.TabIndex = 4;
            btnConnect.Text = "Kết nối";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // txtRemotePort
            // 
            txtRemotePort.Location = new Point(119, 50);
            txtRemotePort.Name = "txtRemotePort";
            txtRemotePort.Size = new Size(100, 23);
            txtRemotePort.TabIndex = 3;
            txtRemotePort.Text = "8080";
            // 
            // lblRemotePort
            // 
            lblRemotePort.AutoSize = true;
            lblRemotePort.Location = new Point(6, 53);
            lblRemotePort.Name = "lblRemotePort";
            lblRemotePort.Size = new Size(39, 15);
            lblRemotePort.TabIndex = 2;
            lblRemotePort.Text = "Cổng:";
            // 
            // txtRemoteIP
            // 
            txtRemoteIP.Location = new Point(119, 20);
            txtRemoteIP.Name = "txtRemoteIP";
            txtRemoteIP.Size = new Size(200, 23);
            txtRemoteIP.TabIndex = 1;
            // 
            // lblRemoteIP
            // 
            lblRemoteIP.AutoSize = true;
            lblRemoteIP.Location = new Point(6, 23);
            lblRemoteIP.Name = "lblRemoteIP";
            lblRemoteIP.Size = new Size(20, 15);
            lblRemoteIP.TabIndex = 0;
            lblRemoteIP.Text = "IP:";
            // 
            // grpDiscovery
            // 
            grpDiscovery.Controls.Add(lstPeers);
            grpDiscovery.Controls.Add(btnDiscoverPeers);
            grpDiscovery.Controls.Add(lblPeers);
            grpDiscovery.Location = new Point(6, 194);
            grpDiscovery.Name = "grpDiscovery";
            grpDiscovery.Size = new Size(780, 220);
            grpDiscovery.TabIndex = 2;
            grpDiscovery.TabStop = false;
            grpDiscovery.Text = "Tìm kiếm Peer";
            // 
            // lstPeers
            // 
            lstPeers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstPeers.FormattingEnabled = true;
            lstPeers.Location = new Point(6, 50);
            lstPeers.Name = "lstPeers";
            lstPeers.Size = new Size(768, 154);
            lstPeers.TabIndex = 2;
            lstPeers.SelectedIndexChanged += lstPeers_SelectedIndexChanged;
            // 
            // btnDiscoverPeers
            // 
            btnDiscoverPeers.Location = new Point(6, 20);
            btnDiscoverPeers.Name = "btnDiscoverPeers";
            btnDiscoverPeers.Size = new Size(120, 23);
            btnDiscoverPeers.TabIndex = 1;
            btnDiscoverPeers.Text = "Tìm kiếm Peers";
            btnDiscoverPeers.UseVisualStyleBackColor = true;
            btnDiscoverPeers.Click += btnDiscoverPeers_Click;
            // 
            // lblPeers
            // 
            lblPeers.AutoSize = true;
            lblPeers.Location = new Point(140, 23);
            lblPeers.Name = "lblPeers";
            lblPeers.Size = new Size(175, 15);
            lblPeers.TabIndex = 0;
            lblPeers.Text = "Danh sách các peer đã tìm thấy:";
            // 
            // tabChat
            // 
            tabChat.Controls.Add(rtbChat);
            tabChat.Controls.Add(pnlMessage);
            tabChat.Location = new Point(4, 24);
            tabChat.Name = "tabChat";
            tabChat.Padding = new Padding(3);
            tabChat.Size = new Size(792, 422);
            tabChat.TabIndex = 1;
            tabChat.Text = "Chat";
            tabChat.UseVisualStyleBackColor = true;
            // 
            // rtbChat
            // 
            rtbChat.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rtbChat.BackColor = Color.White;
            rtbChat.Cursor = Cursors.Hand;
            rtbChat.Font = new Font("Consolas", 9F);
            rtbChat.Location = new Point(6, 6);
            rtbChat.Name = "rtbChat";
            rtbChat.ReadOnly = true;
            rtbChat.Size = new Size(780, 350);
            rtbChat.TabIndex = 0;
            rtbChat.Text = "";
            // 
            // pnlMessage
            // 
            pnlMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlMessage.Controls.Add(btnAttachFile);
            pnlMessage.Controls.Add(btnSendMessage);
            pnlMessage.Controls.Add(txtMessage);
            pnlMessage.Controls.Add(lblMessage);
            pnlMessage.Location = new Point(6, 362);
            pnlMessage.Name = "pnlMessage";
            pnlMessage.Size = new Size(780, 54);
            pnlMessage.TabIndex = 1;
            // 
            // btnAttachFile
            // 
            btnAttachFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAttachFile.Location = new Point(612, 25);
            btnAttachFile.Name = "btnAttachFile";
            btnAttachFile.Size = new Size(81, 23);
            btnAttachFile.TabIndex = 3;
            btnAttachFile.Text = "Gửi File";
            btnAttachFile.UseVisualStyleBackColor = true;
            btnAttachFile.Click += btnSelectAndSendFile_Click;
            // 
            // btnSendMessage
            // 
            btnSendMessage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSendMessage.Location = new Point(699, 25);
            btnSendMessage.Name = "btnSendMessage";
            btnSendMessage.Size = new Size(76, 23);
            btnSendMessage.TabIndex = 2;
            btnSendMessage.Text = "Gửi";
            btnSendMessage.UseVisualStyleBackColor = true;
            btnSendMessage.Click += btnSendMessage_Click;
            // 
            // txtMessage
            // 
            txtMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtMessage.Location = new Point(70, 25);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(536, 23);
            txtMessage.TabIndex = 1;
            txtMessage.KeyPress += txtMessage_KeyPress;
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Location = new Point(3, 28);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(56, 15);
            lblMessage.TabIndex = 0;
            lblMessage.Text = "Tin nhắn:";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip1.Location = new Point(0, 450);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(54, 17);
            lblStatus.Text = "Sẵn sàng";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 472);
            Controls.Add(tabControl1);
            Controls.Add(statusStrip1);
            Name = "MainForm";
            Text = "P2P Chat Application";
            tabControl1.ResumeLayout(false);
            tabConnection.ResumeLayout(false);
            tabConnection.PerformLayout();
            grpServer.ResumeLayout(false);
            grpServer.PerformLayout();
            grpClient.ResumeLayout(false);
            grpClient.PerformLayout();
            grpDiscovery.ResumeLayout(false);
            grpDiscovery.PerformLayout();
            tabChat.ResumeLayout(false);
            pnlMessage.ResumeLayout(false);
            pnlMessage.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}