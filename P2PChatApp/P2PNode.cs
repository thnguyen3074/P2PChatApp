using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace P2PChatApp
{
    public class PeerNode
    {
        private static async Task ReadExactAsync(NetworkStream s, byte[] buf, int off, int len, CancellationToken ct)
        {
            int readTotal = 0;
            while (readTotal < len)
            {
                int r = await s.ReadAsync(buf, off + readTotal, len - readTotal, ct);
                if (r == 0) throw new IOException("Socket closed");
                readTotal += r;
            }
        }

        private static async Task<int> ReadInt32Async(NetworkStream s, CancellationToken ct)
        {
            var b = new byte[4];
            await ReadExactAsync(s, b, 0, 4, ct);
            return BitConverter.ToInt32(b, 0);
        }

        private static async Task WriteInt32Async(NetworkStream s, int value, CancellationToken ct)
        {
            var b = BitConverter.GetBytes(value);
            await s.WriteAsync(b, 0, 4, ct);
        }

        private static async Task WriteBytesAsync(NetworkStream s, byte[] data, CancellationToken ct)
        {
            await s.WriteAsync(data, 0, data.Length, ct);
        }

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
                var s = _stream!;
                var ct = _cts?.Token ?? CancellationToken.None;
                while (_client?.Connected == true && !ct.IsCancellationRequested)
                {
                    int totalLen = await ReadInt32Async(s, ct);
                    var frame = new byte[totalLen];
                    await ReadExactAsync(s, frame, 0, totalLen, ct);

                    int i = 0;
                    byte type = frame[i++];
                    int ReadLen()
                    {
                        int v = BitConverter.ToInt32(frame, i);
                        i += 4;
                        return v;
                    }
                    string ReadString()
                    {
                        int L = ReadLen();
                        var str = Encoding.UTF8.GetString(frame, i, L);
                        i += L;
                        return str;
                    }
                    byte[] ReadBytes()
                    {
                        int L = ReadLen();
                        var b = new byte[L];
                        Buffer.BlockCopy(frame, i, b, 0, L);
                        i += L;
                        return b;
                    }

                    switch (type)
                    {
                        case 1: // MSG
                        {
                            string username = ReadString();
                            string message  = ReadString();
                            MessageReceived?.Invoke(username, message);
                            break;
                        }
                        case 2: // FILE
                        {
                            string filename = ReadString();
                            byte[] data     = ReadBytes();
                            FileReceived?.Invoke(filename, data);
                            break;
                        }
                        default:
                            OnError?.Invoke($"Unknown frame type: {type}");
                            break;
                    }
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
        public async Task SendMessageAsync(string username, string message, CancellationToken cancellationToken = default)
        {
            if (_stream == null) throw new InvalidOperationException("Chưa kết nối");

            var u = Encoding.UTF8.GetBytes(username ?? string.Empty);
            var m = Encoding.UTF8.GetBytes(message ?? string.Empty);

            int payloadLen = 1 + 4 + u.Length + 4 + m.Length;
            try
            {
                await WriteInt32Async(_stream, payloadLen, cancellationToken);
                await _stream.WriteAsync(new byte[] { 1 }, 0, 1, cancellationToken); // type=1 (MSG)
                await WriteInt32Async(_stream, u.Length, cancellationToken); await WriteBytesAsync(_stream, u, cancellationToken);
                await WriteInt32Async(_stream, m.Length, cancellationToken); await WriteBytesAsync(_stream, m, cancellationToken);
                await _stream.FlushAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Lỗi gửi tin nhắn: {ex.Message}");
                Disconnect();
                throw;
            }
        }
        public async Task SendFileAsync(string fileName, byte[] fileData, CancellationToken cancellationToken = default)
        {
            if (_stream == null) throw new InvalidOperationException("Chưa kết nối");

            var f = Encoding.UTF8.GetBytes(fileName ?? string.Empty);
            int payloadLen = 1 + 4 + f.Length + 4 + (fileData?.Length ?? 0);

            try
            {
                await WriteInt32Async(_stream, payloadLen, cancellationToken);
                await _stream.WriteAsync(new byte[] { 2 }, 0, 1, cancellationToken); // type=2 (FILE)
                await WriteInt32Async(_stream, f.Length, cancellationToken);
                await WriteBytesAsync(_stream, f, cancellationToken);
                await WriteInt32Async(_stream, fileData?.Length ?? 0, cancellationToken);
                if (fileData != null && fileData.Length > 0)
                    await WriteBytesAsync(_stream, fileData, cancellationToken);

                await _stream.FlushAsync(cancellationToken);
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