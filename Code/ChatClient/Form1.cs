using System.Data;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatClient;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    // Networking fields
    private TcpClient? _tcp;
    private NetworkStream? _stream;
    private Thread? _recvThread;
    private volatile bool _connected = false;
    private string _username = string.Empty;
    private readonly StringBuilder _recvBuffer = new StringBuilder();

    // --- CÁC BIẾN QUẢN LÝ ---
    private string _userId = string.Empty;
    private string _selectedPrivateUser = string.Empty;
    private string _serverIp = string.Empty;
    // ------------------------

    private void Form1_Load(object sender, EventArgs e)
    {
        dgvUsers.Columns.Clear();
        dgvUsers.Columns.Add("colID", "ID");
        dgvUsers.Columns.Add("colName", "Name");
        dgvUsers.Columns.Add("colSendMsg", "Tin nhắn");

        dgvUsers.DefaultCellStyle.ForeColor = Color.Black;
        dgvUsers.DefaultCellStyle.BackColor = Color.White;

        dgvUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
        dgvUsers.DefaultCellStyle.SelectionForeColor = Color.White;

        dgvUsers.EnableHeadersVisualStyles = false;
        dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 100, 255);
        dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

        using (Frmlogin lf = new Frmlogin())
        {
            DialogResult dr = lf.ShowDialog(this);

            if (dr == DialogResult.OK)
            {
                string user = lf.LoggedInUser ?? "Guest";
                string ip = lf.ServerIp;
                int port = lf.ServerPort;

                string generatedId = "UID_" + new Random().Next(1000, 9999);
                ApplyLoggedInState(generatedId, user, ip, port);
            }
            else
            {
                Application.Exit();
            }
        }
    }

    private void btnChangePort_Click(object? sender, EventArgs e)
    {
        if (int.TryParse(txtChangePort.Text.Trim(), out int newPort))
        {
            if (newPort <= 0 || newPort > 65535)
            {
                MessageBox.Show("Port phải nằm trong khoảng từ 1 đến 65535!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DisconnectFromServer();
            AppendSystemText($"Đang ngắt kết nối và chuyển sang Port mới: {newPort}...");

            // Cập nhật giao diện (label3 đang hiện Port)
            label3.Text = newPort.ToString();

            ThreadPool.QueueUserWorkItem(_ => ConnectToServer(_serverIp, newPort, _username));
            txtChangePort.Clear();
        }
        else
        {
            MessageBox.Show("Vui lòng nhập Port là một số nguyên hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    // ĐÂY LÀ HÀM ĐANG BỊ THIẾU TRONG FILE GIAO DIỆN CỦA BẠN
    private void dgvUsers_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            string? targetUser = dgvUsers.Rows[e.RowIndex].Cells[1].Value?.ToString();

            if (!string.IsNullOrEmpty(targetUser))
            {
                if (string.Equals(targetUser, _username, StringComparison.OrdinalIgnoreCase))
                {
                    _selectedPrivateUser = string.Empty;
                    dgvUsers.ClearSelection();
                    MessageBox.Show("Đã quay trở lại phòng chat chung toàn cục.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _selectedPrivateUser = targetUser;
                MessageBox.Show($"Đã chuyển sang chế độ nhắn tin riêng với: {targetUser}", "Chat Riêng", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

    private void ApplyLoggedInState(string userId, string username, string serverIp, int serverPort)
    {
        _userId = userId;
        _username = username;
        _serverIp = serverIp;

        lblLoggedIn.Text = $"Đã đăng nhập: {username}";
        label2.Text = serverIp;
        label3.Text = serverPort.ToString();
        btnLogoutChat.Visible = true;

        dgvUsers.Rows.Clear();
        dgvUsers.Rows.Add(userId, username, "Bản thân (Tôi)");

        ThreadPool.QueueUserWorkItem(_ => ConnectToServer(serverIp, serverPort, username));

        this.Opacity = 1;
        this.Visible = true;

        BeginInvoke(() => txtMessage.Focus());
    }

    private void btnLogout_Click(object? sender, EventArgs e)
    {
        this.Opacity = 0;
        _selectedPrivateUser = string.Empty;
        DisconnectFromServer();

        using var lf = new Frmlogin();
        lf.StartPosition = FormStartPosition.CenterScreen;
        var dr = lf.ShowDialog();
        if (dr == DialogResult.OK)
        {
            string generatedId = "UID_" + new Random().Next(1000, 9999);
            string user = lf.LoggedInUser ?? "Guest";
            string ip = lf.ServerIp;
            int port = lf.ServerPort;

            ApplyLoggedInState(generatedId, user, ip, port);
            this.Opacity = 1;
        }
        else
        {
            Application.Exit();
        }
    }

    private void ScrollToBottom()
    {
        rtbChat.SelectionStart = rtbChat.TextLength;
        rtbChat.SelectionLength = 0;
        rtbChat.ScrollToCaret();
    }

    private void btnSend_Click(object? sender, EventArgs e)
    {
        var text = txtMessage.Text.Trim();
        if (string.IsNullOrEmpty(text)) return;

        if (_connected && _stream != null)
        {
            try
            {
                if (!string.IsNullOrEmpty(_selectedPrivateUser))
                {
                    var send = $"PRV:{_selectedPrivateUser}:{text}\n";
                    var bytes = Encoding.UTF8.GetBytes(send);
                    _stream.Write(bytes, 0, bytes.Length);
                }
                else
                {
                    var send = $"MSG:{text}\n";
                    var bytes = Encoding.UTF8.GetBytes(send);
                    _stream.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                AppendSystemText("Lỗi gửi tin nhắn: " + ex.Message);
            }
        }
        else
        {
            var user = _username;
            var ts = DateTime.Now.ToString("HH:mm");

            rtbChat.SelectionColor = Color.Yellow;
            rtbChat.SelectionFont = new Font(rtbChat.Font, FontStyle.Bold);
            rtbChat.AppendText($"[{ts}] {user}: ");

            rtbChat.SelectionColor = Color.White;
            rtbChat.SelectionFont = new Font(rtbChat.Font, FontStyle.Regular);
            rtbChat.AppendText(text + Environment.NewLine);
            ScrollToBottom();
        }

        txtMessage.Clear();
        txtMessage.Focus();
    }

    private void AppendSystemText(string text)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => AppendSystemText(text));
            return;
        }
        rtbChat.SelectionColor = Color.FromArgb(200, 200, 200);
        rtbChat.AppendText(text + Environment.NewLine);
        ScrollToBottom();
    }

    private void AppendIncomingText(string text, bool isPrivate)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => AppendIncomingText(text, isPrivate));
            return;
        }
        if (isPrivate)
            rtbChat.SelectionColor = Color.Orange;
        else
            rtbChat.SelectionColor = Color.White;

        rtbChat.SelectionFont = new Font(rtbChat.Font, isPrivate ? FontStyle.Bold : FontStyle.Regular);
        rtbChat.AppendText(text + Environment.NewLine);
        ScrollToBottom();
    }

    private void ConnectToServer(string ip, int port, string username)
    {
        try
        {
            _tcp = new TcpClient();
            _tcp.Connect(ip, port);
            _stream = _tcp.GetStream();
            _connected = true;
            var login = $"LOGIN:{username}\n";
            var b = Encoding.UTF8.GetBytes(login);
            _stream.Write(b, 0, b.Length);

            _recvThread = new Thread(ReceiveLoop) { IsBackground = true };
            _recvThread.Start();

            AppendSystemText($"Đã kết nối tới server {ip}:{port}");
        }
        catch (Exception ex)
        {
            AppendSystemText("Không thể kết nối tới server: " + ex.Message);
            _connected = false;
        }
    }

    private void DisconnectFromServer()
    {
        try
        {
            _connected = false;
            try { _stream?.Close(); } catch { }
            try { _tcp?.Close(); } catch { }
            try { if (_recvThread != null && _recvThread.IsAlive) _recvThread.Join(200); } catch { }
            AppendSystemText("Đã ngắt kết nối khỏi server");
        }
        catch { }
    }

    private void ReceiveLoop()
    {
        var buf = new byte[4096];
        try
        {
            while (_connected && _stream != null)
            {
                int n = _stream.Read(buf, 0, buf.Length);
                if (n <= 0) break;
                var s = Encoding.UTF8.GetString(buf, 0, n);
                _recvBuffer.Append(s);
                string all = _recvBuffer.ToString();
                int idx;
                while ((idx = all.IndexOf('\n')) >= 0)
                {
                    var line = all.Substring(0, idx).Trim();
                    if (line.Length > 0) HandleServerLine(line);
                    all = all.Substring(idx + 1);
                }
                _recvBuffer.Clear();
                _recvBuffer.Append(all);
            }
        }
        catch { }
        finally
        {
            _connected = false;
            if (this.IsHandleCreated)
            {
                BeginInvoke(() => AppendSystemText("Kết nối tới server đã bị đóng."));
            }
        }
    }

    private void HandleServerLine(string line)
    {
        if (line.StartsWith("USERS:", StringComparison.OrdinalIgnoreCase))
        {
            var rest = line.Substring(6);
            var users = rest.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(u => u.Trim())
                            .Where(u => u.Length > 0)
                            .ToList();

            if (this.IsHandleCreated)
            {
                this.BeginInvoke(() =>
                {
                    dgvUsers.Rows.Clear();
                    int gialapID = 1000;

                    foreach (var u in users)
                    {
                        gialapID++;
                        bool isMe = string.Equals(u, _username, StringComparison.OrdinalIgnoreCase);
                        string actionText = isMe ? "Bản thân (Tôi)" : "Nhấn để chat riêng";
                        string userIdText = isMe ? _userId : "UID_" + gialapID;

                        dgvUsers.Rows.Add(userIdText, u, actionText);
                    }

                    if (!string.IsNullOrEmpty(_selectedPrivateUser))
                    {
                        foreach (DataGridViewRow row in dgvUsers.Rows)
                        {
                            if (string.Equals(row.Cells[1].Value?.ToString(), _selectedPrivateUser, StringComparison.OrdinalIgnoreCase))
                            {
                                row.Selected = true;
                                break;
                            }
                        }
                    }
                });
            }
            return;
        }

        if (line.StartsWith("MSG:", StringComparison.OrdinalIgnoreCase))
        {
            var msg = line.Substring(4).Trim();
            AppendIncomingText(msg, false);
            return;
        }

        if (line.StartsWith("PRV:", StringComparison.OrdinalIgnoreCase))
        {
            var msg = line.Substring(4).Trim();
            AppendIncomingText(msg, true);
            return;
        }

        AppendIncomingText(line, false);
    }

    private void btnEmoji_Click(object? sender, EventArgs e)
    {
        var menu = new ContextMenuStrip();
        var emojis = new[] {
            "😀","😁","😂","🤣","😃","😄","😅","😆","😉","😊",
            "🙂","🙃","☺️","😇","🥰","😍","🤩","😘","😗","😙",
            "❤️","💛","💚","💙","🧡","🖤","💯","🔥","⭐","🌟"
        };
        foreach (var em in emojis)
        {
            var item = new ToolStripMenuItem(em);
            item.Click += (s, ea) => InsertEmoji(em);
            menu.Items.Add(item);
        }
        menu.Show(btnEmoji, new System.Drawing.Point(0, btnEmoji.Height));
    }

    private void InsertEmoji(string emoji)
    {
        var sel = txtMessage.SelectionStart;
        txtMessage.Text = txtMessage.Text.Insert(sel, emoji);
        txtMessage.SelectionStart = sel + emoji.Length;
        txtMessage.Focus();
    }

    private void btnSendImage_Click(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog();
        ofd.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
        if (ofd.ShowDialog() != DialogResult.OK) return;

        try
        {
            var user = _username;
            var ts = DateTime.Now.ToString("HH:mm");

            rtbChat.SelectionColor = Color.Yellow;
            rtbChat.SelectionFont = new Font(rtbChat.Font, FontStyle.Bold);
            rtbChat.AppendText($"[{ts}] {user}:{Environment.NewLine}");

            rtbChat.SelectionColor = Color.White;
            rtbChat.SelectionFont = new Font(rtbChat.Font, FontStyle.Regular);

            using var original = Image.FromFile(ofd.FileName);
            int maxW = Math.Min(200, rtbChat.ClientSize.Width - 20);
            float ratio = Math.Min(1f, (float)maxW / original.Width);
            int dispW = (int)(original.Width * ratio);
            int dispH = (int)(original.Height * ratio);

            using var scaled = new Bitmap(original, dispW, dispH);
            Clipboard.SetImage(scaled);

            rtbChat.ReadOnly = false;
            rtbChat.Select(rtbChat.TextLength, 0);
            rtbChat.Paste();
            rtbChat.ReadOnly = true;

            rtbChat.AppendText(Environment.NewLine);

            ScrollToBottom();
            txtMessage.Focus();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Không thể hiển thị ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void label1_Click(object sender, EventArgs e) { }
    private void headerRightPanel_Paint(object sender, PaintEventArgs e) { }

    private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }
}