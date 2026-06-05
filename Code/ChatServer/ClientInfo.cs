using System.Net.Sockets;

namespace ChatServer
{
    public class ClientInfo
    {
        public TcpClient Client { get; set; } = null!;

        public string IP { get; set; } = "";

        public string Username { get; set; } = "";

        public string? Avatar { get; set; } // Base64-encoded avatar image
    }
}