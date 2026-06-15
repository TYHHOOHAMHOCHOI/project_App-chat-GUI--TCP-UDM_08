
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ChatCommon;
namespace ChatServer;

public partial class Form1 : Form
{

    private Socket serverSocket;
    private List<Socket> listClientOnline = new List<Socket>();
    private Dictionary<Socket, string> clientNames = new Dictionary<Socket, string>();
    private bool isRunning = false;
    private MessageRepository? _messageRepo;
    private System.Threading.Timer? _purgeTimer;
    private static readonly TimeSpan DefaultRetention = TimeSpan.FromDays(7);
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        try
        {
            string ipLocal = "127.0.0.1";

            // Lấy danh sách tất cả IP của máy này
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipLocal = ip.ToString();

                    // Ưu tiên chọn IP của mạng LAN
                    if (ipLocal.StartsWith("192.168.") || ipLocal.StartsWith("10.") || ipLocal.StartsWith("172."))
                    {
                        break;
                    }
                }
            }

            // Gán số IP tìm được vào ô TextBox hiển thị trên giao diện Server
            txtAddress.Text = ipLocal;
        }
        catch (Exception)
        {
            // tạm localhost để không bị crash app
            txtAddress.Text = "127.0.0.1";
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {

    }

    private void label1_Click(object sender, EventArgs e)
    {

    }

    private void btnClear_Click(object sender, EventArgs e)
    {

        rtbLog.Clear();
        rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] [Hệ thống] Đã xóa toàn bộ tin nhắn.\r\n");
    }

    private void lblSoClient_Click(object sender, EventArgs e)
    {

    }

    private void btnSend_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtMessage.Text.Trim()))

        {

            MessageBox.Show("Vui lòng nhập tin nhắn trước khi gửi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtMessage.Focus(); // Đưa con trỏ chuột quay lại ô nhập để người dùng gõ luôn
            return;
        }



        if (serverSocket == null || !isRunning)
        {
            MessageBox.Show("Server chưa mở kết nối, không thể gửi tin nhắn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Đọc tên người gửi từ ô Username, nếu trống mặc định là Server
        string senderName = string.IsNullOrEmpty(txtUsername.Text.Trim()) ? "Server" : txtUsername.Text.Trim();

        string timeStamp = DateTime.Now.ToString("HH:mm:ss");
        string msg = $"[{timeStamp}] {senderName}: {txtMessage.Text}";

        rtbLog.AppendText(msg + "\r\n");

        // Gọi hàm phát tin nhắn chung đi
        BroadcastMessage(msg);

        // Lưu tin server broadcast vào database
        try { _messageRepo?.SaveMessage(senderName, null, txtMessage.Text, "server"); } catch { }

        txtMessage.Clear();
        txtMessage.Focus();
    }

    private void BroadcastMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        lock (listClientOnline)
        {
            foreach (Socket client in listClientOnline)
            {
                try
                {
                    if (client != null && client.Connected)
                    {
                        client.Send(data);
                    }
                }
                catch { }
            }
        }
    }

    private void btnOpenServer_Click(object sender, EventArgs e)
    {
        // TRƯỜNG HỢP 1: dừng
        if (isRunning)
        {
            try
            {
                // 1. Ngắt vòng lặp ở luồng ngầm
                isRunning = false;

                // Giải phóng database
                _purgeTimer?.Dispose(); _purgeTimer = null;
                _messageRepo?.Dispose(); _messageRepo = null;

                // 2. Đóng Socket chính của Server
                if (serverSocket != null)
                {
                    if (serverSocket.Connected)
                    {
                        // Dừng việc nhận và gửi dữ liệu ngay lập tức
                        serverSocket.Shutdown(SocketShutdown.Both);
                    }
                    serverSocket.Close();
                }


                lock (listClientOnline)
                {
                    foreach (Socket clientSocket in listClientOnline)
                    {
                        if (clientSocket != null)
                        {
                            try
                            {
                                if (clientSocket.Connected)
                                {
                                    clientSocket.Shutdown(SocketShutdown.Both);
                                }
                                clientSocket.Close();
                            }
                            catch { }
                        }
                    }
                    listClientOnline.Clear();
                }
                lock (clientNames) { clientNames.Clear(); }
                dgvClients.Rows.Clear();

                rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] [Hệ thống] Server đã đóng hoàn toàn và ngừng lắng nghe kết nối.\r\n");
                lblSoClient.Text = "Số client: 0";

                txtMessage.Enabled = false;
                btnDisconectAll.Enabled = false;

                //mở ổ port khi bấm Dừng server
                txtPort.Enabled = true;

                btnOpenServer.Text = "Mở kết nối";
                btnOpenServer.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Server gặp lỗi khi ngắt kết nối: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        // TRƯỜNG HỢP 2: mở server
        else
        {

            if (string.IsNullOrEmpty(txtPort.Text.Trim()))
            {
                MessageBox.Show("Cảnh báo: Ô Port không được để trống! Hệ thống tự động gán Port mặc định là 9050.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPort.Text = "9050";
            }


            //Thay thế dòng int.Parse bằng int.TryParse an toàn
            // Chặn crash nếu người dùng nhập chữ
            if (!int.TryParse(txtPort.Text.Trim(), out int port) || port < 1 || port > 65535)
            {
                MessageBox.Show("Cổng Port nhập vào không hợp lệ!", "Lỗi Nhập Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPort.Focus();
                return;
            }

            try
            {

                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(ipep);
                serverSocket.Listen(10);


                isRunning = true;

                // Khởi tạo database lưu tin nhắn + timer tự dọn tin cũ mỗi 1 giờ
                _messageRepo = new MessageRepository();
                _purgeTimer = new System.Threading.Timer(
                    _ => { try { _messageRepo?.PurgeOlderThan(DefaultRetention); } catch { } },
                    null, TimeSpan.Zero, TimeSpan.FromHours(1));

                rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] [Hệ thống] Server đã mở thành công tại Port: {port}\r\n");
                txtMessage.Enabled = true;
                btnDisconectAll.Enabled = true;


                // Khóa ô nhập port lại khi server đang chạy       
                txtPort.Enabled = false;

                btnOpenServer.Text = "Dừng";
                btnOpenServer.Enabled = true;

                // Khởi chạy luồng ngầm canh cửa đón Client
                Thread threadListen = new Thread(ListenForClients);
                threadListen.IsBackground = true;
                threadListen.Start();
            }
            // Bắt lỗi (Trùng Port)
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
            {
                MessageBox.Show($"Cổng Port {port} hiện đang hoạt động trên máy! Vui lòng đổi số Port khác.", "Lỗi Trùng Port", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không thể mở Socket Server: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    private void ListenForClients()
    {
        while (isRunning)
        {
            try
            {

                Socket clientSocket = serverSocket.Accept();

                // Khi có máy con vào, lưu Socket của nó vào danh sách quản lý
                lock (listClientOnline)
                {
                    listClientOnline.Add(clientSocket);
                }

                string clientEndPoint = clientSocket.RemoteEndPoint?.ToString() ?? "Unknown";

                // Đồng bộ hiển thị lên giao diện RichTextBox và DataGridView một cách an toàn
                this.Invoke((MethodInvoker)delegate
                {
                    //rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] [Hệ thống] Máy con kết nối từ: {clientEndPoint}\r\n");
                    lblSoClient.Text = $"Số client: {listClientOnline.Count}";

                    // Thêm một dòng mới vào bảng DataGridView, gắn Tag = socket để tìm lại
                    //int index = dgvClients.Rows.Add();
                    // dgvClients.Rows[index].Cells["colID"].Value = listClientOnline.Count;
                    //dgvClients.Rows[index].Cells["colName"].Value = clientEndPoint; // Tạm hiện IP, đổi thành Username khi nhận LOGIN
                    //dgvClients.Rows[index].Tag = clientSocket;
                });

                // Khởi chạy luồng ngầm riêng cho client này để phát hiện ngắt kết nối + nhận LOGIN
                Thread threadHandleClient = new Thread(() => HandleClient(clientSocket));
                threadHandleClient.IsBackground = true;
                threadHandleClient.Start();
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show("Máy chủ đã dừng hoạt động. Toàn bộ các kết nối đã được giải phóng thành công!", "Thông Báo : ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });

                //  lệnh break để thoát hẳn vòng lặp while, không bị treo máy!
                break;
            }
        }
    }

    // ===== PHẦN MỚI: Xử lý từng Client riêng biệt =====

    /// <summary>
    /// Luồng ngầm chạy riêng cho mỗi client: nhận lệnh LOGIN và phát hiện ngắt kết nối.
    /// </summary>
    private void HandleClient(Socket clientSocket)
    {
        byte[] buffer = new byte[4096];
        string clientName = "Unknown";
        bool isFirstLogin = true;

        try
        {
            while (isRunning && clientSocket.Connected)
            {
                int received = clientSocket.Receive(buffer);
                if (received <= 0) break;

                string data = Encoding.UTF8.GetString(buffer, 0, received);

                //dung them load tn
                data = data.Trim();

                if (data == "LOAD_PUBLIC")
                {
                    SendPublicHistory(clientSocket);
                    continue;
                }

                if (data.StartsWith("LOAD_PRIVATE:"))
                {
                    string target = data.Substring("LOAD_PRIVATE:".Length);
                    string currentUser = clientNames[clientSocket];
                    //_messageRepo.SaveMessage(sender, receiver, content, "private");
                    SendPrivateHistory(clientSocket, currentUser, target);
                    continue;
                }

                foreach (string line in data.Split('\n', StringSplitOptions.RemoveEmptyEntries))
                {
                    string trimmed = line.Trim();

                    // Trường hợp 1: Nhận gói LOGIN từ máy con
                    if (trimmed.StartsWith("LOGIN:", StringComparison.OrdinalIgnoreCase))
                    {
                        string rawLoginData = trimmed.Substring(6).Trim(); // Cắt bỏ "LOGIN:"

                        string username = rawLoginData;
                        string clientKey = "";

                        // SỬA TẠI ĐÂY: Tách chuỗi theo ký tự '|' để lấy Username và Key bảo mật
                        if (rawLoginData.Contains("|"))
                        {
                            string[] parts = rawLoginData.Split('|');
                            username = parts[0].Trim();
                            if (parts.Length > 1) clientKey = parts[1].Trim();
                        }

                        // Lấy Key đang được cấu hình hiện tại trên giao diện Server công khai
                        string serverKey = "";
                        this.Invoke((MethodInvoker)delegate
                        {
                            serverKey = txtKey.Text.Trim();
                        });

                        // TIẾN HÀNH KIỂM TRA KEY BẢO MẬT
                        if (clientKey != serverKey)
                        {
                            // 1. Gửi gói lệnh ERR_KEY kèm theo dấu ngắt dòng rõ ràng để Client xử lý chuẩn
                            string errorResponse = "ERR_KEY: Mã khóa bảo mật (Key) không chính xác! Vui lòng kiểm tra lại.\n";
                            byte[] errData = Encoding.UTF8.GetBytes(errorResponse);
                            clientSocket.Send(errData);

                            // 2. Ghi log cảnh báo sai Key lên Server
                            this.Invoke((MethodInvoker)delegate
                            {
                                rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] [Cảnh báo] Từ chối kết nối từ IP {((IPEndPoint)clientSocket.RemoteEndPoint).Address} do nhập sai mật mã Key.\r\n");
                            });

                            // 3. Trước khi dứt áo ra đi, đóng Socket ngay tại đây để bên Client lập tức nhận biết và KHÔNG báo "đã kết nối" nữa
                            try
                            {
                                clientSocket.Shutdown(SocketShutdown.Both);
                                clientSocket.Close();
                            }
                            catch { }

                            // 4. Đồng thời xóa Socket này ra khỏi listClientOnline ngay để tránh việc rơi vào khối finally chạy hàm RemoveClient làm rác log "Unknown"
                            lock (listClientOnline)
                            {
                                listClientOnline.Remove(clientSocket);
                            }

                            return; // Thoát hẳn luồng xử lý đơn này luôn
                        }

                        // --- NẾU ĐÚNG KEY, TIẾP TỤC XỬ LÝ ĐĂNG NHẬP NHƯ CŨ ---
                        if (!string.IsNullOrEmpty(username))
                        {
                            clientName = username;
                            lock (clientNames) { clientNames[clientSocket] = username; }

                            if (isFirstLogin)
                            {
                                string okMsg = $"OK: Kết nối thành công!\n";
                                clientSocket.Send(Encoding.UTF8.GetBytes(okMsg));

                                // Copy danh sách ra ngoài lock trước
                                List<string> currentUsers = new List<string>();
                                lock (clientNames)
                                {
                                    foreach (var kv in clientNames)
                                    {
                                        if (kv.Key == clientSocket) continue;
                                        currentUsers.Add(kv.Value);
                                    }
                                }

                                // Gửi sau khi đã thoát khỏi lock
                                foreach (string name in currentUsers)
                                {
                                    string userMsg = $"ONLINE: {name}\n";
                                    clientSocket.Send(Encoding.UTF8.GetBytes(userMsg));
                                }

                                // Thông báo cho tất cả client cũ biết có người mới vào
                                string newUserMsg = $"ONLINE: {username}\n";
                                byte[] newUserData = Encoding.UTF8.GetBytes(newUserMsg);

                                List<Socket> others = new List<Socket>();
                                lock (listClientOnline)
                                {
                                    foreach (Socket other in listClientOnline)
                                    {
                                        if (other != clientSocket) others.Add(other);
                                    }
                                }

                                foreach (Socket other in others)
                                {
                                    try { other.Send(newUserData); } catch { }
                                }


                                string clientIP = ((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString();
                                this.Invoke((MethodInvoker)delegate
                                {
                                    rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] [Hệ thống] {clientName} đã kết nối với IP {clientIP}\r\n");

                                    // Lưu tin hệ thống: kết nối
                                    try { _messageRepo?.SaveMessage("Hệ thống", null, $"{clientName} đã kết nối (IP: {clientIP})", "system"); } catch { }

                                    int index = dgvClients.Rows.Add();
                                    dgvClients.Rows[index].Cells["colName"].Value = clientName;
                                    dgvClients.Rows[index].Tag = clientSocket;

                                    for (int i = 0; i < dgvClients.Rows.Count; i++)
                                    {
                                        dgvClients.Rows[i].Cells["colID"].Value = i + 1;
                                    }
                                });
                                isFirstLogin = false;
                            }
                            else
                            {
                                UpdateClientNameOnGrid(clientSocket, username);
                            }
                        }
                    }
                    // Trường hợp 2: Client chat chung
                    else if (trimmed.StartsWith("PRIVATE:", StringComparison.OrdinalIgnoreCase))
                    {
                        string payload = trimmed.Substring(8);
                        int sep = payload.IndexOf('|');
                        if (sep > 0)
                        {
                            string targetName = payload.Substring(0, sep).Trim();
                            string content = payload.Substring(sep + 1).Trim();
                            string timeStamp = DateTime.Now.ToString("HH:mm:ss");

                            Socket? targetSocket = null;
                            lock (clientNames)
                            {
                                foreach (var kv in clientNames)
                                {
                                    if (kv.Value == targetName)
                                    {
                                        targetSocket = kv.Key;
                                        break;
                                    }
                                }
                            }

                            if (targetSocket != null)
                            {
                                // Người NHẬN thấy rõ tên người gửi
                                string toReceiver = $"PRIVATE_MSG:{clientName}|{timeStamp}|{content}\n";
                                try { targetSocket.Send(Encoding.UTF8.GetBytes(toReceiver)); } catch { }

                                // Người GỬI cũng thấy echo xác nhận
                                string toSender = $"SENT_ACK:{targetName}|{content}\n";
                                try { clientSocket.Send(Encoding.UTF8.GetBytes(toSender)); } catch { }

                                try { _messageRepo?.SaveMessage(clientName, targetName, content, "private"); } catch { }

                                this.Invoke((MethodInvoker)delegate
                                {
                                    rtbLog.AppendText($"[{timeStamp}] [Riêng] {clientName} → {targetName}: {content}\r\n");
                                });
                            }
                            else
                            {
                                string notFound = $"[Hệ thống] {targetName} hiện không online.\n";
                                clientSocket.Send(Encoding.UTF8.GetBytes(notFound));
                            }
                        }
                    }
                    // Trường hợp 2.5: Client trả lời (Reply)
                    else if (trimmed.StartsWith("REPLY_PUBLIC:", StringComparison.OrdinalIgnoreCase))
                    {
                        string payload = trimmed.Substring(13); // bỏ "REPLY_PUBLIC:"
                        string[] parts = payload.Split('|', 3);
                        if (parts.Length == 3)
                        {
                            string targetUser = parts[0].Trim();
                            string targetMsg = parts[1].Trim();
                            string content = parts[2].Trim();
                            string timeStamp = DateTime.Now.ToString("HH:mm:ss");

                            string formattedMsg = $"BROADCAST_REPLY:[{timeStamp}] {clientName}|{targetUser}|{targetMsg}|{content}";

                            this.Invoke((MethodInvoker)delegate
                            {
                                rtbLog.AppendText($"[{timeStamp}] {clientName} (trả lời {targetUser}): {content}\r\n");
                            });

                            BroadcastMessage(formattedMsg + "\n");

                            try { _messageRepo?.SaveMessage(clientName, null, content, "public", targetUser, targetMsg); } catch { }
                        }
                    }
                    else if (trimmed.StartsWith("REPLY_PRIVATE:", StringComparison.OrdinalIgnoreCase))
                    {
                        string payload = trimmed.Substring(14); // bỏ "REPLY_PRIVATE:"
                        string[] parts = payload.Split('|', 4);
                        if (parts.Length == 4)
                        {
                            string targetName = parts[0].Trim();
                            string repliedUser = parts[1].Trim();
                            string repliedMsg = parts[2].Trim();
                            string content = parts[3].Trim();
                            string timeStamp = DateTime.Now.ToString("HH:mm:ss");

                            Socket? targetSocket = null;
                            lock (clientNames)
                            {
                                foreach (var kv in clientNames)
                                {
                                    if (kv.Value == targetName)
                                    {
                                        targetSocket = kv.Key;
                                        break;
                                    }
                                }
                            }

                            if (targetSocket != null)
                            {
                                string toReceiver = $"PRIVATE_REPLY:[{timeStamp}] {clientName}|{repliedUser}|{repliedMsg}|{content}\n";
                                try { targetSocket.Send(Encoding.UTF8.GetBytes(toReceiver)); } catch { }

                                try { _messageRepo?.SaveMessage(clientName, targetName, content, "private", repliedUser, repliedMsg); } catch { }

                                this.Invoke((MethodInvoker)delegate
                                {
                                    rtbLog.AppendText($"[{timeStamp}] [Gửi riêng] {clientName} (trả lời {repliedUser}) -> {targetName}: {content}\r\n");
                                });
                            }
                            else
                            {
                                string notFound = $"[Hệ thống] {targetName} hiện không online.\n";
                                clientSocket.Send(Encoding.UTF8.GetBytes(notFound));
                            }
                        }
                    }
                    // Trường hợp 3: Client chat chung
                    else
                    {
                        if (!string.IsNullOrEmpty(trimmed))
                        {
                            string timeStamp = DateTime.Now.ToString("HH:mm:ss");
                            string formattedMsg = $"[{timeStamp}] {clientName}: {trimmed}";

                            this.Invoke((MethodInvoker)delegate
                            {
                                rtbLog.AppendText(formattedMsg + "\r\n");
                            });

                            BroadcastMessage(formattedMsg + "\n");

                            // Lưu tin nhắn chung vào database
                            try { _messageRepo?.SaveMessage(clientName, null, trimmed, "public"); } catch { }
                        }
                    }
                }
            }
        }
        catch { }
        finally
        {
            RemoveClient(clientSocket, clientName);
        }
    }

    /// <summary>
    /// Cập nhật tên username thật lên DataGridView khi nhận được LOGIN.
    /// </summary>
    private void UpdateClientNameOnGrid(Socket clientSocket, string username)
    {
        if (!this.IsHandleCreated) return;
        try
        {
            this.Invoke((MethodInvoker)delegate
            {
                foreach (DataGridViewRow row in dgvClients.Rows)
                {
                    if (row.Tag == clientSocket)
                    {
                        row.Cells["colName"].Value = username;
                        break;
                    }
                }
                rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] [Hệ thống] Client đã đăng nhập: {username}\r\n");
            });
        }
        catch { }
    }

    /// <summary>
    /// Xóa client khỏi tất cả danh sách, ghi log, cập nhật giao diện.
    /// </summary>
    private void RemoveClient(Socket clientSocket, string clientName)
    {
        bool wasRemoved;
        lock (listClientOnline)
        {
            wasRemoved = listClientOnline.Remove(clientSocket);
        }
        lock (clientNames)
        {
            clientNames.Remove(clientSocket);
        }
        // Phát gói thông báo có người ngắt kết nối cho tất cả client khác (trừ chính nó)
        if (wasRemoved && !string.IsNullOrEmpty(clientName) && clientName != "Unknown")
        {
            string offlineMsg = $"OFFLINE: {clientName}\n";
            byte[] offlineData = Encoding.UTF8.GetBytes(offlineMsg);
            lock (listClientOnline)
            {
                foreach (Socket other in listClientOnline)
                {
                    try { other.Send(offlineData); } catch { }
                }
            }
        }
        try
        {
            if (clientSocket.Connected)
                clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
        catch { }

        // Chỉ cập nhật UI nếu thực sự xóa được (tránh trùng khi server dừng hoặc kick)
        if (!wasRemoved) return;
        if (!this.IsHandleCreated) return;

        try
        {
            this.Invoke((MethodInvoker)delegate
            {
                rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] [Hệ thống] {clientName} đã ngắt kết nối.\r\n");

                // Lưu tin hệ thống: ngắt kết nối
                try { _messageRepo?.SaveMessage("Hệ thống", null, $"{clientName} đã ngắt kết nối.", "system"); } catch { }

                for (int i = dgvClients.Rows.Count - 1; i >= 0; i--)
                {
                    if (dgvClients.Rows[i].Tag == clientSocket)
                    {
                        dgvClients.Rows.RemoveAt(i);
                        break;
                    }
                }

                for (int k = 0; k < dgvClients.Rows.Count; k++)
                {
                    dgvClients.Rows[k].Cells["colID"].Value = k + 1;
                }

                lblSoClient.Text = $"Số client: {listClientOnline.Count}";
            });
        }
        catch { }
    }

    private void btnDisconectAll_Click(object sender, EventArgs e)
    {
        try
        {
            // 1. Duyệt danh sách để đóng kết nối của TỪNG client đang online
            // KHÔNG cho isRunning = false và KHÔNG đóng serverSocket ở đây để Server tiếp tục chạy
            lock (listClientOnline)
            {
                foreach (Socket clientSocket in listClientOnline)
                {
                    if (clientSocket != null)
                    {
                        try
                        {
                            if (clientSocket.Connected)
                            {
                                clientSocket.Shutdown(SocketShutdown.Both);
                            }
                            clientSocket.Close();
                        }
                        catch { }
                    }
                }
                // Xóa sạch danh sách client online trong bộ nhớ bộ quản lý
                listClientOnline.Clear();
            }

            //Xóa sạch danh sách tên Client và xóa các dòng hiển thị trên giao diện bảng
            lock (clientNames) { clientNames.Clear(); }
            dgvClients.Rows.Clear();


            rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] [Hệ thống] Đã ngắt kết nối của toàn bộ Client. Máy chủ vẫn đang tiếp tục hoạt động...\r\n");


            lblSoClient.Text = "Số client: 0";


            btnDisconectAll.Enabled = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khi ngắt kết nối hàng loạt: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void btnIcon_Paint(object sender, PaintEventArgs e)
    {
        //ép về dạng button
        Button btn = (Button)sender;

        // tạo hinh  tròn phù hợp với icon
        System.Drawing.Drawing2D.GraphicsPath nútTròn = new System.Drawing.Drawing2D.GraphicsPath();
        nútTròn.AddEllipse(0, 0, btn.Width, btn.Height);


        // những phần thừa hình vuông bên ngoài tự động gọt bỏ -> hình tròn
        btn.Region = new Region(nútTròn);
    }

    private void chkHideKey_CheckedChanged(object sender, EventArgs e)
    {
        //ép về kiểu checkBox
        CheckBox cb = (CheckBox)sender;


        if (cb.Checked)
        {
            // ẩn biến thành dấu chấm
            txtKey.UseSystemPasswordChar = true;
        }
        else
        {
            //hiện chữ bình thường
            txtKey.UseSystemPasswordChar = false;
        }
    }

    private void txtKey_TextChanged(object sender, EventArgs e)
    {

    }

    private void labelPort_Click(object sender, EventArgs e)
    {

    }

    private void txtPort_TextChanged(object sender, EventArgs e)
    {

    }

    private void button1_Click_1(object sender, EventArgs e)
    {
        // 1. Tạo một Menu ngữ cảnh thả xuống
        ContextMenuStrip emojiMenu = new ContextMenuStrip();

        // 2. Danh sách các icon/emoji bạn muốn cho người dùng chọn
        string[] emojis = { "😀", "😁", "😂", "🤣", "😃", "😄", "😅", "😆", "😉", "😊", "😋", "😎", "😍", "😘", "👍", "👎", "❤️" };

        // 3. Vòng lặp tự động thêm từng Emoji thành một dòng trong Menu
        foreach (string emoji in emojis)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(emoji);

            // Cài đặt cỡ chữ hiển thị icon cho to và rõ ràng hơn (Cỡ 14)
            item.Font = new Font("Segoe UI Emoji", 14);

            // Khi người dùng click chọn vào một icon cụ thể
            item.Click += (s, args) =>
            {
                // Tự động chèn icon được chọn vào vị trí con trỏ đang đứng trong ô Nhập tin nhắn
                txtMessage.AppendText(emoji);
                txtMessage.Focus(); // Giữ con trỏ chuột ở ô nhập để gõ tiếp
            };

            emojiMenu.Items.Add(item);
        }

        // 4. Hiển thị menu xổ xuống ngay sát góc dưới bên trái của nút bấm mặt cười
        Button btn = (Button)sender;
        emojiMenu.Show(btn, new Point(0, btn.Height));
    }

    private void rtbLog_TextChanged(object sender, EventArgs e)
    {

    }

    private void dgvClients_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        // Kiểm tra nếu click vào hàng tiêu đề (Header) thì bỏ qua
        if (e.RowIndex < 0) return;

        try
        {
            // Lấy ra Index dòng vừa click (Dòng 0 tương ứng phần tử số 0 trong List)
            int targetIndex = e.RowIndex;

            // Lấy ID hiển thị để làm thông báo trực quan
            var cellIdValue = dgvClients.Rows[e.RowIndex].Cells["colID"].Value;
            string selectedId = cellIdValue != null ? cellIdValue.ToString() : "Ẩn danh";

            Socket targetClient = null;
            lock (listClientOnline)
            {
                // Kiểm tra an toàn xem vị trí click có khớp với danh sách Online không
                if (targetIndex >= 0 && targetIndex < listClientOnline.Count)
                {
                    targetClient = listClientOnline[targetIndex];
                }
            }

            if (targetClient == null) return;

            // Trường hợp 1: Click cột nút "Disconnect" hoặc "colKick"
            if (dgvClients.Columns[e.ColumnIndex].Name == "colKick" || dgvClients.Columns[e.ColumnIndex].Name == "Disconnect")
            {
                // Lấy tên username thật từ clientNames
                string kickName;
                lock (clientNames)
                {
                    if (!clientNames.TryGetValue(targetClient, out kickName))
                        kickName = selectedId;
                }

                DialogResult res = MessageBox.Show($"Bạn có muốn ngắt kết nối {kickName} không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    // Xóa khỏi danh sách trước khi đóng socket
                    lock (listClientOnline)
                    {
                        listClientOnline.Remove(targetClient);
                    }
                    lock (clientNames)
                    {
                        clientNames.Remove(targetClient);
                    }

                    try { targetClient.Close(); } catch { }

                    dgvClients.Rows.RemoveAt(targetIndex);

                    for (int k = 0; k < dgvClients.Rows.Count; k++)
                    {
                        dgvClients.Rows[k].Cells["colID"].Value = k + 1;
                    }

                    lblSoClient.Text = $"Số client: {listClientOnline.Count}";
                    rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] [Hệ thống] {kickName} đã bị ngắt kết nối (Kick).\r\n");
                }
            }

            // Trường hợp 2: Click cột nút "Gửi tin nhắn" hoặc "colSend"
            if (dgvClients.Columns[e.ColumnIndex].Name == "colSend" || dgvClients.Columns[e.ColumnIndex].Name == "Gửi tin nhắn")
            {
                if (string.IsNullOrEmpty(txtMessage.Text))
                {
                    MessageBox.Show("Hãy gõ nội dung vào ô 'Nhập tin nhắn' trước khi click gửi riêng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string timeStampPriv = DateTime.Now.ToString("HH:mm:ss");
                string privateMsg = $"[{timeStampPriv}] [Gửi riêng] Server -> Bạn: {txtMessage.Text}";
                byte[] data = Encoding.UTF8.GetBytes(privateMsg);
                targetClient.Send(data);

                rtbLog.AppendText($"[{timeStampPriv}] [Gửi riêng] Tới Client {selectedId}: {txtMessage.Text}\r\n");
                txtMessage.Clear();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi tương tác ô dữ liệu bảng: " + ex.Message);
        }
    }

    private void txtUsername_TextChanged(object sender, EventArgs e)
    {

    }

    private void txtMessage_TextChanged(object sender, EventArgs e)
    {

    }

    private void txtMessage_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true; // Chặn tiếng "beep" khó chịu của Windows
            btnSend.PerformClick();    // Kích hoạt lệnh click của nút Gửi
        }
    }

    private void label1_Click_1(object sender, EventArgs e)
    {

    }
    ///dung them load tn
    private void SendPublicHistory(Socket client)
    {
        if (_messageRepo == null)
            return;

        var service = new HistoryService(_messageRepo);

        var list = service.LoadPublic();

        if (list.Count == 0)
        {
            client.Send(
                Encoding.UTF8.GetBytes(
                    "HISTORY_EMPTY\n"));
            return;
        }

        foreach (var msg in list)
        {
            string line;
            if (!string.IsNullOrEmpty(msg.ReplyToUser))
            {
                line = $"HISTORY_REPLY:[{msg.SentAt:HH:mm:ss}] {msg.Sender}|{msg.ReplyToUser}|{msg.ReplyToMessage}|{msg.Content}\n";
            }
            else
            {
                line = $"HISTORY:[{msg.SentAt:HH:mm:ss}] {msg.Sender}: {msg.Content}\n";
            }

            client.Send(
                Encoding.UTF8.GetBytes(line));
        }
    }
    private void SendPrivateHistory(
    Socket client,
    string currentUser,
    string targetUser)
    {
        if (_messageRepo == null)
            return;

        var service = new HistoryService(_messageRepo);

        var list =
            service.LoadPrivate(
                currentUser,
                targetUser);

        if (list.Count == 0)
        {
            client.Send(
                Encoding.UTF8.GetBytes(
                    "HISTORY_EMPTY\n"));
            return;
        }

        foreach (var msg in list)
        {
            string line;
            if (!string.IsNullOrEmpty(msg.ReplyToUser))
            {
                line = $"HISTORY_PRIVATE_REPLY:[{msg.SentAt:HH:mm:ss}] {msg.Sender}->{msg.Receiver}|{msg.ReplyToUser}|{msg.ReplyToMessage}|{msg.Content}\n";
            }
            else
            {
                line = $"HISTORY_PRIVATE:[{msg.SentAt:HH:mm:ss}] {msg.Sender}->{msg.Receiver}: {msg.Content}\n";
            }

            client.Send(
                Encoding.UTF8.GetBytes(line));
        }
    }
    private void btnLoadPublic_Click(object sender, EventArgs e)
    {
        if (_messageRepo == null)
        {
            MessageBox.Show("Server chưa mở!");
            return;
        }

        var list = _messageRepo.GetPublicMessages(200);

        rtbLog.Clear();

        if (list.Count == 0)
        {
            rtbLog.AppendText("Chưa có lịch sử chat chung.\r\n");
            return;
        }

        foreach (var data in list)
        {
            rtbLog.AppendText(
                $"[CHUNG] [{data.SentAt:HH:mm:ss}] {data.Sender}: {data.Content}\r\n");
        }
    }
    private void btnLoadPrivate_Click(object sender, EventArgs e)
    {
        if (_messageRepo == null)
        {
            MessageBox.Show("Server chưa mở!");
            return;
        }

        string keyword = txtSearchUser.Text.Trim();

        var list = _messageRepo.GetPrivateMessages(200);

        if (!string.IsNullOrEmpty(keyword))
        {
            list = list.Where(x =>
                (x.Sender != null && x.Sender.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                (x.Receiver != null && x.Receiver.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        rtbLog.Clear();

        if (list.Count == 0)
        {
            rtbLog.AppendText("Không tìm thấy tin nhắn riêng.\r\n");
            return;
        }

        foreach (var data in list)
        {
            rtbLog.AppendText(
                $"[RIÊNG] [{data.SentAt:HH:mm:ss}] {data.Sender} -> {data.Receiver}: {data.Content}\r\n");
        }
    }

    private void txtAddress_TextChanged(object sender, EventArgs e)
    {

    }
}