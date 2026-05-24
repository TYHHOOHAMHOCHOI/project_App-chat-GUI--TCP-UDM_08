using System;

namespace ChatCommon
{
    public class Packet
    {
        public PacketType Type { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Content { get; set; }
        
        // Hỗ trợ mảng byte trực tiếp trong JSON cho các file (tối đa ~10MB theo yêu cầu)
        public byte[] FileData { get; set; } 
        
        public DateTime Timestamp { get; set; }

        public Packet()
        {
            Timestamp = DateTime.Now;
        }
    }
}
