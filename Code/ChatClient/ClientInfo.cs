using System.Net.Sockets;

namespace ChatServer
{
    public class ClientInfo
    {
        public TcpClient Client { get; set; } = null!;

        public string IP { get; set; } = "";

        public string Username { get; set; } = "";
    }
}