namespace ChatCommon
{
    /// <summary>
    /// Model đại diện 1 tin nhắn được lưu trong database.
    /// Dùng chung cho cả Server (ghi) và Client (đọc ở Phase 2).
    /// </summary>
    public class ChatMessage
    {
        public long Id { get; set; }

        /// <summary>Username người gửi.</summary>
        public string Sender { get; set; } = string.Empty;

        /// <summary>
        /// Username người nhận. 
        /// NULL = tin nhắn chung (public/broadcast/system).
        /// </summary>
        public string? Receiver { get; set; }

        /// <summary>Nội dung tin nhắn.</summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Loại tin nhắn: "public", "private", "server", "system".
        /// </summary>
        public string MessageType { get; set; } = "public";

        /// <summary>Thời điểm gửi.</summary>
        public DateTime SentAt { get; set; } = DateTime.Now;

        /// <summary>Username của tin nhắn đang trả lời (nếu có).</summary>
        public string? ReplyToUser { get; set; }

        /// <summary>Nội dung của tin nhắn đang trả lời (nếu có).</summary>
        public string? ReplyToMessage { get; set; }
    }
}
