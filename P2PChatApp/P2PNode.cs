using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P2PChatApp
{
    public class PeerNode
    {
        private TcpListener? _listener;
        private TcpClient? _client;
        private NetworkStream? _stream;
        private readonly int _port;
        private CancellationTokenSource? _cts;

        public event Action<string, string>? MessageReceived;
        public event Action<string, byte[]>? FileReceived;
        public event Action<IPEndPoint>? OnConnected;
        public event Action? OnDisconnected;
        public event Action<string>? OnError;
        private bool _isListening = false;
        public bool IsListening => _isListening;
        public bool IsConnected => _client?.Connected ?? false;

        public PeerNode(int port) => _port = port;
        public PeerNode(){}
        string Escape(string s) => s.Replace("\\", "\\\\").Replace("|", "\\|").Replace("\n", "\\n");
        string Unescape(string s) => s.Replace("\\n", "\n").Replace("\\|", "|").Replace("\\\\", "\\");


        public async Task StartListeningAsync()
        {
            if (_isListening) return;
            try
            {
                _cts = new CancellationTokenSource();
                _listener = new TcpListener(IPAddress.Any, _port);
                _listener.Start();
                _isListening = true;
                _ = Task.Run(async () => await AcceptLoop(_cts.Token));
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Lỗi lắng nghe: {ex.Message}");
            }
        }

        private async Task AcceptLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var client = await _listener!.AcceptTcpClientAsync();
                    var endpoint = (IPEndPoint?)client.Client.RemoteEndPoint;

                    Attach(client);
                    OnConnected?.Invoke(endpoint!);
                }
            }
            catch { }
        }

        public async Task ConnectToPeerAsync(IPEndPoint endPoint)
        {
            try
            {
                var client = new TcpClient();
                await client.ConnectAsync(endPoint.Address, endPoint.Port);

                Attach(client);
                OnConnected?.Invoke(endPoint);
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Lỗi kết nối: {ex.Message}");
                throw;
            }
        }

        private void Attach(TcpClient client)
        {
            _client?.Close();
            _client = client;
            _stream = client.GetStream();

            _ = Task.Run(async () => await ReceiveLoop());
        }

        private async Task ReceiveLoop()
        {
            try
            {
                using var reader = new StreamReader(_stream!, Encoding.UTF8, leaveOpen: true);

                while (_client?.Connected == true)
                {
                    string? line = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(line)) break;

                    ProcessMessage(line);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Lỗi nhận: {ex.Message}");
            }
            finally
            {
                Disconnect();
            }
        }

        private void ProcessMessage(string packet)
        {
            try
            {
                var parts = packet.Split('|');
                if (parts.Length < 2) return;

                string type = parts[0];

                switch (type)
                {
                    case "MSG":
                        if (parts.Length >= 3)
                        {
                            string username = Unescape(parts[1]);
                            string message = Unescape(parts[2]);
                            MessageReceived?.Invoke(username, message);
                        }
                        break;

                    case "FILE":
                        if (parts.Length >= 3)
                        {
                            string filename = Unescape(parts[1]);
                            byte[] fileData = Convert.FromBase64String(parts[2]);
                            FileReceived?.Invoke(filename, fileData);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Lỗi xử lý message: {ex.Message}");
            }
        }

        public async Task SendMessageAsync(string username, string message)
        {
            if (_stream == null || !IsConnected)
                throw new InvalidOperationException("Chưa kết nối");

            try
            {
                string packet = $"MSG|{Escape(username)}|{Escape(message)}\n";
                byte[] data = Encoding.UTF8.GetBytes(packet);

                await _stream.WriteAsync(data, 0, data.Length);
                await _stream.FlushAsync();
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Lỗi gửi tin nhắn: {ex.Message}");
                Disconnect();
                throw;
            }
        }

        public async Task SendFileAsync(string fileName, byte[] fileData)
        {
            if (_stream == null || !IsConnected)
                throw new InvalidOperationException("Chưa kết nối");
            const long MAX_FILE_SIZE = 10 * 1024 * 1024;
            if (fileData.Length > MAX_FILE_SIZE)
            {
                OnError?.Invoke("Kích thước file vượt quá 10MB, vui lòng chọn file nhỏ hơn.");
                return;
            }

            try
            {
                string base64Data = Convert.ToBase64String(fileData);
                string packet = $"FILE|{Escape(fileName)}|{base64Data}\n";
                byte[] data = Encoding.UTF8.GetBytes(packet);

                await _stream.WriteAsync(data, 0, data.Length);
                await _stream.FlushAsync();
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Lỗi gửi file: {ex.Message}");
                Disconnect();
                throw;
            }
        }

        public void Disconnect()
        {
            try
            {
                _stream?.Close();
                _client?.Close();
                OnDisconnected?.Invoke();
            }
            catch { }
        }

        public void Stop()
        {
            _cts?.Cancel();
            _isListening = false;
            _listener?.Stop();
        }
    }
}