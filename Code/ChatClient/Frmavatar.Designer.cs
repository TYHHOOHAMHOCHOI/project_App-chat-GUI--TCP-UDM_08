namespace ChatClient
{
    partial class Frmavatar
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

        private void InitializeComponent()
        {
            pbCurrentAvatar = new PictureBox();
            pbPreview = new PictureBox();
            btnBrowse = new Button();
            btnSave = new Button();
            btnRemove = new Button();
            btnClose = new Button();
            lblUsername = new Label();
            lblCurrent = new Label();
            lblPreview = new Label();
            lblPreviewInfo = new Label();
            ((System.ComponentModel.ISupportInitialize)pbCurrentAvatar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbPreview).BeginInit();
            SuspendLayout();

            // pbCurrentAvatar
            pbCurrentAvatar.BackColor = SystemColors.Control;
            pbCurrentAvatar.BorderStyle = BorderStyle.FixedSingle;
            pbCurrentAvatar.Location = new Point(20, 80);
            pbCurrentAvatar.Name = "pbCurrentAvatar";
            pbCurrentAvatar.Size = new Size(150, 150);
            pbCurrentAvatar.SizeMode = PictureBoxSizeMode.StretchImage;
            pbCurrentAvatar.TabIndex = 0;
            pbCurrentAvatar.TabStop = false;

            // pbPreview
            pbPreview.BackColor = SystemColors.Control;
            pbPreview.BorderStyle = BorderStyle.FixedSingle;
            pbPreview.Location = new Point(200, 80);
            pbPreview.Name = "pbPreview";
            pbPreview.Size = new Size(150, 150);
            pbPreview.SizeMode = PictureBoxSizeMode.StretchImage;
            pbPreview.TabIndex = 1;
            pbPreview.TabStop = false;

            // btnBrowse
            btnBrowse.BackColor = Color.LightBlue;
            btnBrowse.Font = new Font("Segoe UI", 10F);
            btnBrowse.Location = new Point(200, 240);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(150, 40);
            btnBrowse.TabIndex = 2;
            btnBrowse.Text = "Chọn ảnh";
            btnBrowse.UseVisualStyleBackColor = false;
            btnBrowse.Click += btnBrowse_Click;

            // btnSave
            btnSave.BackColor = Color.LightGreen;
            btnSave.Font = new Font("Segoe UI", 10F);
            btnSave.Location = new Point(200, 290);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(150, 40);
            btnSave.TabIndex = 3;
            btnSave.Text = "Lưu Avatar";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;

            // btnRemove
            btnRemove.BackColor = Color.LightCoral;
            btnRemove.Font = new Font("Segoe UI", 10F);
            btnRemove.Location = new Point(200, 340);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(150, 40);
            btnRemove.TabIndex = 4;
            btnRemove.Text = "Xóa Avatar";
            btnRemove.UseVisualStyleBackColor = false;
            btnRemove.Click += btnRemove_Click;

            // btnClose
            btnClose.BackColor = Color.Gray;
            btnClose.Font = new Font("Segoe UI", 10F);
            btnClose.Location = new Point(200, 390);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(150, 40);
            btnClose.TabIndex = 5;
            btnClose.Text = "Đóng";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;

            // lblUsername
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblUsername.Location = new Point(20, 20);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(100, 28);
            lblUsername.TabIndex = 6;
            lblUsername.Text = "Người dùng";

            // lblCurrent
            lblCurrent.AutoSize = true;
            lblCurrent.Font = new Font("Segoe UI", 10F);
            lblCurrent.Location = new Point(20, 55);
            lblCurrent.Name = "lblCurrent";
            lblCurrent.Size = new Size(100, 22);
            lblCurrent.TabIndex = 7;
            lblCurrent.Text = "Avatar hiện tại";

            // lblPreview
            lblPreview.AutoSize = true;
            lblPreview.Font = new Font("Segoe UI", 10F);
            lblPreview.Location = new Point(200, 55);
            lblPreview.Name = "lblPreview";
            lblPreview.Size = new Size(70, 22);
            lblPreview.TabIndex = 8;
            lblPreview.Text = "Xem trước";

            // lblPreviewInfo
            lblPreviewInfo.AutoSize = true;
            lblPreviewInfo.Font = new Font("Segoe UI", 9F);
            lblPreviewInfo.ForeColor = Color.Gray;
            lblPreviewInfo.Location = new Point(200, 235);
            lblPreviewInfo.Name = "lblPreviewInfo";
            lblPreviewInfo.Size = new Size(150, 20);
            lblPreviewInfo.TabIndex = 9;
            lblPreviewInfo.Text = "Chọn ảnh mới để thay đổi";

            // AvatarForm
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightGray;
            ClientSize = new Size(380, 450);
            Controls.Add(lblPreviewInfo);
            Controls.Add(lblPreview);
            Controls.Add(lblCurrent);
            Controls.Add(lblUsername);
            Controls.Add(btnClose);
            Controls.Add(btnRemove);
            Controls.Add(btnSave);
            Controls.Add(btnBrowse);
            Controls.Add(pbPreview);
            Controls.Add(pbCurrentAvatar);
            Font = new Font("Segoe UI", 10F);
            Name = "AvatarForm";
            Text = "Quản lý Avatar";
            StartPosition = FormStartPosition.CenterParent;
            Load += AvatarForm_Load;
            ((System.ComponentModel.ISupportInitialize)pbCurrentAvatar).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbPreview).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private PictureBox pbCurrentAvatar;
        private PictureBox pbPreview;
        private Button btnBrowse;
        private Button btnSave;
        private Button btnRemove;
        private Button btnClose;
        private Label lblUsername;
        private Label lblCurrent;
        private Label lblPreview;
        private Label lblPreviewInfo;
    }
}
