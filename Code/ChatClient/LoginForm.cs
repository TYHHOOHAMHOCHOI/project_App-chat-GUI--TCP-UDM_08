using System.Windows.Forms;

namespace ChatClient
{
    public partial class LoginForm : Form
    {
        public string? LoggedInUser { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object? sender, EventArgs e)
        {
            var user = txtUser.Text.Trim();
            var pass = txtPass.Text;
            if (AccountManager.Register(user, pass, out var message))
            {
                MessageBox.Show(message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            var user = txtUser.Text.Trim();
            var pass = txtPass.Text;
            if (AccountManager.Authenticate(user, pass))
            {
                LoggedInUser = user;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
