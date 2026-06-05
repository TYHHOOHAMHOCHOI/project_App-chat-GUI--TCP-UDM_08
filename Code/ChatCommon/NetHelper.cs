using System;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatCommon
{
    public static class NetHelper
    {
        /// <summary>
        /// Serialize packet thành mảng byte với 4 byte đầu tiên lưu trữ độ dài của gói dữ liệu.
        /// Sử dụng System.Text.Json. (Hỗ trợ file tối đa 10MB)
        /// </summary>
        public static byte[] Serialize(Packet packet)
        {
            var jsonString = JsonSerializer.Serialize(packet);
            var bodyBytes = System.Text.Encoding.UTF8.GetBytes(jsonString);

            // Cấu trúc: [4 bytes độ dài] + [Nội dung JSON]
            var lengthBytes = BitConverter.GetBytes(bodyBytes.Length);

            var finalPacket = new byte[lengthBytes.Length + bodyBytes.Length];
            Buffer.BlockCopy(lengthBytes, 0, finalPacket, 0, lengthBytes.Length);
            Buffer.BlockCopy(bodyBytes, 0, finalPacket, lengthBytes.Length, bodyBytes.Length);

            return finalPacket;
        }

        /// <summary>
        /// Gửi Packet qua NetworkStream một cách an toàn.
        /// </summary>
        public static async Task SendPacketAsync(NetworkStream stream, Packet packet)
        {
            if (stream == null || !stream.CanWrite) return;

            byte[] data = Serialize(packet);
            await stream.WriteAsync(data, 0, data.Length);
            await stream.FlushAsync();
        }

        /// <summary>
        /// Nhận Packet từ NetworkStream bằng cách đọc 4 bytes độ dài trước, sau đó đọc đủ body.
        /// Giải quyết triệt để vấn đề bị dính gói (TCP sticky packets).
        /// </summary>
        public static async Task<Packet> ReceivePacketAsync(NetworkStream stream)
        {
            if (stream == null || !stream.CanRead) return null;

            // 1. Đọc 4 byte độ dài
            byte[] lengthBuffer = new byte[4];
            int totalBytesRead = 0;
            while (totalBytesRead < 4)
            {
                int read = await stream.ReadAsync(lengthBuffer, totalBytesRead, 4 - totalBytesRead);
                if (read == 0) return null; // Disconnected
                totalBytesRead += read;
            }

            int bodyLength = BitConverter.ToInt32(lengthBuffer, 0);

            // Giới hạn chống tràn bộ nhớ (Max 15MB để dự phòng cho Base64 encoding của 10MB file + overhead)
            if (bodyLength <= 0 || bodyLength > 15 * 1024 * 1024)
            {
                return null; // Gói tin bất thường
            }

            // 2. Đọc body theo đúng độ dài
            byte[] bodyBuffer = new byte[bodyLength];
            totalBytesRead = 0;
            while (totalBytesRead < bodyLength)
            {
                int read = await stream.ReadAsync(bodyBuffer, totalBytesRead, bodyLength - totalBytesRead);
                if (read == 0) return null; // Disconnected
                totalBytesRead += read;
            }

            // 3. Deserialize JSON -> Packet
            string jsonString = System.Text.Encoding.UTF8.GetString(bodyBuffer);
            try
            {
                return JsonSerializer.Deserialize<Packet>(jsonString);
            }
            catch
            {
                return null; // Bỏ qua nếu lỗi định dạng JSON
            }
        }
    }
}
