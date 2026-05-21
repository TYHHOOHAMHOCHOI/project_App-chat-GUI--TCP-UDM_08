namespace ChatClient;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        lstUsers = new ListBox();
        rtbChat = new RichTextBox();
        txtMessage = new TextBox();
        btnSend = new Button();
        btnSendImage = new Button();
        btnEmoji = new Button();
        headerPanel = new Panel();
        lblTitle = new Label();
        headerRightPanel = new FlowLayoutPanel();
        lblLoggedIn = new Label();
        lblServerIp = new Label();
        label2 = new Label();
        label1 = new Label();
        label3 = new Label();
        panel1 = new Panel();
        btnLogoutChat = new Button();
        headerPanel.SuspendLayout();
        headerRightPanel.SuspendLayout();
        panel1.SuspendLayout();
        SuspendLayout();
        // 
        // lstUsers
        // 
        lstUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        lstUsers.BackColor = Color.FromArgb(64, 64, 64);
        lstUsers.Font = new Font("Segoe UI", 9F);
        lstUsers.ForeColor = SystemColors.Window;
        lstUsers.FormattingEnabled = true;
        lstUsers.Location = new Point(1047, 90);
        lstUsers.Margin = new Padding(4);
        lstUsers.Name = "lstUsers";
        lstUsers.Size = new Size(243, 548);
        lstUsers.TabIndex = 0;
        lstUsers.SelectedIndexChanged += lstUsers_SelectedIndexChanged;
        // 
        // rtbChat
        // 
        rtbChat.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        rtbChat.BackColor = Color.FromArgb(64, 64, 64);
        rtbChat.Font = new Font("Segoe UI", 10F);
        rtbChat.ForeColor = SystemColors.Window;
        rtbChat.Location = new Point(16, 90);
        rtbChat.Margin = new Padding(4);
        rtbChat.Name = "rtbChat";
        rtbChat.ReadOnly = true;
        rtbChat.Size = new Size(1009, 588);
        rtbChat.TabIndex = 1;
        rtbChat.Text = "";
        // 
        // txtMessage
        // 
        txtMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtMessage.BackColor = Color.FromArgb(64, 64, 64);
        txtMessage.Font = new Font("Segoe UI", 9F);
        txtMessage.ForeColor = SystemColors.Window;
        txtMessage.Location = new Point(16, 692);
        txtMessage.Margin = new Padding(4);
        txtMessage.Name = "txtMessage";
        txtMessage.Size = new Size(744, 39);
        txtMessage.TabIndex = 2;
        // 
        // btnSend
        // 
        btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSend.BackColor = Color.FromArgb(64, 64, 64);
        btnSend.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnSend.ForeColor = SystemColors.ControlLightLight;
        btnSend.Location = new Point(821, 689);
        btnSend.Margin = new Padding(4);
        btnSend.Name = "btnSend";
        btnSend.Size = new Size(109, 38);
        btnSend.TabIndex = 4;
        btnSend.Text = "Gửi";
        btnSend.UseVisualStyleBackColor = false;
        btnSend.Click += btnSend_Click;
        // 
        // btnSendImage
        // 
        btnSendImage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSendImage.BackColor = Color.FromArgb(64, 64, 64);
        btnSendImage.Font = new Font("Segoe UI", 9F);
        btnSendImage.ForeColor = SystemColors.Window;
        btnSendImage.Location = new Point(938, 689);
        btnSendImage.Margin = new Padding(4);
        btnSendImage.Name = "btnSendImage";
        btnSendImage.Size = new Size(117, 38);
        btnSendImage.TabIndex = 5;
        btnSendImage.Text = "Gửi ảnh";
        btnSendImage.UseVisualStyleBackColor = false;
        btnSendImage.Click += btnSendImage_Click;
        // 
        // btnEmoji
        // 
        btnEmoji.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnEmoji.BackColor = Color.FromArgb(64, 64, 64);
        btnEmoji.Font = new Font("Segoe UI Emoji", 10F);
        btnEmoji.ForeColor = SystemColors.Window;
        btnEmoji.Location = new Point(769, 689);
        btnEmoji.Margin = new Padding(4);
        btnEmoji.Name = "btnEmoji";
        btnEmoji.Size = new Size(44, 38);
        btnEmoji.TabIndex = 3;
        btnEmoji.Text = "😊";
        btnEmoji.UseVisualStyleBackColor = false;
        btnEmoji.Click += btnEmoji_Click;
        // 
        // headerPanel
        // 
        headerPanel.BackColor = Color.FromArgb(128, 128, 255);
        headerPanel.Controls.Add(lblTitle);
        headerPanel.Controls.Add(headerRightPanel);
        headerPanel.Controls.Add(panel1);
        headerPanel.Dock = DockStyle.Top;
        headerPanel.Location = new Point(0, 0);
        headerPanel.Margin = new Padding(4);
        headerPanel.Name = "headerPanel";
        headerPanel.Padding = new Padding(13, 10, 13, 10);
        headerPanel.Size = new Size(1307, 129);
        headerPanel.TabIndex = 6;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblTitle.ForeColor = Color.Yellow;
        lblTitle.Location = new Point(16, 8);
        lblTitle.Margin = new Padding(4, 0, 4, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(239, 65);
        lblTitle.TabIndex = 20;
        lblTitle.Text = "Chat App";
        // 
        // headerRightPanel
        // 
        headerRightPanel.Controls.Add(lblLoggedIn);
        headerRightPanel.Controls.Add(lblServerIp);
        headerRightPanel.Controls.Add(label2);
        headerRightPanel.Controls.Add(label1);
        headerRightPanel.Controls.Add(label3);
        headerRightPanel.Location = new Point(348, 8);
        headerRightPanel.Margin = new Padding(4);
        headerRightPanel.Name = "headerRightPanel";
        headerRightPanel.Size = new Size(794, 109);
        headerRightPanel.TabIndex = 21;
        headerRightPanel.WrapContents = false;
        // 
        // lblLoggedIn
        // 
        lblLoggedIn.AutoEllipsis = true;
        lblLoggedIn.AutoSize = true;
        lblLoggedIn.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        lblLoggedIn.ForeColor = Color.Yellow;
        lblLoggedIn.Location = new Point(8, 15);
        lblLoggedIn.Margin = new Padding(8, 15, 16, 15);
        lblLoggedIn.MaximumSize = new Size(286, 0);
        lblLoggedIn.Name = "lblLoggedIn";
        lblLoggedIn.Size = new Size(156, 32);
        lblLoggedIn.TabIndex = 21;
        lblLoggedIn.Text = "Not logged in";
        // 
        // lblServerIp
        // 
        lblServerIp.AutoSize = true;
        lblServerIp.ForeColor = Color.Yellow;
        lblServerIp.Location = new Point(196, 15);
        lblServerIp.Margin = new Padding(16, 15, 8, 15);
        lblServerIp.Name = "lblServerIp";
        lblServerIp.Size = new Size(112, 32);
        lblServerIp.TabIndex = 22;
        lblServerIp.Text = "IP Server:";
        // 
        // label2
        // 
        label2.AutoEllipsis = true;
        label2.AutoSize = true;
        label2.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        label2.ForeColor = Color.Yellow;
        label2.Location = new Point(324, 15);
        label2.Margin = new Padding(8, 15, 16, 15);
        label2.MaximumSize = new Size(286, 0);
        label2.Name = "label2";
        label2.Size = new Size(44, 32);
        label2.TabIndex = 22;
        label2.Text = "---";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.ForeColor = Color.Yellow;
        label1.Location = new Point(400, 15);
        label1.Margin = new Padding(16, 15, 8, 15);
        label1.Name = "label1";
        label1.Size = new Size(61, 32);
        label1.TabIndex = 26;
        label1.Text = "Port:";
        label1.Click += label1_Click;
        // 
        // label3
        // 
        label3.AutoEllipsis = true;
        label3.AutoSize = true;
        label3.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        label3.ForeColor = Color.Yellow;
        label3.Location = new Point(477, 15);
        label3.Margin = new Padding(8, 15, 16, 15);
        label3.MaximumSize = new Size(286, 0);
        label3.Name = "label3";
        label3.Size = new Size(44, 32);
        label3.TabIndex = 28;
        label3.Text = "---";
        // 
        // panel1
        // 
        panel1.Controls.Add(btnLogoutChat);
        panel1.Dock = DockStyle.Right;
        panel1.Location = new Point(1149, 10);
        panel1.Name = "panel1";
        panel1.Size = new Size(145, 109);
        panel1.TabIndex = 29;
        // 
        // btnLogoutChat
        // 
        btnLogoutChat.AutoSize = true;
        btnLogoutChat.BackColor = Color.FromArgb(128, 128, 255);
        btnLogoutChat.Font = new Font("Segoe UI", 9F);
        btnLogoutChat.ForeColor = Color.White;
        btnLogoutChat.Location = new Point(12, 12);
        btnLogoutChat.Margin = new Padding(8, 13, 8, 13);
        btnLogoutChat.Name = "btnLogoutChat";
        btnLogoutChat.Size = new Size(129, 54);
        btnLogoutChat.TabIndex = 24;
        btnLogoutChat.Text = "Logout";
        btnLogoutChat.UseVisualStyleBackColor = false;
        btnLogoutChat.Click += btnLogout_Click;
        // 
        // Form1
        // 
        AcceptButton = btnSend;
        AutoScaleDimensions = new SizeF(13F, 32F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(0, 0, 64);
        ClientSize = new Size(1307, 756);
        Controls.Add(btnSendImage);
        Controls.Add(btnSend);
        Controls.Add(btnEmoji);
        Controls.Add(txtMessage);
        Controls.Add(rtbChat);
        Controls.Add(lstUsers);
        Controls.Add(headerPanel);
        ForeColor = SystemColors.Window;
        Margin = new Padding(4);
        MinimumSize = new Size(928, 569);
        Name = "Form1";
        Text = "Chat Client";
        Load += Form1_Load;
        headerPanel.ResumeLayout(false);
        headerPanel.PerformLayout();
        headerRightPanel.ResumeLayout(false);
        headerRightPanel.PerformLayout();
        panel1.ResumeLayout(false);
        panel1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    // Designer fields
    private System.Windows.Forms.ListBox lstUsers;
    private System.Windows.Forms.RichTextBox rtbChat;
    private System.Windows.Forms.TextBox txtMessage;
    private System.Windows.Forms.Button btnSend;
    private System.Windows.Forms.Button btnSendImage;
    private System.Windows.Forms.Button btnEmoji;
    private System.Windows.Forms.Panel headerPanel;
    private System.Windows.Forms.FlowLayoutPanel headerRightPanel;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblLoggedIn;
    private System.Windows.Forms.Label lblServerIp;
    private System.Windows.Forms.Button btnLogoutChat;
    private Label label1;
    private Label label2;
    private Label label3;
    private Panel panel1;
}