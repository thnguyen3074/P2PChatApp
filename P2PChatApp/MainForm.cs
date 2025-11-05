using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace P2PChatApp
{
    public partial class MainForm : Form
    {
        private PeerNode? _peerNode;
        private PeerDiscovery _peerDiscovery;
        private CancellationTokenSource? _discoveryCts;
        private Dictionary<string, IPEndPoint> _discoveredPeers = new Dictionary<string, IPEndPoint>();
        private Dictionary<string, byte[]> _receivedFiles = new Dictionary<string, byte[]>();

        public MainForm()
        {
            InitializeComponent();
            _peerDiscovery = new PeerDiscovery();
            UpdateLocalIP();
            rtbChat.MouseClick += RtbChat_MouseClick;
            UpdateStatus("Sẵn sàng", Color.Gray);
        }

        private void UpdateLocalIP()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        txtLocalIP.Text = ip.ToString();
                        return;
                    }
                }
                txtLocalIP.Text = "127.0.0.1";
            }
            catch
            {
                txtLocalIP.Text = "127.0.0.1";
            }
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

                _peerNode = new PeerNode(port);

                _peerNode.MessageReceived += (username, msg) => Invoke((Action)(() =>
                {
                    string displayName = string.IsNullOrWhiteSpace(username) ? "Peer" : username;
                    AppendChatMessage($"[{DateTime.Now:HH:mm:ss}] {displayName}: {msg}", Color.Green);
                }));

                _peerNode.FileReceived += OnFileReceivedFromPeer;

                _peerNode.OnConnected += endpoint => Invoke((Action)(() =>
                {
                    UpdateStatus($"Đã kết nối với {endpoint}", Color.Green);
                    EnableChatControls(true);
                    btnConnect.Enabled = false;
                    btnDisconnect.Enabled = true;
                    txtUserName.Enabled = false;
                }));

                _peerNode.OnDisconnected += () => Invoke((Action)(() =>
                {
                    UpdateStatus("Đã ngắt kết nối", Color.Orange);
                    EnableChatControls(false);
                    btnConnect.Enabled = true;
                    btnDisconnect.Enabled = false;
                    txtUserName.Enabled = true;
                }));

                _peerNode.OnError += error => Invoke((Action)(() =>
                {
                    AppendChatMessage($"[Lỗi] {error}", Color.Red);
                }));

                Task.Run(async () => await _peerNode.StartListeningAsync());

                _discoveryCts = new CancellationTokenSource();
                Task.Run(async () => await _peerDiscovery.StartRespondingAsync(port, _discoveryCts.Token));

                btnStartServer.Enabled = false;
                btnStopServer.Enabled = true;
                txtLocalPort.Enabled = false;
                UpdateStatus($"Đang lắng nghe trên cổng {port}", Color.Blue);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            try
            {
                _peerNode?.Stop();
                _discoveryCts?.Cancel();
                btnStartServer.Enabled = true;
                btnStopServer.Enabled = false;
                txtLocalPort.Enabled = true;
                txtUserName.Enabled = true;
                EnableChatControls(false);
                UpdateStatus("Đã dừng", Color.Gray);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                if (_peerNode == null)
                {
                    MessageBox.Show("Vui lòng khởi động Node trước", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int myPort = int.Parse(txtLocalPort.Text);
                if (IsLocalIPAddress(ipAddress) && port == myPort)
                {
                    MessageBox.Show("Không thể kết nối đến chính mình!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var endPoint = new IPEndPoint(ipAddress, port);
                UpdateStatus($"Đang kết nối đến {endPoint}...", Color.Orange);

                await _peerNode.ConnectToPeerAsync(endPoint);

                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                txtRemoteIP.Enabled = false;
                txtRemotePort.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus($"Kết nối thất bại", Color.Red);
                btnConnect.Enabled = true;
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                _peerNode?.Disconnect();
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                txtRemoteIP.Enabled = true;
                txtRemotePort.Enabled = true;
                txtUserName.Enabled = true;
                EnableChatControls(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                UpdateStatus("Đang tìm kiếm peers...", Color.Blue);

                int port = int.TryParse(txtLocalPort.Text, out int p) ? p : 8080;
                var peers = await _peerDiscovery.DiscoverAsync(port, TimeSpan.FromSeconds(3));

                foreach (var peer in peers)
                {
                    string displayText = $"{peer.Address}:{peer.Port}";
                    if (!_discoveredPeers.ContainsKey(displayText))
                    {
                        _discoveredPeers[displayText] = peer;
                        lstPeers.Items.Add(displayText);
                    }
                }

                UpdateStatus($"Tìm thấy {peers.Count} peer(s)", Color.Green);
                btnDiscoverPeers.Enabled = true;
                btnDiscoverPeers.Text = "Tìm kiếm Peers";
            }
            catch (Exception ex)
            {
                UpdateStatus($"Lỗi: {ex.Message}", Color.Red);
                btnDiscoverPeers.Enabled = true;
                btnDiscoverPeers.Text = "Tìm kiếm Peers";
            }
        }

        private void lstPeers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstPeers.SelectedItem != null && _discoveredPeers.TryGetValue(lstPeers.SelectedItem.ToString()!, out IPEndPoint? peer))
            {
                txtRemoteIP.Text = peer.Address.ToString();
                txtRemotePort.Text = peer.Port.ToString();
            }
        }

        private async void btnSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                string message = txtMessage.Text.Trim();
                if (string.IsNullOrEmpty(message)) return;

                if (_peerNode == null || !_peerNode.IsConnected)
                {
                    MessageBox.Show("Chưa kết nối đến peer", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string userName = string.IsNullOrWhiteSpace(txtUserName.Text) ? "Peer" : txtUserName.Text.Trim();

                await _peerNode.SendMessageAsync(userName, message);

                AppendChatMessage($"[{DateTime.Now:HH:mm:ss}] Bạn: {message}", Color.Blue);
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (_peerNode == null || !_peerNode.IsConnected)
                {
                    MessageBox.Show("Chưa kết nối đến peer", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using var openFileDialog = new OpenFileDialog { Filter = "All Files (*.*)|*.*" };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = Path.GetFileName(openFileDialog.FileName);
                    byte[] fileData = File.ReadAllBytes(openFileDialog.FileName);

                    UpdateStatus($"Đang gửi file: {fileName}...", Color.Blue);

                    Task.Run(async () =>
                    {
                        await _peerNode.SendFileAsync(fileName, fileData);
                        Invoke((Action)(() =>
                        {
                            AppendChatMessage($"[{DateTime.Now:HH:mm:ss}] Bạn đã gửi file: {fileName} ({FormatFileSize(fileData.Length)})", Color.Blue);
                            UpdateStatus($"Đã gửi file: {fileName}", Color.Green);
                        }));
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnFileReceivedFromPeer(string fileName, byte[] fileData)
        {
            Invoke((Action)(() =>
            {
                string fileKey = $"{fileName}_{DateTime.Now.Ticks}";
                _receivedFiles[fileKey] = fileData;
                string message = $"[{DateTime.Now:HH:mm:ss}] [DOWNLOAD:{fileKey}] Đã nhận file: {fileName} ({FormatFileSize(fileData.Length)}) - Click để tải về";
                AppendChatMessage(message, Color.Green);
                UpdateStatus($"Đã nhận file: {fileName}", Color.Green);
            }));
        }



        private void RtbChat_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                int index = rtbChat.GetCharIndexFromPosition(e.Location);
                int lineIndex = rtbChat.GetLineFromCharIndex(index);
                int lineStart = rtbChat.GetFirstCharIndexFromLine(lineIndex);
                int lineEnd = lineIndex < rtbChat.Lines.Length - 1 ? rtbChat.GetFirstCharIndexFromLine(lineIndex + 1) : rtbChat.TextLength;
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

                            using var saveFileDialog = new SaveFileDialog { FileName = fileName, Filter = "All Files (*.*)|*.*" };
                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                File.WriteAllBytes(saveFileDialog.FileName, _receivedFiles[fileKey]);
                                MessageBox.Show($"Đã lưu file: {saveFileDialog.FileName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                UpdateStatus($"Đã lưu file: {fileName}", Color.Green);
                                _receivedFiles.Remove(fileKey);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Lỗi: {ex.Message}", Color.Red);
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
                len /= 1024;
            }
            return $"{len:0.##} {sizes[order]}";
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

        private void UpdateStatus(string status, Color color)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => UpdateStatus(status, color)));
                return;
            }
            lblStatus.Text = status;
            lblStatus.ForeColor = color;
        }

        private void EnableChatControls(bool enabled)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => EnableChatControls(enabled)));
                return;
            }
            txtMessage.Enabled = enabled;
            btnSendMessage.Enabled = enabled;
            btnAttachFile.Enabled = enabled;
        }

        private bool IsLocalIPAddress(IPAddress ipAddress)
        {
            try
            {
                if (IPAddress.IsLoopback(ipAddress)) return true;

                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork && ip.Equals(ipAddress))
                        return true;
                }
            }
            catch { }
            return false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                _peerNode?.Stop();
                _discoveryCts?.Cancel();
                _peerDiscovery?.Dispose(); 
            }
            catch { }
            base.OnFormClosing(e);
        }
    }
}