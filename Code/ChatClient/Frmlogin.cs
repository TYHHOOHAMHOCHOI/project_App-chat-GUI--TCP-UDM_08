using System.Reflection;

namespace ChatClient;

public partial class Frmlogin : Form
{

    private string? _loggedInUser;



    public string? LoggedInUser
    {
        get
        {
            return _loggedInUser;
        }

        private set
        {
            _loggedInUser = value;
        }

    }

    public Frmlogin()
    {
        InitializeComponent();
    }

  
    private void Form1_Load(object sender, EventArgs e)
    {
        loginusername.Clear();
        loginpassword.Clear();
    }


    private void button2_Click(object sender, EventArgs e)
    {

        var user = loginusername.Text.Trim();
        var pass = loginpassword.Text;

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
        {

            MessageBox.Show("Vui lòng nhập username và password.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
      
        if (AccountManager.Authenticate(user, pass))
        {
            LoggedInUser = user;
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

    private void signup_Click(object sender, EventArgs e)
    {
        using var frm = new Frmsign();
        frm.ShowDialog(this);

    }

}