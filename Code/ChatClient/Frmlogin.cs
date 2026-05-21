namespace ChatClient;

public partial class Frmlogin : Form
{
    public string? LoggedInUser { get; private set; }
    public string ServerIp { get; private set; } = "127.0.0.1";
    public int ServerPort { get; private set; } = 8080;

    public Frmlogin()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        loginusername.Clear();
        loginpassword.Clear();
        textBox1.Text = "127.0.0.1";
        textBox2.Text = "8080";
    }

    // Nút "Đăng nhập"
    private void button2_Click(object sender, EventArgs e)
    {
        var user = loginusername.Text.Trim();
        var pass = loginpassword.Text;
        var ip = textBox1.Text.Trim();
        var portStr = textBox2.Text.Trim();

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
        {
            MessageBox.Show("Vui lòng nhập username và password.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrEmpty(ip))
        {
            MessageBox.Show("Vui lòng nhập địa chỉ IP server.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            textBox1.Focus();
            return;
        }

        if (!int.TryParse(portStr, out var port) || port < 1 || port > 65535)
        {
            MessageBox.Show("Port không hợp lệ (1 – 65535).", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            textBox2.Focus();
            return;
        }

        if (AccountManager.Authenticate(user, pass))
        {
            LoggedInUser = user;
            ServerIp = ip;
            ServerPort = port;
            DialogResult = DialogResult.OK;
            Close();
        }
        else
        {
            MessageBox.Show("Sai username hoặc password.", "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            loginpassword.Clear();
            loginpassword.Focus();
        }
    }

    // Nhấn "Đăng ký" → mở Frmsign
    private void signup_Click(object sender, EventArgs e)
    {
        using var frm = new Frmsign();
        frm.ShowDialog(this);
    }

    // --- các handler giữ nguyên để Designer không bị lỗi ---
    private void textBox1_TextChanged(object sender, EventArgs e) { }
    private void username_Click(object sender, EventArgs e) { }
    private void password_Click(object sender, EventArgs e) { }
    private void label1_Click(object sender, EventArgs e) { }
    private void label2_Click(object sender, EventArgs e) { }
    private void label2_Click_1(object sender, EventArgs e) { }
    private void button1_Click(object sender, EventArgs e) { Close(); }

    private void textBox2_TextChanged(object sender, EventArgs e)
    {

    }
}