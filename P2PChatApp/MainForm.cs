using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace P2PChatApp
{
    public partial class MainForm : Form
    {
        private P2PNode? _p2pNode;
        private PeerDiscovery _peerDiscovery;
        private Dictionary<string, IPEndPoint> _discoveredPeers = new Dictionary<string, IPEndPoint>();
        private Dictionary<string, byte[]> _receivedFiles = new Dictionary<string, byte[]>();

        public MainForm()
        {
            InitializeComponent();
            _peerDiscovery = new PeerDiscovery();
            SetupEventHandlers();
            UpdateLocalIP();
            
            rtbChat.MouseClick += RtbChat_MouseClick;
        }

        private void SetupEventHandlers()
        {
            _peerDiscovery.PeerDiscovered += OnPeerDiscovered;
            _peerDiscovery.OnLog += LogMessage;
        }

        private void UpdateLocalIP()
        {
            try
            {
                string localIP = GetLocalIPAddress();
                txtLocalIP.Text = localIP;
            }
            catch (Exception ex)
            {
                LogMessage($"Lỗi khi lấy địa chỉ IP: {ex.Message}");
            }
        }

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtLocalPort.Text, out int port))
                {
                    MessageBox.Show("Cổng không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _p2pNode = new P2PNode(port);
                _p2pNode.MessageReceived += OnMessageReceived;
                _p2pNode.FileReceived += OnFileReceivedFromPeer;
                _p2pNode.OnLog += LogMessage;
                Task.Run(async () => await _p2pNode.StartListeningAsync());
                Task.Run(async () => await _peerDiscovery.StartListeningAsync(port));

                btnStartServer.Enabled = false;
                btnStopServer.Enabled = true;
                txtLocalPort.Enabled = false;

                UpdateStatus($"Đang lắng nghe trên cổng {port}");
                LogMessage($"P2P Node đã khởi động trên cổng {port}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi khởi động server: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Lỗi khi khởi động server: {ex.Message}");
            }
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            try
            {
                _p2pNode?.Stop();
                _peerDiscovery.StopListening();

                btnStartServer.Enabled = true;
                btnStopServer.Enabled = false;
                txtLocalPort.Enabled = true;

                UpdateStatus("Đã dừng");
                LogMessage("P2P Node đã dừng");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi dừng server: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Lỗi khi dừng server: {ex.Message}");
            }
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IPAddress.TryParse(txtRemoteIP.Text, out IPAddress? ipAddress))
                {
                    MessageBox.Show("Địa chỉ IP không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txtRemotePort.Text, out int port))
                {
                    MessageBox.Show("Cổng không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (_p2pNode == null)
                {
                    MessageBox.Show("Vui lòng khởi động P2P Node trước", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var endPoint = new IPEndPoint(ipAddress, port);
                await _p2pNode.ConnectToPeerAsync(endPoint);

                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                txtRemoteIP.Enabled = false;
                txtRemotePort.Enabled = false;

                UpdateStatus($"Đã kết nối đến {endPoint}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi kết nối: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Lỗi khi kết nối: {ex.Message}");
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                _p2pNode?.Disconnect();

                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                txtRemoteIP.Enabled = true;
                txtRemotePort.Enabled = true;

                UpdateStatus("Đã ngắt kết nối");
                LogMessage("Đã ngắt kết nối");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi ngắt kết nối: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Lỗi khi ngắt kết nối: {ex.Message}");
            }
        }

        private async void btnDiscoverPeers_Click(object sender, EventArgs e)
        {
            try
            {
                lstPeers.Items.Clear();
                _discoveredPeers.Clear();
                btnDiscoverPeers.Enabled = false;
                btnDiscoverPeers.Text = "Đang tìm kiếm...";

                var peers = await _peerDiscovery.DiscoverPeersAsync(8080, 3);

                Invoke((Action)(() =>
                {
                    foreach (var peer in peers)
                    {
                        string displayText = $"{peer.Address}:{peer.Port}";
                        if (!_discoveredPeers.ContainsKey(displayText))
                        {
                            _discoveredPeers[displayText] = peer;
                            lstPeers.Items.Add(displayText);
                        }
                    }

                    btnDiscoverPeers.Enabled = true;
                    btnDiscoverPeers.Text = "Tìm kiếm Peers";
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm peers: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Lỗi khi tìm kiếm peers: {ex.Message}");
                Invoke((Action)(() =>
                {
                    btnDiscoverPeers.Enabled = true;
                    btnDiscoverPeers.Text = "Tìm kiếm Peers";
                }));
            }
        }

        private void lstPeers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstPeers.SelectedItem != null)
            {
                string selectedPeer = lstPeers.SelectedItem.ToString()!;
                if (_discoveredPeers.TryGetValue(selectedPeer, out IPEndPoint? peer))
                {
                    txtRemoteIP.Text = peer.Address.ToString();
                    txtRemotePort.Text = peer.Port.ToString();
                }
            }
        }

        private async void btnSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                string message = txtMessage.Text.Trim();
                if (string.IsNullOrEmpty(message))
                    return;

                if (_p2pNode == null || !_p2pNode.IsConnected)
                {
                    MessageBox.Show("Chưa kết nối đến peer nào", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string userName = !string.IsNullOrEmpty(txtUserName.Text) ? txtUserName.Text : "Bạn";
                string fullMessage = $"[{DateTime.Now:HH:mm:ss}] {userName}: {message}";

                await _p2pNode.SendMessageAsync(fullMessage);
                AppendChatMessage(fullMessage, Color.Blue);
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi tin nhắn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Lỗi khi gửi tin nhắn: {ex.Message}");
            }
        }

        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSendMessage_Click(sender, e);
                e.Handled = true;
            }
        }

        private void btnSelectAndSendFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (_p2pNode == null || !_p2pNode.IsConnected)
                {
                    MessageBox.Show("Chưa kết nối đến peer nào", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "All Files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string fileName = Path.GetFileName(filePath);

                    byte[] fileData = File.ReadAllBytes(filePath);

                    Task.Run(async () =>
                    {
                        await _p2pNode.SendFileAsync(fileName, fileData);

                        string userName = !string.IsNullOrEmpty(txtUserName.Text) ? txtUserName.Text : "Bạn";
                        string message = $"[{DateTime.Now:HH:mm:ss}] {userName} đã gửi file: {fileName} ({FormatFileSize(fileData.Length)})";

                        Invoke((Action)(() =>
                        {
                            AppendChatMessage(message, Color.Blue);
                        }));
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Lỗi khi gửi file: {ex.Message}");
            }
        }

        private void OnFileReceivedFromPeer(string fileName, byte[] fileData)
        {
            Invoke((Action)(() =>
            {
                string fileKey = $"{fileName}_{DateTime.Now.Ticks}";
                _receivedFiles[fileKey] = fileData;

                string message = $"[{DateTime.Now:HH:mm:ss}] Đã nhận file: {fileName} ({FormatFileSize(fileData.Length)}) - Click để tải về";
                AppendClickableFileMessage(message, fileName, fileKey, Color.Green);
            }));
        }

        private void AppendClickableFileMessage(string message, string fileName, string fileKey, Color color)
        {
            int startIndex = rtbChat.TextLength;

            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionLength = 0;
            rtbChat.SelectionColor = color;
            rtbChat.SelectionFont = new Font(rtbChat.Font, FontStyle.Bold | FontStyle.Underline);

            rtbChat.AppendText(message + $" [DOWNLOAD:{fileKey}]");
            rtbChat.AppendText(Environment.NewLine);

            rtbChat.SelectionColor = rtbChat.ForeColor;
            rtbChat.SelectionFont = rtbChat.Font;
            rtbChat.ScrollToCaret();
        }

        private void RtbChat_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                int index = rtbChat.GetCharIndexFromPosition(e.Location);

                int lineIndex = rtbChat.GetLineFromCharIndex(index);
                int lineStart = rtbChat.GetFirstCharIndexFromLine(lineIndex);
                int lineEnd = lineIndex < rtbChat.Lines.Length - 1
                    ? rtbChat.GetFirstCharIndexFromLine(lineIndex + 1)
                    : rtbChat.TextLength;

                string line = rtbChat.Text.Substring(lineStart, lineEnd - lineStart);

                if (line.Contains("[DOWNLOAD:"))
                {
                    int startIdx = line.IndexOf("[DOWNLOAD:") + 10;
                    int endIdx = line.IndexOf("]", startIdx);
                    if (endIdx > startIdx)
                    {
                        string fileKey = line.Substring(startIdx, endIdx - startIdx);

                        if (_receivedFiles.ContainsKey(fileKey))
                        {
                            int fileNameStart = line.IndexOf("Đã nhận file: ") + 14;
                            int fileNameEnd = line.IndexOf(" (", fileNameStart);
                            string fileName = line.Substring(fileNameStart, fileNameEnd - fileNameStart);

                            using var saveFileDialog = new SaveFileDialog();
                            saveFileDialog.FileName = fileName;
                            saveFileDialog.Filter = "All Files (*.*)|*.*";

                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                File.WriteAllBytes(saveFileDialog.FileName, _receivedFiles[fileKey]);
                                MessageBox.Show($"Đã lưu file: {saveFileDialog.FileName}", "Thành công",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                _receivedFiles.Remove(fileKey);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Lỗi khi xử lý click: {ex.Message}");
            }
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private void OnPeerDiscovered(IPEndPoint peer)
        {
        }

        private void OnMessageReceived(string message)
        {
            Invoke((Action)(() =>
            {
                AppendChatMessage(message, Color.Green);
            }));
        }

        private void AppendChatMessage(string message, Color color)
        {
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionLength = 0;
            rtbChat.SelectionColor = color;
            rtbChat.AppendText(message + Environment.NewLine);
            rtbChat.SelectionColor = rtbChat.ForeColor;
            rtbChat.ScrollToCaret();
        }


        private void LogMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => LogMessage(message)));
                return;
            }

            string timestampedMessage = $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}";
            rtbLog.AppendText(timestampedMessage);
            rtbLog.ScrollToCaret();
        }

        private void UpdateStatus(string status)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => UpdateStatus(status)));
                return;
            }

            lblStatus.Text = status;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _p2pNode?.Stop();
            _peerDiscovery.StopListening();
            base.OnFormClosing(e);
        }
    }
}