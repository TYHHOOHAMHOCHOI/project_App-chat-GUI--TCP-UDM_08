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
        this.txtServerIp = new System.Windows.Forms.TextBox();
        this.lblServerIp = new System.Windows.Forms.Label();
        this.txtUsername = new System.Windows.Forms.TextBox();
        this.lblUsername = new System.Windows.Forms.Label();
        this.txtPassword = new System.Windows.Forms.TextBox();
        this.lblPassword = new System.Windows.Forms.Label();
        this.btnRegister = new System.Windows.Forms.Button();
        this.btnLogin = new System.Windows.Forms.Button();
        this.lstUsers = new System.Windows.Forms.ListBox();
        this.rtbChat = new System.Windows.Forms.RichTextBox();
        this.txtMessage = new System.Windows.Forms.TextBox();
        this.btnSend = new System.Windows.Forms.Button();
        this.btnSendImage = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // lblServerIp
        // 
        this.lblServerIp.AutoSize = true;
        this.lblServerIp.Location = new System.Drawing.Point(520, 60);
        this.lblServerIp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        this.lblServerIp.Name = "lblServerIp";
        this.lblServerIp.Size = new System.Drawing.Size(70, 20);
        this.lblServerIp.TabIndex = 0;
        this.lblServerIp.Text = "IP Server:";
        // 
        // txtServerIp
        // 
        this.txtServerIp.Location = new System.Drawing.Point(590, 57);
        this.txtServerIp.Name = "txtServerIp";
        this.txtServerIp.Size = new System.Drawing.Size(180, 27);
        this.txtServerIp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        this.txtServerIp.TabIndex = 1;
        this.txtServerIp.Text = "127.0.0.1";
        // 
        // lblUsername
        // 
        this.lblUsername.AutoSize = true;
        this.lblUsername.Location = new System.Drawing.Point(12, 90);
        this.lblUsername.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
        this.lblUsername.Name = "lblUsername";
        this.lblUsername.Size = new System.Drawing.Size(77, 20);
        this.lblUsername.TabIndex = 2;
        this.lblUsername.Text = "Username:";
        // 
        // txtUsername
        // 
        this.txtUsername.Location = new System.Drawing.Point(90, 87);
        this.txtUsername.Name = "txtUsername";
        this.txtUsername.Size = new System.Drawing.Size(180, 27);
        this.txtUsername.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
        this.txtUsername.TabIndex = 3;
        // 
        // lblPassword
        // 
        this.lblPassword.AutoSize = true;
        this.lblPassword.Location = new System.Drawing.Point(12, 125);
        this.lblPassword.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
        this.lblPassword.Name = "lblPassword";
        this.lblPassword.Size = new System.Drawing.Size(73, 20);
        this.lblPassword.TabIndex = 4;
        this.lblPassword.Text = "Password:";
        // 
        // txtPassword
        // 
        this.txtPassword.Location = new System.Drawing.Point(90, 122);
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.Size = new System.Drawing.Size(180, 27);
        this.txtPassword.TabIndex = 5;
        this.txtPassword.UseSystemPasswordChar = true;
        this.txtPassword.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
        this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        // 
        // btnRegister
        // 
        this.btnRegister.Location = new System.Drawing.Point(290, 52);
        this.btnRegister.Name = "btnRegister";
        this.btnRegister.Size = new System.Drawing.Size(90, 30);
        this.btnRegister.TabIndex = 6;
        this.btnRegister.Text = "Đăng ký";
        this.btnRegister.UseVisualStyleBackColor = true;
        this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
        this.btnRegister.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
        this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        // 
        // btnLogin
        // 
        this.btnLogin.Location = new System.Drawing.Point(290, 92);
        this.btnLogin.Name = "btnLogin";
        this.btnLogin.Size = new System.Drawing.Size(90, 30);
        this.btnLogin.TabIndex = 7;
        this.btnLogin.Text = "Đăng nhập";
        this.btnLogin.UseVisualStyleBackColor = true;
        this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
        this.btnLogin.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
        this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        // 
        // lstUsers
        // 
        this.lstUsers.FormattingEnabled = true;
        this.lstUsers.ItemHeight = 20;
        // position the user list aligned with the top of the chat area
        this.lstUsers.Location = new System.Drawing.Point(400, 170);
        this.lstUsers.Name = "lstUsers";
        // match height to the chat area for a horizontal alignment
        this.lstUsers.Size = new System.Drawing.Size(188, 270);
        this.lstUsers.TabIndex = 8;
        this.lstUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Right));
        this.lstUsers.SelectedIndexChanged += new System.EventHandler(this.lstUsers_SelectedIndexChanged);
        this.lstUsers.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        // 
        // rtbChat
        // 
        this.rtbChat.Location = new System.Drawing.Point(12, 170);
        this.rtbChat.Name = "rtbChat";
        this.rtbChat.ReadOnly = true;
        this.rtbChat.Size = new System.Drawing.Size(372, 270);
        this.rtbChat.TabIndex = 9;
        this.rtbChat.Text = "";
        this.rtbChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
        this.rtbChat.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.rtbChat.BackColor = System.Drawing.Color.WhiteSmoke;
        // 
        // txtMessage
        // 
        this.txtMessage.Location = new System.Drawing.Point(12, 448);
        this.txtMessage.Name = "txtMessage";
        this.txtMessage.Size = new System.Drawing.Size(220, 27);
        this.txtMessage.TabIndex = 10;
        this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
        this.txtMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        // 
        // btnSend
        // 
        this.btnSend.Location = new System.Drawing.Point(280, 446);
        this.btnSend.Name = "btnSend";
        this.btnSend.Size = new System.Drawing.Size(84, 30);
        this.btnSend.TabIndex = 11;
        this.btnSend.Text = "Gửi";
        this.btnSend.UseVisualStyleBackColor = true;
        this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
        this.btnSend.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        this.btnSend.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        // 
        // btnSendImage
        // 
        this.btnSendImage.Location = new System.Drawing.Point(400, 446);
        this.btnSendImage.Name = "btnSendImage";
        this.btnSendImage.Size = new System.Drawing.Size(120, 30);
        this.btnSendImage.TabIndex = 12;
        this.btnSendImage.Text = "Gửi ảnh";
        this.btnSendImage.UseVisualStyleBackColor = true;
        this.btnSendImage.Click += new System.EventHandler(this.btnSendImage_Click);
        this.btnSendImage.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        this.btnSendImage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        // 
        // btnEmoji
        // 
        this.btnEmoji = new System.Windows.Forms.Button();
        this.btnEmoji.Location = new System.Drawing.Point(360, 446);
        this.btnEmoji.Name = "btnEmoji";
        this.btnEmoji.Size = new System.Drawing.Size(34, 30);
        this.btnEmoji.TabIndex = 13;
        this.btnEmoji.Text = "😊";
        this.btnEmoji.UseVisualStyleBackColor = true;
        this.btnEmoji.Click += new System.EventHandler(this.btnEmoji_Click);
        this.btnEmoji.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        this.btnEmoji.Font = new System.Drawing.Font("Segoe UI Emoji", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 600);
        this.MinimumSize = new System.Drawing.Size(720, 520);
        this.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
        this.Controls.Add(this.btnSendImage);
        this.Controls.Add(this.btnSend);
        this.Controls.Add(this.btnEmoji);
        this.Controls.Add(this.txtMessage);
        this.Controls.Add(this.rtbChat);
        this.Controls.Add(this.lstUsers);
        this.Controls.Add(this.btnLogin);
        this.Controls.Add(this.btnRegister);
        this.Controls.Add(this.txtPassword);
        this.Controls.Add(this.lblPassword);
        this.Controls.Add(this.txtUsername);
        this.Controls.Add(this.lblUsername);
        // IP controls are added into headerRightPanel instead of directly to form
        // header
        this.lblTitle = new System.Windows.Forms.Label();
        this.lblLoggedIn = new System.Windows.Forms.Label();
        // 
        // headerPanel
        // 
        this.headerPanel = new System.Windows.Forms.Panel();
        this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
        this.headerPanel.Height = 56;
        this.headerPanel.BackColor = System.Drawing.Color.FromArgb(250, 250, 252);
        this.headerPanel.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
        // 
        // lblTitle
        // 
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(34, 34, 34);
        this.lblTitle.Location = new System.Drawing.Point(12, 6);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(176, 41);
        this.lblTitle.TabIndex = 20;
        this.lblTitle.Text = "Chat App";
        // 
        // headerRightPanel
        // 
        this.headerRightPanel = new System.Windows.Forms.FlowLayoutPanel();
        this.headerRightPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
        this.headerRightPanel.Dock = System.Windows.Forms.DockStyle.Right;
        this.headerRightPanel.Width = 380;
        this.headerRightPanel.WrapContents = false;
        this.headerRightPanel.Padding = new System.Windows.Forms.Padding(0);
        // 
        // lblLoggedIn
        // 
        this.lblLoggedIn.AutoSize = true;
        this.lblLoggedIn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
        this.lblLoggedIn.ForeColor = System.Drawing.Color.FromArgb(90, 90, 90);
        this.lblLoggedIn.Margin = new System.Windows.Forms.Padding(6, 12, 12, 12);
        this.lblLoggedIn.Name = "lblLoggedIn";
        this.lblLoggedIn.Size = new System.Drawing.Size(120, 20);
        this.lblLoggedIn.TabIndex = 21;
        this.lblLoggedIn.Text = "Not logged in";
        this.lblLoggedIn.AutoEllipsis = true;
        this.lblLoggedIn.MaximumSize = new System.Drawing.Size(220, 0);
        // 
        // lblServerIp
        // 
        this.lblServerIp.Margin = new System.Windows.Forms.Padding(12, 12, 6, 12);
        // 
        // txtServerIp
        // 
        this.txtServerIp.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
        // 
        // header logout removed; only chat logout button remains
        // 
        // Add header children
        // 
        this.headerPanel.Controls.Add(this.lblTitle);
        this.headerRightPanel.Controls.Add(this.lblLoggedIn);
        this.headerRightPanel.Controls.Add(this.lblServerIp);
        this.headerRightPanel.Controls.Add(this.txtServerIp);
        this.headerPanel.Controls.Add(this.headerRightPanel);
        this.Controls.Add(this.headerPanel);
        // make sure header is in front of other controls
        this.headerPanel.BringToFront();
        this.headerRightPanel.BringToFront();
        // 
        // btnLogoutChat
        // 
        this.btnLogoutChat = new System.Windows.Forms.Button();
        this.btnLogoutChat.Text = "Logout";
        this.btnLogoutChat.AutoSize = true;
        this.btnLogoutChat.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        this.btnLogoutChat.Location = new System.Drawing.Point(720, 70);
        this.btnLogoutChat.Click += new System.EventHandler(this.btnLogout_Click);
        this.btnLogoutChat.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.btnLogoutChat.Visible = true;
        this.Controls.Add(this.btnLogoutChat);
        // header elements are contained in headerPanel/headerRightPanel; no SetChildIndex needed
        // duplicate btnLogout block removed; btnLogout added to headerRightPanel above
        this.Name = "Form1";
        this.Text = "Chat Client";
        this.Load += new System.EventHandler(this.Form1_Load);
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    // Designer fields - only btnLogoutChat should be declared here
    private System.Windows.Forms.Panel headerPanel;
    private System.Windows.Forms.FlowLayoutPanel headerRightPanel;
    private System.Windows.Forms.Button btnLogoutChat;
}
