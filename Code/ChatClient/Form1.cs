using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.NetworkInformation;

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

    private Panel pnlReply = null!;
    private Label lblReplyText = null!;
    private Button btnCancelReply = null!;
    private string? _replyTargetUser = null;
    private string? _replyTargetMessage = null;

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
        try
        {
            int portToCheck = 9988; // Đổi số này thành số Port mặc định của bạn
            bool isServerRunningLocal = false;

            // Lấy danh sách các Port TCP đang ở trạng thái Lắng nghe (Listen) trên máy này
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpConnListeners = ipGlobalProperties.GetActiveTcpListeners();

            foreach (IPEndPoint tcpi in tcpConnListeners)
            {
                if (tcpi.Port == portToCheck)
                {
                    isServerRunningLocal = true;
                    break;
                }
            }

            
            if (isServerRunningLocal)
            {
                textBox2.Text = "127.0.0.1"; 
            }
            else
            {
                textBox2.Text = string.Empty;
            }
        }
        catch (Exception)
        {
            textBox2.Text = string.Empty;
        }

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

        InitializeReplyPanel();
        chatBubblePanel.OnReplyClicked += ChatBubblePanel_OnReplyClicked;
        chatBubblePanel.OnForwardClicked += ChatBubblePanel_OnForwardClicked;
    }

    private void InitializeReplyPanel()
    {
        pnlReply = new Panel();
        pnlReply.Height = 30;
        pnlReply.BackColor = Color.LightGray;
        pnlReply.Visible = false;
        pnlReply.Location = new Point(10, 495); // Ngay trên txtMessage
        pnlReply.Width = 566;
        pnlReply.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        lblReplyText = new Label();
        lblReplyText.AutoSize = true;
        lblReplyText.Location = new Point(5, 5);
        lblReplyText.ForeColor = Color.DarkSlateGray;
        lblReplyText.Font = new Font("Segoe UI", 9F, FontStyle.Italic);

        btnCancelReply = new Button();
        btnCancelReply.Text = "X";
        btnCancelReply.Size = new Size(25, 25);
        btnCancelReply.Location = new Point(pnlReply.Width - 30, 2);
        btnCancelReply.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCancelReply.FlatStyle = FlatStyle.Flat;
        btnCancelReply.FlatAppearance.BorderSize = 0;
        btnCancelReply.ForeColor = Color.Red;
        btnCancelReply.Cursor = Cursors.Hand;
        btnCancelReply.Click += (s, e) => CancelReply();

        pnlReply.Controls.Add(lblReplyText);
        pnlReply.Controls.Add(btnCancelReply);
        this.Controls.Add(pnlReply);
        pnlReply.BringToFront();
    }

    private void ChatBubblePanel_OnReplyClicked(string senderName, string messageText)
    {
        _replyTargetUser = senderName;
        _replyTargetMessage = messageText;
        string shortMsg = messageText.Length > 40 ? messageText.Substring(0, 40) + "..." : messageText;
        lblReplyText.Text = $"Đang trả lời {senderName}: {shortMsg}";
        pnlReply.Visible = true;
        txtMessage.Focus();
    }

    private void ChatBubblePanel_OnForwardClicked(string senderName, string messageText)
    {
        if (!isConnected || clientSocket == null)
        {
            MessageBox.Show("Chưa kết nối tới server!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string forwardText = $"[Chuyển tiếp từ {senderName}]: {messageText}";
        var userAvatarBase64 = AccountManager.GetAvatar(_loggedInUser);

        if (_privateTarget != null)
        {
            SendRaw($"PRIVATE:{_privateTarget}|{forwardText}\n");
            AppendChatWithAvatar(_loggedInUser, $"[Gửi riêng tới {_privateTarget}] {forwardText}", true, userAvatarBase64);
        }
        else
        {
            SendRaw($"{forwardText}\n");
            AppendChatWithAvatar(_loggedInUser, forwardText, true, userAvatarBase64);
        }
    }

    private void CancelReply()
    {
        _replyTargetUser = null;
        _replyTargetMessage = null;
        pnlReply.Visible = false;
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
                            string historyMsg = line.Substring(8).Trim();

                            string historySender = ExtractSenderFromMessage(historyMsg);
                            string historyText = ExtractMessageContent(historyMsg);

                            string? histAvatar = GetOrFetchAvatar(historySender);
                            AppendChatWithAvatar(
                                historySender,
                                historyText,
                                historySender == _loggedInUser,
                                histAvatar);

                            continue;
                        }

                        if (line.StartsWith("HISTORY_PRIVATE:"))
                        {
                            string historyMsg = line.Substring(16).Trim();

                            int endTime = historyMsg.IndexOf(']');
                            string remain = historyMsg.Substring(endTime + 1).Trim();

                            int arrow = remain.IndexOf("->");
                            int colon = remain.IndexOf(':');

                            string historySender = "";
                            string historyText = "";

                            if (arrow > 0 && colon > arrow)
                            {
                                historySender = remain.Substring(0, arrow).Trim();
                                historyText = remain.Substring(colon + 1).Trim();
                            }

                            string? histPrivAvatar = GetOrFetchAvatar(historySender);
                            AppendChatWithAvatar(
                                historySender,
                                "[Riêng] " + historyText,
                                historySender == _loggedInUser,
                                histPrivAvatar);

                            continue;
                        }
                        if (line.StartsWith("HISTORY_REPLY:"))
                        {
                            string payload = line.Substring(14);
                            int bracketEnd = payload.IndexOf(']');
                            string afterTimestamp = payload.Substring(bracketEnd + 1).TrimStart();
                            string[] parts = afterTimestamp.Split('|', 4);
                            if (parts.Length == 4)
                            {
                                string sName = parts[0].Trim();
                                if (sName.EndsWith(":")) sName = sName.Substring(0, sName.Length - 1);
                                string rUser = parts[1].Trim();
                                string rMsg = parts[2].Trim();
                                string rContent = parts[3].Trim();
                                string? histReplyAvatar = GetOrFetchAvatar(sName);
                                AppendChatWithReply(sName, rContent, sName == _loggedInUser, rUser, rMsg, histReplyAvatar);
                            }
                            continue;
                        }
                        if (line.StartsWith("HISTORY_PRIVATE_REPLY:"))
                        {
                            string payload = line.Substring(22);
                            int bracketEnd = payload.IndexOf(']');
                            string afterTimestamp = payload.Substring(bracketEnd + 1).TrimStart();
                            string[] parts = afterTimestamp.Split('|', 4);
                            if (parts.Length == 4)
                            {
                                string sName = parts[0].Split('-')[0].Trim();
                                string rUser = parts[1].Trim();
                                string rMsg = parts[2].Trim();
                                string rContent = parts[3].Trim();
                                string? histPrivReplyAvatar = GetOrFetchAvatar(sName);
                                AppendChatWithReply(sName, rContent, sName == _loggedInUser, rUser, rMsg, histPrivReplyAvatar);
                            }
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

                        if (trimmed.StartsWith("ERR_DUPLICATE:", StringComparison.OrdinalIgnoreCase))
                        {
                            string errMsg = trimmed.Substring(14).Trim();
                            isConnected = false;
                            MessageBox.Show(errMsg, "Tài khoản đã đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Disconnect("Kết nối bị từ chối do tài khoản đang được dùng ở nơi khác.");
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

                        if (trimmed.StartsWith("BROADCAST_REPLY:", StringComparison.OrdinalIgnoreCase))
                        {
                            string payload = trimmed.Substring(16);
                            int bracketEnd = payload.IndexOf(']');
                            string afterTimestamp = payload.Substring(bracketEnd + 1).TrimStart();
                            string[] parts = afterTimestamp.Split('|', 4);
                            if (parts.Length == 4)
                            {
                                string replySenderName = parts[0].Trim();
                                string targetUser = parts[1].Trim();
                                string targetMsg = parts[2].Trim();
                                string content = parts[3].Trim();

                                if (replySenderName == _loggedInUser) continue;

                                string? replySenderAvatarBase64 = null;
                                if (_userAvatars.TryGetValue(replySenderName, out var cachedAvatar))
                                {
                                    replySenderAvatarBase64 = cachedAvatar;
                                }
                                else
                                {
                                    try
                                    {
                                        replySenderAvatarBase64 = AccountManager.GetAvatar(replySenderName);
                                        if (!string.IsNullOrEmpty(replySenderAvatarBase64)) _userAvatars[replySenderName] = replySenderAvatarBase64;
                                    }
                                    catch { }
                                }
                                AppendChatWithReply(replySenderName, content, false, targetUser, targetMsg, replySenderAvatarBase64);
                            }
                            continue;
                        }

                        if (trimmed.StartsWith("PRIVATE_REPLY:", StringComparison.OrdinalIgnoreCase))
                        {
                            string payload = trimmed.Substring(14);
                            int bracketEnd = payload.IndexOf(']');
                            string afterTimestamp = payload.Substring(bracketEnd + 1).TrimStart();
                            string[] parts = afterTimestamp.Split('|', 4);
                            if (parts.Length == 4)
                            {
                                string replySenderName = parts[0].Trim();
                                string targetUser = parts[1].Trim();
                                string targetMsg = parts[2].Trim();
                                string content = parts[3].Trim();

                                if (replySenderName == _loggedInUser) continue;

                                string? replySenderAvatarBase64 = null;
                                if (_userAvatars.TryGetValue(replySenderName, out var cachedAvatar))
                                {
                                    replySenderAvatarBase64 = cachedAvatar;
                                }
                                else
                                {
                                    try
                                    {
                                        replySenderAvatarBase64 = AccountManager.GetAvatar(replySenderName);
                                        if (!string.IsNullOrEmpty(replySenderAvatarBase64)) _userAvatars[replySenderName] = replySenderAvatarBase64;
                                    }
                                    catch { }
                                }
                                AppendChatWithReply(replySenderName, "[Gửi riêng] " + content, false, targetUser, targetMsg, replySenderAvatarBase64);
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
            if (_replyTargetUser != null)
            {
                SendRaw($"REPLY_PRIVATE:{_privateTarget}|{_replyTargetUser}|{_replyTargetMessage}|{text}\n");
                AppendChatWithReply(_loggedInUser, $"[Gửi riêng tới {_privateTarget}] {text}", true, _replyTargetUser, _replyTargetMessage, userAvatarBase64);
            }
            else
            {
                SendRaw($"PRIVATE:{_privateTarget}|{text}\n");
                AppendChatWithAvatar(_loggedInUser, $"[Gửi riêng tới {_privateTarget}] {text}", true, userAvatarBase64);
            }
        }
        else
        {
            if (_replyTargetUser != null)
            {
                SendRaw($"REPLY_PUBLIC:{_replyTargetUser}|{_replyTargetMessage}|{text}\n");
                AppendChatWithReply(_loggedInUser, text, true, _replyTargetUser, _replyTargetMessage, userAvatarBase64);
            }
            else
            {
                SendRaw($"{text}\n");
                AppendChatWithAvatar(_loggedInUser, text, true, userAvatarBase64);
            }
        }

        CancelReply();

        txtMessage.Clear(); // Xóa khung nhập liệu
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
        // Lọc: không thêm "Server" hoặc các tên hệ thống vào danh sách user
        if (senderName.Equals("Server", StringComparison.OrdinalIgnoreCase)) return;
        if (senderName.StartsWith("[")) return;
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

    private void AppendChatWithReply(string senderName, string messageText, bool isOwnMessage, string? replyToUser, string? replyToMessage, string? avatarBase64 = null)
    {
        if (chatBubblePanel.InvokeRequired)
        {
            SafeInvoke(() => AppendChatWithReply(senderName, messageText, isOwnMessage, replyToUser, replyToMessage, avatarBase64));
            return;
        }

        chatBubblePanel.AddMessageWithReply(senderName, messageText, DateTime.Now, isOwnMessage, replyToUser, replyToMessage, avatarBase64);

        if (!string.IsNullOrEmpty(avatarBase64))
        {
            _userAvatars[senderName] = avatarBase64;
        }
    }

    /// <summary>Lấy avatar từ cache _userAvatars, nếu không có thì tra AccountManager. Trả về null nếu không có.</summary>
    private string? GetOrFetchAvatar(string username)
    {
        if (string.IsNullOrEmpty(username)) return null;
        if (_userAvatars.TryGetValue(username, out var cached)) return cached;
        try
        {
            var avatar = AccountManager.GetAvatar(username);
            if (!string.IsNullOrEmpty(avatar))
                _userAvatars[username] = avatar;
            return avatar;
        }
        catch { return null; }
    }

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
            MessageBox.Show("Chưa kết nối Server");
            return;
        }

        chatBubblePanel.ClearMessages();

        if (_privateTarget == null)
        {
            SendRaw("LOAD_PUBLIC\n");
        }
        else
        {
            SendRaw($"LOAD_PRIVATE:{_privateTarget}\n");
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
