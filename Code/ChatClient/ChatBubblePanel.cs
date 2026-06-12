using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ChatClient
{
    public class ChatBubblePanel : FlowLayoutPanel
    {
        private Dictionary<string, Image?> _userAvatars = new Dictionary<string, Image?>();
        public event Action<string, string>? OnReplyClicked;

        public ChatBubblePanel()
        {
            this.AutoScroll = true;
            this.FlowDirection = FlowDirection.TopDown;
            this.WrapContents = false;
            this.BackColor = Color.White;
        }

        public void AddMessage(string senderName, string messageText, DateTime timestamp, bool isOwnMessage, string? avatarBase64 = null)
        {
            try
            {
                // Get or load avatar
                Image? avatarImage = null;
                if (!string.IsNullOrEmpty(avatarBase64))
                {
                    if (!_userAvatars.ContainsKey(senderName))
                    {
                        _userAvatars[senderName] = AccountManager.ConvertBase64ToImage(avatarBase64);
                    }
                    avatarImage = _userAvatars[senderName];
                }

                // Create message bubble
                var bubble = new MessageBubble();
                bubble.Width = this.Width - 20;
                bubble.Margin = new Padding(10, 5, 10, 5);
                bubble.SetMessage(senderName, messageText, timestamp, avatarImage, isOwnMessage);
                bubble.OnReplyClicked += (sender, text) => OnReplyClicked?.Invoke(sender, text);

                this.Controls.Add(bubble);

                // Scroll to bottom
                this.AutoScrollPosition = new Point(0, int.MaxValue);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding message: {ex.Message}");
            }
        }

        public void AddMessageWithReply(string senderName, string messageText, DateTime timestamp, bool isOwnMessage, string? replyToUser, string? replyToMessage, string? avatarBase64 = null)
        {
            try
            {
                // Get or load avatar
                Image? avatarImage = null;
                if (!string.IsNullOrEmpty(avatarBase64))
                {
                    if (!_userAvatars.ContainsKey(senderName))
                    {
                        _userAvatars[senderName] = AccountManager.ConvertBase64ToImage(avatarBase64);
                    }
                    avatarImage = _userAvatars[senderName];
                }

                // Create message bubble
                var bubble = new MessageBubble();
                bubble.Width = this.Width - 20;
                bubble.Margin = new Padding(10, 5, 10, 5);
                bubble.SetMessage(senderName, messageText, timestamp, avatarImage, isOwnMessage, replyToUser, replyToMessage);
                bubble.OnReplyClicked += (sender, text) => OnReplyClicked?.Invoke(sender, text);

                this.Controls.Add(bubble);

                // Scroll to bottom
                this.AutoScrollPosition = new Point(0, int.MaxValue);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding message: {ex.Message}");
            }
        }

        public void ClearMessages()
        {
            this.Controls.Clear();
            _userAvatars.Clear();
        }
    }
}
