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
        colID = new DataGridViewTextBoxColumn();
        colName = new DataGridViewTextBoxColumn();
        colChat = new DataGridViewTextBoxColumn();
        chatBubblePanel = new ChatBubblePanel();
        txtMessage = new PasteTextBox();
        btnSend = new Button();
        btnEmoji = new Button();
        headerPanel = new Panel();
        btnManageAvatar = new Button();
        pbUserAvatar = new PictureBox();
        btnPublic = new Button();
        textBox3 = new TextBox();
        textBox2 = new TextBox();
        txtkey = new TextBox();
        txtUsername = new TextBox();
        unconection = new Button();
        label3 = new Label();
        label2 = new Label();
        conection = new Button();
        label1 = new Label();
        lblLoggedIn = new Label();
        lblServerIp = new Label();
        lblTitle = new Label();
        btnLogoutChat = new Button();
        btnLoadHistory = new Button();
        ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
        headerPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pbUserAvatar).BeginInit();
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
        dgvUsers.Location = new Point(658, 135);
        dgvUsers.Margin = new Padding(2, 2, 2, 2);
        dgvUsers.Name = "dgvUsers";
        dgvUsers.ReadOnly = true;
        dgvUsers.RowHeadersVisible = false;
        dgvUsers.RowHeadersWidth = 51;
        dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvUsers.Size = new Size(278, 382);
        dgvUsers.TabIndex = 0;
        dgvUsers.CellClick += dgvUsers_CellClick;
        dgvUsers.CellContentClick += dgvUsers_CellContentClick;
        // 
        // colID
        // 
        colID.HeaderText = "ID";
        colID.MinimumWidth = 10;
        colID.Name = "colID";
        colID.ReadOnly = true;
        // 
        // colName
        // 
        colName.HeaderText = "Name";
        colName.MinimumWidth = 10;
        colName.Name = "colName";
        colName.ReadOnly = true;
        // 
        // colChat
        // 
        colChat.HeaderText = "Gửi tin nhắn";
        colChat.MinimumWidth = 10;
        colChat.Name = "colChat";
        colChat.ReadOnly = true;
        // 
        // chatBubblePanel
        // 
        chatBubblePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        chatBubblePanel.AutoScroll = true;
        chatBubblePanel.BackColor = Color.White;
        chatBubblePanel.FlowDirection = FlowDirection.TopDown;
        chatBubblePanel.Location = new Point(10, 135);
        chatBubblePanel.Margin = new Padding(2, 2, 2, 2);
        chatBubblePanel.Name = "chatBubblePanel";
        chatBubblePanel.Size = new Size(630, 381);
        chatBubblePanel.TabIndex = 1;
        chatBubblePanel.WrapContents = false;
        // 
        // txtMessage
        // 
        txtMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtMessage.BackColor = Color.White;
        txtMessage.Font = new Font("Segoe UI Emoji", 9F);
        txtMessage.ForeColor = Color.Black;
        txtMessage.Location = new Point(10, 528);
        txtMessage.Margin = new Padding(2, 2, 2, 2);
        txtMessage.Name = "txtMessage";
        txtMessage.Size = new Size(566, 27);
        txtMessage.TabIndex = 2;
        // 
        // btnSend
        // 
        btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSend.BackColor = Color.White;
        btnSend.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnSend.ForeColor = Color.Black;
        btnSend.Location = new Point(612, 529);
        btnSend.Margin = new Padding(2, 2, 2, 2);
        btnSend.Name = "btnSend";
        btnSend.Size = new Size(76, 26);
        btnSend.TabIndex = 4;
        btnSend.Text = "Gửi";
        btnSend.UseVisualStyleBackColor = false;
        btnSend.Click += btnSend_Click;
        // 
        // btnEmoji
        // 
        btnEmoji.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnEmoji.BackColor = SystemColors.InactiveCaption;
        btnEmoji.Font = new Font("Segoe UI Emoji", 10F);
        btnEmoji.ForeColor = Color.Black;
        btnEmoji.Location = new Point(580, 527);
        btnEmoji.Margin = new Padding(2, 2, 2, 2);
        btnEmoji.Name = "btnEmoji";
        btnEmoji.Size = new Size(27, 33);
        btnEmoji.TabIndex = 3;
        btnEmoji.Text = "😀";
        btnEmoji.UseVisualStyleBackColor = false;
        btnEmoji.Click += btnEmoji_Click;
        // 
        // headerPanel
        // 
        headerPanel.BackColor = SystemColors.InactiveCaption;
        headerPanel.Controls.Add(btnManageAvatar);
        headerPanel.Controls.Add(pbUserAvatar);
        headerPanel.Controls.Add(btnPublic);
        headerPanel.Controls.Add(textBox3);
        headerPanel.Controls.Add(textBox2);
        headerPanel.Controls.Add(txtkey);
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
        headerPanel.Margin = new Padding(2, 2, 2, 2);
        headerPanel.Name = "headerPanel";
        headerPanel.Padding = new Padding(8, 6, 8, 6);
        headerPanel.Size = new Size(945, 131);
        headerPanel.TabIndex = 6;
        headerPanel.Paint += headerPanel_Paint;
        // 
        // btnManageAvatar
        // 
        btnManageAvatar.BackColor = Color.LightBlue;
        btnManageAvatar.Font = new Font("Segoe UI", 8F);
        btnManageAvatar.Location = new Point(835, 64);
        btnManageAvatar.Margin = new Padding(2, 2, 2, 2);
        btnManageAvatar.Name = "btnManageAvatar";
        btnManageAvatar.Size = new Size(70, 28);
        btnManageAvatar.TabIndex = 40;
        btnManageAvatar.Text = "Quản lý Avatar";
        btnManageAvatar.UseVisualStyleBackColor = false;
        btnManageAvatar.Click += btnManageAvatar_Click;
        // 
        // pbUserAvatar
        // 
        pbUserAvatar.BackColor = SystemColors.Control;
        pbUserAvatar.BorderStyle = BorderStyle.FixedSingle;
        pbUserAvatar.Cursor = Cursors.Hand;
        pbUserAvatar.Location = new Point(855, 9);
        pbUserAvatar.Margin = new Padding(2, 2, 2, 2);
        pbUserAvatar.Name = "pbUserAvatar";
        pbUserAvatar.Size = new Size(50, 51);
        pbUserAvatar.SizeMode = PictureBoxSizeMode.StretchImage;
        pbUserAvatar.TabIndex = 39;
        pbUserAvatar.TabStop = false;
        // 
        // btnPublic
        // 
        btnPublic.ForeColor = Color.Black;
        btnPublic.Location = new Point(843, 94);
        btnPublic.Margin = new Padding(2, 2, 2, 2);
        btnPublic.Name = "btnPublic";
        btnPublic.Size = new Size(92, 29);
        btnPublic.TabIndex = 25;
        btnPublic.Text = "nhắn chung";
        btnPublic.UseVisualStyleBackColor = true;
        btnPublic.Click += btnPublic_Click_1;
        // 
        // textBox3
        // 
        textBox3.Location = new Point(503, 9);
        textBox3.Name = "textBox3";
        textBox3.Size = new Size(96, 27);
        textBox3.TabIndex = 38;
        // 
        // textBox2
        // 
        textBox2.Location = new Point(263, 57);
        textBox2.Name = "textBox2";
        textBox2.Size = new Size(191, 27);
        textBox2.TabIndex = 37;
        textBox2.Text = "127.0.0.1";
        // 
        // txtkey
        // 
        txtkey.Location = new Point(503, 57);
        txtkey.Name = "txtkey";
        txtkey.Size = new Size(96, 27);
        txtkey.TabIndex = 36;
        txtkey.TextChanged += txtkey_TextChanged;
        // 
        // txtUsername
        // 
        txtUsername.Location = new Point(263, 9);
        txtUsername.Name = "txtUsername";
        txtUsername.ReadOnly = true;
        txtUsername.Size = new Size(191, 27);
        txtUsername.TabIndex = 35;
        txtUsername.TextChanged += txtUsername_TextChanged;
        // 
        // unconection
        // 
        unconection.ForeColor = Color.Black;
        unconection.Location = new Point(667, 61);
        unconection.Margin = new Padding(2, 2, 2, 2);
        unconection.Name = "unconection";
        unconection.Size = new Size(115, 29);
        unconection.TabIndex = 34;
        unconection.Text = "Ngắt kết nối";
        unconection.UseVisualStyleBackColor = true;
        unconection.Click += unconection_Click;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.ForeColor = Color.Black;
        label3.Location = new Point(460, 57);
        label3.Margin = new Padding(10, 9, 5, 9);
        label3.Name = "label3";
        label3.Size = new Size(36, 20);
        label3.TabIndex = 33;
        label3.Text = "Key:";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.ForeColor = Color.Black;
        label2.Location = new Point(171, 15);
        label2.Margin = new Padding(10, 9, 5, 9);
        label2.Name = "label2";
        label2.Size = new Size(78, 20);
        label2.TabIndex = 32;
        label2.Text = "Username:";
        // 
        // conection
        // 
        conection.ForeColor = Color.Black;
        conection.Location = new Point(667, 4);
        conection.Margin = new Padding(2, 2, 2, 2);
        conection.Name = "conection";
        conection.Size = new Size(115, 29);
        conection.TabIndex = 31;
        conection.Text = "Kết nối";
        conection.UseVisualStyleBackColor = true;
        conection.Click += conection_Click;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.ForeColor = Color.Black;
        label1.Location = new Point(458, 13);
        label1.Margin = new Padding(10, 9, 5, 9);
        label1.Name = "label1";
        label1.Size = new Size(38, 20);
        label1.TabIndex = 26;
        label1.Text = "Port:";
        label1.Click += label1_Click;
        // 
        // lblLoggedIn
        // 
        lblLoggedIn.AutoEllipsis = true;
        lblLoggedIn.AutoSize = true;
        lblLoggedIn.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        lblLoggedIn.ForeColor = Color.Black;
        lblLoggedIn.Location = new Point(21, 61);
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
        lblServerIp.ForeColor = Color.Black;
        lblServerIp.Location = new Point(178, 57);
        lblServerIp.Margin = new Padding(10, 9, 5, 9);
        lblServerIp.Name = "lblServerIp";
        lblServerIp.Size = new Size(69, 20);
        lblServerIp.TabIndex = 22;
        lblServerIp.Text = "IP Server:";
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblTitle.ForeColor = Color.Black;
        lblTitle.Location = new Point(10, 5);
        lblTitle.Margin = new Padding(2, 0, 2, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(150, 41);
        lblTitle.TabIndex = 20;
        lblTitle.Text = "Chat App";
        // 
        // btnLogoutChat
        // 
        btnLogoutChat.AutoSize = true;
        btnLogoutChat.BackColor = Color.LightSkyBlue;
        btnLogoutChat.Font = new Font("Segoe UI", 9F);
        btnLogoutChat.ForeColor = Color.White;
        btnLogoutChat.Location = new Point(831, 523);
        btnLogoutChat.Margin = new Padding(5, 8, 5, 8);
        btnLogoutChat.Name = "btnLogoutChat";
        btnLogoutChat.Size = new Size(99, 42);
        btnLogoutChat.TabIndex = 24;
        btnLogoutChat.Text = "Logout";
        btnLogoutChat.UseVisualStyleBackColor = false;
        btnLogoutChat.Click += btnLogout_Click;
        // 
        // btnLoadHistory
        // 
        btnLoadHistory.ForeColor = Color.Black;
        btnLoadHistory.Location = new Point(702, 528);
        btnLoadHistory.Margin = new Padding(2, 2, 2, 2);
        btnLoadHistory.Name = "btnLoadHistory";
        btnLoadHistory.Size = new Size(105, 26);
        btnLoadHistory.TabIndex = 25;
        btnLoadHistory.Text = "Load tin nhắn";
        btnLoadHistory.UseVisualStyleBackColor = true;
        btnLoadHistory.Click += btnLoadHistory_Click;
        // 
        // Form1
        // 
        AcceptButton = btnSend;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = SystemColors.InactiveCaption;
        ClientSize = new Size(945, 568);
        Controls.Add(btnSend);
        Controls.Add(btnEmoji);
        Controls.Add(txtMessage);
        Controls.Add(chatBubblePanel);
        Controls.Add(btnLogoutChat);
        Controls.Add(btnLoadHistory);
        Controls.Add(dgvUsers);
        Controls.Add(headerPanel);
        ForeColor = SystemColors.Window;
        Margin = new Padding(2, 2, 2, 2);
        MinimumSize = new Size(572, 355);
        Name = "Form1";
        Text = "Chat Client";
        Load += Form1_Load;
        ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
        headerPanel.ResumeLayout(false);
        headerPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)pbUserAvatar).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.DataGridView dgvUsers;
    private ChatBubblePanel chatBubblePanel;
    private PasteTextBox txtMessage;
    private System.Windows.Forms.Button btnSend;
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
    private TextBox txtkey;
    private TextBox txtUsername;
    private DataGridViewTextBoxColumn colID;
    private DataGridViewTextBoxColumn colName;
    private DataGridViewTextBoxColumn colChat;
    private Button btnPublic;
    private Button btnLoadHistory;
    private PictureBox pbUserAvatar;
    private Button btnManageAvatar;
}
