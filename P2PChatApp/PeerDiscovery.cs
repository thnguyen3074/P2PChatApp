using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P2PChatApp
{
    public sealed class PeerDiscovery : IDisposable
    {
        private const int DISCOVERY_PORT = 44555;
        private readonly UdpClient _udpClient;

        public PeerDiscovery()
        {
            _udpClient = new UdpClient { EnableBroadcast = true };
        }

        public async Task<List<IPEndPoint>> DiscoverAsync(int myPort, TimeSpan timeout)
        {
            var discoveredPeers = new List<IPEndPoint>();
            var discoveredAddresses = new HashSet<string>();

            var localIPs = GetLocalIPAddresses();

            try
            {
                using var receiver = new UdpClient(DISCOVERY_PORT + 1)
                {
                    EnableBroadcast = true,
                    Client = { ReceiveTimeout = (int)timeout.TotalMilliseconds }
                };

                byte[] discoveryMsg = Encoding.UTF8.GetBytes($"DISCOVER?{myPort}");
                var broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, DISCOVERY_PORT);
                await _udpClient.SendAsync(discoveryMsg, discoveryMsg.Length, broadcastEndpoint);

                var deadline = DateTime.UtcNow + timeout;
                while (DateTime.UtcNow < deadline)
                {
                    if (receiver.Available > 0)
                    {
                        var result = await receiver.ReceiveAsync();
                        string response = Encoding.UTF8.GetString(result.Buffer);

                        if (response.StartsWith("HERE"))
                        {
                            var parts = response.Split(':');
                            int port = parts.Length > 1 && int.TryParse(parts[1], out int p) ? p : myPort;

                            var peerEndpoint = new IPEndPoint(result.RemoteEndPoint.Address, port);
                            string key = peerEndpoint.ToString();

                            bool isSelf = localIPs.Any(localIP =>
                                localIP.Equals(result.RemoteEndPoint.Address) && port == myPort);

                            if (!discoveredAddresses.Contains(key) && !isSelf)
                            {
                                discoveredPeers.Add(peerEndpoint);
                                discoveredAddresses.Add(key);
                            }
                        }
                    }
                    await Task.Delay(50);
                }
            }
            catch { }

            return discoveredPeers;
        }

        public async Task StartRespondingAsync(int myPort, CancellationToken ct)
        {
            try
            {
                using var listener = new UdpClient(DISCOVERY_PORT) { EnableBroadcast = true };

                while (!ct.IsCancellationRequested)
                {
                    var result = await listener.ReceiveAsync();
                    string message = Encoding.UTF8.GetString(result.Buffer);

                    if (message.StartsWith("DISCOVER?"))
                    {
                        string response = $"HERE:{myPort}";
                        byte[] responseData = Encoding.UTF8.GetBytes(response);
                        var responseEndpoint = new IPEndPoint(result.RemoteEndPoint.Address, DISCOVERY_PORT + 1);
                        await listener.SendAsync(responseData, responseData.Length, responseEndpoint);
                    }
                }
            }
            catch { }
        }

        private List<IPAddress> GetLocalIPAddresses()
        {
            var localIPs = new List<IPAddress>();

            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIPs.Add(ip);
                    }
                }

                localIPs.Add(IPAddress.Loopback);
                localIPs.Add(IPAddress.Parse("127.0.0.1"));
            }
            catch { }

            return localIPs;
        }

        public void Dispose()
        {
            try
            {
                _udpClient?.Close();
                _udpClient?.Dispose();
            }
            catch { }
        }
    }
}