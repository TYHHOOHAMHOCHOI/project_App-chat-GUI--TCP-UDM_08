namespace ChatClient;

public partial class Form1 : Form
{
    // UI controls
    private System.Windows.Forms.TextBox txtServerIp;
    private System.Windows.Forms.Label lblServerIp;
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.Label lblUsername;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.Button btnRegister;
    private System.Windows.Forms.Button btnLogin;
    private System.Windows.Forms.ListBox lstUsers;
    private System.Windows.Forms.RichTextBox rtbChat;
    private System.Windows.Forms.TextBox txtMessage;
    private System.Windows.Forms.Button btnSend;
    private System.Windows.Forms.Button btnSendImage;
    private System.Windows.Forms.Button btnEmoji;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblLoggedIn;
    // single logout button rendered in chat area

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        // Show login dialog on startup
        using var lf = new LoginForm();
        var dr = lf.ShowDialog(this);
        if (dr == DialogResult.OK && !string.IsNullOrEmpty(lf.LoggedInUser))
        {
            txtUsername.Text = lf.LoggedInUser;
            // hide register/login controls after successful login
            txtUsername.Visible = false;
            lblUsername.Visible = false;
            txtPassword.Visible = false;
            lblPassword.Visible = false;
            btnRegister.Visible = false;
            btnLogin.Visible = false;
            // show logged-in user
            if (this.lblLoggedIn != null)
            {
                this.lblLoggedIn.Text = $"Logged in: {lf.LoggedInUser}";
            }
            // show logout button
            if (this.btnLogoutChat != null) this.btnLogoutChat.Visible = true;
        }
        else
        {
            // If user cancels login, close the app
            Close();
        }
    }

    private void btnRegister_Click(object? sender, EventArgs e)
    {
        // open login/register dialog
        using var lf = new LoginForm();
        lf.ShowDialog(this);
    }

    private void btnLogin_Click(object? sender, EventArgs e)
    {
        using var lf = new LoginForm();
        // show centered dialog not owned so it doesn't overlap header
        var dr = lf.ShowDialog();
        if (dr == DialogResult.OK && !string.IsNullOrEmpty(lf.LoggedInUser))
        {
            txtUsername.Text = lf.LoggedInUser;
            txtUsername.Visible = false;
            lblUsername.Visible = false;
            txtPassword.Visible = false;
            lblPassword.Visible = false;
            btnRegister.Visible = false;
            btnLogin.Visible = false;
            if (this.lblLoggedIn != null)
            {
                this.lblLoggedIn.Text = $"Logged in: {lf.LoggedInUser}";
            }
            if (this.btnLogoutChat != null) this.btnLogoutChat.Visible = true;
        }
    }

    private void btnLogout_Click(object? sender, EventArgs e)
    {
        // Show login dialog again. Hide main form while prompting.
        this.Hide();
        using var lf = new LoginForm();
        lf.StartPosition = FormStartPosition.CenterScreen;
        lf.TopMost = true;
        var dr = lf.ShowDialog();
        if (dr == DialogResult.OK && !string.IsNullOrEmpty(lf.LoggedInUser))
        {
            // update UI for new logged in user
            txtUsername.Text = lf.LoggedInUser;
            txtUsername.Visible = false;
            lblUsername.Visible = false;
            txtPassword.Visible = false;
            lblPassword.Visible = false;
            btnRegister.Visible = false;
            btnLogin.Visible = false;
            if (this.lblLoggedIn != null) this.lblLoggedIn.Text = $"Logged in: {lf.LoggedInUser}";
            if (this.btnLogoutChat != null) this.btnLogoutChat.Visible = true;
            this.Show();
        }
        else
        {
            // user cancelled -> exit
            Close();
        }
    }

    private void lstUsers_SelectedIndexChanged(object? sender, EventArgs e)
    {
        // Optionally select user to chat privately
    }

    private void btnSend_Click(object? sender, EventArgs e)
    {
        var text = txtMessage.Text.Trim();
        if (string.IsNullOrEmpty(text)) return;
        var user = txtUsername.Visible ? txtUsername.Text : (lblLoggedIn != null ? lblLoggedIn.Text.Replace("Logged in: ", "") : "Me");
        var ts = DateTime.Now.ToString("HH:mm");
        rtbChat.SelectionColor = System.Drawing.Color.DarkBlue;
        rtbChat.AppendText($"[{ts}] {user}: ");
        rtbChat.SelectionColor = System.Drawing.Color.Black;
        rtbChat.AppendText(text + Environment.NewLine);
        txtMessage.Clear();
        // TODO: send message to server
    }

    private void btnEmoji_Click(object? sender, EventArgs e)
    {
        // Simple emoji menu
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
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            rtbChat.AppendText($"[Image sent: {Path.GetFileName(ofd.FileName)}]{Environment.NewLine}");
            // TODO: send image bytes to server
        }
    }
}
