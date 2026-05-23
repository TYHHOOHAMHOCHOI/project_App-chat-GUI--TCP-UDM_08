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
        dgvUsers = new DataGridView();
        rtbChat = new RichTextBox();
        txtMessage = new TextBox();
        btnSend = new Button();
        btnSendImage = new Button();
        btnEmoji = new Button();
        headerPanel = new Panel();
        lblTitle = new Label();
        lblLoggedIn = new Label();
        lblServerIp = new Label();
        label1 = new Label();
        btnLogoutChat = new Button();
        conection = new Button();
        label2 = new Label();
        label3 = new Label();
        unconection = new Button();
        txtUsername = new TextBox();
        textBox1 = new TextBox();
        textBox2 = new TextBox();
        textBox3 = new TextBox();
        colID = new DataGridViewTextBoxColumn();
        colName = new DataGridViewTextBoxColumn();
        colChat = new DataGridViewTextBoxColumn();
        ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
        headerPanel.SuspendLayout();
        SuspendLayout();
        // 
        // dgvUsers
        // 
        dgvUsers.AllowUserToAddRows = false;
        dgvUsers.AllowUserToDeleteRows = false;
        dgvUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvUsers.BackgroundColor = SystemColors.Window;
        dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvUsers.Columns.AddRange(new DataGridViewColumn[] { colID, colName, colChat });
        dgvUsers.Font = new Font("Segoe UI", 9F);
        dgvUsers.Location = new Point(1070, 216);
        dgvUsers.Name = "dgvUsers";
        dgvUsers.ReadOnly = true;
        dgvUsers.RowHeadersVisible = false;
        dgvUsers.RowHeadersWidth = 51;
        dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvUsers.Size = new Size(452, 611);
        dgvUsers.TabIndex = 0;
        dgvUsers.CellClick += dgvUsers_CellClick;
        dgvUsers.CellContentClick += dgvUsers_CellContentClick;
        // 
        // rtbChat
        // 
        rtbChat.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        rtbChat.BackColor = SystemColors.Control;
        rtbChat.Font = new Font("Segoe UI", 10F);
        rtbChat.ForeColor = SystemColors.Window;
        rtbChat.Location = new Point(16, 216);
        rtbChat.Name = "rtbChat";
        rtbChat.ReadOnly = true;
        rtbChat.Size = new Size(1025, 611);
        rtbChat.TabIndex = 1;
        rtbChat.Text = "";
        // 
        // txtMessage
        // 
        txtMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtMessage.BackColor = Color.White;
        txtMessage.Font = new Font("Segoe UI", 9F);
        txtMessage.ForeColor = Color.Black;
        txtMessage.Location = new Point(16, 844);
        txtMessage.Name = "txtMessage";
        txtMessage.Size = new Size(918, 39);
        txtMessage.TabIndex = 2;
        // 
        // btnSend
        // 
        btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSend.BackColor = Color.White;
        btnSend.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnSend.ForeColor = Color.Black;
        btnSend.Location = new Point(994, 846);
        btnSend.Name = "btnSend";
        btnSend.Size = new Size(124, 42);
        btnSend.TabIndex = 4;
        btnSend.Text = "Gửi";
        btnSend.UseVisualStyleBackColor = false;
        btnSend.Click += btnSend_Click;
        // 
        // btnSendImage
        // 
        btnSendImage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSendImage.BackColor = Color.White;
        btnSendImage.Font = new Font("Segoe UI", 9F);
        btnSendImage.ForeColor = Color.Black;
        btnSendImage.Location = new Point(1124, 847);
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
        btnEmoji.BackColor = SystemColors.InactiveCaption;
        btnEmoji.Font = new Font("Segoe UI Emoji", 10F);
        btnEmoji.ForeColor = Color.Black;
        btnEmoji.Location = new Point(943, 843);
        btnEmoji.Name = "btnEmoji";
        btnEmoji.Size = new Size(44, 53);
        btnEmoji.TabIndex = 3;
        btnEmoji.Text = "😀";
        btnEmoji.UseVisualStyleBackColor = false;
        btnEmoji.Click += btnEmoji_Click;
        // 
        // headerPanel
        // 
        headerPanel.BackColor = SystemColors.InactiveCaption;
        headerPanel.Controls.Add(textBox3);
        headerPanel.Controls.Add(textBox2);
        headerPanel.Controls.Add(textBox1);
        headerPanel.Controls.Add(txtUsername);
        headerPanel.Controls.Add(unconection);
        headerPanel.Controls.Add(label3);
        headerPanel.Controls.Add(label2);
        headerPanel.Controls.Add(conection);
        headerPanel.Controls.Add(label1);
        headerPanel.Controls.Add(lblLoggedIn);
        headerPanel.Controls.Add(lblServerIp);
        headerPanel.Controls.Add(lblTitle);
        headerPanel.Dock = DockStyle.Top;
        headerPanel.Location = new Point(0, 0);
        headerPanel.Name = "headerPanel";
        headerPanel.Padding = new Padding(13, 10, 13, 10);
        headerPanel.Size = new Size(1535, 210);
        headerPanel.TabIndex = 6;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblTitle.ForeColor = Color.Black;
        lblTitle.Location = new Point(16, 8);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(239, 65);
        lblTitle.TabIndex = 20;
        lblTitle.Text = "Chat App";
        // 
        // lblLoggedIn
        // 
        lblLoggedIn.AutoEllipsis = true;
        lblLoggedIn.AutoSize = true;
        lblLoggedIn.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        lblLoggedIn.ForeColor = Color.Black;
        lblLoggedIn.Location = new Point(34, 98);
        lblLoggedIn.Margin = new Padding(8, 14, 16, 14);
        lblLoggedIn.MaximumSize = new Size(286, 0);
        lblLoggedIn.Name = "lblLoggedIn";
        lblLoggedIn.Size = new Size(156, 32);
        lblLoggedIn.TabIndex = 21;
        lblLoggedIn.Text = "Not logged in";
        // 
        // lblServerIp
        // 
        lblServerIp.AutoSize = true;
        lblServerIp.ForeColor = Color.Black;
        lblServerIp.Location = new Point(289, 91);
        lblServerIp.Margin = new Padding(16, 14, 8, 14);
        lblServerIp.Name = "lblServerIp";
        lblServerIp.Size = new Size(112, 32);
        lblServerIp.TabIndex = 22;
        lblServerIp.Text = "IP Server:";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.ForeColor = Color.Black;
        label1.Location = new Point(744, 21);
        label1.Margin = new Padding(16, 14, 8, 14);
        label1.Name = "label1";
        label1.Size = new Size(61, 32);
        label1.TabIndex = 26;
        label1.Text = "Port:";
        label1.Click += label1_Click;
        // 
        // btnLogoutChat
        // 
        btnLogoutChat.AutoSize = true;
        btnLogoutChat.BackColor = Color.LightSkyBlue;
        btnLogoutChat.Font = new Font("Segoe UI", 9F);
        btnLogoutChat.ForeColor = Color.White;
        btnLogoutChat.Location = new Point(1351, 837);
        btnLogoutChat.Margin = new Padding(8, 13, 8, 13);
        btnLogoutChat.Name = "btnLogoutChat";
        btnLogoutChat.Size = new Size(161, 67);
        btnLogoutChat.TabIndex = 24;
        btnLogoutChat.Text = "Logout";
        btnLogoutChat.UseVisualStyleBackColor = false;
        btnLogoutChat.Click += btnLogout_Click;
        // 
        // conection
        // 
        conection.ForeColor = Color.Black;
        conection.Location = new Point(1084, 7);
        conection.Name = "conection";
        conection.Size = new Size(187, 46);
        conection.TabIndex = 31;
        conection.Text = "Mở kết nối";
        conection.UseVisualStyleBackColor = true;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.ForeColor = Color.Black;
        label2.Location = new Point(278, 24);
        label2.Margin = new Padding(16, 14, 8, 14);
        label2.Name = "label2";
        label2.Size = new Size(126, 32);
        label2.TabIndex = 32;
        label2.Text = "Username:";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.ForeColor = Color.Black;
        label3.Location = new Point(747, 91);
        label3.Margin = new Padding(16, 14, 8, 14);
        label3.Name = "label3";
        label3.Size = new Size(58, 32);
        label3.TabIndex = 33;
        label3.Text = "Key:";
        // 
        // unconection
        // 
        unconection.ForeColor = Color.Black;
        unconection.Location = new Point(1084, 98);
        unconection.Name = "unconection";
        unconection.Size = new Size(187, 46);
        unconection.TabIndex = 34;
        unconection.Text = "Ngắt kết nối";
        unconection.UseVisualStyleBackColor = true;
        // 
        // txtUsername
        // 
        txtUsername.Location = new Point(427, 14);
        txtUsername.Margin = new Padding(5);
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(308, 39);
        txtUsername.TabIndex = 35;
        // 
        // textBox1
        // 
        textBox1.Location = new Point(818, 98);
        textBox1.Margin = new Padding(5);
        textBox1.Name = "textBox1";
        textBox1.Size = new Size(154, 39);
        textBox1.TabIndex = 36;
        // 
        // textBox2
        // 
        textBox2.Location = new Point(427, 98);
        textBox2.Margin = new Padding(5);
        textBox2.Name = "textBox2";
        textBox2.Size = new Size(308, 39);
        textBox2.TabIndex = 37;
        textBox2.Text = "127.0.0.1";
        // 
        // textBox3
        // 
        textBox3.Location = new Point(818, 14);
        textBox3.Margin = new Padding(5);
        textBox3.Name = "textBox3";
        textBox3.Size = new Size(154, 39);
        textBox3.TabIndex = 38;
        // 
        // ID
        // 
        colID.HeaderText = "ID";
        colID.MinimumWidth = 10;
        colID.Name = "ID";
        colID.ReadOnly = true;
        // 
        // Name
        // 
        colName.HeaderText = "Name";
        colName.MinimumWidth = 10;
        colName.Name = "Name";
        colName.ReadOnly = true;
        // 
        // Chat
        // 
        colChat.HeaderText = "Gửi tin nhắn";
        colChat.MinimumWidth = 10;
        colChat.Name = "Chat";
        colChat.ReadOnly = true;
        // 
        // Form1
        // 
        AcceptButton = btnSend;
        AutoScaleDimensions = new SizeF(13F, 32F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = SystemColors.InactiveCaption;
        ClientSize = new Size(1535, 908);
        Controls.Add(btnSendImage);
        Controls.Add(btnSend);
        Controls.Add(btnEmoji);
        Controls.Add(txtMessage);
        Controls.Add(rtbChat);
        Controls.Add(btnLogoutChat);
        Controls.Add(dgvUsers);
        Controls.Add(headerPanel);
        ForeColor = SystemColors.Window;
        MinimumSize = new Size(923, 554);
        Name = "Form1";
        Text = "Chat Client";
        Load += Form1_Load;
        ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
        headerPanel.ResumeLayout(false);
        headerPanel.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.DataGridView dgvUsers;
    private System.Windows.Forms.RichTextBox rtbChat;
    private System.Windows.Forms.TextBox txtMessage;
    private System.Windows.Forms.Button btnSend;
    private System.Windows.Forms.Button btnSendImage;
    private System.Windows.Forms.Button btnEmoji;
    private System.Windows.Forms.Panel headerPanel;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblLoggedIn;
    private System.Windows.Forms.Label lblServerIp;
    private System.Windows.Forms.Button btnLogoutChat;
    private Label label1;
    private Button conection;
    private Label label2;
    private Label label3;
    private Button unconection;
    private TextBox textBox3;
    private TextBox textBox2;
    private TextBox textBox1;
    private TextBox txtUsername;
    private DataGridViewTextBoxColumn colID;
    private DataGridViewTextBoxColumn colName;
    private DataGridViewTextBoxColumn colChat;
}