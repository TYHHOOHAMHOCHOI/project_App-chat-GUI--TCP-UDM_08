

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

    // ── Danh sách người dùng ────────────────────────────────────────────────
    private readonly Dictionary<string, int> _userMap = new();
    private int _nextUserId = 1;

    // ── Người dùng gửi riêng ────────────────────────────────────────────────
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

        dgvUsers.Columns.Clear();
        dgvUsers.Columns.Add("colID", "ID");
        dgvUsers.Columns.Add("colName", "Name");
        dgvUsers.Columns.Add("colChat", "Tin nhắn");

        dgvUsers.DefaultCellStyle.SelectionBackColor =
            Color.FromArgb(0, 122, 204);

        dgvUsers.DefaultCellStyle.SelectionForeColor = Color.White;

        dgvUsers.EnableHeadersVisualStyles = false;

        dgvUsers.ColumnHeadersDefaultCellStyle.BackColor =
            Color.FromArgb(100, 100, 255);

        dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.White;

        conection.Click += conection_Click;
        unconection.Click += unconection_Click;
    }

    // ════════════════════════════════════════════════════════════════════════
    // KẾT NỐI / NGẮT KẾT NỐI
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>Mở kết nối</summary>
    private void conection_Click(object sender, EventArgs e)
    {
        // kiểm tra đã kết nối chưa
        if (isConnected)
        {
            MessageBox.Show("Client đã kết nối !");
            return;
        }

        string ip = textBox2.Text.Trim();
        string portStr = textBox3.Text.Trim();

        if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(portStr))
        {
            MessageBox.Show(
                "Vui lòng nhập IP và Port.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        // kiểm tra port
        if (!int.TryParse(portStr, out int port))
        {
            MessageBox.Show(
                "Port không hợp lệ!",
                "Lỗi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        try
        {
            clientSocket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));

            isConnected = true;
            SetConnectedState(true);

            // 1. Đổi lại hiển thị thông báo hệ thống tại Client
            AppendChat($"[Hệ thống] Bạn đã kết nối đến server với Port {portStr}");

            // 2. Lấy tên user và chuẩn bị chuỗi đăng nhập đúng Server đang đợi
            string username = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(username))
            {
                username = "Client_An_Danh"; // Đề phòng trường hợp chưa nhập tên
            }

            // Tạo chuỗi dạng "LOGIN:" để bên Server cắt chuỗi (Split('\n')) bắt được
            string loginMsg = $"LOGIN: {username}\n";

            // Gửi gói tin LOGIN này lên Server ngay lập tức sau khi kết nối thành công
            SendRaw(loginMsg);

            // tạo luồng nhận dữ liệu từ Server về
            Thread recvThread = new Thread(ReceiveLoop)
            {
                IsBackground = true
            };
            recvThread.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Không thể kết nối: {ex.Message}",
                "Lỗi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            clientSocket?.Close();
            clientSocket = null;
        }
    }



    /// <summary>Ngắt kết nối</summary>
    private void unconection_Click(object sender, EventArgs e)
    {
        // kiểm tra chưa kết nối
        if (!isConnected)
        {
            MessageBox.Show("Client chưa kết nối!");
            return;
        }

        Disconnect("Bạn đã ngắt kết nối.");
    }

    private void Disconnect(string reason)
    {
        if (!isConnected)
            return;

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
    // NHẬN DỮ LIỆU
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
                    foreach (string line in msg.Split('\n', StringSplitOptions.RemoveEmptyEntries))
                    {
                        string trimmed = line.TrimEnd('\r');
                        if (string.IsNullOrEmpty(trimmed)) continue;

                        AppendChat(trimmed);

                        // SỬA TẠI ĐÂY: Bọc try-catch riêng để bảo vệ luồng không bị sập ngầm
                        try
                        {
                            TryRegisterUserFromMessage(trimmed);
                        }
                        catch
                        {
                            // Nuốt lỗi bóc tách để giữ luồng nhận tin luôn chạy
                        }
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
            MessageBox.Show("Chưa kết nối tới server!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string myName = txtUsername.Text.Trim();
        if (string.IsNullOrEmpty(myName)) myName = "Ẩn danh";

        string timeStamp = DateTime.Now.ToString("HH:mm:ss");

        if (_privateTarget != null)
        {
            string pmMsg = $"[{timeStamp}] [Gửi riêng] {myName} -> {_privateTarget}: {text}";
            SendRaw(pmMsg);
            AppendChat(pmMsg, Color.DarkViolet);
        }
        // SỬA TẠI ĐÂY: Chat chung chỉ gửi text thô và \n
        else
        {
            string broadMsg = $"{text}\n";
            SendRaw(broadMsg);
        }

        txtMessage.Clear();
        txtMessage.Focus();
    }

    private void SendRaw(string message)
    {
        if (clientSocket == null || !isConnected)
            return;

        try
        {
            byte[] data =
                Encoding.UTF8.GetBytes(message);

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
            MessageBox.Show(
                "Chưa kết nối tới server!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        using OpenFileDialog ofd = new OpenFileDialog();

        ofd.Filter =
            "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

        ofd.Title = "Chọn ảnh để gửi";

        if (ofd.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            byte[] imgBytes =
                System.IO.File.ReadAllBytes(ofd.FileName);

            string base64 =
                Convert.ToBase64String(imgBytes);

            string myName = txtUsername.Text.Trim();

            string timeStamp =
                DateTime.Now.ToString("HH:mm:ss");

            string imgMsg =
                $"[{timeStamp}] {myName}: [IMG]{base64}";

            SendRaw(imgMsg);

            AppendChat(
                $"[{timeStamp}] {myName}: " +
                $"[Đã gửi ảnh: " +
                $"{System.IO.Path.GetFileName(ofd.FileName)}]");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Lỗi gửi ảnh: {ex.Message}",
                "Lỗi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    // EMOJI
    // ════════════════════════════════════════════════════════════════════════
    private void btnEmoji_Click(object sender, EventArgs e)
    {
        ContextMenuStrip menu =
            new ContextMenuStrip();

        string[] emojis =
        {
            "😀","😁","😂","🤣","😃","😄","😅","😆",
            "😉","😊","😋","😎","😍","😘","👍","👎",
            "❤️","🔥","🎉","🥺","😭","😤","🙏","💯"
        };

        foreach (string emoji in emojis)
        {
            ToolStripMenuItem item =
                new ToolStripMenuItem(emoji)
                {
                    Font =
                        new Font("Segoe UI Emoji", 14)
                };

            string cap = emoji;

            item.Click += (s, args) =>
            {
                txtMessage.AppendText(cap);
                txtMessage.Focus();
            };

            menu.Items.Add(item);
        }

        Button btn = (Button)sender;

        menu.Show(btn, new Point(0, btn.Height));
    }

    // ════════════════════════════════════════════════════════════════════════
    // DANH SÁCH NGƯỜI DÙNG
    // ════════════════════════════════════════════════════════════════════════
    private void TryRegisterUserFromMessage(string line)
    {
        if (line.Contains("[Hệ thống]") ||
            line.Contains("[Gửi riêng]"))
            return;

        int bracketEnd = line.IndexOf(']');

        if (bracketEnd < 0)
            return;

        string afterBracket =
            line.Substring(bracketEnd + 1)
            .TrimStart();

        int colonIdx =
            afterBracket.IndexOf(':');

        if (colonIdx <= 0)
            return;

        string senderName =
            afterBracket.Substring(0, colonIdx)
            .Trim();

        if (string.IsNullOrEmpty(senderName))
            return;

        if (senderName ==
            txtUsername.Text.Trim())
            return;

        if (!_userMap.ContainsKey(senderName))
        {
            _userMap[senderName] =
                _nextUserId++;

            int rowIdx = dgvUsers.Rows.Add();

            dgvUsers.Rows[rowIdx]
                .Cells["colID"].Value =
                _userMap[senderName];

            dgvUsers.Rows[rowIdx]
                .Cells["colName"].Value =
                senderName;

            dgvUsers.Rows[rowIdx]
                .Cells["colChat"].Value =
                "Gửi riêng";
        }
    }

    private void dgvUsers_CellClick(
        object sender,
        DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
            return;

        if (dgvUsers.Columns[e.ColumnIndex].Name
            == "colChat")
        {
            var cell =
                dgvUsers.Rows[e.RowIndex]
                .Cells["colName"];

            if (cell.Value == null)
                return;

            string targetName =
                cell.Value.ToString()!;

            if (_privateTarget == targetName)
            {
                _privateTarget = null;

                dgvUsers.Rows[e.RowIndex]
                    .DefaultCellStyle.BackColor =
                    Color.Empty;

                lblLoggedIn.Text =
                    $"Đã đăng nhập: " +
                    $"{txtUsername.Text.Trim()}";

                AppendChat(
                    "[Hệ thống] Đã chuyển sang chế độ gửi chung.");
            }
            else
            {
                foreach (DataGridViewRow row
                    in dgvUsers.Rows)
                {
                    row.DefaultCellStyle.BackColor =
                        Color.Empty;
                }

                _privateTarget = targetName;

                dgvUsers.Rows[e.RowIndex]
                    .DefaultCellStyle.BackColor =
                    Color.LightYellow;

                lblLoggedIn.Text =
                    $"Đang gửi riêng tới: {targetName}";

                AppendChat(
                    $"[Hệ thống] Đang gửi riêng tới [{targetName}]");
            }
        }
    }

    private void dgvUsers_CellContentClick(
        object sender,
        DataGridViewCellEventArgs e)
    {
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
        DialogResult res =
            MessageBox.Show(
                "Bạn có chắc muốn đăng xuất?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

        if (res != DialogResult.Yes)
            return;

        Disconnect("Đã đăng xuất.");

        Hide();

        var loginForm = new Frmlogin();

        if (loginForm.ShowDialog() == DialogResult.OK)
        {
            txtUsername.Text =
                loginForm.LoggedInUser;

            Show();

            lblLoggedIn.Text =
                $"Đã đăng nhập: " +
                $"{loginForm.LoggedInUser}";
        }
        else
        {
            Application.Exit();
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    // HELPERS
    // ════════════════════════════════════════════════════════════════════════
    private void AppendChat(
        string text,
        Color? color = null)
    {
        if (rtbChat.InvokeRequired)
        {
            SafeInvoke(() =>
                AppendChat(text, color));

            return;
        }

        if (color.HasValue)
        {
            rtbChat.SelectionStart =
                rtbChat.TextLength;

            rtbChat.SelectionLength = 0;

            rtbChat.SelectionColor =
                color.Value;
        }

        if (text.Contains("[IMG]"))
        {
            int imgIdx =
                text.IndexOf("[IMG]");

            string prefix =
                text.Substring(0, imgIdx);

            string b64 =
                text.Substring(imgIdx + 5);

            rtbChat.AppendText(
                prefix + "[Ảnh đính kèm]\r\n");

            try
            {
                byte[] imgBytes =
                    Convert.FromBase64String(b64);

                using var ms =
                    new System.IO.MemoryStream(imgBytes);

                Image img =
                    Image.FromStream(ms);

                Clipboard.SetImage(img);

                rtbChat.Paste();

                rtbChat.AppendText("\r\n");
            }
            catch
            {
                rtbChat.AppendText(
                    "[Không thể hiển thị ảnh]\r\n");
            }
        }
        else
        {
            if (text.Contains("[Gửi riêng]") &&
                !color.HasValue)
            {
                rtbChat.SelectionColor =
                    Color.DarkViolet;
            }
            else if (text.Contains("[Hệ thống]") &&
                     !color.HasValue)
            {
                rtbChat.SelectionColor =
                    Color.Gray;
            }
            else if (!color.HasValue)
            {
                rtbChat.SelectionColor =
                    Color.Black;
            }

            rtbChat.AppendText(text + "\r\n");
        }

        rtbChat.SelectionColor = Color.Black;

        rtbChat.ScrollToCaret();
    }

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
            lblLoggedIn.Text =
                $"Đã đăng nhập: " +
                $"{txtUsername.Text.Trim()}";

            lblServerIp.Text =
                $"IP Server: " +
                $"{textBox2.Text.Trim()}";
        }
        else
        {
            lblLoggedIn.Text =
                "Chưa kết nối";
        }
    }

    private void SafeInvoke(Action action)
    {
        if (IsDisposed)
            return;

        if (InvokeRequired)
            Invoke(action);
        else
            action();
    }

    // ════════════════════════════════════════════════════════════════════════
    // CÁC HANDLER GIỮ NGUYÊN
    // ════════════════════════════════════════════════════════════════════════
    private void textBox1_TextChanged(object sender, EventArgs e) { }
    private void txtUsername_TextChanged(object sender, EventArgs e) { }
    private void label1_Click(object sender, EventArgs e) { }
    private void headerRightPanel_Paint(object sender, PaintEventArgs e) { }

    private void rtbChat_TextChanged(object sender, EventArgs e)
    {

    }
}