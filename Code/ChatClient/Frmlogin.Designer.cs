namespace ChatClient;

partial class Frmlogin
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        loginlogin = new Button();
        panel1 = new Panel();
        panel3 = new Panel();
        label2 = new Label();
        textBox2 = new TextBox();
        textBox1 = new TextBox();
        tpserver = new Label();
        signup = new Label();
        label1 = new Label();
        loginpassword = new TextBox();
        password = new Label();
        loginusername = new TextBox();
        username = new Label();
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
        loginlogin.Location = new Point(42, 383);
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
        panel3.Controls.Add(label2);
        panel3.Controls.Add(textBox2);
        panel3.Controls.Add(textBox1);
        panel3.Controls.Add(tpserver);
        panel3.Controls.Add(signup);
        panel3.Controls.Add(label1);
        panel3.Controls.Add(loginpassword);
        panel3.Controls.Add(loginlogin);
        panel3.Controls.Add(password);
        panel3.Controls.Add(loginusername);
        panel3.Controls.Add(username);
        panel3.Controls.Add(panel4);
        panel3.Dock = DockStyle.Right;
        panel3.Location = new Point(468, 0);
        panel3.Name = "panel3";
        panel3.Size = new Size(503, 504);
        panel3.TabIndex = 3;
        // 
        // label2
        // 
        label2.AccessibleRole = AccessibleRole.TitleBar;
        label2.AutoSize = true;
        label2.BackColor = Color.Gainsboro;
        label2.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        label2.ForeColor = Color.Blue;
        label2.LiveSetting = System.Windows.Forms.Automation.AutomationLiveSetting.Assertive;
        label2.Location = new Point(328, 59);
        label2.Name = "label2";
        label2.Size = new Size(91, 36);
        label2.TabIndex = 10;
        label2.Text = "Port";
        label2.Click += label2_Click_1;
        // 
        // textBox2
        // 
        textBox2.Location = new Point(328, 108);
        textBox2.Multiline = true;
        textBox2.Name = "textBox2";
        textBox2.Size = new Size(141, 43);
        textBox2.TabIndex = 9;
        textBox2.TextChanged += textBox2_TextChanged;
        // 
        // textBox1
        // 
        textBox1.Location = new Point(43, 108);
        textBox1.Multiline = true;
        textBox1.Name = "textBox1";
        textBox1.Size = new Size(271, 43);
        textBox1.TabIndex = 8;
        // 
        // tpserver
        // 
        tpserver.AccessibleRole = AccessibleRole.TitleBar;
        tpserver.AutoSize = true;
        tpserver.BackColor = Color.Gainsboro;
        tpserver.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        tpserver.ForeColor = Color.Blue;
        tpserver.LiveSetting = System.Windows.Forms.Automation.AutomationLiveSetting.Assertive;
        tpserver.Location = new Point(42, 59);
        tpserver.Name = "tpserver";
        tpserver.Size = new Size(186, 36);
        tpserver.TabIndex = 7;
        tpserver.Text = "IP Server";
        tpserver.Click += label2_Click;
        // 
        // signup
        // 
        signup.AutoSize = true;
        signup.Font = new Font("Courier New", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        signup.ForeColor = Color.Red;
        signup.Location = new Point(344, 444);
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
        label1.Location = new Point(67, 444);
        label1.Name = "label1";
        label1.Size = new Size(278, 27);
        label1.TabIndex = 5;
        label1.Text = "Không có tài khoản?";
        label1.Click += label1_Click;
        // 
        // loginpassword
        // 
        loginpassword.Location = new Point(42, 311);
        loginpassword.Multiline = true;
        loginpassword.Name = "loginpassword";
        loginpassword.PasswordChar = '*';
        loginpassword.Size = new Size(427, 43);
        loginpassword.TabIndex = 4;
        // 
        // password
        // 
        password.AccessibleRole = AccessibleRole.TitleBar;
        password.AutoSize = true;
        password.BackColor = Color.Gainsboro;
        password.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        password.ForeColor = Color.Blue;
        password.LiveSetting = System.Windows.Forms.Automation.AutomationLiveSetting.Assertive;
        password.Location = new Point(42, 266);
        password.Name = "password";
        password.Size = new Size(167, 36);
        password.TabIndex = 3;
        password.Text = "Password";
        password.Click += password_Click;
        // 
        // loginusername
        // 
        loginusername.Location = new Point(42, 207);
        loginusername.Multiline = true;
        loginusername.Name = "loginusername";
        loginusername.Size = new Size(427, 43);
        loginusername.TabIndex = 2;
        loginusername.TextChanged += textBox1_TextChanged;
        // 
        // username
        // 
        username.AccessibleRole = AccessibleRole.TitleBar;
        username.AutoSize = true;
        username.BackColor = Color.Gainsboro;
        username.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        username.ForeColor = Color.Blue;
        username.LiveSetting = System.Windows.Forms.Automation.AutomationLiveSetting.Assertive;
        username.Location = new Point(41, 158);
        username.Name = "username";
        username.Size = new Size(167, 36);
        username.TabIndex = 1;
        username.Text = "Username";
        username.Click += username_Click;
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
    private Label username;
    private Panel panel4;
    private TextBox loginpassword;
    private Label password;
    private Label label1;
    private Label signup;
    private TextBox textBox1;
    private Label tpserver;
    private Label label2;
    private TextBox textBox2;
}
