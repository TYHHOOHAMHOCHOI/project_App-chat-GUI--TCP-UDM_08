using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    public partial class ServerForm : Form
    {
        private TcpListener? _listener;
        private bool _running = false;

        private readonly List<ClientInfo> _clients = new();

        public ServerForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartServer();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopServer();
        }

        private void StartServer()
        {
            try
            {
                _listener = new TcpListener(IPAddress.Any, 8080);
                _listener.Start();

                _running = true;

                AddLog("Server đã mở.");

                Thread thread = new Thread(ListenLoop);
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void StopServer()
        {
            try
            {
                _running = false;

                foreach (var c in _clients)
                {
                    c.Client.Close();
                }

                _listener?.Stop();

                AddLog("Server đã đóng.");
            }
            catch { }
        }

        private void ListenLoop()
        {
            while (_running)
            {
                try
                {
                    TcpClient tcp = _listener!.AcceptTcpClient();

                    string ip =
                        ((IPEndPoint)tcp.Client.RemoteEndPoint!).Address.ToString();

                    ClientInfo info = new ClientInfo()
                    {
                        Client = tcp,
                        IP = ip,
                        Username = "Unknown"
                    };

                    lock (_clients)
                    {
                        _clients.Add(info);
                    }

                    AddLog($"Client đã kết nối: {ip}");

                    UpdateClientList();

                    Thread t = new Thread(() => HandleClient(info));
                    t.IsBackground = true;
                    t.Start();
                }
                catch { }
            }
        }

        private void HandleClient(ClientInfo info)
        {
            try
            {
                NetworkStream stream = info.Client.GetStream();

                byte[] buffer = new byte[4096];

                while (_running)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);

                    if (read <= 0)
                        break;

                    string msg = Encoding.UTF8.GetString(buffer, 0, read);

                    if (msg.StartsWith("LOGIN:"))
                    {
                        info.Username = msg
                            .Substring(6)
                            .Trim();

                        AddLog($"User đăng nhập: {info.Username}");

                        UpdateClientList();
                    }
                }
            }
            catch
            {

            }
            finally
            {
                lock (_clients)
                {
                    _clients.Remove(info);
                }

                AddLog($"Client đã ngắt kết nối: {info.IP}");

                UpdateClientList();

                try
                {
                    info.Client.Close();
                }
                catch { }
            }
        }

        private void AddLog(string text)
        {
            if (InvokeRequired)
            {
                Invoke(() => AddLog(text));
                return;
            }

            string time = DateTime.Now.ToString("HH:mm:ss");

            rtbLog.AppendText($"[{time}] {text}\n");

            rtbLog.ScrollToCaret();
        }

        private void UpdateClientList()
        {
            if (InvokeRequired)
            {
                Invoke(() => UpdateClientList());
                return;
            }

            dgvClients.Rows.Clear();

            lock (_clients)
            {
                foreach (var c in _clients)
                {
                    dgvClients.Rows.Add(c.IP, c.Username);
                }
            }
        }
    }
}