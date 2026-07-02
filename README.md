# App Chat GUI TCP - Nhóm UDM_08

Một ứng dụng trò chuyện (Chat Application) hoàn chỉnh được phát triển bằng ngôn ngữ **C# (.NET)**, sử dụng giao diện **Windows Forms (WinForms)** và truyền thông mạng qua giao thức **TCP Socket**. Ứng dụng tích hợp hệ quản trị cơ sở dữ liệu **SQLite** để lưu trữ tài khoản và lịch sử tin nhắn.

---

## 📌 Thông tin dự án
*   **Tên dự án:** App Chat GUI TCP
*   **Mã nhóm / Project Code:** UDM_08
*   **Môn học:** Lập trình mạng

---

## 📂 Cấu trúc thư mục (Directory Structure)

Thư mục dự án được tổ chức đồng bộ theo yêu cầu cấu trúc chuẩn:

```text
project_App-chat-GUI--TCP-UDM_08/
├── Code/                   # Chứa mã nguồn của dự án
│   ├── ChatClient/         # Dự án Client (WinForms: Đăng nhập, giao diện chat bong bóng)
│   ├── ChatServer/         # Dự án Server (WinForms: Quản lý kết nối, giám sát log hệ thống)
│   ├── ChatCommon/         # Các lớp thư viện dùng chung (Mô hình hóa Packet, SQLite Repository)
│   └── Database/           # Chứa kịch bản khởi tạo cơ sở dữ liệu SQL (chatapp.sql)
├── DOCX/                   # Báo cáo Word của dự án (DOCX.docx)
├── PPTX/                   # Slide thuyết trình nhóm (Lập trình mạng UDM_08.pptx)
├── Extra/                  # File Excel chứa kịch bản kiểm thử (TEST CASE.xlsx)
└── README.md               # Hướng dẫn chi tiết dự án (Tệp tin này)
```

---

## 🚀 Các tính năng chính (Core Features)

### 1. Phía Client (ChatClient)
*   **Đăng ký & Đăng nhập bảo mật:** Người dùng có thể đăng ký tài khoản mới. Mật khẩu được mã hóa an toàn bằng thuật toán SHA-256 kết hợp mã Salt ngẫu nhiên trước khi lưu trữ.
*   **Quản lý ảnh đại diện (Avatar):** Cho phép người dùng tùy chọn và thay đổi Avatar trực tiếp từ tệp tin ảnh máy tính cá nhân. Ảnh được mã hóa sang dạng chuỗi Base64 và đồng bộ qua cơ sở dữ liệu.
*   **Nhắn tin nhóm (Public Chat):** Gửi tin nhắn công khai đến tất cả thành viên đang trực tuyến trong phòng chat chung.
*   **Nhắn tin riêng tư (Private Chat):** Lựa chọn một người dùng cụ thể từ danh sách Online (thông qua bảng DataGridView tương tác) để bắt đầu nhắn tin riêng tư 1-to-1.
*   **Phản hồi tin nhắn (Reply):** Hỗ trợ tính năng trả lời trực tiếp một tin nhắn cụ thể, trích dẫn nội dung tin nhắn cũ giúp cuộc hội thoại mạch lạc hơn.
*   **Chuyển tiếp tin nhắn (Forward):** Cho phép chuyển tiếp nhanh nội dung tin nhắn của một người dùng khác sang kênh chat chung hoặc chat riêng.
*   **Bảng chọn Emoji:** Tích hợp bộ biểu tượng cảm xúc trực quan để người dùng nhanh chóng lựa chọn và chèn vào tin nhắn.
*   **Lịch sử tin nhắn (Message History):** Tự động tải lại lịch sử tin nhắn công khai và riêng tư ngay khi kết nối thành công tới Server.

### 2. Phía Server (ChatServer)
*   **Mở/Đóng cổng kết nối:** Cho phép thiết lập cấu hình Port linh hoạt (ví dụ: `9988`) và kiểm tra tính sẵn dùng của Port (tránh lỗi xung đột AddressAlreadyInUse).
*   **Bảo mật bằng khóa (Verification Key):** Hỗ trợ thiết lập một mã khóa bảo mật chung tại Server. Client chỉ có thể kết nối thành công khi nhập đúng mã khóa này.
*   **Chống đăng nhập trùng lặp (Duplicate Login Prevention):** Từ chối kết nối nếu một tài khoản đang hoạt động cố tình đăng nhập ở một thiết bị hoặc phiên làm việc khác.
*   **Theo dõi danh sách trực tuyến:** Hiển thị trực quan số lượng và danh sách chi tiết các Client đang online thông qua DataGridView của Server.
*   **Gửi thông báo hệ thống (Broadcast):** Server có thể trực tiếp gửi thông điệp cảnh báo hoặc thông báo chung tới toàn bộ các Client đang trực tuyến.
*   **Giám sát thời gian thực:** Ghi nhật ký (Log) chi tiết mọi hoạt động kết nối, ngắt kết nối, gửi/nhận tin nhắn công khai/riêng tư kèm mốc thời gian rõ ràng.
*   **Tự động dọn dẹp dữ liệu (Auto-purge):** Định kỳ mỗi giờ tự động giải phóng các tin nhắn cũ hơn 7 ngày trong SQLite để tối ưu dung lượng đĩa cứng.

---

## 🛠️ Công nghệ sử dụng (Technology Stack)
*   **Ngôn ngữ lập trình:** C#
*   **Framework giao diện:** Windows Forms (WinForms) với thiết kế hiện đại (Chat Bubbles, Panel lồng ghép mượt mà)
*   **Giao thức truyền thông:** TCP Socket (`System.Net.Sockets`)
*   **Cơ sở dữ liệu:** SQLite (Quản lý tài khoản, lưu trữ lịch sử tin nhắn)
*   **Định dạng dữ liệu:** JSON (Dùng cho cấu hình hoặc dữ liệu tài khoản dự phòng) & Custom Socket Protocols (Định dạng gói lệnh văn bản ngăn cách bằng ký tự đặc biệt).

---

## 📖 Hướng dẫn khởi chạy ứng dụng

### 1. Chuẩn bị môi trường
*   Hệ điều hành Windows (hỗ trợ .NET Windows Forms).
*   Đã cài đặt .NET SDK tương ứng với dự án.

### 2. Khởi chạy Server
1.  Mở mã nguồn dự án trong Visual Studio hoặc VS Code.
2.  Khởi chạy ứng dụng `ChatServer`.
3.  Nhập cổng mạng **Port** mong muốn (Mặc định: `9988`) và mã khóa bảo mật **Key**.
4.  Nhấn nút **"Mở kết nối"**. Lúc này Server sẽ chuyển sang trạng thái lắng nghe (Listen) từ các Client.

### 3. Khởi chạy Client
1.  Khởi chạy ứng dụng `ChatClient`.
2.  Nếu chưa có tài khoản, bấm vào **"Đăng ký"** để tạo mới (hệ thống tự động kích hoạt tài khoản đầu tiên nếu dữ liệu trống).
3.  Đăng nhập bằng tài khoản và mật khẩu đã tạo.
4.  Nhập địa chỉ **IP** của Server (sử dụng `127.0.0.1` nếu chạy trên cùng máy tính), số **Port** và mã khóa **Key** khớp với thông số của Server đang chạy.
5.  Nhấn **"Kết nối"** để bắt đầu tham gia trò chuyện.
