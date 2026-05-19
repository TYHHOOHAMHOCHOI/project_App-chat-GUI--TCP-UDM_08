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
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        SuspendLayout();
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
        btnOpenServer.Click += button1_Click;
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
        lableUsername.Location = new Point(176, 28);
        lableUsername.Name = "lableUsername";
        lableUsername.Size = new Size(78, 20);
        lableUsername.TabIndex = 2;
        lableUsername.Text = "Username:";
        lableUsername.Click += label1_Click;
        // 
        // labelAddress
        // 
        labelAddress.AutoSize = true;
        labelAddress.Location = new Point(176, 65);
        labelAddress.Name = "labelAddress";
        labelAddress.Size = new Size(65, 20);
        labelAddress.TabIndex = 3;
        labelAddress.Text = "Address:";
        // 
        // txtUsername
        // 
        txtUsername.Location = new Point(260, 25);
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(191, 27);
        txtUsername.TabIndex = 4;
        // 
        // txtAddress
        // 
        txtAddress.Location = new Point(260, 62);
        txtAddress.Name = "txtAddress";
        txtAddress.ReadOnly = true;
        txtAddress.Size = new Size(191, 27);
        txtAddress.TabIndex = 5;
        txtAddress.Text = "127.0.0.1";
        // 
        // labelPort
        // 
        labelPort.AutoSize = true;
        labelPort.Location = new Point(481, 28);
        labelPort.Name = "labelPort";
        labelPort.Size = new Size(38, 20);
        labelPort.TabIndex = 6;
        labelPort.Text = "Port:";
        // 
        // labelKey
        // 
        labelKey.AutoSize = true;
        labelKey.Location = new Point(481, 65);
        labelKey.Name = "labelKey";
        labelKey.Size = new Size(36, 20);
        labelKey.TabIndex = 7;
        labelKey.Text = "Key:";
        // 
        // txtPort
        // 
        txtPort.Location = new Point(523, 25);
        txtPort.Name = "txtPort";
        txtPort.Size = new Size(125, 27);
        txtPort.TabIndex = 8;
        // 
        // txtKey
        // 
        txtKey.Location = new Point(523, 62);
        txtKey.Name = "txtKey";
        txtKey.Size = new Size(125, 27);
        txtKey.TabIndex = 9;
        // 
        // btnDisconectAll
        // 
        btnDisconectAll.Enabled = false;
        btnDisconectAll.Location = new Point(512, 156);
        btnDisconectAll.Name = "btnDisconectAll";
        btnDisconectAll.Size = new Size(136, 29);
        btnDisconectAll.TabIndex = 10;
        btnDisconectAll.Text = "Ngắt kết nối";
        btnDisconectAll.UseVisualStyleBackColor = true;
        // 
        // rtbLog
        // 
        rtbLog.Location = new Point(12, 191);
        rtbLog.Name = "rtbLog";
        rtbLog.ReadOnly = true;
        rtbLog.ScrollBars = RichTextBoxScrollBars.Vertical;
        rtbLog.Size = new Size(636, 347);
        rtbLog.TabIndex = 11;
        rtbLog.Text = "";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(12, 541);
        label1.Name = "label1";
        label1.Size = new Size(102, 20);
        label1.TabIndex = 12;
        label1.Text = "Nhập tin nhắn";
        // 
        // txtMessage
        // 
        txtMessage.AcceptsReturn = true;
        txtMessage.Enabled = false;
        txtMessage.Location = new Point(12, 564);
        txtMessage.Multiline = true;
        txtMessage.Name = "txtMessage";
        txtMessage.ScrollBars = ScrollBars.Vertical;
        txtMessage.Size = new Size(392, 34);
        txtMessage.TabIndex = 13;
        // 
        // btnSend
        // 
        btnSend.Location = new Point(523, 564);
        btnSend.Name = "btnSend";
        btnSend.Size = new Size(101, 34);
        btnSend.TabIndex = 14;
        btnSend.Text = "Gửi";
        btnSend.UseVisualStyleBackColor = true;
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
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = SystemColors.InactiveCaption;
        ClientSize = new Size(982, 603);
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
        Name = "Form1";
        Text = "Server";
        Load += Form1_Load;
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button btnOpenServer;
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
}
