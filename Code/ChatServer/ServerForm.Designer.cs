namespace ChatServer
{
    partial class ServerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtPort = new TextBox();
            btnStart = new Button();
            btnStop = new Button();
            lsvClients = new ListBox();
            btnDisconnectClient = new Button();
            folderBrowserDialog1 = new FolderBrowserDialog();
            txtMessage = new TextBox();
            btnSend = new Button();
            btnSendImage = new Button();
            rtbLog = new RichTextBox();
            SuspendLayout();
            // 
            // txtPort
            // 
            txtPort.Font = new Font("Segoe UI Light", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtPort.Location = new Point(25, 64);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(81, 27);
            txtPort.TabIndex = 0;
            txtPort.Text = "8888";
            // 
            // btnStart
            // 
            btnStart.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStart.Location = new Point(25, 109);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(94, 29);
            btnStart.TabIndex = 1;
            btnStart.Text = "Mở Server";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Enabled = false;
            btnStop.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStop.Location = new Point(140, 109);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(113, 29);
            btnStop.TabIndex = 2;
            btnStop.Text = "Đóng Server";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // lsvClients
            // 
            lsvClients.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lsvClients.FormattingEnabled = true;
            lsvClients.Location = new Point(25, 161);
            lsvClients.Name = "lsvClients";
            lsvClients.Size = new Size(150, 104);
            lsvClients.TabIndex = 3;
            lsvClients.SelectedIndexChanged += lsvClients_SelectedIndexChanged;
            // 
            // btnDisconnectClient
            // 
            btnDisconnectClient.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDisconnectClient.Location = new Point(25, 283);
            btnDisconnectClient.Name = "btnDisconnectClient";
            btnDisconnectClient.Size = new Size(163, 29);
            btnDisconnectClient.TabIndex = 4;
            btnDisconnectClient.Text = "Disconnect Clients";
            btnDisconnectClient.UseVisualStyleBackColor = true;
            btnDisconnectClient.Click += btnDisconnectClient_Click;
            // 
            // txtMessage
            // 
            txtMessage.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            txtMessage.Location = new Point(308, 285);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(125, 27);
            txtMessage.TabIndex = 6;
            txtMessage.Text = "Nhập tin nhắn";
            txtMessage.TextChanged += txtMessage_TextChanged;
            // 
            // btnSend
            // 
            btnSend.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSend.Location = new Point(308, 330);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(94, 29);
            btnSend.TabIndex = 7;
            btnSend.Text = "Gửi";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // btnSendImage
            // 
            btnSendImage.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSendImage.Location = new Point(460, 330);
            btnSendImage.Name = "btnSendImage";
            btnSendImage.Size = new Size(94, 29);
            btnSendImage.TabIndex = 8;
            btnSendImage.Text = "Gửi ảnh";
            btnSendImage.UseVisualStyleBackColor = true;
            btnSendImage.Click += btnSendImage_Click;
            // 
            // rtbLog
            // 
            rtbLog.Location = new Point(308, 64);
            rtbLog.Name = "rtbLog";
            rtbLog.ReadOnly = true;
            rtbLog.Size = new Size(386, 213);
            rtbLog.TabIndex = 5;
            rtbLog.Text = "";
            // 
            // ServerForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnDisconnectClient);
            Controls.Add(txtPort);
            Controls.Add(lsvClients);
            Controls.Add(btnStart);
            Controls.Add(rtbLog);
            Controls.Add(btnStop);
            Controls.Add(btnSendImage);
            Controls.Add(btnSend);
            Controls.Add(txtMessage);
            Name = "ServerForm";
            Text = "ServerForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtPort;
        private Button btnStart;
        private Button btnStop;
        private ListBox lsvClients;
        private Button btnDisconnectClient;
        private FolderBrowserDialog folderBrowserDialog1;
        private TextBox txtMessage;
        private Button btnSend;
        private Button btnSendImage;
        private RichTextBox rtbLog;
    }
}