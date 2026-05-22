namespace ChatClient;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        using var lf = new Frmlogin();
        var dr = lf.ShowDialog(this);
        if (dr == DialogResult.OK && !string.IsNullOrEmpty(lf.LoggedInUser))
        {
            ApplyLoggedInState(lf.LoggedInUser, lf.ServerIp, lf.ServerPort);
        }
        else
        {
            Close();
        }
    }

    private void ApplyLoggedInState(string username, string serverIp, int serverPort)
    {
        lblLoggedIn.Text = $"Đã đăng nhập: {username}";
        label2.Text = serverIp;
        label3.Text = serverPort.ToString();
        btnLogoutChat.Visible = true;

        BeginInvoke(() => txtMessage.Focus());
    }

    private void btnLogout_Click(object? sender, EventArgs e)
    {
        this.Opacity = 0;

        using var lf = new Frmlogin();
        lf.StartPosition = FormStartPosition.CenterScreen;
        var dr = lf.ShowDialog();
        if (dr == DialogResult.OK && !string.IsNullOrEmpty(lf.LoggedInUser))
        {
            ApplyLoggedInState(lf.LoggedInUser, lf.ServerIp, lf.ServerPort);
            this.Opacity = 1;
        }
        else
        {
            Close();
        }
    }

    // Helper: cuộn xuống cuối rtbChat
    private void ScrollToBottom()
    {
        rtbChat.SelectionStart = rtbChat.TextLength;
        rtbChat.SelectionLength = 0;
        rtbChat.ScrollToCaret();
    }

    private void lstUsers_SelectedIndexChanged(object? sender, EventArgs e) { }

    private void btnSend_Click(object? sender, EventArgs e)
    {
        var text = txtMessage.Text.Trim();
        if (string.IsNullOrEmpty(text)) return;

        var user = lblLoggedIn.Text.Replace("Đã đăng nhập: ", "");
        var ts = DateTime.Now.ToString("HH:mm");

        rtbChat.SelectionColor = Color.Yellow;
        rtbChat.SelectionFont = new Font(rtbChat.Font, FontStyle.Bold);
        rtbChat.AppendText($"[{ts}] {user}: ");

        rtbChat.SelectionColor = Color.White;
        rtbChat.SelectionFont = new Font(rtbChat.Font, FontStyle.Regular);
        rtbChat.AppendText(text + Environment.NewLine);

        ScrollToBottom();
        txtMessage.Clear();
        // TODO: gửi tin nhắn lên server → label2.Text (IP) : label3.Text (Port)
    }

    private void btnEmoji_Click(object? sender, EventArgs e)
    {
        var menu = new ContextMenuStrip();
        var emojis = new[] {
            "😀","😁","😂","🤣","😃","😄","😅","😆","😉","😊",
            "🙂","🙃","☺️","😇","🥰","😍","🤩","😘","😗","😙",
            "😚","😋","😛","😝","😜","🤪","🤨","🧐","🤓","😎",
            "🤗","🤡","🤠","😏","😒","😞","😔","😟","😕","🙁",
            "☹️","😣","😖","😫","😩","🥺","😢","😭","😤","😠",
            "😡","🤬","🤯","😳","🥵","🥶","😱","😨","😰","😥",
            "😓","🤤","😴","😵","🤐","🥴","🤢","🤮","🤧","🤒",
            "🤕","🤑","🤠","😺","😸","😹","😻","😼","😽","🙀",
            "😿","😾","👍","👎","👏","🙌","👐","🤝","🤞","✌️",
            "🤟","🤘","👋","🤚","🖐","✋","🙏","💪","💖","💔",
            "❤️","💛","💚","💙","🧡","🖤","💯","🔥","⭐","🌟",
            "🎉","🎊","🎈","🎂","🥳","😺","👀","💤","📷","🎵"
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
            var user = lblLoggedIn.Text.Replace("Đã đăng nhập: ", "");
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
            MessageBox.Show($"Không thể hiển thị ảnh: {ex.Message}", "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void label1_Click(object sender, EventArgs e) { }

    private void headerRightPanel_Paint(object sender, PaintEventArgs e)
    {

    }
}