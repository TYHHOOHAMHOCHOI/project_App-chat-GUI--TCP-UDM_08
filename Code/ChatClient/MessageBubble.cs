using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class MessageBubble : UserControl
    {
        private Image? _avatarImage;
        private string _senderName = string.Empty;
        private string _messageText = string.Empty;
        private DateTime _timestamp;
        private bool _isOwnMessage;
        private string? _replyToUser;
        private string? _replyToMessage;
        private Button btnReply;
        private Button btnForward;
        
        public event Action<string, string>? OnReplyClicked;
        public event Action<string, string>? OnForwardClicked;
        private const int AVATAR_SIZE = 40;
        private const int BUBBLE_PADDING = 10;
        private const int CORNER_RADIUS = 10;
        private static readonly Font MessageFont = new Font("Segoe UI Emoji", 9F);
        private static readonly Font ReplyButtonFont = new Font("Segoe UI", 7F, FontStyle.Bold);
        private static readonly Size ReplyButtonSize = new Size(55, 20);
        private static readonly Size ForwardButtonSize = new Size(65, 20);

        public MessageBubble()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.AutoSize = false;

            btnReply = CreateActionButton("↩ Reply", ReplyButtonSize, () => OnReplyClicked?.Invoke(_senderName, _messageText));
            this.Controls.Add(btnReply);

            btnForward = CreateActionButton("➡ Forward", ForwardButtonSize, () => OnForwardClicked?.Invoke(_senderName, _messageText));
            this.Controls.Add(btnForward);
        }

        private static Button CreateActionButton(string text, Size size, Action clickHandler)
        {
            var button = new Button
            {
                Text = text,
                Font = ReplyButtonFont,
                Size = size,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.Teal,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            button.Click += (s, e) => clickHandler();

            return button;
        }

        public void SetMessage(string senderName, string messageText, DateTime timestamp, Image? avatarImage, bool isOwnMessage, string? replyToUser = null, string? replyToMessage = null)
        {
            _senderName = senderName;
            _messageText = messageText;
            _timestamp = timestamp;
            _avatarImage = avatarImage;
            _isOwnMessage = isOwnMessage;
            _replyToUser = replyToUser;
            _replyToMessage = replyToMessage;

            var menu = new ContextMenuStrip();
            var replyItem = new ToolStripMenuItem("Trả lời");
            replyItem.Click += (s, e) => OnReplyClicked?.Invoke(_senderName, _messageText);
            menu.Items.Add(replyItem);

            var forwardItem = new ToolStripMenuItem("Chuyển tiếp");
            forwardItem.Click += (s, e) => OnForwardClicked?.Invoke(_senderName, _messageText);
            menu.Items.Add(forwardItem);

            this.ContextMenuStrip = menu;

            // Calculate height based on text
            int replyHeight = string.IsNullOrEmpty(_replyToUser) ? 0 : 40;

            using (var g = this.CreateGraphics())
            {
                var size = g.MeasureString(_messageText, MessageFont, this.Width - AVATAR_SIZE - 60);
                var preferredHeight = (int)size.Height + BUBBLE_PADDING * 3 + 30 + replyHeight;
                this.Height = Math.Max(preferredHeight, AVATAR_SIZE + BUBBLE_PADDING * 2);
            }

            this.Invalidate();

            // Set button position at the bottom of the bubble
            int btnY = this.Height - 20;
            if (_isOwnMessage)
            {
                int bubbleWidth = this.Width - AVATAR_SIZE - 20;
                int bubbleX = this.Width - bubbleWidth - 10;
                btnReply.Location = new Point(bubbleX + bubbleWidth - 135, btnY);
                btnForward.Location = new Point(bubbleX + bubbleWidth - 75, btnY);
            }
            else
            {
                int bubbleX = AVATAR_SIZE + 10;
                btnReply.Location = new Point(bubbleX + 40, btnY);
                btnForward.Location = new Point(bubbleX + 100, btnY);
            }
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

            int textOffsetY = bubbleY + BUBBLE_PADDING;
            if (!string.IsNullOrEmpty(_replyToUser))
            {
                var quoteRect = new Rectangle(bubbleX + BUBBLE_PADDING, textOffsetY, bubbleWidth - BUBBLE_PADDING * 2 - 10, 35);
                DrawRoundedRectangle(g, quoteRect, 5, new SolidBrush(Color.FromArgb(180, 210, 240)));
                g.DrawString($"Trả lời {_replyToUser}:", new Font("Segoe UI", 8F, FontStyle.Italic), new SolidBrush(Color.DarkGray), new PointF(quoteRect.X + 5, quoteRect.Y + 2));
                string shortMsg = (_replyToMessage?.Length > 30 ? _replyToMessage.Substring(0, 30) + "..." : _replyToMessage) ?? string.Empty;
                g.DrawString(shortMsg ?? "", new Font("Segoe UI", 8F), new SolidBrush(Color.Gray), new PointF(quoteRect.X + 5, quoteRect.Y + 16));
                textOffsetY += 40;
            }

            // Draw text
            var textRect = new Rectangle(bubbleX + BUBBLE_PADDING, textOffsetY, 
                                        bubbleWidth - BUBBLE_PADDING * 3 - 10, this.Height - textOffsetY - 10);
            g.DrawString(_messageText, MessageFont, new SolidBrush(Color.Black), textRect, StringFormat.GenericDefault);

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

            int textOffsetY = bubbleY + BUBBLE_PADDING + 15;
            if (!string.IsNullOrEmpty(_replyToUser))
            {
                var quoteRect = new Rectangle(bubbleX + BUBBLE_PADDING, textOffsetY, bubbleWidth - BUBBLE_PADDING * 2 - 10, 35);
                DrawRoundedRectangle(g, quoteRect, 5, new SolidBrush(Color.FromArgb(220, 220, 220)));
                g.DrawString($"Trả lời {_replyToUser}:", new Font("Segoe UI", 8F, FontStyle.Italic), new SolidBrush(Color.Gray), new PointF(quoteRect.X + 5, quoteRect.Y + 2));
                string shortMsg = (_replyToMessage?.Length > 30 ? _replyToMessage.Substring(0, 30) + "..." : _replyToMessage) ?? string.Empty;
                g.DrawString(shortMsg ?? "", new Font("Segoe UI", 8F), new SolidBrush(Color.DimGray), new PointF(quoteRect.X + 5, quoteRect.Y + 16));
                textOffsetY += 40;
            }

            // Draw text
            var textRect = new Rectangle(bubbleX + BUBBLE_PADDING, textOffsetY, 
                                        bubbleWidth - BUBBLE_PADDING * 3 - 10, this.Height - textOffsetY - 10);
            g.DrawString(_messageText, MessageFont, new SolidBrush(Color.Black), textRect, StringFormat.GenericDefault);

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
