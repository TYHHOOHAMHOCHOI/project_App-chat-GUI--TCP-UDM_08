using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class MessageBubble : UserControl
    {
        private Image? _avatarImage;
        private string _senderName;
        private string _messageText;
        private DateTime _timestamp;
        private bool _isOwnMessage;
        private const int AVATAR_SIZE = 40;
        private const int BUBBLE_PADDING = 10;
        private const int CORNER_RADIUS = 10;

        public MessageBubble()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.AutoSize = false;
        }

        public void SetMessage(string senderName, string messageText, DateTime timestamp, Image? avatarImage, bool isOwnMessage)
        {
            _senderName = senderName;
            _messageText = messageText;
            _timestamp = timestamp;
            _avatarImage = avatarImage;
            _isOwnMessage = isOwnMessage;

            // Calculate height based on text
            using (var g = this.CreateGraphics())
            {
                var font = new Font("Segoe UI", 9F);
                var size = g.MeasureString(_messageText, font, this.Width - AVATAR_SIZE - 60);
                var preferredHeight = (int)size.Height + BUBBLE_PADDING * 3 + 30;
                this.Height = Math.Max(preferredHeight, AVATAR_SIZE + BUBBLE_PADDING * 2);
            }

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (_isOwnMessage)
            {
                DrawOwnMessage(e.Graphics);
            }
            else
            {
                DrawOthersMessage(e.Graphics);
            }
        }

        private void DrawOwnMessage(Graphics g)
        {
            int bubbleWidth = this.Width - AVATAR_SIZE - 20;
            int bubbleX = this.Width - bubbleWidth - 10;
            int bubbleY = 5;

            // Draw bubble (bên phải, màu xanh)
            var bubbleRect = new Rectangle(bubbleX, bubbleY, bubbleWidth - 10, this.Height - 10);
            DrawRoundedRectangle(g, bubbleRect, CORNER_RADIUS, new SolidBrush(Color.FromArgb(200, 230, 255)));

            // Draw text
            var textRect = new Rectangle(bubbleX + BUBBLE_PADDING, bubbleY + BUBBLE_PADDING, 
                                        bubbleWidth - BUBBLE_PADDING * 3 - 10, this.Height - BUBBLE_PADDING * 2 - 10);
            g.DrawString(_messageText, new Font("Segoe UI", 9F), new SolidBrush(Color.Black), textRect, StringFormat.GenericDefault);

            // Draw timestamp (nhỏ, dưới tin nhắn)
            var timeStr = _timestamp.ToString("HH:mm");
            g.DrawString(timeStr, new Font("Segoe UI", 7F), new SolidBrush(Color.Gray), 
                        new PointF(bubbleX, this.Height - 15));

            // Draw avatar (bên phải)
            DrawCircularAvatar(g, this.Width - AVATAR_SIZE - 5, 5);
        }

        private void DrawOthersMessage(Graphics g)
        {
            int bubbleWidth = this.Width - AVATAR_SIZE - 20;
            int bubbleX = AVATAR_SIZE + 10;
            int bubbleY = 5;

            // Draw name
            g.DrawString(_senderName, new Font("Segoe UI", 8F, FontStyle.Bold), new SolidBrush(Color.DarkBlue), 
                        new PointF(bubbleX, bubbleY - 2));

            // Draw bubble (bên trái, màu trắng)
            var bubbleRect = new Rectangle(bubbleX, bubbleY + 15, bubbleWidth - 10, this.Height - bubbleY - 25);
            DrawRoundedRectangle(g, bubbleRect, CORNER_RADIUS, new SolidBrush(Color.FromArgb(240, 240, 240)));

            // Draw text
            var textRect = new Rectangle(bubbleX + BUBBLE_PADDING, bubbleY + BUBBLE_PADDING + 15, 
                                        bubbleWidth - BUBBLE_PADDING * 3 - 10, this.Height - BUBBLE_PADDING * 2 - 25);
            g.DrawString(_messageText, new Font("Segoe UI", 9F), new SolidBrush(Color.Black), textRect, StringFormat.GenericDefault);

            // Draw timestamp
            var timeStr = _timestamp.ToString("HH:mm");
            g.DrawString(timeStr, new Font("Segoe UI", 7F), new SolidBrush(Color.Gray), 
                        new PointF(bubbleX, this.Height - 15));

            // Draw avatar (bên trái)
            DrawCircularAvatar(g, 5, 5);
        }

        private void DrawCircularAvatar(Graphics g, int x, int y)
        {
            // Draw circle border
            var circleBrush = new SolidBrush(Color.LightGray);
            g.FillEllipse(circleBrush, x, y, AVATAR_SIZE, AVATAR_SIZE);

            // Draw avatar image if available
            if (_avatarImage != null)
            {
                // Crop to circle
                var path = new GraphicsPath();
                path.AddEllipse(x, y, AVATAR_SIZE, AVATAR_SIZE);

                var oldClip = g.Clip;
                g.Clip = new Region(path);
                g.DrawImage(_avatarImage, x, y, AVATAR_SIZE, AVATAR_SIZE);
                g.Clip = oldClip;
            }

            // Draw border circle
            g.DrawEllipse(new Pen(Color.DarkGray, 1), x, y, AVATAR_SIZE, AVATAR_SIZE);
        }

        private void DrawRoundedRectangle(Graphics g, Rectangle rect, int radius, Brush brush)
        {
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();

            g.FillPath(brush, path);
            g.DrawPath(new Pen(Color.DarkGray, 1), path);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Invalidate();
        }
    }
}
