using System;
using System.Net.Sockets;
using System.Text;
using System.Timers;
using Timer = System.Timers.Timer;

namespace P2PChatApp
{
    public class ConnectionMonitor
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _stream;
        private readonly Action _onDisconnect;
        private readonly Action<string>? _onError;
        private Timer? _timer;
        private DateTime _lastReceivedTime = DateTime.Now;

        public ConnectionMonitor(TcpClient client, NetworkStream stream, Action onDisconnect, Action<string>? onError)
        {
            _client = client;
            _stream = stream;
            _onDisconnect = onDisconnect;
            _onError = onError;
        }

        public void Start()
        {
            _timer = new Timer(3000); // kiểm tra mỗi 3 giây
            _timer.Elapsed += async (s, e) =>
            {
                try
                {
                    if (_client == null || !_client.Connected)
                    {
                        _onDisconnect();
                        return;
                    }

                    // Nếu quá 10 giây không nhận được gì => mất kết nối
                    if ((DateTime.Now - _lastReceivedTime).TotalSeconds > 10)
                    {
                        _onError?.Invoke("Mất tín hiệu từ peer, tự động ngắt kết nối.");
                        _onDisconnect();
                        return;
                    }

                    // Gửi gói ping giữ kết nối
                    byte[] ping = Encoding.UTF8.GetBytes("PING\n");
                    await _stream.WriteAsync(ping, 0, ping.Length);
                    await _stream.FlushAsync();
                }
                catch
                {
                    _onDisconnect();
                }
            };
            _timer.Start();
        }

        public void UpdateHeartbeat()
        {
            _lastReceivedTime = DateTime.Now;
        }

        public void Stop()
        {
            _timer?.Stop();
            _timer?.Dispose();
            _timer = null;
        }
    }
}
