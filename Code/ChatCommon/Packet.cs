using System;

namespace ChatCommon
{
    public class Packet
    {
        public PacketType Type { get; set; }

        // Fix CS8618: Gán giá trị rỗng mặc định để tránh cảnh báo Nullable
        public string Sender { get; set; } = string.Empty;
        public string Receiver { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        // Fix CS0102: Đã xóa dòng khai báo FileData bị trùng lặp.
        // Thêm dấu "?" để cho phép FileData được null (vì không phải tin nhắn nào cũng có file)
        public byte[]? FileData { get; set; }

        // Avatar data in base64 format
        public string? Avatar { get; set; }

        public DateTime Timestamp { get; set; }

        public Packet()
        {
            Timestamp = DateTime.Now;
        }
    }
}