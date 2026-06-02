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
    private Socket? clientSocket;
    private bool isConnected = false;
    private readonly Dictionary<string, int> _userMap = new();
    private int _nextUserId = 1;
    private string? _privateTarget = null;
    private readonly string _loggedInUser;

    public Form1(string loggedInUser)
    {
        InitializeComponent();
        _loggedInUser = loggedInUser;
    }

    private void Form1_Load(object? sender, EventArgs e)
    {
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

                        // ── TIN NHẮN THƯỜNG (chung hoặc riêng từ người khác gửi đến) ──
                        AppendChat(trimmed);
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

        string timeStamp = DateTime.Now.ToString("HH:mm:ss");

        if (_privateTarget != null)
        {
            // Gửi protocol PRIVATE: lên Server để Server định tuyến đúng người nhận
            // Server KHÔNG broadcast → chỉ người nhận thấy, không bị lặp
            SendRaw($"PRIVATE:{_privateTarget}|{text}\n");
            // Tự hiện phía mình gửi ngay, không cần chờ Server echo lại
            AppendChat($"[{timeStamp}] [Gửi riêng] Bạn → {_privateTarget}: {text}", Color.DarkViolet);
        }
        else
        {
            SendRaw($"{text}\n");
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
        if (rtbChat.InvokeRequired) { SafeInvoke(() => AppendChat(text, color)); return; }
        rtbChat.SelectionStart = rtbChat.TextLength;
        rtbChat.SelectionLength = 0;
        if (color.HasValue)
            rtbChat.SelectionColor = color.Value;
        else if (text.Contains("[Gửi riêng]"))
            rtbChat.SelectionColor = Color.HotPink;
        else if (text.Contains("[Hệ thống]"))
            rtbChat.SelectionColor = Color.Gray;
        else
            rtbChat.SelectionColor = Color.Black;
        rtbChat.AppendText(text + "\r\n");
        rtbChat.SelectionColor = Color.Black;
        rtbChat.ScrollToCaret();
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

    private void txtkey_TextChanged(object? sender, EventArgs e) { }
    private void txtUsername_TextChanged(object? sender, EventArgs e) { }
    private void label1_Click(object? sender, EventArgs e) { }
    private void rtbChat_TextChanged(object? sender, EventArgs e) { }
    private void headerPanel_Paint(object sender, PaintEventArgs e) { }


}