namespace ChatClient;

partial class Form1
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
        txtServerIp = new TextBox();
        lblServerIp = new Label();
        txtUsername = new TextBox();
        lblUsername = new Label();
        txtPassword = new TextBox();
        lblPassword = new Label();
        btnRegister = new Button();
        btnLogin = new Button();
        lstUsers = new ListBox();
        rtbChat = new RichTextBox();
        txtMessage = new TextBox();
        btnSend = new Button();
        btnSendImage = new Button();
        btnEmoji = new Button();
        lblTitle = new Label();
        lblLoggedIn = new Label();
        headerPanel = new Panel();
        headerRightPanel = new FlowLayoutPanel();
        btnLogoutChat = new Button();
        headerPanel.SuspendLayout();
        headerRightPanel.SuspendLayout();
        SuspendLayout();
        // 
        // txtServerIp
        // 
        txtServerIp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtServerIp.Location = new Point(200, 6);
        txtServerIp.Margin = new Padding(5, 6, 5, 6);
        txtServerIp.Name = "txtServerIp";
        txtServerIp.Size = new Size(145, 27);
        txtServerIp.TabIndex = 1;
        txtServerIp.Text = "127.0.0.1";
        // 
        // lblServerIp
        // 
        lblServerIp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblServerIp.AutoSize = true;
        lblServerIp.Location = new Point(121, 10);
        lblServerIp.Margin = new Padding(10, 10, 5, 10);
        lblServerIp.Name = "lblServerIp";
        lblServerIp.Size = new Size(69, 20);
        lblServerIp.TabIndex = 0;
        lblServerIp.Text = "IP Server:";
        // 
        // txtUsername
        // 
        txtUsername.Location = new Point(83, 71);
        txtUsername.Margin = new Padding(2);
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(145, 27);
        txtUsername.TabIndex = 3;
        txtUsername.TextChanged += txtUsername_TextChanged;
        // 
        // lblUsername
        // 
        lblUsername.AutoSize = true;
        lblUsername.Location = new Point(10, 72);
        lblUsername.Margin = new Padding(2, 0, 2, 0);
        lblUsername.Name = "lblUsername";
        lblUsername.Size = new Size(78, 20);
        lblUsername.TabIndex = 2;
        lblUsername.Text = "Username:";
        // 
        // txtPassword
        // 
        txtPassword.Font = new Font("Segoe UI", 9F);
        txtPassword.Location = new Point(83, 97);
        txtPassword.Margin = new Padding(2);
        txtPassword.Name = "txtPassword";
        txtPassword.Size = new Size(145, 27);
        txtPassword.TabIndex = 5;
        txtPassword.UseSystemPasswordChar = true;
        // 
        // lblPassword
        // 
        lblPassword.AutoSize = true;
        lblPassword.Location = new Point(10, 100);
        lblPassword.Margin = new Padding(2, 0, 2, 0);
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(73, 20);
        lblPassword.TabIndex = 4;
        lblPassword.Text = "Password:";
        // 
        // btnRegister
        // 
        btnRegister.Font = new Font("Segoe UI", 9F);
        btnRegister.Location = new Point(232, 56);
        btnRegister.Margin = new Padding(2);
        btnRegister.Name = "btnRegister";
        btnRegister.Size = new Size(92, 36);
        btnRegister.TabIndex = 6;
        btnRegister.Text = "Đăng ký";
        btnRegister.UseVisualStyleBackColor = true;
        btnRegister.Click += btnRegister_Click;
        // 
        // btnLogin
        // 
        btnLogin.Font = new Font("Segoe UI", 9F);
        btnLogin.Location = new Point(232, 96);
        btnLogin.Margin = new Padding(2);
        btnLogin.Name = "btnLogin";
        btnLogin.Size = new Size(92, 36);
        btnLogin.TabIndex = 7;
        btnLogin.Text = "Đăng nhập";
        btnLogin.UseVisualStyleBackColor = true;
        btnLogin.Click += btnLogin_Click;
        // 
        // lstUsers
        // 
        lstUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        lstUsers.Font = new Font("Segoe UI", 9F);
        lstUsers.FormattingEnabled = true;
        lstUsers.Location = new Point(320, 136);
        lstUsers.Margin = new Padding(2);
        lstUsers.Name = "lstUsers";
        lstUsers.Size = new Size(151, 204);
        lstUsers.TabIndex = 8;
        lstUsers.SelectedIndexChanged += lstUsers_SelectedIndexChanged;
        // 
        // rtbChat
        // 
        rtbChat.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        rtbChat.BackColor = Color.WhiteSmoke;
        rtbChat.Font = new Font("Segoe UI", 10F);
        rtbChat.Location = new Point(10, 136);
        rtbChat.Margin = new Padding(2);
        rtbChat.Name = "rtbChat";
        rtbChat.ReadOnly = true;
        rtbChat.Size = new Size(298, 217);
        rtbChat.TabIndex = 9;
        rtbChat.Text = "";
        // 
        // txtMessage
        // 
        txtMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtMessage.Font = new Font("Segoe UI", 9F);
        txtMessage.Location = new Point(10, 358);
        txtMessage.Margin = new Padding(2);
        txtMessage.Name = "txtMessage";
        txtMessage.Size = new Size(177, 27);
        txtMessage.TabIndex = 10;
        // 
        // btnSend
        // 
        btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSend.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnSend.Location = new Point(217, 358);
        btnSend.Margin = new Padding(2);
        btnSend.Name = "btnSend";
        btnSend.Size = new Size(67, 28);
        btnSend.TabIndex = 11;
        btnSend.Text = "Gửi";
        btnSend.UseVisualStyleBackColor = true;
        btnSend.Click += btnSend_Click;
        // 
        // btnSendImage
        // 
        btnSendImage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSendImage.Font = new Font("Segoe UI", 9F);
        btnSendImage.Location = new Point(320, 357);
        btnSendImage.Margin = new Padding(2);
        btnSendImage.Name = "btnSendImage";
        btnSendImage.Size = new Size(96, 28);
        btnSendImage.TabIndex = 12;
        btnSendImage.Text = "Gửi ảnh";
        btnSendImage.UseVisualStyleBackColor = true;
        btnSendImage.Click += btnSendImage_Click;
        // 
        // btnEmoji
        // 
        btnEmoji.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnEmoji.Font = new Font("Segoe UI Emoji", 10F);
        btnEmoji.Location = new Point(288, 357);
        btnEmoji.Margin = new Padding(2);
        btnEmoji.Name = "btnEmoji";
        btnEmoji.Size = new Size(27, 29);
        btnEmoji.TabIndex = 13;
        btnEmoji.Text = "😊";
        btnEmoji.UseVisualStyleBackColor = true;
        btnEmoji.Click += btnEmoji_Click;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(34, 34, 34);
        lblTitle.Location = new Point(10, 5);
        lblTitle.Margin = new Padding(2, 0, 2, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(150, 41);
        lblTitle.TabIndex = 20;
        lblTitle.Text = "Chat App";
        // 
        // lblLoggedIn
        // 
        lblLoggedIn.AutoEllipsis = true;
        lblLoggedIn.AutoSize = true;
        lblLoggedIn.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        lblLoggedIn.ForeColor = Color.FromArgb(90, 90, 90);
        lblLoggedIn.Location = new Point(5, 10);
        lblLoggedIn.Margin = new Padding(5, 10, 10, 10);
        lblLoggedIn.MaximumSize = new Size(176, 0);
        lblLoggedIn.Name = "lblLoggedIn";
        lblLoggedIn.Size = new Size(96, 20);
        lblLoggedIn.TabIndex = 21;
        lblLoggedIn.Text = "Not logged in";
        // 
        // headerPanel
        // 
        headerPanel.BackColor = Color.FromArgb(250, 250, 252);
        headerPanel.Controls.Add(lblTitle);
        headerPanel.Controls.Add(headerRightPanel);
        headerPanel.Dock = DockStyle.Top;
        headerPanel.Location = new Point(0, 0);
        headerPanel.Margin = new Padding(2);
        headerPanel.Name = "headerPanel";
        headerPanel.Padding = new Padding(8, 6, 8, 6);
        headerPanel.Size = new Size(640, 45);
        headerPanel.TabIndex = 14;
        // 
        // headerRightPanel
        // 
        headerRightPanel.Controls.Add(lblLoggedIn);
        headerRightPanel.Controls.Add(lblServerIp);
        headerRightPanel.Controls.Add(txtServerIp);
        headerRightPanel.Dock = DockStyle.Right;
        headerRightPanel.Location = new Point(328, 6);
        headerRightPanel.Margin = new Padding(2);
        headerRightPanel.Name = "headerRightPanel";
        headerRightPanel.Size = new Size(304, 33);
        headerRightPanel.TabIndex = 21;
        headerRightPanel.WrapContents = false;
        // 
        // btnLogoutChat
        // 
        btnLogoutChat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnLogoutChat.AutoSize = true;
        btnLogoutChat.Font = new Font("Segoe UI", 9F);
        btnLogoutChat.Location = new Point(570, 56);
        btnLogoutChat.Margin = new Padding(2);
        btnLogoutChat.Name = "btnLogoutChat";
        btnLogoutChat.Size = new Size(66, 30);
        btnLogoutChat.TabIndex = 15;
        btnLogoutChat.Text = "Logout";
        btnLogoutChat.Click += btnLogout_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 245, 248);
        ClientSize = new Size(640, 480);
        Controls.Add(btnSendImage);
        Controls.Add(btnSend);
        Controls.Add(btnEmoji);
        Controls.Add(txtMessage);
        Controls.Add(rtbChat);
        Controls.Add(lstUsers);
        Controls.Add(btnLogin);
        Controls.Add(btnRegister);
        Controls.Add(txtPassword);
        Controls.Add(lblPassword);
        Controls.Add(txtUsername);
        Controls.Add(lblUsername);
        Controls.Add(headerPanel);
        Controls.Add(btnLogoutChat);
        Margin = new Padding(2);
        MinimumSize = new Size(580, 425);
        Name = "Form1";
        Text = "Chat Client";
        Load += Form1_Load;
        headerPanel.ResumeLayout(false);
        headerPanel.PerformLayout();
        headerRightPanel.ResumeLayout(false);
        headerRightPanel.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    // Designer fields - only btnLogoutChat should be declared here
    private System.Windows.Forms.Panel headerPanel;
    private System.Windows.Forms.FlowLayoutPanel headerRightPanel;
    private System.Windows.Forms.Button btnLogoutChat;
}
