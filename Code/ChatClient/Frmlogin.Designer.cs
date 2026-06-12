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

    private void InitializeComponent() // hàm tự động sinh ,xây dựng winform
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
        loginlogin.Location = new Point(26, 209);
        loginlogin.Margin = new Padding(2, 2, 2, 2);
        loginlogin.Name = "loginlogin";
        loginlogin.Size = new Size(263, 30);
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
        panel1.Location = new Point(106, 74);
        panel1.Margin = new Padding(2, 2, 2, 2);
        panel1.Name = "panel1";
        panel1.Size = new Size(598, 315);
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
        panel3.Location = new Point(288, 0);
        panel3.Margin = new Padding(2, 2, 2, 2);
        panel3.Name = "panel3";
        panel3.Size = new Size(310, 315);
        panel3.TabIndex = 3;
        // 
        // signup
        // 
        signup.AutoSize = true;
        signup.Font = new Font("Courier New", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        signup.ForeColor = Color.Red;
        signup.Location = new Point(212, 248);
        signup.Margin = new Padding(2, 0, 2, 0);
        signup.Name = "signup";
        signup.Size = new Size(71, 17);
        signup.TabIndex = 6;
        signup.Text = "Đăng ký";
        signup.Click += signup_Click;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        label1.ForeColor = Color.FromArgb(64, 0, 0);
        label1.Location = new Point(29, 248);
        label1.Margin = new Padding(2, 0, 2, 0);
        label1.Name = "label1";
        label1.Size = new Size(179, 17);
        label1.TabIndex = 5;
        label1.Text = "Không có tài khoản?";
        // 
        // loginpassword
        // 
        loginpassword.Location = new Point(26, 164);
        loginpassword.Margin = new Padding(2, 2, 2, 2);
        loginpassword.Multiline = true;
        loginpassword.Name = "loginpassword";
        loginpassword.PasswordChar = '*';
        loginpassword.Size = new Size(264, 28);
        loginpassword.TabIndex = 4;
        // 
        // password
        // 
        password.AutoSize = true;
        password.BackColor = Color.Gainsboro;
        password.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        password.ForeColor = Color.Blue;
        password.Location = new Point(26, 136);
        password.Margin = new Padding(2, 0, 2, 0);
        password.Name = "password";
        password.Size = new Size(106, 22);
        password.TabIndex = 3;
        password.Text = "Password";
        // 
        // loginusername
        // 
        loginusername.Location = new Point(26, 99);
        loginusername.Margin = new Padding(2, 2, 2, 2);
        loginusername.Multiline = true;
        loginusername.Name = "loginusername";
        loginusername.Size = new Size(264, 28);
        loginusername.TabIndex = 2;
        // 
        // account
        // 
        account.AutoSize = true;
        account.BackColor = Color.Gainsboro;
        account.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        account.ForeColor = Color.Blue;
        account.Location = new Point(25, 69);
        account.Margin = new Padding(2, 0, 2, 0);
        account.Name = "account";
        account.Size = new Size(94, 22);
        account.TabIndex = 1;
        account.Text = "Account";
        // 
        // panel4
        // 
        panel4.Dock = DockStyle.Top;
        panel4.Location = new Point(0, 0);
        panel4.Margin = new Padding(2, 2, 2, 2);
        panel4.Name = "panel4";
        panel4.Size = new Size(310, 36);
        panel4.TabIndex = 0;
        // 
        // panel2
        // 
        panel2.Controls.Add(pictureBox1);
        panel2.Dock = DockStyle.Left;
        panel2.Location = new Point(0, 0);
        panel2.Margin = new Padding(2, 2, 2, 2);
        panel2.Name = "panel2";
        panel2.Size = new Size(288, 315);
        panel2.TabIndex = 2;
        // 
        // pictureBox1
        // 
        pictureBox1.Dock = DockStyle.Fill;
        pictureBox1.Image = Properties.Resources.z7835285204860_04b38b5a740b207c4913c6ca6ec0bff5;
        pictureBox1.Location = new Point(0, 0);
        pictureBox1.Margin = new Padding(2, 2, 2, 2);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(288, 315);
        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        pictureBox1.TabIndex = 0;
        pictureBox1.TabStop = false;
        // 
        // Frmlogin
        // 
        AcceptButton = loginlogin;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(0, 0, 64);
        ClientSize = new Size(804, 472);
        Controls.Add(panel1);
        ForeColor = SystemColors.ButtonHighlight;
        Margin = new Padding(2, 2, 2, 2);
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