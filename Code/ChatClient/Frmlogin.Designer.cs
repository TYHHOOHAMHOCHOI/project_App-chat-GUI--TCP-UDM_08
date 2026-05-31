namespace ChatClient;

partial class Frmlogin
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        loginlogin = new Button();
        panel1 = new Panel();
        panel3 = new Panel();
        signup = new Label();
        label1 = new Label();
        loginpassword = new TextBox();
        password = new Label();
        loginusername = new TextBox();
        account = new Label();
        panel4 = new Panel();
        panel2 = new Panel();
        pictureBox1 = new PictureBox();
        panel1.SuspendLayout();
        panel3.SuspendLayout();
        panel2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        SuspendLayout();
        // 
        // loginlogin
        // 
        loginlogin.BackColor = Color.FromArgb(128, 128, 255);
        loginlogin.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        loginlogin.ForeColor = Color.White;
        loginlogin.Location = new Point(42, 335);
        loginlogin.Name = "loginlogin";
        loginlogin.Size = new Size(427, 48);
        loginlogin.TabIndex = 1;
        loginlogin.Text = "đăng nhập";
        loginlogin.UseVisualStyleBackColor = false;
        loginlogin.Click += button2_Click;
        // 
        // panel1
        // 
        panel1.BackColor = Color.Gainsboro;
        panel1.Controls.Add(panel3);
        panel1.Controls.Add(panel2);
        panel1.Location = new Point(173, 119);
        panel1.Name = "panel1";
        panel1.Size = new Size(971, 504);
        panel1.TabIndex = 2;
        // 
        // panel3
        // 
        panel3.Controls.Add(signup);
        panel3.Controls.Add(label1);
        panel3.Controls.Add(loginpassword);
        panel3.Controls.Add(loginlogin);
        panel3.Controls.Add(password);
        panel3.Controls.Add(loginusername);
        panel3.Controls.Add(account);
        panel3.Controls.Add(panel4);
        panel3.Dock = DockStyle.Right;
        panel3.Location = new Point(468, 0);
        panel3.Name = "panel3";
        panel3.Size = new Size(503, 504);
        panel3.TabIndex = 3;
        // 
        // signup
        // 
        signup.AutoSize = true;
        signup.Font = new Font("Courier New", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        signup.ForeColor = Color.Red;
        signup.Location = new Point(344, 396);
        signup.Name = "signup";
        signup.Size = new Size(110, 27);
        signup.TabIndex = 6;
        signup.Text = "Đăng ký";
        signup.Click += signup_Click;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        label1.ForeColor = Color.FromArgb(64, 0, 0);
        label1.Location = new Point(67, 396);
        label1.Name = "label1";
        label1.Size = new Size(278, 27);
        label1.TabIndex = 5;
        label1.Text = "Không có tài khoản?";
        label1.Click += label1_Click;
        // 
        // loginpassword
        // 
        loginpassword.Location = new Point(42, 263);
        loginpassword.Multiline = true;
        loginpassword.Name = "loginpassword";
        loginpassword.PasswordChar = '*';
        loginpassword.Size = new Size(427, 43);
        loginpassword.TabIndex = 4;
        // 
        // password
        // 
        password.AutoSize = true;
        password.BackColor = Color.Gainsboro;
        password.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        password.ForeColor = Color.Blue;
        password.Location = new Point(42, 218);
        password.Name = "password";
        password.Size = new Size(167, 36);
        password.TabIndex = 3;
        password.Text = "Password";
        password.Click += password_Click;
        // 
        // loginusername
        // 
        loginusername.Location = new Point(42, 159);
        loginusername.Multiline = true;
        loginusername.Name = "loginusername";
        loginusername.Size = new Size(427, 43);
        loginusername.TabIndex = 2;
        loginusername.TextChanged += textBox1_TextChanged;
        // 
        // account
        // 
        account.AutoSize = true;
        account.BackColor = Color.Gainsboro;
        account.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        account.ForeColor = Color.Blue;
        account.Location = new Point(41, 110);
        account.Name = "account";
        account.Size = new Size(148, 36);
        account.TabIndex = 1;
        account.Text = "Account";
        account.Click += username_Click;
        // 
        // panel4
        // 
        panel4.Dock = DockStyle.Top;
        panel4.Location = new Point(0, 0);
        panel4.Name = "panel4";
        panel4.Size = new Size(503, 57);
        panel4.TabIndex = 0;
        // 
        // panel2
        // 
        panel2.Controls.Add(pictureBox1);
        panel2.Dock = DockStyle.Left;
        panel2.Location = new Point(0, 0);
        panel2.Name = "panel2";
        panel2.Size = new Size(468, 504);
        panel2.TabIndex = 2;
        // 
        // pictureBox1
        // 
        pictureBox1.Dock = DockStyle.Fill;
        pictureBox1.Image = Properties.Resources.z7835285204860_04b38b5a740b207c4913c6ca6ec0bff5;
        pictureBox1.Location = new Point(0, 0);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(468, 504);
        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        pictureBox1.TabIndex = 0;
        pictureBox1.TabStop = false;
        // 
        // Frmlogin
        // 
        AcceptButton = loginlogin;
        AutoScaleDimensions = new SizeF(13F, 32F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(0, 0, 64);
        ClientSize = new Size(1307, 756);
        Controls.Add(panel1);
        ForeColor = SystemColors.ButtonHighlight;
        Margin = new Padding(4);
        Name = "Frmlogin";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "login";
        Load += Form1_Load;
        panel1.ResumeLayout(false);
        panel3.ResumeLayout(false);
        panel3.PerformLayout();
        panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Button loginlogin;
    private Panel panel1;
    private Panel panel2;
    private Panel panel3;
    private PictureBox pictureBox1;
    private TextBox loginusername;
    private Label account;
    private Panel panel4;
    private TextBox loginpassword;
    private Label password;
    private Label label1;
    private Label signup;
}