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

            if (AccountManager.Register(user, pass, out var message))
            {
                MessageBox.Show(message, "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                MessageBox.Show(message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}