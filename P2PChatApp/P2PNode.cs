using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P2PChatApp
{
    public class P2PNode
    {
        private TcpListener? _tcpListener;
        private List<ConnectedPeer> _connectedPeers = new List<ConnectedPeer>();
        private int _port;
        private CancellationTokenSource? _cancellationTokenSource;

        public event Action<string>? MessageReceived;
        public event Action<string, byte[]>? FileReceived;
        public event Action<string>? OnLog;

        public bool IsConnected => _connectedPeers.Any(p => p.IsConnected);

        public P2PNode(int port)
        {
            _port = port;
        }

        public async Task StartListeningAsync()
        {
            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                _tcpListener = new TcpListener(IPAddress.Any, _port);
                _tcpListener.Start();
                OnLog?.Invoke($"Dang lang nghe tren cong {_port}");

                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    var tcpClient = await _tcpListener.AcceptTcpClientAsync();
                    OnLog?.Invoke($"Peer da ket noi: {tcpClient.Client.RemoteEndPoint}");
                    var peer = new ConnectedPeer(tcpClient);
                    _connectedPeers.Add(peer);
                    _ = Task.Run(async () => await HandlePeerAsync(peer, _cancellationTokenSource.Token));
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Loi khi lang nghe: {ex.Message}");
            }
        }

        private async Task HandlePeerAsync(ConnectedPeer peer, CancellationToken cancellationToken)
        {
            try
            {
               var networkStream = peer.TcpClient.GetStream();

                while (!cancellationToken.IsCancellationRequested && peer.IsConnected)
                {
                    var headerBuffer = new byte[4];
                    int headerRead = await networkStream.ReadAsync(headerBuffer, 0, 4, cancellationToken);
                    if (headerRead == 0)
                    {
                        OnLog?.Invoke($"Peer da ngat ket noi: {peer.TcpClient.Client.RemoteEndPoint}");
                        break; 
                    }

                    string messageType = Encoding.ASCII.GetString(headerBuffer, 0, 4);

                    if (messageType == "FILE")
                    {
                        await HandleFileReceive(networkStream, cancellationToken);
                    }
                    else if (messageType == "TEXT")
                    {
                        await HandleTextMessage(networkStream, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Loi khi xu ly peer: {ex.Message}");
            }
            finally
            {
                peer.Disconnect();
                _connectedPeers.Remove(peer);
            }
        }

        private async Task HandleTextMessage(NetworkStream stream, CancellationToken cancellationToken)
        {
            var lengthBuffer = new byte[4];
            await stream.ReadAsync(lengthBuffer, 0, 4, cancellationToken);
            int messageLength = BitConverter.ToInt32(lengthBuffer, 0);

            var messageBuffer = new byte[messageLength];
            int totalRead = 0;
            while (totalRead < messageLength)
            {
                int read = await stream.ReadAsync(messageBuffer, totalRead, messageLength - totalRead, cancellationToken);
                if (read == 0) break;
                totalRead += read;
            }

            string message = Encoding.UTF8.GetString(messageBuffer, 0, totalRead);
            MessageReceived?.Invoke(message);
        }

        private async Task HandleFileReceive(NetworkStream stream, CancellationToken cancellationToken)
        {
            try
            {
                var fileNameLengthBuffer = new byte[4];
                await stream.ReadAsync(fileNameLengthBuffer, 0, 4, cancellationToken);
                int fileNameLength = BitConverter.ToInt32(fileNameLengthBuffer, 0);

                var fileNameBuffer = new byte[fileNameLength];
                await stream.ReadAsync(fileNameBuffer, 0, fileNameLength, cancellationToken);
                string fileName = Encoding.UTF8.GetString(fileNameBuffer);

                var fileSizeBuffer = new byte[8];
                await stream.ReadAsync(fileSizeBuffer, 0, 8, cancellationToken);
                long fileSize = BitConverter.ToInt64(fileSizeBuffer, 0);

                OnLog?.Invoke($"Dang nhan file: {fileName} ({fileSize} bytes)");

                var fileData = new byte[fileSize];
                long totalRead = 0;
                while (totalRead < fileSize)
                {
                    int bytesToRead = (int)Math.Min(8192, fileSize - totalRead);
                    int bytesRead = await stream.ReadAsync(fileData, (int)totalRead, bytesToRead, cancellationToken);
                    if (bytesRead == 0) break;
                    totalRead += bytesRead;
                }

                OnLog?.Invoke($"Da nhan file thanh cong: {fileName}");
                FileReceived?.Invoke(fileName, fileData);
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Loi khi nhan file: {ex.Message}");
            }
        }

        public async Task ConnectToPeerAsync(IPEndPoint endPoint)
        {
            try
            {
                var tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(endPoint.Address, endPoint.Port);
                var peer = new ConnectedPeer(tcpClient);
                _connectedPeers.Add(peer);
                OnLog?.Invoke($"Da ket noi den peer: {endPoint}");
                _ = Task.Run(async () => await HandlePeerAsync(peer, CancellationToken.None));
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Loi khi ket noi den peer: {ex.Message}");
                throw;
            }
        }

        public async Task SendMessageAsync(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);

            foreach (var peer in _connectedPeers.ToList())
            {
                if (peer.IsConnected)
                {
                    try
                    {
                        var stream = peer.TcpClient.GetStream();

                        var header = Encoding.ASCII.GetBytes("TEXT");
                        await stream.WriteAsync(header, 0, 4);

                        var lengthBytes = BitConverter.GetBytes(messageBytes.Length);
                        await stream.WriteAsync(lengthBytes, 0, 4);

                        await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
                    }
                    catch (Exception ex)
                    {
                        OnLog?.Invoke($"Loi khi gui tin nhan: {ex.Message}");
                        peer.Disconnect();
                        _connectedPeers.Remove(peer);
                    }
                }
            }
        }

        public async Task SendFileAsync(string fileName, byte[] fileData)
        {
            try
            {
                OnLog?.Invoke($"Bat dau gui file: {fileName} ({fileData.Length} bytes)");

                foreach (var peer in _connectedPeers.ToList())
                {
                    if (peer.IsConnected)
                    {
                        try
                        {
                            var stream = peer.TcpClient.GetStream();

                            var header = Encoding.ASCII.GetBytes("FILE");
                            await stream.WriteAsync(header, 0, 4);

                            var fileNameBytes = Encoding.UTF8.GetBytes(fileName);
                            var fileNameLengthBytes = BitConverter.GetBytes(fileNameBytes.Length);
                            await stream.WriteAsync(fileNameLengthBytes, 0, 4);

                            await stream.WriteAsync(fileNameBytes, 0, fileNameBytes.Length);

                            var fileSizeBytes = BitConverter.GetBytes((long)fileData.Length);
                            await stream.WriteAsync(fileSizeBytes, 0, 8);

                            await stream.WriteAsync(fileData, 0, fileData.Length);

                            OnLog?.Invoke($"Da gui file thanh cong: {fileName}");
                        }
                        catch (Exception ex)
                        {
                            OnLog?.Invoke($"Loi khi gui file: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Loi khi gui file: {ex.Message}");
            }
        }

        public void Disconnect()
        {
            foreach (var peer in _connectedPeers.ToList())
            {
                peer.Disconnect();
            }
            _connectedPeers.Clear();
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
            Disconnect();
            _tcpListener?.Stop();
            OnLog?.Invoke("Da dung P2P Node");
        }

        private class ConnectedPeer
        {
            public TcpClient TcpClient { get; }
            public bool IsConnected => TcpClient?.Connected ?? false;

            public ConnectedPeer(TcpClient tcpClient)
            {
                TcpClient = tcpClient;
            }

            public void Disconnect()
            {
                try
                {
                    TcpClient?.Close();
                }
                catch { }
            }
        }
    }
}