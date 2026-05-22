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
        lstUsers.Location = new Point(644, 56);
        lstUsers.Margin = new Padding(2, 2, 2, 2);
        lstUsers.Name = "lstUsers";
        lstUsers.Size = new Size(151, 344);
        lstUsers.TabIndex = 0;
        lstUsers.SelectedIndexChanged += lstUsers_SelectedIndexChanged;
        // 
        // rtbChat
        // 
        rtbChat.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        rtbChat.BackColor = Color.FromArgb(64, 64, 64);
        rtbChat.Font = new Font("Segoe UI", 10F);
        rtbChat.ForeColor = SystemColors.Window;
        rtbChat.Location = new Point(10, 56);
        rtbChat.Margin = new Padding(2, 2, 2, 2);
        rtbChat.Name = "rtbChat";
        rtbChat.ReadOnly = true;
        rtbChat.Size = new Size(622, 369);
        rtbChat.TabIndex = 1;
        rtbChat.Text = "";
        // 
        // txtMessage
        // 
        txtMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtMessage.BackColor = Color.FromArgb(64, 64, 64);
        txtMessage.Font = new Font("Segoe UI", 9F);
        txtMessage.ForeColor = SystemColors.Window;
        txtMessage.Location = new Point(10, 432);
        txtMessage.Margin = new Padding(2, 2, 2, 2);
        txtMessage.Name = "txtMessage";
        txtMessage.Size = new Size(459, 27);
        txtMessage.TabIndex = 2;
        // 
        // btnSend
        // 
        btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSend.BackColor = Color.FromArgb(64, 64, 64);
        btnSend.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnSend.ForeColor = SystemColors.ControlLightLight;
        btnSend.Location = new Point(505, 431);
        btnSend.Margin = new Padding(2, 2, 2, 2);
        btnSend.Name = "btnSend";
        btnSend.Size = new Size(67, 24);
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
        btnSendImage.Location = new Point(577, 431);
        btnSendImage.Margin = new Padding(2, 2, 2, 2);
        btnSendImage.Name = "btnSendImage";
        btnSendImage.Size = new Size(72, 24);
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
        btnEmoji.Location = new Point(473, 431);
        btnEmoji.Margin = new Padding(2, 2, 2, 2);
        btnEmoji.Name = "btnEmoji";
        btnEmoji.Size = new Size(27, 24);
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
        headerPanel.Margin = new Padding(2, 2, 2, 2);
        headerPanel.Name = "headerPanel";
        headerPanel.Padding = new Padding(8, 6, 8, 6);
        headerPanel.Size = new Size(804, 81);
        headerPanel.TabIndex = 6;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblTitle.ForeColor = Color.Yellow;
        lblTitle.Location = new Point(10, 5);
        lblTitle.Margin = new Padding(2, 0, 2, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(150, 41);
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
        headerRightPanel.Location = new Point(214, 5);
        headerRightPanel.Margin = new Padding(2, 2, 2, 2);
        headerRightPanel.Name = "headerRightPanel";
        headerRightPanel.Size = new Size(489, 68);
        headerRightPanel.TabIndex = 21;
        headerRightPanel.WrapContents = false;
        headerRightPanel.Paint += headerRightPanel_Paint;
        // 
        // lblLoggedIn
        // 
        lblLoggedIn.AutoEllipsis = true;
        lblLoggedIn.AutoSize = true;
        lblLoggedIn.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        lblLoggedIn.ForeColor = Color.Yellow;
        lblLoggedIn.Location = new Point(5, 9);
        lblLoggedIn.Margin = new Padding(5, 9, 10, 9);
        lblLoggedIn.MaximumSize = new Size(176, 0);
        lblLoggedIn.Name = "lblLoggedIn";
        lblLoggedIn.Size = new Size(96, 20);
        lblLoggedIn.TabIndex = 21;
        lblLoggedIn.Text = "Not logged in";
        // 
        // lblServerIp
        // 
        lblServerIp.AutoSize = true;
        lblServerIp.ForeColor = Color.Yellow;
        lblServerIp.Location = new Point(121, 9);
        lblServerIp.Margin = new Padding(10, 9, 5, 9);
        lblServerIp.Name = "lblServerIp";
        lblServerIp.Size = new Size(69, 20);
        lblServerIp.TabIndex = 22;
        lblServerIp.Text = "IP Server:";
        // 
        // label2
        // 
        label2.AutoEllipsis = true;
        label2.AutoSize = true;
        label2.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        label2.ForeColor = Color.Yellow;
        label2.Location = new Point(200, 9);
        label2.Margin = new Padding(5, 9, 10, 9);
        label2.MaximumSize = new Size(176, 0);
        label2.Name = "label2";
        label2.Size = new Size(27, 20);
        label2.TabIndex = 22;
        label2.Text = "---";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.ForeColor = Color.Yellow;
        label1.Location = new Point(247, 9);
        label1.Margin = new Padding(10, 9, 5, 9);
        label1.Name = "label1";
        label1.Size = new Size(38, 20);
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
        label3.Location = new Point(295, 9);
        label3.Margin = new Padding(5, 9, 10, 9);
        label3.MaximumSize = new Size(176, 0);
        label3.Name = "label3";
        label3.Size = new Size(27, 20);
        label3.TabIndex = 28;
        label3.Text = "---";
        // 
        // panel1
        // 
        panel1.Controls.Add(btnLogoutChat);
        panel1.Dock = DockStyle.Right;
        panel1.Location = new Point(707, 6);
        panel1.Margin = new Padding(2, 2, 2, 2);
        panel1.Name = "panel1";
        panel1.Size = new Size(89, 69);
        panel1.TabIndex = 29;
        // 
        // btnLogoutChat
        // 
        btnLogoutChat.AutoSize = true;
        btnLogoutChat.BackColor = Color.FromArgb(128, 128, 255);
        btnLogoutChat.Font = new Font("Segoe UI", 9F);
        btnLogoutChat.ForeColor = Color.White;
        btnLogoutChat.Location = new Point(7, 8);
        btnLogoutChat.Margin = new Padding(5, 8, 5, 8);
        btnLogoutChat.Name = "btnLogoutChat";
        btnLogoutChat.Size = new Size(79, 34);
        btnLogoutChat.TabIndex = 24;
        btnLogoutChat.Text = "Logout";
        btnLogoutChat.UseVisualStyleBackColor = false;
        btnLogoutChat.Click += btnLogout_Click;
        // 
        // Form1
        // 
        AcceptButton = btnSend;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(0, 0, 64);
        ClientSize = new Size(804, 472);
        Controls.Add(btnSendImage);
        Controls.Add(btnSend);
        Controls.Add(btnEmoji);
        Controls.Add(txtMessage);
        Controls.Add(rtbChat);
        Controls.Add(lstUsers);
        Controls.Add(headerPanel);
        ForeColor = SystemColors.Window;
        Margin = new Padding(2, 2, 2, 2);
        MinimumSize = new Size(578, 373);
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