using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ChatClient;

public partial class Form1 : Form
{
    // Quản lý kết nối và trạng thái
    private Socket? clientSocket;
    private bool isConnected = false;
    //Quản lý danh sách người dùng online và ánh xạ tên người dùng → ID (dùng để hiển thị trong DataGridView)
    private readonly Dictionary<string, int> _userMap = new();
    private int _nextUserId = 1;
    //biến cờ để biết đang nhắn riêng với ai (nếu có), null = đang nhắn chung
    private string? _privateTarget = null;
    private readonly string _loggedInUser;
    // Cache avatar người dùng để tránh phải gọi lại nhiều lần
    private Dictionary<string, string?> _userAvatars = new();

    public Form1(string loggedInUser)
    {
        // Khởi tạo form và lưu tên người dùng đã đăng nhập
        InitializeComponent();
        _loggedInUser = loggedInUser;
    }
    // Thiết lập giao diện và sự kiện khi form load
    private void Form1_Load(object? sender, EventArgs e)
    {
        // Hiển thị tên người dùng đã đăng nhập và thiết lập trạng thái ban đầu
        txtUsername.Text = _loggedInUser;
        SetConnectedState(false);
        dgvUsers.Columns.Clear();
        dgvUsers.Columns.Add("colID", "ID");
        dgvUsers.Columns.Add("colName", "Name");
        dgvUsers.Columns.Add("colChat", "Tin nhắn");
        dgvUsers.EnableHeadersVisualStyles = false;
        dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 100, 255);
        dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
        dgvUsers.DefaultCellStyle.ForeColor = Color.Black;
        dgvUsers.DefaultCellStyle.BackColor = Color.White;
        dgvUsers.RowsDefaultCellStyle.ForeColor = Color.Black;
        dgvUsers.RowsDefaultCellStyle.BackColor = Color.White;
        dgvUsers.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
        dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
        dgvUsers.RowTemplate.DefaultCellStyle.ForeColor = Color.Black;
        dgvUsers.RowTemplate.DefaultCellStyle.BackColor = Color.White;
        dgvUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
        dgvUsers.DefaultCellStyle.SelectionForeColor = Color.Black;
        conection.Click += conection_Click;
        unconection.Click += unconection_Click;
        LoadUserAvatar();
        pbUserAvatar.Click += pbUserAvatar_Click;
        pbUserAvatar.Cursor = Cursors.Hand;
    }
    private void LoadUserAvatar()
    {
        try
        {
            var avatarBase64 = AccountManager.GetAvatar(_loggedInUser);
            if (!string.IsNullOrEmpty(avatarBase64))
            {
                var avatarImage = AccountManager.ConvertBase64ToImage(avatarBase64);
                if (avatarImage != null)
                {
                    pbUserAvatar.Image = avatarImage;
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading avatar: {ex.Message}");
        }
    }
    private void pbUserAvatar_Click(object? sender, EventArgs e)
    {
        ChangeUserAvatar();
    }
    private void ChangeUserAvatar()
    {
        using var openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";
        openFileDialog.Title = "Chọn ảnh Avatar mới";

        if (openFileDialog.ShowDialog(this) == DialogResult.OK)
        {
            try
            {
                if (AccountManager.SetAvatar(_loggedInUser, openFileDialog.FileName, out var message))
                {
                    LoadUserAvatar();
                    MessageBox.Show(message, "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AppendChat($"[Hệ thống] Avatar đã được cập nhật.", Color.Green);
                }
                else
                {
                    MessageBox.Show(message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    private void btnManageAvatar_Click(object? sender, EventArgs e)
    {
        using var avatarForm = new Frmavatar(_loggedInUser);
        if (avatarForm.ShowDialog(this) == DialogResult.OK)
        {
            // Reload avatar after changes
            LoadUserAvatar();
        }
    }

    private void conection_Click(object? sender, EventArgs e)
    {
        if (isConnected) { MessageBox.Show("Client đã kết nối !"); return; }
        string ip = textBox2.Text.Trim();
        string portStr = textBox3.Text.Trim();
        if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(portStr))
        {
            MessageBox.Show("Vui lòng nhập IP và Port.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (!int.TryParse(portStr, out int port))
        {
            MessageBox.Show("Port không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        try
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            isConnected = true;
            string username = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(username)) username = "Client_An_Danh";
            string clientKey = txtkey.Text.Trim();
            string loginMsg = $"LOGIN: {username}|{clientKey}\n";
            SendRaw(loginMsg);
            Thread recvThread = new Thread(ReceiveLoop) { IsBackground = true };
            recvThread.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Không thể kết nối: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            clientSocket?.Close();
            clientSocket = null;
        }
    }

    private void unconection_Click(object? sender, EventArgs e)
    {
        if (!isConnected) { MessageBox.Show("Client chưa kết nối!"); return; }
        Disconnect("Bạn đã ngắt kết nối.");
    }

    private void Disconnect(string reason)
    {
        if (!isConnected) return;
        isConnected = false;
        try { clientSocket?.Shutdown(SocketShutdown.Both); } catch { }
        clientSocket?.Close();
        clientSocket = null;
        SafeInvoke(() =>
        {
            SetConnectedState(false);
            AppendChat($"[Hệ thống] {reason}");
            ClearUserList();
        });
    }

    private void ReceiveLoop()
    {
        byte[] buffer = new byte[4096];
        while (isConnected && clientSocket != null)
        {
            try
            {
                int received = clientSocket.Receive(buffer);
                if (received == 0) { Disconnect("Server đã đóng kết nối."); break; }
                string msg = Encoding.UTF8.GetString(buffer, 0, received);
                SafeInvoke(() =>
                {
                    foreach (string line in msg.Split('\n', StringSplitOptions.RemoveEmptyEntries))
                    {
                        //duung them load tn
                        if (line.StartsWith("HISTORY:"))
                        {
                            AppendChat(line.Substring(8), Color.Blue);
                            continue;
                        }
                        if (line.StartsWith("HISTORY_PRIVATE:"))
                        {
                            AppendChat(line.Substring(16), Color.DarkViolet);
                            continue;
                        }
                        if (line.Trim() == "HISTORY_EMPTY")
                        {
                            AppendChat("[Hệ thống] Chưa có lịch sử tin nhắn.");
                            continue;
                        }

                        string trimmed = line.TrimEnd('\r');
                        if (string.IsNullOrEmpty(trimmed)) continue;

                        if (trimmed.StartsWith("ERR_KEY:", StringComparison.OrdinalIgnoreCase))
                        {
                            string errMsg = trimmed.Substring(8).Trim();
                            isConnected = false;
                            MessageBox.Show(errMsg, "Lỗi Bảo Mật", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            Disconnect("Kết nối bị từ chối do sai mã khóa (Key).");
                            return;
                        }

                        if (trimmed.StartsWith("OK:", StringComparison.OrdinalIgnoreCase))
                        {
                            SetConnectedState(true);
                            AppendChat($"[Hệ thống] {trimmed.Substring(3).Trim()}");
                            continue;
                        }
                        if (trimmed.StartsWith("ONLINE:", StringComparison.OrdinalIgnoreCase))
                        {
                            string onlineName = trimmed.Substring(7).Trim();
                            if (!_userMap.ContainsKey(onlineName))
                            {
                                _userMap[onlineName] = _nextUserId++;
                                int rowIdx = dgvUsers.Rows.Add();
                                //dgvUsers.Rows[rowIdx].Cells["colID"].Value = _userMap[onlineName];
                                dgvUsers.Rows[rowIdx].Cells["colName"].Value = onlineName;
                                dgvUsers.Rows[rowIdx].Cells["colChat"].Value = "Gửi riêng";

                                for (int i = 0; i < dgvUsers.Rows.Count; i++)
                                {
                                    dgvUsers.Rows[i].Cells["colID"].Value = i + 1;
                                }
                            }
                            continue;
                        }

                        if (trimmed.StartsWith("OFFLINE:", StringComparison.OrdinalIgnoreCase))
                        {
                            string offlineName = trimmed.Substring(8).Trim();
                            if (_userMap.ContainsKey(offlineName))
                            {
                                _userMap.Remove(offlineName);
                                foreach (DataGridViewRow row in dgvUsers.Rows)
                                {
                                    if (row.Cells["colName"].Value?.ToString() == offlineName)
                                    {
                                        dgvUsers.Rows.Remove(row);
                                        break;
                                    }
                                }

                                for (int k = 0; k < dgvUsers.Rows.Count; k++)
                                {
                                    dgvUsers.Rows[k].Cells["colID"].Value = k + 1;
                                }
                            }
                            // Nếu đang nhắn riêng với người vừa offline → tự động thoát chế độ riêng
                            if (_privateTarget == offlineName)
                            {
                                _privateTarget = null;
                                lblLoggedIn.Text = $"Đã đăng nhập: {txtUsername.Text.Trim()}";
                                AppendChat($"[Hệ thống] {offlineName} đã offline. Đã chuyển về chế độ gửi chung.", Color.OrangeRed);
                            }
                            continue;
                        }
                        if (trimmed.StartsWith("SENT_ACK:"))
                        {
                            // Tùy chọn: parse và hiển thị nhẹ nhàng hơn, hoặc đơn giản là bỏ qua
                            continue; // hoặc return
                        }
                        if (trimmed.StartsWith("PRIVATE_MSG:"))
                        {
                            string payload = trimmed.Substring("PRIVATE_MSG:".Length);
                            string[] parts = payload.Split('|');
                            if (parts.Length >= 3)
                            {
                                string privateSender = parts[0];   // ← đổi tên
                                string privateContent = parts[2];  // ← đổi tên

                                string? privateAvatar = null;
                                if (_userAvatars.TryGetValue(privateSender, out var cached))
                                    privateAvatar = cached;
                                else
                                {
                                    try
                                    {
                                        privateAvatar = AccountManager.GetAvatar(privateSender);
                                        if (!string.IsNullOrEmpty(privateAvatar))
                                            _userAvatars[privateSender] = privateAvatar;
                                    }
                                    catch { }
                                }

                                AppendChatWithAvatar(privateSender, $"[Riêng] {privateContent}", false, privateAvatar);
                            }
                            continue;
                        }

                        // ── TIN NHẮN THƯỜNG (chung hoặc riêng từ người khác gửi đến) ──
                        // Parse sender name from message format "[HH:mm:ss] [SenderName]: message"
                        string senderName = ExtractSenderFromMessage(trimmed);
                        string messageText = ExtractMessageContent(trimmed);

                        // Skip if this is our own message (already displayed in btnSend_Click)
                        if (senderName == _loggedInUser)
                        {
                            // This is our own message echoed back from server, skip it
                            continue;
                        }

                        // Get sender's avatar if available
                        string? senderAvatarBase64 = null;
                        if (!string.IsNullOrEmpty(senderName) && senderName != "[Hệ thống]")
                        {
                            // Try to get from cache first
                            if (_userAvatars.TryGetValue(senderName, out var cachedAvatar))
                            {
                                senderAvatarBase64 = cachedAvatar;
                            }
                            else
                            {
                                // Try to get from AccountManager (if available on client)
                                try
                                {
                                    senderAvatarBase64 = AccountManager.GetAvatar(senderName);
                                    if (!string.IsNullOrEmpty(senderAvatarBase64))
                                    {
                                        _userAvatars[senderName] = senderAvatarBase64;
                                    }
                                }
                                catch
                                {
                                    // Avatar not available
                                }
                            }
                        }

                        // Display as bubble (incoming message = left-aligned)
                        AppendChatWithAvatar(senderName, messageText, false, senderAvatarBase64);
                        try { TryRegisterUserFromMessage(trimmed); } catch { }
                    }
                });
            }
            catch
            {
                if (isConnected) Disconnect("Mất kết nối với server.");
                break;
            }
        }
    }

    private void btnSend_Click(object? sender, EventArgs e)
    {
        string text = txtMessage.Text.Trim();
        if (string.IsNullOrEmpty(text)) { txtMessage.Focus(); return; }
        if (!isConnected || clientSocket == null)
        {
            MessageBox.Show("Chưa kết nối tới server!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        //string timeStamp = DateTime.Now.ToString("HH:mm:ss"); // biến đang dư thừa vì đã có messagebubble.cs

        // Get current user's avatar
        var userAvatarBase64 = AccountManager.GetAvatar(_loggedInUser);

        if (_privateTarget != null)
        {
            // Gửi protocol PRIVATE: lên Server để Server định tuyến đúng người nhận
            SendRaw($"PRIVATE:{_privateTarget}|{text}\n");
            // Hiển thị bubble của tin nhắn của bạn gửi (bên phải với avatar)
            AppendChatWithAvatar(_loggedInUser, $"[Gửi riêng tới {_privateTarget}] {text}", true, userAvatarBase64);
        }
        else
        {
            SendRaw($"{text}\n");
            // Hiển thị bubble của tin nhắn của bạn gửi (bên phải với avatar)
            AppendChatWithAvatar(_loggedInUser, text, true, userAvatarBase64);
        }

        txtMessage.Clear();
        txtMessage.Focus();
    }

    private void SendRaw(string message)
    {
        if (clientSocket == null || !isConnected) return;
        try { clientSocket.Send(Encoding.UTF8.GetBytes(message)); }
        catch (Exception ex) { AppendChat($"[Lỗi gửi] {ex.Message}"); }
    }

    private void btnEmoji_Click(object? sender, EventArgs e)
    {
        ContextMenuStrip menu = new ContextMenuStrip();
        string[] emojis = { "😀", "😁", "😂", "🤣", "😃", "😄", "😅", "😆", "😉", "😊", "😋", "😎", "😍", "😘", "👍", "👎", "❤️", "🔥", "🎉", "🥺", "😭", "😤", "🙏", "💯" };
        foreach (string emoji in emojis)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(emoji) { Font = new Font("Segoe UI Emoji", 14) };
            string cap = emoji;
            item.Click += (s, args) => { txtMessage.AppendText(cap); txtMessage.Focus(); };
            menu.Items.Add(item);
        }
        if (sender is Button btn) menu.Show(btn, new Point(0, btn.Height));
    }

    private void TryRegisterUserFromMessage(string line)
    {
        if (line.Contains("[Hệ thống]") || line.Contains("[Gửi riêng]")) return;
        int bracketEnd = line.IndexOf(']');
        if (bracketEnd < 0) return;
        string afterBracket = line.Substring(bracketEnd + 1).TrimStart();
        int colonIdx = afterBracket.IndexOf(':');
        if (colonIdx <= 0) return;
        string senderName = afterBracket.Substring(0, colonIdx).Trim();
        if (string.IsNullOrEmpty(senderName) || senderName == txtUsername.Text.Trim()) return;
        if (!_userMap.ContainsKey(senderName))
        {
            _userMap[senderName] = _nextUserId++;
            int rowIdx = dgvUsers.Rows.Add();
            dgvUsers.Rows[rowIdx].Cells["colID"].Value = _userMap[senderName];
            dgvUsers.Rows[rowIdx].Cells["colName"].Value = senderName;
            dgvUsers.Rows[rowIdx].Cells["colChat"].Value = "Gửi riêng";
        }
    }

    private void dgvUsers_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (dgvUsers.Columns[e.ColumnIndex].Name == "colChat")
        {
            var cell = dgvUsers.Rows[e.RowIndex].Cells["colName"];
            if (cell.Value == null) return;
            string targetName = cell.Value.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(targetName)) return;
            if (_privateTarget == targetName)
            {
                _privateTarget = null;
                dgvUsers.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                lblLoggedIn.Text = $"Đã đăng nhập: {txtUsername.Text.Trim()}";
                AppendChat("[Hệ thống] Đã chuyển sang chế độ gửi chung.");
            }
            else
            {
                foreach (DataGridViewRow row in dgvUsers.Rows) row.DefaultCellStyle.BackColor = Color.White;
                _privateTarget = targetName;
                dgvUsers.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
                lblLoggedIn.Text = $"Đang gửi riêng tới: {targetName}";
                AppendChat($"[Hệ thống] Đang gửi riêng tới [{targetName}]");
            }
        }
    }

    private void dgvUsers_CellContentClick(object? sender, DataGridViewCellEventArgs e) { }

    private void ClearUserList()
    {
        dgvUsers.Rows.Clear();
        _userMap.Clear();
        _nextUserId = 1;
        _privateTarget = null;
        chatBubblePanel.ClearMessages();
        _userAvatars.Clear();
    }

    private void btnLogout_Click(object? sender, EventArgs e)
    {
        DialogResult res = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (res != DialogResult.Yes) return;
        Disconnect("Đã đăng xuất.");
        Hide();
        var loginForm = new Frmlogin();
        if (loginForm.ShowDialog() == DialogResult.OK)
        {
            txtUsername.Text = loginForm.LoggedInUser ?? "Ẩn danh";
            Show();
            lblLoggedIn.Text = $"Đã đăng nhập: {txtUsername.Text.Trim()}";
        }
        else { Application.Exit(); }
    }

    private void AppendChat(string text, Color? color = null)
    {
        if (chatBubblePanel.InvokeRequired) { SafeInvoke(() => AppendChat(text, color)); return; }

        // For system messages and special cases, just add text
        chatBubblePanel.AddMessage("[Hệ thống]", text, DateTime.Now, false);
    }

    /// <summary>Append chat message with sender's avatar</summary>
    private void AppendChatWithAvatar(string senderName, string messageText, bool isOwnMessage, string? avatarBase64 = null)
    {
        if (chatBubblePanel.InvokeRequired)
        {
            SafeInvoke(() => AppendChatWithAvatar(senderName, messageText, isOwnMessage, avatarBase64));
            return;
        }

        chatBubblePanel.AddMessage(senderName, messageText, DateTime.Now, isOwnMessage, avatarBase64);

        // Store avatar for later reference
        if (!string.IsNullOrEmpty(avatarBase64))
        {
            _userAvatars[senderName] = avatarBase64;
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    // NÚT NHẮN CHUNG
    // ════════════════════════════════════════════════════════════════════════
    private void btnPublic_Click_1(object? sender, EventArgs e)
    {
        if (_privateTarget == null)
        {
            AppendChat("[Hệ thống] Bạn đang ở chế độ nhắn chung rồi.");
            return;
        }

        // Reset màu dòng đang được chọn trong dgvUsers
        foreach (DataGridViewRow row in dgvUsers.Rows)
            row.DefaultCellStyle.BackColor = Color.White;

        _privateTarget = null;
        lblLoggedIn.Text = $"Đã đăng nhập: {txtUsername.Text.Trim()}";
        AppendChat("[Hệ thống] Đã chuyển sang chế độ nhắn chung.");

    }
    //dung them load tn
    private void btnLoadHistory_Click(object sender, EventArgs e)
    {
        if (!isConnected)
        {
            MessageBox.Show(
                "Chưa kết nối Server");
            return;
        }

        if (_privateTarget == null)
        {
            SendRaw("LOAD_PUBLIC\n");
        }
        else
        {
            SendRaw(
                $"LOAD_PRIVATE:{_privateTarget}\n");
        }
    }

    private void SetConnectedState(bool connected)
    {
        conection.Enabled = !connected;
        unconection.Enabled = connected;
        btnSend.Enabled = connected;
        btnEmoji.Enabled = connected;
        txtMessage.Enabled = connected;
        btnPublic.Enabled = connected;
        lblLoggedIn.Text = connected ? $"Đã đăng nhập: {txtUsername.Text.Trim()}" : "Chưa kết nối";
    }

    private void SafeInvoke(Action action)
    {
        if (IsDisposed) return;
        if (InvokeRequired) Invoke(action); else action();
    }

    /// <summary>Extracts sender name from message format "[HH:mm:ss] [SenderName]: message"</summary>
    private string ExtractSenderFromMessage(string message)
    {
        // Format: "[HH:mm:ss] [SenderName]: message" or "[HH:mm:ss] SenderName: message"
        try
        {
            // Remove timestamp at the beginning "[HH:mm:ss]"
            int firstBracketEnd = message.IndexOf(']');
            if (firstBracketEnd < 0) return "Unknown";

            string afterTimestamp = message.Substring(firstBracketEnd + 1).TrimStart();

            // Try to extract bracketed sender: "[SenderName]"
            if (afterTimestamp.StartsWith("["))
            {
                int closeBracket = afterTimestamp.IndexOf(']');
                if (closeBracket > 0)
                {
                    return afterTimestamp.Substring(1, closeBracket - 1).Trim();
                }
            }

            // Or extract unbracketed sender: "SenderName:"
            int colonIdx = afterTimestamp.IndexOf(':');
            if (colonIdx > 0)
            {
                return afterTimestamp.Substring(0, colonIdx).Trim();
            }

            return "Unknown";
        }
        catch
        {
            return "Unknown";
        }
    }

    /// <summary>Extracts message content from message format "[HH:mm:ss] [SenderName]: message"</summary>
    private string ExtractMessageContent(string message)
    {
        // Format: "[HH:mm:ss] [SenderName]: message" or "[HH:mm:ss] SenderName: message"
        try
        {
            // Remove timestamp "[HH:mm:ss]"
            int firstBracketEnd = message.IndexOf(']');
            if (firstBracketEnd < 0) return message;

            string afterTimestamp = message.Substring(firstBracketEnd + 1).TrimStart();

            // Try bracketed format: "[SenderName]: message"
            if (afterTimestamp.StartsWith("["))
            {
                int closeBracket = afterTimestamp.IndexOf(']');
                if (closeBracket > 0)
                {
                    int colonIdx = afterTimestamp.IndexOf(':', closeBracket);
                    if (colonIdx >= 0)
                    {
                        return afterTimestamp.Substring(colonIdx + 1).TrimStart();
                    }
                }
            }

            // Try unbracketed format: "SenderName: message"
            int colonIdx2 = afterTimestamp.IndexOf(':');
            if (colonIdx2 > 0)
            {
                return afterTimestamp.Substring(colonIdx2 + 1).TrimStart();
            }

            return afterTimestamp;
        }
        catch
        {
            return message;
        }
    }

    private void txtkey_TextChanged(object? sender, EventArgs e) { }
    private void txtUsername_TextChanged(object? sender, EventArgs e) { }
    private void label1_Click(object? sender, EventArgs e) { }
    private void headerPanel_Paint(object sender, PaintEventArgs e) { }


}

//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using System.Windows.Forms;

//namespace ChatClient;

//public partial class Form1 : Form
//{
//    // KHAI BÁO CÁC BIẾN TOÀN CỤC (FIELDS)
//    private Socket? clientSocket;
//    private bool isConnected = false; 
//    private readonly Dictionary<string, int> _userMap = new(); 
//    private int _nextUserId = 1;
//    private string? _privateTarget = null; 
//    private readonly string _loggedInUser;
//    private Dictionary<string, string?> _userAvatars = new(); // Cache lưu mã Base64 avatar của mọi người để tránh load lại nhiều lần

//    // KHỞI TẠO FORM VÀ GIAO DIỆN
//    public Form1(string loggedInUser)
//    {
//        InitializeComponent();
//        _loggedInUser = loggedInUser;
//    }

//    private void Form1_Load(object? sender, EventArgs e)
//    {
//        txtUsername.Text = _loggedInUser;
//        SetConnectedState(false); // Ban đầu chưa kết nối -> khóa các nút gửi tin

//        //Thiết lập các cột và giao diện cho bảng danh sách User Onlinne
//        dgvUsers.Columns.Clear();
//        dgvUsers.Columns.Add("colID", "ID");
//        dgvUsers.Columns.Add("colName", "Name");
//        dgvUsers.Columns.Add("colChat", "Tin nhắn");

//        //Định dạng màu sắc, viền, font
//        dgvUsers.EnableHeadersVisualStyles = false;
//        dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 100, 255);
//        dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
//        dgvUsers.DefaultCellStyle.ForeColor = Color.Black;
//        dgvUsers.DefaultCellStyle.BackColor = Color.White;
//        dgvUsers.RowsDefaultCellStyle.ForeColor = Color.Black;
//        dgvUsers.RowsDefaultCellStyle.BackColor = Color.White;
//        dgvUsers.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
//        dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
//        dgvUsers.RowTemplate.DefaultCellStyle.ForeColor = Color.Black;
//        dgvUsers.RowTemplate.DefaultCellStyle.BackColor = Color.White;
//        dgvUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
//        dgvUsers.DefaultCellStyle.SelectionForeColor = Color.Black;

//        //Gán sự kiện cho nút Kết nối và Ngắt kết nối
//        conection.Click += conection_Click;
//        unconection.Click += unconection_Click;

//        //Tải ảnh đại diện của bản thân và gán sự kiện click để đổi ảnh
//        LoadUserAvatar();
//        pbUserAvatar.Click += pbUserAvatar_Click;
//        pbUserAvatar.Cursor = Cursors.Hand;
//    }
//    //XỬ LÝ AVATAR (ẢNH ĐẠI DIỆN)

//    //Tải và hiển thị ảnh đại diện của tài khoản đang đăng nhập
//    private void LoadUserAvatar()
//    {
//        try
//        {
//            // Lấy chuỗi Base64 từ hệ thống quản lý tài khoản
//            var avatarBase64 = AccountManager.GetAvatar(_loggedInUser);
//            if (!string.IsNullOrEmpty(avatarBase64))
//            {
//                // Chuyển Base64 thành đối tượng Image của WinForms
//                var avatarImage = AccountManager.ConvertBase64ToImage(avatarBase64);
//                if (avatarImage != null)
//                {
//                    pbUserAvatar.Image = avatarImage;
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            // Nếu có lỗi (ví dụ file hỏng) thì chỉ ghi log ẩn, không làm crash app
//            System.Diagnostics.Debug.WriteLine($"Error loading avatar: {ex.Message}");
//        }
//    }

//    //click vào ảnh đại diện trên giao diện
//    private void pbUserAvatar_Click(object? sender, EventArgs e)
//    {
//        ChangeUserAvatar();
//    }

//    //hộp thoại chọn file ảnh để đổi Avatar
//    private void ChangeUserAvatar()
//    {
//        using var openFileDialog = new OpenFileDialog();
//        openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*"; // Chỉ cho chọn file ảnh
//        openFileDialog.Title = "Chọn ảnh Avatar mới";

//        if (openFileDialog.ShowDialog(this) == DialogResult.OK)
//        {
//            try
//            {
//                // Lưu avatar mới qua AccountManager
//                if (AccountManager.SetAvatar(_loggedInUser, openFileDialog.FileName, out var message))
//                {
//                    LoadUserAvatar(); // Tải lại ảnh lên màn hình
//                    MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                    AppendChat($"[Hệ thống] Avatar đã được cập nhật.", Color.Green);
//                }
//                else
//                {
//                    MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }
//    }

//    //Mở Form Quản lý Avatar (nếu có form phụ)
//    private void btnManageAvatar_Click(object? sender, EventArgs e)
//    {
//        using var avatarForm = new Frmavatar(_loggedInUser);
//        if (avatarForm.ShowDialog(this) == DialogResult.OK)
//        {
//            LoadUserAvatar(); // Tải lại ảnh nếu có thay đổi từ form kia
//        }
//    }
//    // KHỐI KẾT NỐI VÀ NGẮT KẾT NỐI MẠNG
//    private void conection_Click(object? sender, EventArgs e)
//    {
//        if (isConnected) { MessageBox.Show("Client đã kết nối !"); return; }

//        // Lấy IP và Port từ textbox
//        string ip = textBox2.Text.Trim();
//        string portStr = textBox3.Text.Trim();

//        if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(portStr))
//        {
//            MessageBox.Show("Vui lòng nhập IP và Port.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//            return;
//        }
//        if (!int.TryParse(portStr, out int port))
//        {
//            MessageBox.Show("Port không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            return;
//        }
//        try
//        {
//            // Khởi tạo Socket TCP/IPv4
//            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//            clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
//            isConnected = true;

//            // Chuẩn bị thông tin đăng nhập (Username và Key bảo mật)
//            string username = txtUsername.Text.Trim();
//            if (string.IsNullOrEmpty(username)) username = "Client_An_Danh";
//            string clientKey = txtkey.Text.Trim();

//            // Gửi gói tin LOGIN lên Server
//            string loginMsg = $"LOGIN: {username}|{clientKey}\n";
//            SendRaw(loginMsg);

//            // Khởi chạy 1 Luồng (Thread) chạy ngầm chuyên để nghe tin nhắn Server trả về
//            Thread recvThread = new Thread(ReceiveLoop) { IsBackground = true };
//            recvThread.Start();
//        }
//        catch (Exception ex)
//        {
//            MessageBox.Show($"Không thể kết nối: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            clientSocket?.Close();
//            clientSocket = null;
//        }
//    }

//    private void unconection_Click(object? sender, EventArgs e)
//    {
//        if (!isConnected) { MessageBox.Show("Client chưa kết nối!"); return; }
//        Disconnect("Bạn đã ngắt kết nối.");
//    }

//    //Hàm dùng chung để dọn dẹp, ngắt mạng an toàn
//    private void Disconnect(string reason)
//    {
//        if (!isConnected) return;
//        isConnected = false;
//        try { clientSocket?.Shutdown(SocketShutdown.Both); } catch { } // Dừng truyền/nhận
//        clientSocket?.Close(); // Đóng hoàn toàn Socket
//        clientSocket = null;

//        // Cập nhật lại giao diện (Khóa nút, thông báo, xóa danh sách online)
//        SafeInvoke(() =>
//        {
//            SetConnectedState(false);
//            AppendChat($"[Hệ thống] {reason}");
//            ClearUserList();
//        });
//    }
//    // ĐỢI NHẬN DỮ LIỆU TỪ SERVER 

//    private void ReceiveLoop()
//    {
//        byte[] buffer = new byte[4096]; // Bộ đệm 4KB
//        while (isConnected && clientSocket != null)
//        {
//            try
//            {
//                int received = clientSocket.Receive(buffer);
//                if (received == 0) { Disconnect("Server đã đóng kết nối."); break; }

//                string msg = Encoding.UTF8.GetString(buffer, 0, received); // Dịch byte thành chuỗi UTF8

//                // SafeInvoke bắt buộc dùng để chuyển dữ liệu từ Thread mạng về Thread Giao diện (UI)
//                SafeInvoke(() =>
//                {
//                    // Tách tin nhắn theo dấu \n (xuống dòng) vì TCP có thể ghép nhiều gói tin
//                    foreach (string line in msg.Split('\n', StringSplitOptions.RemoveEmptyEntries))
//                    {
//                        // 1. Nhận Lịch sử tin nhắn
//                        if (line.StartsWith("HISTORY:"))
//                        {
//                            AppendChat(line.Substring(8), Color.Blue);
//                            continue;
//                        }
//                        if (line.StartsWith("HISTORY_PRIVATE:"))
//                        {
//                            AppendChat(line.Substring(16), Color.DarkViolet);
//                            continue;
//                        }
//                        if (line.Trim() == "HISTORY_EMPTY")
//                        {
//                            AppendChat("[Hệ thống] Chưa có lịch sử tin nhắn.");
//                            continue;
//                        }

//                        string trimmed = line.TrimEnd('\r');
//                        if (string.IsNullOrEmpty(trimmed)) continue;

//                        // 2. Kiểm tra lỗi sai Key bảo mật
//                        if (trimmed.StartsWith("ERR_KEY:", StringComparison.OrdinalIgnoreCase))
//                        {
//                            string errMsg = trimmed.Substring(8).Trim();
//                            isConnected = false;
//                            MessageBox.Show(errMsg, "Lỗi Bảo Mật", MessageBoxButtons.OK, MessageBoxIcon.Stop);
//                            Disconnect("Kết nối bị từ chối do sai mã khóa (Key).");
//                            return;
//                        }

//                        // 3. Đăng nhập thành công
//                        if (trimmed.StartsWith("OK:", StringComparison.OrdinalIgnoreCase))
//                        {
//                            SetConnectedState(true);
//                            AppendChat($"[Hệ thống] {trimmed.Substring(3).Trim()}");
//                            continue;
//                        }

//                        // 4. Có người mới đăng nhập (Cập nhật bảng DataGridView)
//                        if (trimmed.StartsWith("ONLINE:", StringComparison.OrdinalIgnoreCase))
//                        {
//                            string onlineName = trimmed.Substring(7).Trim();
//                            if (!_userMap.ContainsKey(onlineName))
//                            {
//                                _userMap[onlineName] = _nextUserId++;
//                                int rowIdx = dgvUsers.Rows.Add();
//                                dgvUsers.Rows[rowIdx].Cells["colName"].Value = onlineName;
//                                dgvUsers.Rows[rowIdx].Cells["colChat"].Value = "Gửi riêng";

//                                // Đánh lại số thứ tự ID cho đẹp
//                                for (int i = 0; i < dgvUsers.Rows.Count; i++)
//                                {
//                                    dgvUsers.Rows[i].Cells["colID"].Value = i + 1;
//                                }
//                            }
//                            continue;
//                        }

//                        // 5. Có người thoát (Xóa khỏi bảng DataGridView)
//                        if (trimmed.StartsWith("OFFLINE:", StringComparison.OrdinalIgnoreCase))
//                        {
//                            string offlineName = trimmed.Substring(8).Trim();
//                            if (_userMap.ContainsKey(offlineName))
//                            {
//                                _userMap.Remove(offlineName);
//                                foreach (DataGridViewRow row in dgvUsers.Rows)
//                                {
//                                    if (row.Cells["colName"].Value?.ToString() == offlineName)
//                                    {
//                                        dgvUsers.Rows.Remove(row);
//                                        break;
//                                    }
//                                }
//                                for (int k = 0; k < dgvUsers.Rows.Count; k++)
//                                {
//                                    dgvUsers.Rows[k].Cells["colID"].Value = k + 1;
//                                }
//                            }

//                            // Nếu người vừa thoát là người mình đang nhắn riêng -> Thoát chế độ riêng
//                            if (_privateTarget == offlineName)
//                            {
//                                _privateTarget = null;
//                                lblLoggedIn.Text = $"Đã đăng nhập: {txtUsername.Text.Trim()}";
//                                AppendChat($"[Hệ thống] {offlineName} đã offline. Đã chuyển về chế độ gửi chung.", Color.OrangeRed);
//                            }
//                            continue;
//                        }

//                        //TIN NHẮN THƯỜNG (Từ người khác gửi đến)

//                        // Cắt chuỗi để lấy ra tên người gửi và nội dung
//                        string senderName = ExtractSenderFromMessage(trimmed);
//                        string messageText = ExtractMessageContent(trimmed);

//                        // Bỏ qua nếu là tin nhắn do chính mình gửi (vì đã hiển thị lúc bấm nút Gửi rồi)
//                        if (senderName == _loggedInUser)
//                        {
//                            continue;
//                        }

//                        // Lấy ảnh Avatar của người gửi
//                        string? senderAvatarBase64 = null;
//                        if (!string.IsNullOrEmpty(senderName) && senderName != "[Hệ thống]")
//                        {
//                            // Ưu tiên lấy từ cache (RAM) trước cho nhanh
//                            if (_userAvatars.TryGetValue(senderName, out var cachedAvatar))
//                            {
//                                senderAvatarBase64 = cachedAvatar;
//                            }
//                            else
//                            {
//                                // Nếu chưa có trong cache thì gọi AccountManager để lấy
//                                try
//                                {
//                                    senderAvatarBase64 = AccountManager.GetAvatar(senderName);
//                                    if (!string.IsNullOrEmpty(senderAvatarBase64))
//                                    {
//                                        _userAvatars[senderName] = senderAvatarBase64; // Lưu lại vào cache
//                                    }
//                                }
//                                catch { /* Bỏ qua nếu không lấy được avatar */ }
//                            }
//                        }

//                        // Hiển thị bong bóng chat của người đó lên màn hình
//                        AppendChatWithAvatar(senderName, messageText, false, senderAvatarBase64);
//                        try { TryRegisterUserFromMessage(trimmed); } catch { }
//                    }
//                });
//            }
//            catch
//            {
//                if (isConnected) Disconnect("Mất kết nối với server.");
//                break; // Thoát vòng lặp luồng nếu mất mạng
//            }
//        }
//    }
//    // GỬI TIN NHẮN 
//    private void btnSend_Click(object? sender, EventArgs e)
//    {
//        string text = txtMessage.Text.Trim();
//        if (string.IsNullOrEmpty(text)) { txtMessage.Focus(); return; } // Khung chat rỗng thì không làm gì

//        if (!isConnected || clientSocket == null)
//        {
//            MessageBox.Show("Chưa kết nối tới server!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//            return;
//        }

//        var userAvatarBase64 = AccountManager.GetAvatar(_loggedInUser);

//        // NẾU ĐANG CHỌN NGƯỜI NHẮN RIÊNG
//        if (_privateTarget != null)
//        {
//            // Đóng gói theo chuẩn Protocol: PRIVATE:Tên|Nội_Dung
//            SendRaw($"PRIVATE:{_privateTarget}|{text}\n");
//            // Tự vẽ bong bóng chat của mình ở bên phải (isOwnMessage = true)
//            AppendChatWithAvatar(_loggedInUser, $"[Gửi riêng tới {_privateTarget}] {text}", true, userAvatarBase64);
//        }
//        // NẾU ĐANG NHẮN CHUNG (PUBLIC)
//        else
//        {
//            SendRaw($"{text}\n");
//            // Tự vẽ bong bóng chat của mình ở bên phải
//            AppendChatWithAvatar(_loggedInUser, text, true, userAvatarBase64);
//        }

//        txtMessage.Clear(); // Xóa khung nhập liệu
//        txtMessage.Focus();
//    }

//    /// Hàm nén chuỗi thành mảng byte và đẩy vào Socket
//    private void SendRaw(string message)
//    {
//        if (clientSocket == null || !isConnected) return;
//        try { clientSocket.Send(Encoding.UTF8.GetBytes(message)); }
//        catch (Exception ex) { AppendChat($"[Lỗi gửi] {ex.Message}"); }
//    }

//    // KHỐI GIAO DIỆN & TÍNH NĂNG PHỤ
//    //Sự kiện mở Menu chứa icon Emoji
//    private void btnEmoji_Click(object? sender, EventArgs e)
//    {
//        ContextMenuStrip menu = new ContextMenuStrip();
//        string[] emojis = { "😀", "😁", "😂", "🤣", "😃", "😄", "😅", "😆", "😉", "😊", "😋", "😎", "😍", "😘", "👍", "👎", "❤️", "🔥", "🎉", "🥺", "😭", "😤", "🙏", "💯" };
//        foreach (string emoji in emojis)
//        {
//            ToolStripMenuItem item = new ToolStripMenuItem(emoji) { Font = new Font("Segoe UI Emoji", 14) };
//            string cap = emoji;
//            // Khi bấm vào Emoji -> Chèn vào textbox nhập liệu
//            item.Click += (s, args) => { txtMessage.AppendText(cap); txtMessage.Focus(); };
//            menu.Items.Add(item);
//        }
//        if (sender is Button btn) menu.Show(btn, new Point(0, btn.Height));
//    }

//    //Hỗ trợ bắt thông tin user nếu lỡ Server chưa gửi tín hiệu ONLINE
//    private void TryRegisterUserFromMessage(string line)
//    {
//        if (line.Contains("[Hệ thống]") || line.Contains("[Gửi riêng]")) return;
//        int bracketEnd = line.IndexOf(']');
//        if (bracketEnd < 0) return;
//        string afterBracket = line.Substring(bracketEnd + 1).TrimStart();
//        int colonIdx = afterBracket.IndexOf(':');
//        if (colonIdx <= 0) return;
//        string senderName = afterBracket.Substring(0, colonIdx).Trim();
//        if (string.IsNullOrEmpty(senderName) || senderName == txtUsername.Text.Trim()) return;
//        if (!_userMap.ContainsKey(senderName))
//        {
//            _userMap[senderName] = _nextUserId++;
//            int rowIdx = dgvUsers.Rows.Add();
//            dgvUsers.Rows[rowIdx].Cells["colID"].Value = _userMap[senderName];
//            dgvUsers.Rows[rowIdx].Cells["colName"].Value = senderName;
//            dgvUsers.Rows[rowIdx].Cells["colChat"].Value = "Gửi riêng";
//        }
//    }

//    //Chuyển đổi chế độ Nhắn Riêng khi click vào danh sách User
//    private void dgvUsers_CellClick(object? sender, DataGridViewCellEventArgs e)
//    {
//        if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // Click ra ngoài bảng

//        // Nếu click đúng vào cột "Tin nhắn"
//        if (dgvUsers.Columns[e.ColumnIndex].Name == "colChat")
//        {
//            var cell = dgvUsers.Rows[e.RowIndex].Cells["colName"];
//            if (cell.Value == null) return;
//            string targetName = cell.Value.ToString() ?? string.Empty;
//            if (string.IsNullOrEmpty(targetName)) return;

//            // Nếu click lại vào người đang nhắn riêng -> Hủy (Trở về nhắn chung)
//            if (_privateTarget == targetName)
//            {
//                _privateTarget = null;
//                dgvUsers.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
//                lblLoggedIn.Text = $"Đã đăng nhập: {txtUsername.Text.Trim()}";
//                AppendChat("[Hệ thống] Đã chuyển sang chế độ gửi chung.");
//            }
//            // Nếu click vào người mới -> Chuyển sang nhắn riêng người đó
//            else
//            {
//                foreach (DataGridViewRow row in dgvUsers.Rows) row.DefaultCellStyle.BackColor = Color.White; // Reset màu toàn bảng
//                _privateTarget = targetName;
//                dgvUsers.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightYellow; // Tô vàng người đang nhắn
//                lblLoggedIn.Text = $"Đang gửi riêng tới: {targetName}";
//                AppendChat($"[Hệ thống] Đang gửi riêng tới [{targetName}]");
//            }
//        }
//    }

//    private void dgvUsers_CellContentClick(object? sender, DataGridViewCellEventArgs e) { }

//    //Xóa trắng dữ liệu bảng và form chat
//    private void ClearUserList()
//    {
//        dgvUsers.Rows.Clear();
//        _userMap.Clear();
//        _nextUserId = 1;
//        _privateTarget = null;
//        chatBubblePanel.ClearMessages();
//        _userAvatars.Clear();
//    }

//    //Đăng xuất tài khoản, trở về màn hình Login
//    private void btnLogout_Click(object? sender, EventArgs e)
//    {
//        DialogResult res = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//        if (res != DialogResult.Yes) return;
//        Disconnect("Đã đăng xuất.");
//        Hide();
//        var loginForm = new Frmlogin();
//        if (loginForm.ShowDialog() == DialogResult.OK)
//        {
//            txtUsername.Text = loginForm.LoggedInUser ?? "Ẩn danh";
//            Show();
//            lblLoggedIn.Text = $"Đã đăng nhập: {txtUsername.Text.Trim()}";
//        }
//        else { Application.Exit(); }
//    }

//    //Hỗ trợ in thông báo hệ thống không có Avatar
//    private void AppendChat(string text, Color? color = null)
//    {
//        if (chatBubblePanel.InvokeRequired) { SafeInvoke(() => AppendChat(text, color)); return; }
//        chatBubblePanel.AddMessage("[Hệ thống]", text, DateTime.Now, false);
//    }

//    //tin nhắn lên màn hình có kèm Avatar
//    private void AppendChatWithAvatar(string senderName, string messageText, bool isOwnMessage, string? avatarBase64 = null)
//    {
//        if (chatBubblePanel.InvokeRequired)
//        {
//            SafeInvoke(() => AppendChatWithAvatar(senderName, messageText, isOwnMessage, avatarBase64));
//            return;
//        }

//        chatBubblePanel.AddMessage(senderName, messageText, DateTime.Now, isOwnMessage, avatarBase64);

//        // Lưu lại avatar vào cache nếu có
//        if (!string.IsNullOrEmpty(avatarBase64))
//        {
//            _userAvatars[senderName] = avatarBase64;
//        }
//    }

//    // Nút chức năng ép quay về chế độ nhắn chung
//    private void btnPublic_Click_1(object? sender, EventArgs e)
//    {
//        if (_privateTarget == null)
//        {
//            AppendChat("[Hệ thống] Bạn đang ở chế độ nhắn chung rồi.");
//            return;
//        }

//        foreach (DataGridViewRow row in dgvUsers.Rows)
//            row.DefaultCellStyle.BackColor = Color.White;

//        _privateTarget = null;
//        lblLoggedIn.Text = $"Đã đăng nhập: {txtUsername.Text.Trim()}";
//        AppendChat("[Hệ thống] Đã chuyển sang chế độ nhắn chung.");
//    }

//    // Yêu cầu Server trả về lịch sử chat
//    private void btnLoadHistory_Click(object sender, EventArgs e)
//    {
//        if (!isConnected)
//        {
//            MessageBox.Show("Chưa kết nối Server");
//            return;
//        }

//        if (_privateTarget == null)
//        {
//            SendRaw("LOAD_PUBLIC\n");
//        }
//        else
//        {
//            SendRaw($"LOAD_PRIVATE:{_privateTarget}\n");
//        }
//    }

//    //Khóa/Mở khóa các công cụ tùy theo việc đã kết nối hay chưa
//    private void SetConnectedState(bool connected)
//    {
//        conection.Enabled = !connected;
//        unconection.Enabled = connected;
//        btnSend.Enabled = connected;
//        btnEmoji.Enabled = connected;
//        txtMessage.Enabled = connected;
//        btnPublic.Enabled = connected;
//        lblLoggedIn.Text = connected ? $"Đã đăng nhập: {txtUsername.Text.Trim()}" : "Chưa kết nối";
//    }
//    // KHỐI HÀM CÔNG CỤ (HELPERS & PARSING)
//    //Giải quyết lỗi Cross-Thread (Luồng Mạng muốn đụng vào Luồng UI)
//    private void SafeInvoke(Action action)
//    {
//        if (IsDisposed) return;
//        if (InvokeRequired) Invoke(action); else action();
//    }

//    //Dùng string.IndexOf để bóc tách lấy TÊN NGƯỜI GỬI từ cấu trúc mạng
//    private string ExtractSenderFromMessage(string message)
//    {
//        // Phân tích định dạng: "[HH:mm:ss] [SenderName]: message" hoặc "[HH:mm:ss] SenderName: message"
//        try
//        {
//            int firstBracketEnd = message.IndexOf(']');
//            if (firstBracketEnd < 0) return "Unknown";

//            string afterTimestamp = message.Substring(firstBracketEnd + 1).TrimStart();

//            if (afterTimestamp.StartsWith("["))
//            {
//                int closeBracket = afterTimestamp.IndexOf(']');
//                if (closeBracket > 0)
//                {
//                    return afterTimestamp.Substring(1, closeBracket - 1).Trim();
//                }
//            }

//            int colonIdx = afterTimestamp.IndexOf(':');
//            if (colonIdx > 0)
//            {
//                return afterTimestamp.Substring(0, colonIdx).Trim();
//            }

//            return "Unknown";
//        }
//        catch
//        {
//            return "Unknown";
//        }
//    }

//    //Dùng string.IndexOf để bóc tách lấy NỘI DUNG TIN NHẮN từ cấu trúc mạng
//    private string ExtractMessageContent(string message)
//    {
//        try
//        {
//            int firstBracketEnd = message.IndexOf(']');
//            if (firstBracketEnd < 0) return message;

//            string afterTimestamp = message.Substring(firstBracketEnd + 1).TrimStart();

//            if (afterTimestamp.StartsWith("["))
//            {
//                int closeBracket = afterTimestamp.IndexOf(']');
//                if (closeBracket > 0)
//                {
//                    int colonIdx = afterTimestamp.IndexOf(':', closeBracket);
//                    if (colonIdx >= 0)
//                    {
//                        return afterTimestamp.Substring(colonIdx + 1).TrimStart();
//                    }
//                }
//            }

//            int colonIdx2 = afterTimestamp.IndexOf(':');
//            if (colonIdx2 > 0)
//            {
//                return afterTimestamp.Substring(colonIdx2 + 1).TrimStart();
//            }

//            return afterTimestamp;
//        }
//        catch
//        {
//            return message;
//        }
//    }

//    // Các event trống (Được tạo ra bởi Designer, không chứa logic)
//    private void txtkey_TextChanged(object? sender, EventArgs e) { }
//    private void txtUsername_TextChanged(object? sender, EventArgs e) { }
//    private void label1_Click(object? sender, EventArgs e) { }
//    private void headerPanel_Paint(object sender, PaintEventArgs e) { }
//}