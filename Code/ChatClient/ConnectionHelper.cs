using System.Drawing;
using System.Windows.Forms;

namespace ChatClient
{
    public static class ConnectionHelper
    {
        public static void UpdateStatus(Label lbl, ConnectionState state)
        {
            switch (state)
            {
                case ConnectionState.Disconnected:
                    lbl.Text = "Trạng thái: Chưa kết nối";
                    lbl.ForeColor = Color.Gray;
                    break;

                case ConnectionState.Connecting:
                    lbl.Text = "Trạng thái: Đang kết nối...";
                    lbl.ForeColor = Color.Orange;
                    break;

                case ConnectionState.Connected:
                    lbl.Text = "Trạng thái: Đã kết nối";
                    lbl.ForeColor = Color.Green;
                    break;

                case ConnectionState.LostConnection:
                    lbl.Text = "Trạng thái: Mất kết nối";
                    lbl.ForeColor = Color.Red;
                    break;

                case ConnectionState.ManualDisconnect:
                    lbl.Text = "Trạng thái: Đã ngắt kết nối";
                    lbl.ForeColor = Color.DarkRed;
                    break;
            }
        }
    }
}