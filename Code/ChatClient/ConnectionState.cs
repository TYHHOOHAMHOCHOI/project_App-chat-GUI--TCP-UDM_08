namespace ChatClient
{
    public enum ConnectionState
    {
        Disconnected,     // Chưa kết nối
        Connecting,       // Đang kết nối
        Connected,        // Đã kết nối
        LostConnection,   // Mất kết nối
        ManualDisconnect  // Đã ngắt kết nối
    }
}