using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class AvatarForm : Form
    {
        private readonly string _username;
        private string? _selectedAvatarPath;

        public AvatarForm(string username)
        {
            InitializeComponent();
            _username = username;
        }

        private void AvatarForm_Load(object sender, EventArgs e)
        {
            this.Text = $"Quản lý Avatar - {_username}";
            lblUsername.Text = $"Người dùng: {_username}";

            // Load current avatar
            LoadCurrentAvatar();
        }

        private void LoadCurrentAvatar()
        {
            try
            {
                var avatarBase64 = AccountManager.GetAvatar(_username);
                if (!string.IsNullOrEmpty(avatarBase64))
                {
                    var avatarImage = AccountManager.ConvertBase64ToImage(avatarBase64);
                    if (avatarImage != null)
                    {
                        pbCurrentAvatar.Image = avatarImage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải avatar: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";
            openFileDialog.Title = "Chọn ảnh Avatar";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                _selectedAvatarPath = openFileDialog.FileName;
                try
                {
                    // Show preview
                    pbPreview.Image = Image.FromFile(_selectedAvatarPath);
                    lblPreviewInfo.Text = $"Đã chọn: {System.IO.Path.GetFileName(_selectedAvatarPath)}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _selectedAvatarPath = null;
                    pbPreview.Image = null;
                    lblPreviewInfo.Text = "Không có ảnh được chọn";
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedAvatarPath))
            {
                MessageBox.Show("Vui lòng chọn ảnh trước.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (AccountManager.SetAvatar(_username, _selectedAvatarPath, out var message))
            {
                MessageBox.Show(message, "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCurrentAvatar();
                _selectedAvatarPath = null;
                pbPreview.Image = null;
                lblPreviewInfo.Text = "Chọn ảnh mới để thay đổi";
            }
            else
            {
                MessageBox.Show(message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Bạn có chắc muốn xóa avatar?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (res == DialogResult.Yes)
            {
                if (AccountManager.RemoveAvatar(_username, out var message))
                {
                    MessageBox.Show(message, "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pbCurrentAvatar.Image = null;
                    _selectedAvatarPath = null;
                    pbPreview.Image = null;
                    lblPreviewInfo.Text = "Chọn ảnh mới để thay đổi";
                }
                else
                {
                    MessageBox.Show(message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
