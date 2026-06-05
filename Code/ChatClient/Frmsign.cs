namespace ChatClient
{
    public partial class Frmsign : Form
    {
        public Frmsign()
        {
            InitializeComponent();
        }

        private void Frmsign_Load(object sender, EventArgs e)
        {
            reusername.Clear();
            repassword.Clear();
            recopassword.Clear();
        }

        // Nút "Đăng ký"
        private void button1_Click(object sender, EventArgs e)
        {
            var user = reusername.Text.Trim();
            var pass = repassword.Text;
            var confirm = recopassword.Text;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(confirm))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (pass != confirm)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                recopassword.Clear();
                recopassword.Focus();
                return;
            }

            // Gọi hàm Register cơ bản, không có avatar
            if (AccountManager.Register(user, pass, out var message))
            {
                MessageBox.Show(message, "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close(); // đóng Frmsign → quay về Frmlogin
            }
            else
            {
                MessageBox.Show(message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // label5 — "Đăng nhập": chỉ đóng Frmsign, Frmlogin hiện lại tự động
        private void label5_Click(object sender, EventArgs e)
        {
            Close();
        }

        // --- các handler giữ nguyên để Designer không bị lỗi ---
        private void label4_Click_1(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
    }
}