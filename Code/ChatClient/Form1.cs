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
    // ── Kết nối ──────────────────────────────────────────────────────────────
    private Socket? clientSocket;
    private bool isConnected = false;

    // ── Danh sách người dùng: key = tên, value = số thứ tự (ID hiển thị) ───
    // Server hiện tại không gửi danh sách user riêng, nên client tự parse
    // tên từ tin nhắn broadcast để xây danh sách.
    private readonly Dictionary<string, int> _userMap = new();
    private int _nextUserId = 1;

    // ── Người dùng đang được chọn để gửi riêng (null = gửi tất cả) ──────────
    private string? _privateTarget = null;

    private readonly string _loggedInUser;

    public Form1(string loggedInUser)
    {
        InitializeComponent();
        _loggedInUser = loggedInUser;
    }

    // ════════════════════════════════════════════════════════════════════════
    // KHỞI TẠO FORM
    // ════════════════════════════════════════════════════════════════════════
    private void Form1_Load(object sender, EventArgs e)
    {
        txtUsername.Text = _loggedInUser;
        SetConnectedState(false);
    }

    // ════════════════════════════════════════════════════════════════════════
    // KẾT NỐI / NGẮT KẾT NỐI
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>Nút "Mở kết nối"</summary>
    private void conection_Click(object sender, EventArgs e)
    {
        if (isConnected)
        {
            AppendChat("[Hệ thống] Đã kết nối rồi.");
            return;
        }

        string ip = textBox2.Text.Trim();
        string portStr = textBox3.Text.Trim();
        string key = textBox1.Text.Trim(); // key dùng cho tương lai

        if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(portStr))
        {
            MessageBox.Show("Vui lòng nhập IP và Port.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!int.TryParse(portStr, out int port))
        {
            MessageBox.Show("Port không hợp lệ.", "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));

            isConnected = true;
            SetConnectedState(true);

            AppendChat($"[Hệ thống] Đã kết nối tới {ip}:{port}");

            // Gửi tên đăng nhập lên server ngay khi kết nối (server sẽ broadcast)
            string joinMsg = $"[{txtUsername.Text.Trim()}] đã tham gia phòng chat.";
            SendRaw(joinMsg);

            // Bắt đầu luồng nhận dữ liệu
            Thread recvThread = new Thread(ReceiveLoop) { IsBackground = true };
            recvThread.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Không thể kết nối: {ex.Message}", "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            clientSocket?.Close();
            clientSocket = null;
        }
    }

    /// <summary>Nút "Ngắt kết nối"</summary>
    private void unconection_Click(object sender, EventArgs e)
    {
        Disconnect("Bạn đã ngắt kết nối.");
    }

    private void Disconnect(string reason)
    {
        if (!isConnected) return;

        isConnected = false;
        try
        {
            clientSocket?.Shutdown(SocketShutdown.Both);
        }
        catch { }
        clientSocket?.Close();
        clientSocket = null;

        SafeInvoke(() =>
        {
            SetConnectedState(false);
            AppendChat($"[Hệ thống] {reason}");
            ClearUserList();
        });
    }

    // ════════════════════════════════════════════════════════════════════════
    // NHẬN DỮ LIỆU TỪ SERVER (chạy ở luồng nền)
    // ════════════════════════════════════════════════════════════════════════
    private void ReceiveLoop()
    {
        byte[] buffer = new byte[4096];
        while (isConnected && clientSocket != null)
        {
            try
            {
                int received = clientSocket.Receive(buffer);
                if (received == 0)
                {
                    Disconnect("Server đã đóng kết nối.");
                    break;
                }

                string msg = Encoding.UTF8.GetString(buffer, 0, received);

                SafeInvoke(() =>
                {
                    // Mỗi gói có thể chứa nhiều dòng, tách ra xử lý từng dòng
                    foreach (string line in msg.Split('\n', StringSplitOptions.RemoveEmptyEntries))
                    {
                        string trimmed = line.TrimEnd('\r');
                        if (string.IsNullOrEmpty(trimmed)) continue;

                        AppendChat(trimmed);
                        TryRegisterUserFromMessage(trimmed);
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

    // ════════════════════════════════════════════════════════════════════════
    // GỬI TIN NHẮN
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>Nút "Gửi" (broadcast cho tất cả)</summary>
    private void btnSend_Click(object sender, EventArgs e)
    {
        string text = txtMessage.Text.Trim();
        if (string.IsNullOrEmpty(text))
        {
            txtMessage.Focus();
            return;
        }

        if (!isConnected || clientSocket == null)
        {
            MessageBox.Show("Chưa kết nối tới server!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string myName = txtUsername.Text.Trim();
        if (string.IsNullOrEmpty(myName)) myName = "Ẩn danh";

        string timeStamp = DateTime.Now.ToString("HH:mm:ss");

        if (_privateTarget != null)
        {
            // Gửi riêng: thêm prefix [PM] để server / người nhận nhận biết
            // (Server hiện tại broadcast tất cả — client nhận sẽ lọc hiển thị)
            string pmMsg = $"[{timeStamp}] [Gửi riêng] {myName} -> {_privateTarget}: {text}";
            SendRaw(pmMsg);
            AppendChat(pmMsg, Color.DarkViolet);
        }
        else
        {
            string broadMsg = $"[{timeStamp}] {myName}: {text}";
            SendRaw(broadMsg);
            // Không tự hiển thị ở đây vì server sẽ broadcast lại cho mình
        }

        txtMessage.Clear();
        txtMessage.Focus();
    }

    /// <summary>Gửi raw bytes lên server</summary>
    private void SendRaw(string message)
    {
        if (clientSocket == null || !isConnected) return;
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);
        }
        catch (Exception ex)
        {
            AppendChat($"[Lỗi gửi] {ex.Message}");
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    // GỬI ẢNH
    // ════════════════════════════════════════════════════════════════════════
    private void btnSendImage_Click(object sender, EventArgs e)
    {
        if (!isConnected)
        {
            MessageBox.Show("Chưa kết nối tới server!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
        ofd.Title = "Chọn ảnh để gửi";
        if (ofd.ShowDialog() != DialogResult.OK) return;

        try
        {
            byte[] imgBytes = System.IO.File.ReadAllBytes(ofd.FileName);
            string base64 = Convert.ToBase64String(imgBytes);
            string myName = txtUsername.Text.Trim();
            string timeStamp = DateTime.Now.ToString("HH:mm:ss");

            // Giao thức: [IMG]base64data
            string imgMsg = $"[{timeStamp}] {myName}: [IMG]{base64}";
            SendRaw(imgMsg);

            AppendChat($"[{timeStamp}] {myName}: [Đã gửi ảnh: {System.IO.Path.GetFileName(ofd.FileName)}]");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi gửi ảnh: {ex.Message}", "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    // EMOJI
    // ════════════════════════════════════════════════════════════════════════
    private void btnEmoji_Click(object sender, EventArgs e)
    {
        ContextMenuStrip menu = new ContextMenuStrip();
        string[] emojis = { "😀", "😁", "😂", "🤣", "😃", "😄", "😅", "😆",
                             "😉", "😊", "😋", "😎", "😍", "😘", "👍", "👎",
                             "❤️", "🔥", "🎉", "🥺", "😭", "😤", "🙏", "💯" };

        foreach (string emoji in emojis)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(emoji)
            {
                Font = new Font("Segoe UI Emoji", 14)
            };
            string cap = emoji; // capture
            item.Click += (s, args) => { txtMessage.AppendText(cap); txtMessage.Focus(); };
            menu.Items.Add(item);
        }

        Button btn = (Button)sender;
        menu.Show(btn, new Point(0, btn.Height));
    }

    // ════════════════════════════════════════════════════════════════════════
    // DANH SÁCH NGƯỜI DÙNG (dgvUsers)
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Parse tên người gửi từ tin nhắn dạng "[HH:mm:ss] TênNgười: nội dung"
    /// rồi thêm vào bảng nếu chưa có.
    /// </summary>
    private void TryRegisterUserFromMessage(string line)
    {
        // Bỏ qua tin hệ thống
        if (line.Contains("[Hệ thống]") || line.Contains("[Gửi riêng]")) return;

        // Dạng: "[HH:mm:ss] TênNgười: nội dung"
        int bracketEnd = line.IndexOf(']');
        if (bracketEnd < 0) return;

        string afterBracket = line.Substring(bracketEnd + 1).TrimStart();
        int colonIdx = afterBracket.IndexOf(':');
        if (colonIdx <= 0) return;

        string senderName = afterBracket.Substring(0, colonIdx).Trim();
        if (string.IsNullOrEmpty(senderName)) return;

        // Không thêm chính mình vào danh sách (tuỳ ý — bỏ dòng này nếu muốn thấy mình)
        if (senderName == txtUsername.Text.Trim()) return;

        if (!_userMap.ContainsKey(senderName))
        {
            _userMap[senderName] = _nextUserId++;
            int rowIdx = dgvUsers.Rows.Add();
            dgvUsers.Rows[rowIdx].Cells["colID"].Value = _userMap[senderName];
            dgvUsers.Rows[rowIdx].Cells["colName"].Value = senderName;
            dgvUsers.Rows[rowIdx].Cells["colChat"].Value = "Gửi riêng";
        }
    }

    /// <summary>Click vào dòng người dùng để chọn gửi riêng hoặc bỏ chọn</summary>
    private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        // Click cột "Gửi tin nhắn"
        if (dgvUsers.Columns[e.ColumnIndex].Name == "colChat")
        {
            var cell = dgvUsers.Rows[e.RowIndex].Cells["colName"];
            if (cell.Value == null) return;
            string targetName = cell.Value.ToString()!;

            if (_privateTarget == targetName)
            {
                // Bỏ chọn nếu click lại cùng người
                _privateTarget = null;
                dgvUsers.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Empty;
                lblLoggedIn.Text = $"Đã đăng nhập: {txtUsername.Text.Trim()}";
                AppendChat($"[Hệ thống] Đã chuyển sang chế độ gửi chung.");
            }
            else
            {
                // Bỏ tô cũ
                foreach (DataGridViewRow row in dgvUsers.Rows)
                    row.DefaultCellStyle.BackColor = Color.Empty;

                _privateTarget = targetName;
                dgvUsers.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
                lblLoggedIn.Text = $"Đang gửi riêng tới: {targetName}  (click lại để hủy)";
                AppendChat($"[Hệ thống] Đang gửi riêng tới [{targetName}]. Click lại cột 'Gửi tin nhắn' của họ để hủy chọn.");
            }
        }
    }

    private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        // Đã xử lý trong dgvUsers_CellClick
    }

    private void ClearUserList()
    {
        dgvUsers.Rows.Clear();
        _userMap.Clear();
        _nextUserId = 1;
        _privateTarget = null;
    }

    // ════════════════════════════════════════════════════════════════════════
    // LOGOUT
    // ════════════════════════════════════════════════════════════════════════
    private void btnLogout_Click(object sender, EventArgs e)
    {
        DialogResult res = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (res != DialogResult.Yes) return;

        Disconnect("Đã đăng xuất.");

        // Quay về màn hình đăng nhập
        Hide();
        var loginForm = new Frmlogin();
        if (loginForm.ShowDialog() == DialogResult.OK)
        {
            txtUsername.Text = loginForm.LoggedInUser;
            Show();
            lblLoggedIn.Text = $"Đã đăng nhập: {loginForm.LoggedInUser}";
        }
        else
        {
            Application.Exit();
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    // HELPERS
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>Thêm dòng vào rtbChat với màu tuỳ chọn, thread-safe.</summary>
    private void AppendChat(string text, Color? color = null)
    {
        if (rtbChat.InvokeRequired) { SafeInvoke(() => AppendChat(text, color)); return; }

        if (color.HasValue)
        {
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionLength = 0;
            rtbChat.SelectionColor = color.Value;
        }

        // Phát hiện ảnh base64 và hiển thị thông báo thay vì dump dữ liệu thô
        if (text.Contains("[IMG]"))
        {
            int imgIdx = text.IndexOf("[IMG]");
            string prefix = text.Substring(0, imgIdx);
            string b64 = text.Substring(imgIdx + 5);
            rtbChat.AppendText(prefix + "[Ảnh đính kèm]\r\n");

            try
            {
                byte[] imgBytes = Convert.FromBase64String(b64);
                using var ms = new System.IO.MemoryStream(imgBytes);
                Image img = Image.FromStream(ms);

                // Chèn ảnh vào RichTextBox qua Clipboard tạm thời
                Clipboard.SetImage(img);
                rtbChat.Paste();
                rtbChat.AppendText("\r\n");
            }
            catch
            {
                rtbChat.AppendText("[Không thể hiển thị ảnh]\r\n");
            }
        }
        else
        {
            // Tô màu tin nhắn [Gửi riêng]
            if (text.Contains("[Gửi riêng]") && !color.HasValue)
            {
                rtbChat.SelectionColor = Color.DarkViolet;
            }
            // Tô màu tin hệ thống
            else if (text.Contains("[Hệ thống]") && !color.HasValue)
            {
                rtbChat.SelectionColor = Color.Gray;
            }
            else if (!color.HasValue)
            {
                rtbChat.SelectionColor = Color.Black;
            }

            rtbChat.AppendText(text + "\r\n");
        }

        rtbChat.SelectionColor = Color.Black;
        rtbChat.ScrollToCaret();
    }

    /// <summary>Kích hoạt/tắt các control tuỳ trạng thái kết nối.</summary>
    private void SetConnectedState(bool connected)
    {
        conection.Enabled = !connected;
        unconection.Enabled = connected;
        btnSend.Enabled = connected;
        btnSendImage.Enabled = connected;
        btnEmoji.Enabled = connected;
        txtMessage.Enabled = connected;

        if (connected)
        {
            lblLoggedIn.Text = $"Đã đăng nhập: {txtUsername.Text.Trim()}";
            lblServerIp.Text = $"IP Server: {textBox2.Text.Trim()}";
        }
        else
        {
            lblLoggedIn.Text = "Chưa kết nối";
        }
    }

    private void SafeInvoke(Action action)
    {
        if (IsDisposed) return;
        if (InvokeRequired)
            Invoke(action);
        else
            action();
    }

    // ════════════════════════════════════════════════════════════════════════
    // CÁC HANDLER GIỮ NGUYÊN ĐỂ DESIGNER KHÔNG LỖI
    // ════════════════════════════════════════════════════════════════════════
    private void textBox1_TextChanged(object sender, EventArgs e) { }
    private void txtUsername_TextChanged(object sender, EventArgs e) { }
    private void label1_Click(object sender, EventArgs e) { }
}