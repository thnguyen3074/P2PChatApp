using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatApp
{
    public class PeerDiscovery
    {
        private const int DISCOVERY_PORT = 44555;
        private const int RESPONSE_PORT = 44556;
        private const string DISCOVERY_MESSAGE = "P2P_DISCOVERY";
        private const string RESPONSE_MESSAGE = "P2P_RESPONSE";

        private UdpClient? _discoveryClient;
        private UdpClient? _responseClient;
        private bool _isListening = false;
        private CancellationTokenSource? _cancellationTokenSource;

        public event Action<IPEndPoint>? PeerDiscovered;
        public event Action<string>? OnLog;

        public async Task StartListeningAsync(int myPort)
        {
            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                _isListening = true;

                _discoveryClient = new UdpClient(DISCOVERY_PORT);
                _discoveryClient.EnableBroadcast = true;

                _responseClient = new UdpClient(RESPONSE_PORT);
                _responseClient.EnableBroadcast = true;

                OnLog?.Invoke($"Bắt đầu lắng nghe discovery trên cổng {DISCOVERY_PORT}");

                var discoveryTask = ListenForDiscoveryRequestsAsync(myPort, _cancellationTokenSource.Token);
                var responseTask = ListenForResponsesAsync(_cancellationTokenSource.Token);

                await Task.WhenAny(discoveryTask, responseTask);
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Lỗi khi bắt đầu discovery: {ex.Message}");
            }
        }

        private async Task ListenForDiscoveryRequestsAsync(int myPort, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && _isListening)
            {
                try
                {
                    var result = await _discoveryClient!.ReceiveAsync();
                    string message = Encoding.UTF8.GetString(result.Buffer);

                    if (message == DISCOVERY_MESSAGE)
                    {
                        string response = $"{RESPONSE_MESSAGE}:{myPort}";
                        byte[] responseData = Encoding.UTF8.GetBytes(response);
                        await _discoveryClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);
                        OnLog?.Invoke($"Đã phản hồi discovery request từ {result.RemoteEndPoint}");
                    }
                }
                catch (Exception ex)
                {
                    OnLog?.Invoke($"Lỗi khi xử lý discovery request: {ex.Message}");
                }
            }
        }

        private async Task ListenForResponsesAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && _isListening)
            {
                try
                {
                    var result = await _responseClient!.ReceiveAsync();
                    string message = Encoding.UTF8.GetString(result.Buffer);

                    if (message.StartsWith(RESPONSE_MESSAGE))
                    {
                        var parts = message.Split(':');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int port))
                        {
                            var peerEndPoint = new IPEndPoint(result.RemoteEndPoint.Address, port);
                            PeerDiscovered?.Invoke(peerEndPoint);
                            OnLog?.Invoke($"Phát hiện peer: {peerEndPoint}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    OnLog?.Invoke($"Lỗi khi xử lý response: {ex.Message}");
                }
            }
        }

        public async Task<List<IPEndPoint>> DiscoverPeersAsync(int myPort, int timeoutSeconds = 5)
        {
            var discoveredPeers = new List<IPEndPoint>();
            var discoveredEndPoints = new HashSet<string>();

            try
            {
                using var client = new UdpClient();
                client.EnableBroadcast = true;

                byte[] discoveryData = Encoding.UTF8.GetBytes(DISCOVERY_MESSAGE);
                var broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, DISCOVERY_PORT);
                await client.SendAsync(discoveryData, discoveryData.Length, broadcastEndPoint);

                OnLog?.Invoke("Đã gửi discovery message");

                var startTime = DateTime.Now;
                var timeout = TimeSpan.FromSeconds(timeoutSeconds);

                while (DateTime.Now - startTime < timeout)
                {
                    if (client.Available > 0)
                    {
                        var result = await client.ReceiveAsync();
                        string message = Encoding.UTF8.GetString(result.Buffer);

                        if (message.StartsWith(RESPONSE_MESSAGE))
                        {
                            var parts = message.Split(':');
                            if (parts.Length == 2 && int.TryParse(parts[1], out int port))
                            {
                                var peerEndPoint = new IPEndPoint(result.RemoteEndPoint.Address, port);
                                string endPointKey = peerEndPoint.ToString();

                                if (!discoveredEndPoints.Contains(endPointKey))
                                {
                                    discoveredPeers.Add(peerEndPoint);
                                    discoveredEndPoints.Add(endPointKey);
                                    OnLog?.Invoke($"Tìm thấy peer: {peerEndPoint}");
                                }
                            }
                        }
                    }
                    await Task.Delay(100);
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Lỗi khi discover peers: {ex.Message}");
            }

            return discoveredPeers;
        }

        public void StopListening()
        {
            _isListening = false;
            _cancellationTokenSource?.Cancel();
            _discoveryClient?.Close();
            _responseClient?.Close();
            OnLog?.Invoke("Đã dừng discovery listening");
        }
    }
}
