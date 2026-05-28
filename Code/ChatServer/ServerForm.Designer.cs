namespace ChatServer
{
    partial class ServerForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            rtbLog = new RichTextBox();
            dgvClients = new DataGridView();
            colIP = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            btnStart = new Button();
            btnStop = new Button();

            ((System.ComponentModel.ISupportInitialize)dgvClients).BeginInit();
            SuspendLayout();

            // rtbLog
            rtbLog.Location = new Point(12, 12);
            rtbLog.Size = new Size(760, 320);
            rtbLog.ReadOnly = true;

            // dgvClients
            dgvClients.Location = new Point(12, 350);
            dgvClients.Size = new Size(760, 250);
            dgvClients.Columns.AddRange(new DataGridViewColumn[]
            {
                colIP,
                colName
            });

            // colIP
            colIP.HeaderText = "Client IP";
            colIP.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // colName
            colName.HeaderText = "Username";
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // btnStart
            btnStart.Location = new Point(12, 620);
            btnStart.Size = new Size(150, 45);
            btnStart.Text = "Mở Server";
            btnStart.Click += btnStart_Click;

            // btnStop
            btnStop.Location = new Point(180, 620);
            btnStop.Size = new Size(150, 45);
            btnStop.Text = "Đóng Server";
            btnStop.Click += btnStop_Click;

            // ServerForm
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 681);
            Controls.Add(rtbLog);
            Controls.Add(dgvClients);
            Controls.Add(btnStart);
            Controls.Add(btnStop);

            Name = "ServerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Chat Server";

            ((System.ComponentModel.ISupportInitialize)dgvClients).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox rtbLog;
        private DataGridView dgvClients;
        private Button btnStart;
        private Button btnStop;

        private DataGridViewTextBoxColumn colIP;
        private DataGridViewTextBoxColumn colName;
    }
}