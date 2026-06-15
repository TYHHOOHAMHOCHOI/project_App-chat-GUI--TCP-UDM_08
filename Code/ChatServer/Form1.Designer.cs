namespace ChatServer;

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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
        txtSearchUser = new TextBox();
        btnLoadPublic = new Button();
        btnLoadPrivate = new Button();
        btnOpenServer = new Button();
        btnClear = new Button();
        lableUsername = new Label();
        labelAddress = new Label();
        txtUsername = new TextBox();
        txtAddress = new TextBox();
        labelPort = new Label();
        labelKey = new Label();
        txtPort = new TextBox();
        txtKey = new TextBox();
        btnDisconectAll = new Button();
        rtbLog = new RichTextBox();
        label1 = new Label();
        txtMessage = new TextBox();
        btnSend = new Button();
        pictureBox1 = new PictureBox();
        label2 = new Label();
        dgvClients = new DataGridView();
        colAvatar = new DataGridViewImageColumn();
        colID = new DataGridViewTextBoxColumn();
        colName = new DataGridViewTextBoxColumn();
        colKick = new DataGridViewButtonColumn();
        colSend = new DataGridViewButtonColumn();
        lblSoClient = new Label();
        button1 = new Button();
        checkBox1 = new CheckBox();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvClients).BeginInit();
        SuspendLayout();
        // 
        // txtSearchUser
        // 
        txtSearchUser.Location = new Point(270, 126);
        txtSearchUser.Name = "txtSearchUser";
        txtSearchUser.PlaceholderText = "Tên người dùng";
        txtSearchUser.Size = new Size(120, 27);
        txtSearchUser.TabIndex = 23;
        // 
        // btnLoadPublic
        // 
        btnLoadPublic.Location = new Point(140, 156);
        btnLoadPublic.Name = "btnLoadPublic";
        btnLoadPublic.Size = new Size(120, 30);
        btnLoadPublic.TabIndex = 21;
        btnLoadPublic.Text = "Load Chung";
        btnLoadPublic.Click += btnLoadPublic_Click;
        // 
        // btnLoadPrivate
        // 
        btnLoadPrivate.Location = new Point(270, 156);
        btnLoadPrivate.Name = "btnLoadPrivate";
        btnLoadPrivate.Size = new Size(120, 30);
        btnLoadPrivate.TabIndex = 22;
        btnLoadPrivate.Text = "Load Riêng";
        btnLoadPrivate.Click += btnLoadPrivate_Click;
        // 
        // btnOpenServer
        // 
        btnOpenServer.AccessibleRole = AccessibleRole.None;
        btnOpenServer.Location = new Point(12, 107);
        btnOpenServer.Name = "btnOpenServer";
        btnOpenServer.Size = new Size(110, 29);
        btnOpenServer.TabIndex = 0;
        btnOpenServer.Text = "Mở kết nối ";
        btnOpenServer.UseVisualStyleBackColor = true;
        btnOpenServer.Click += btnOpenServer_Click;
        // 
        // btnClear
        // 
        btnClear.Location = new Point(12, 156);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(110, 29);
        btnClear.TabIndex = 1;
        btnClear.Text = "Xóa tin nhắn";
        btnClear.UseVisualStyleBackColor = true;
        btnClear.Click += btnClear_Click;
        // 
        // lableUsername
        // 
        lableUsername.AutoSize = true;
        lableUsername.Location = new Point(143, 25);
        lableUsername.Name = "lableUsername";
        lableUsername.Size = new Size(78, 20);
        lableUsername.TabIndex = 2;
        lableUsername.Text = "Username:";
        lableUsername.Click += label1_Click;
        // 
        // labelAddress
        // 
        labelAddress.AutoSize = true;
        labelAddress.Location = new Point(156, 65);
        labelAddress.Name = "labelAddress";
        labelAddress.Size = new Size(65, 20);
        labelAddress.TabIndex = 3;
        labelAddress.Text = "Address:";
        // 
        // txtUsername
        // 
        txtUsername.Location = new Point(227, 22);
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(191, 27);
        txtUsername.TabIndex = 4;
        txtUsername.TextChanged += txtUsername_TextChanged;
        // 
        // txtAddress
        // 
        txtAddress.Location = new Point(227, 65);
        txtAddress.Name = "txtAddress";
        txtAddress.ReadOnly = true;
        txtAddress.Size = new Size(191, 27);
        txtAddress.TabIndex = 5;
        txtAddress.Text = "0.0.0.0";
        txtAddress.TextChanged += txtAddress_TextChanged;
        // 
        // labelPort
        // 
        labelPort.AutoSize = true;
        labelPort.Location = new Point(464, 25);
        labelPort.Name = "labelPort";
        labelPort.Size = new Size(38, 20);
        labelPort.TabIndex = 6;
        labelPort.Text = "Port:";
        labelPort.Click += labelPort_Click;
        // 
        // labelKey
        // 
        labelKey.AutoSize = true;
        labelKey.Location = new Point(464, 68);
        labelKey.Name = "labelKey";
        labelKey.Size = new Size(36, 20);
        labelKey.TabIndex = 7;
        labelKey.Text = "Key:";
        // 
        // txtPort
        // 
        txtPort.Location = new Point(508, 22);
        txtPort.Name = "txtPort";
        txtPort.Size = new Size(125, 27);
        txtPort.TabIndex = 8;
        txtPort.TextChanged += txtPort_TextChanged;
        // 
        // txtKey
        // 
        txtKey.Location = new Point(508, 65);
        txtKey.Name = "txtKey";
        txtKey.Size = new Size(125, 27);
        txtKey.TabIndex = 9;
        txtKey.TextChanged += txtKey_TextChanged;
        // 
        // btnDisconectAll
        // 
        btnDisconectAll.Enabled = false;
        btnDisconectAll.Location = new Point(497, 156);
        btnDisconectAll.Name = "btnDisconectAll";
        btnDisconectAll.Size = new Size(136, 29);
        btnDisconectAll.TabIndex = 10;
        btnDisconectAll.Text = "Ngắt kết nối";
        btnDisconectAll.UseVisualStyleBackColor = true;
        btnDisconectAll.Click += btnDisconectAll_Click;
        // 
        // rtbLog
        // 
        rtbLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        rtbLog.Location = new Point(12, 191);
        rtbLog.Name = "rtbLog";
        rtbLog.ReadOnly = true;
        rtbLog.ScrollBars = RichTextBoxScrollBars.Vertical;
        rtbLog.Size = new Size(621, 370);
        rtbLog.TabIndex = 11;
        rtbLog.Text = "";
        rtbLog.TextChanged += rtbLog_TextChanged;
        // 
        // label1
        // 
        label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        label1.AutoSize = true;
        label1.Location = new Point(12, 571);
        label1.Name = "label1";
        label1.Size = new Size(102, 20);
        label1.TabIndex = 12;
        label1.Text = "Nhập tin nhắn";
        // 
        // txtMessage
        // 
        txtMessage.AcceptsReturn = true;
        txtMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtMessage.Location = new Point(12, 598);
        txtMessage.Multiline = true;
        txtMessage.Name = "txtMessage";
        txtMessage.ScrollBars = ScrollBars.Vertical;
        txtMessage.Size = new Size(392, 34);
        txtMessage.TabIndex = 13;
        txtMessage.TextChanged += txtMessage_TextChanged;
        txtMessage.KeyDown += txtMessage_KeyDown;
        // 
        // btnSend
        // 
        btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSend.Location = new Point(488, 598);
        btnSend.Name = "btnSend";
        btnSend.Size = new Size(101, 34);
        btnSend.TabIndex = 14;
        btnSend.Text = "Gửi";
        btnSend.UseVisualStyleBackColor = true;
        btnSend.Click += btnSend_Click;
        // 
        // pictureBox1
        // 
        pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
        pictureBox1.Location = new Point(18, 13);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(69, 76);
        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        pictureBox1.TabIndex = 15;
        pictureBox1.TabStop = false;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(87, 62);
        label2.Name = "label2";
        label2.Size = new Size(0, 20);
        label2.TabIndex = 16;
        // 
        // dgvClients
        // 
        dgvClients.AllowUserToAddRows = false;
        dgvClients.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        dgvClients.BackgroundColor = SystemColors.ButtonHighlight;
        dgvClients.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvClients.Columns.AddRange(new DataGridViewColumn[] { colAvatar, colID, colName, colKick, colSend });
        dgvClients.Location = new Point(639, 28);
        dgvClients.Name = "dgvClients";
        dgvClients.RowHeadersVisible = false;
        dgvClients.RowHeadersWidth = 51;
        dgvClients.Size = new Size(318, 533);
        dgvClients.TabIndex = 17;
        dgvClients.CellContentClick += dgvClients_CellContentClick;
        // 
        // colAvatar
        // 
        colAvatar.MinimumWidth = 6;
        colAvatar.Name = "colAvatar";
        colAvatar.Width = 125;
        // 
        // colID
        // 
        colID.HeaderText = "ID";
        colID.MinimumWidth = 6;
        colID.Name = "colID";
        colID.Width = 40;
        // 
        // colName
        // 
        colName.HeaderText = "Name";
        colName.MinimumWidth = 6;
        colName.Name = "colName";
        colName.Width = 60;
        // 
        // colKick
        // 
        colKick.HeaderText = "Disconnect";
        colKick.MinimumWidth = 6;
        colKick.Name = "colKick";
        colKick.Text = "Kick";
        colKick.UseColumnTextForButtonValue = true;
        colKick.Width = 90;
        // 
        // colSend
        // 
        colSend.HeaderText = "Gửi tin nhắn";
        colSend.MinimumWidth = 6;
        colSend.Name = "colSend";
        colSend.Text = "Gửi";
        colSend.UseColumnTextForButtonValue = true;
        colSend.Width = 125;
        // 
        // lblSoClient
        // 
        lblSoClient.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblSoClient.AutoSize = true;
        lblSoClient.Location = new Point(664, 564);
        lblSoClient.Name = "lblSoClient";
        lblSoClient.Size = new Size(81, 20);
        lblSoClient.TabIndex = 18;
        lblSoClient.Text = "Số client: 0";
        lblSoClient.Click += lblSoClient_Click;
        // 
        // button1
        // 
        button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        button1.BackColor = Color.Transparent;
        button1.FlatAppearance.BorderSize = 0;
        button1.FlatStyle = FlatStyle.Flat;
        button1.Font = new Font("Segoe UI Emoji", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
        button1.Image = (Image)resources.GetObject("button1.Image");
        button1.Location = new Point(421, 588);
        button1.Name = "button1";
        button1.Size = new Size(65, 46);
        button1.TabIndex = 19;
        button1.UseVisualStyleBackColor = false;
        button1.Click += button1_Click_1;
        button1.Paint += btnIcon_Paint;
        // 
        // checkBox1
        // 
        checkBox1.AutoSize = true;
        checkBox1.Location = new Point(508, 98);
        checkBox1.Name = "checkBox1";
        checkBox1.Size = new Size(49, 24);
        checkBox1.TabIndex = 20;
        checkBox1.Text = "Ẩn";
        checkBox1.UseVisualStyleBackColor = true;
        checkBox1.CheckedChanged += chkHideKey_CheckedChanged;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = SystemColors.InactiveCaption;
        ClientSize = new Size(966, 664);
        Controls.Add(checkBox1);
        Controls.Add(button1);
        Controls.Add(lblSoClient);
        Controls.Add(dgvClients);
        Controls.Add(label2);
        Controls.Add(pictureBox1);
        Controls.Add(btnSend);
        Controls.Add(txtMessage);
        Controls.Add(label1);
        Controls.Add(rtbLog);
        Controls.Add(btnDisconectAll);
        Controls.Add(txtKey);
        Controls.Add(txtPort);
        Controls.Add(labelKey);
        Controls.Add(labelPort);
        Controls.Add(txtAddress);
        Controls.Add(txtUsername);
        Controls.Add(labelAddress);
        Controls.Add(lableUsername);
        Controls.Add(btnClear);
        Controls.Add(btnOpenServer);
        Controls.Add(btnLoadPublic);
        Controls.Add(btnLoadPrivate);
        Controls.Add(txtSearchUser);
        Name = "Form1";
        Text = "Server";
        Load += Form1_Load;
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvClients).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private Button btnClear;
    private Label lableUsername;
    private Label labelAddress;
    private TextBox txtUsername;
    private TextBox txtAddress;
    private Label labelPort;
    private Label labelKey;
    private TextBox txtPort;
    private TextBox txtKey;
    private Button btnDisconectAll;
    private RichTextBox rtbLog;
    private Label label1;
    private TextBox txtMessage;
    private Button btnSend;
    private PictureBox pictureBox1;
    private Label label2;
    private DataGridView dgvClients;
    private Label lblSoClient;
    private DataGridViewTextBoxColumn colID;
    private DataGridViewTextBoxColumn colName;
    private DataGridViewImageColumn colAvatar;
    private DataGridViewButtonColumn colKick;
    private DataGridViewButtonColumn colSend;
    private Button button1;
    private Button btnOpenServer;
    private CheckBox checkBox1;


    private Button btnLoadPublic;
    private Button btnLoadPrivate;
    private TextBox txtSearchUser;
}
