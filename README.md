# P2P Chat Application

Ứng dụng chat P2P (Peer-to-Peer) được phát triển bằng C# Windows Forms, cho phép người dùng trò chuyện và chia sẻ file trực tiếp với nhau mà không cần server trung tâm.

## Tính năng chính

### 🔗 Kết nối P2P
- **Mô hình P2P thuần túy**: Mỗi node vừa có thể lắng nghe (server) vừa có thể kết nối (client)
- **Không cần server trung tâm**: Kết nối trực tiếp giữa các peer
- **Tự động phát hiện peer**: Tìm kiếm các peer khác trong mạng LAN qua UDP broadcast

### 💬 Chat trực tiếp
- **Giao thức TCP**: Đảm bảo tin nhắn được gửi và nhận đầy đủ
- **Giao diện thân thiện**: Rich text chat với màu sắc phân biệt
- **Lịch sử chat**: Hiển thị thời gian và người gửi

### 📁 Chia sẻ file
- **Truyền file qua TCP**: Đảm bảo tính toàn vẹn dữ liệu
- **Hiển thị tiến độ**: Progress bar và thống kê chi tiết
- **Hỗ trợ mọi loại file**: Không giới hạn định dạng

### 🔍 Discovery tự động
- **UDP Broadcast**: Tìm kiếm peer trong mạng LAN
- **Danh sách peer**: Hiển thị và chọn peer để kết nối
- **Kết nối nhanh**: Click để kết nối trực tiếp

## Cấu trúc dự án

```
P2PChatApp/
├── MainForm.cs              # Giao diện chính
├── MainForm.Designer.cs     # Designer cho giao diện
├── P2PNode.cs               # P2P Node (vừa server vừa client)
├── PeerDiscovery.cs         # UDP discovery
├── Program.cs               # Entry point
└── P2PChatApp.csproj       # Project file
```

## Cách sử dụng

### 1. Khởi động ứng dụng
```bash
dotnet run
```

### 2. Khởi động P2P Node
- Mở tab **"Kết nối"**
- Nhập cổng local (mặc định: 8080)
- Click **"Bắt đầu"** để khởi động node P2P
- Node sẽ tự động lắng nghe kết nối từ các peer khác

### 3. Tìm kiếm Peer
- Click **"Tìm kiếm Peers"** để tìm các peer khác trong LAN
- Danh sách peer hiển thị các node đang hoạt động
- Chọn peer từ danh sách, IP và cổng sẽ được tự động điền

### 4. Kết nối đến Peer khác
- Node của bạn vừa là server (lắng nghe) vừa là client (kết nối)
- Nhập IP và cổng của peer đích
- Click **"Kết nối"** để thiết lập kết nối P2P

### 5. Chat
- Mở tab **"Chat"**
- Nhập tin nhắn và nhấn Enter hoặc click **"Gửi"**

## Yêu cầu hệ thống

- **.NET 9.0**
- **Windows** (Windows Forms)
- **Mạng LAN** để discovery peer

## Công nghệ sử dụng

### Lập trình mạng
- **TCP**: Chat và file transfer
- **UDP**: Peer discovery với broadcast
- **Socket Programming**: TcpClient, TcpListener, UdpClient

### Đa luồng và bất đồng bộ
- **async/await**: Xử lý bất đồng bộ
- **Task**: Background tasks
- **CancellationToken**: Graceful shutdown

### Giao diện
- **Windows Forms**: GUI framework
- **RichTextBox**: Hiển thị chat với màu sắc
- **ProgressBar**: Hiển thị tiến độ file transfer

### 🔧 Có thể mở rộng
- **Encryption**: Mã hóa tin nhắn và file
- **Authentication**: Xác thực người dùng  
- **Group Chat**: Chat với nhiều peer cùng lúc
- **NAT Traversal**: Kết nối qua internet (hiện tại chỉ hoạt động trong LAN)
- **Message Persistence**: Lưu trữ tin nhắn
- **File Compression**: Nén file trước khi gửi
