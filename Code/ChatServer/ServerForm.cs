using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace ChatServer
{
    public partial class ServerForm : Form
    {
        private TcpListener server;
        private List<TcpClient> clientList;
        private Dictionary<TcpClient, string> clientNames;
        private bool isRunning;

        public ServerForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false; // Ngăn lỗi xung đột luồng khi cập nhật giao diện
            clientList = new List<TcpClient>();
            clientNames = new Dictionary<TcpClient, string>();
            isRunning = false;
        }

        // ==========================================
        // 1. CÁC HÀM SỰ KIỆN CLICK NÚT TRÊN GIAO DIỆN
        // ==========================================

        // Sự kiện Click nút Mở Server
        private void btnStart_Click(object sender, EventArgs e)
        {
            BatDauServer();
        }

        // Sự kiện Click nút Đóng Server
        private void btnStop_Click(object sender, EventArgs e)
        {
            DungServer();
        }

        // Sự kiện Click nút Gửi tin nhắn
        private void btnSend_Click(object sender, EventArgs e)
        {
            ServerGuiTinNhan();
        }

        // Sự kiện Click nút Gửi ảnh
        private void btnSendImage_Click(object sender, EventArgs e)
        {
            ServerGuiAnh();
        }

        // Sự kiện Click nút Ngắt kết nối Client được chọn
        private void btnDisconnectClient_Click(object sender, EventArgs e)
        {
            KickClient();
        }


        // ==========================================
        // 2. LOGIC XỬ LÝ HỆ THỐNG SERVER SOCKET
        // ==========================================

        // Khởi chạy lắng nghe cổng kết nối
        public void BatDauServer()
        {
            try
            {
                int port = int.Parse(txtPort.Text);
                server = new TcpListener(IPAddress.Any, port);
                server.Start();
                isRunning = true;

                rtbLog.AppendText($"[HỆ THỐNG] Server đã mở thành công trên Port: {port}\n");
                btnStart.Enabled = false;
                btnStop.Enabled = true;

                Thread listenThread = new Thread(ListenForClients);
                listenThread.IsBackground = true;
                listenThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi động Server: " + ex.Message);
            }
        }

        private void ListenForClients()
        {
            while (isRunning)
            {
                try
                {
                    TcpClient client = server.AcceptTcpClient();
                    lock (clientList)
                    {
                        clientList.Add(client);
                    }

                    Thread receiveThread = new Thread(ReceiveData);
                    receiveThread.IsBackground = true;
                    receiveThread.Start(client);
                }
                catch
                {
                    break;
                }
            }
        }

        // Tự động nhận dữ liệu (Chữ hoặc Mảng byte Ảnh) từ Client gửi lên
        private void ReceiveData(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();
            string clientEP = client.Client.RemoteEndPoint.ToString();

            clientNames[client] = clientEP;
            lsvClients.Items.Add(clientEP);

            byte[] buffer = new byte[1024 * 5000]; // Bộ đệm tối đa 5MB hỗ trợ file ảnh

            while (isRunning)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    object receivedObj = Deserialize(buffer);

                    if (receivedObj is string msg)
                    {
                        if (msg.StartsWith("NAME:"))
                        {
                            string name = msg.Substring(5);
                            clientNames[client] = name;

                            lsvClients.Items.Remove(clientEP);
                            lsvClients.Items.Add(name);
                            rtbLog.AppendText($"[HỆ THỐNG] {name} đã tham gia phòng chat.\n");
                            Broadcast($"[HỆ THỐNG] {name} đã tham gia phòng chat.");
                        }
                        else
                        {
                            rtbLog.AppendText(msg + "\n");
                            Broadcast(msg);
                        }
                    }
                    else if (receivedObj is byte[] imgBytes)
                    {
                        rtbLog.AppendText($"[{clientNames[client]}] đã gửi một hình ảnh.\n");
                        Broadcast(imgBytes);
                    }
                }
                catch
                {
                    break;
                }
            }

            CloseSingleClient(client);
        }

        // Phát tín hiệu truyền tin cho toàn bộ các máy Client đang online
        private void Broadcast(object data)
        {
            byte[] sendData = Serialize(data);
            lock (clientList)
            {
                foreach (TcpClient client in clientList)
                {
                    try
                    {
                        if (client.Connected)
                        {
                            client.GetStream().Write(sendData, 0, sendData.Length);
                        }
                    }
                    catch { }
                }
            }
        }

        // Server phát tin nhắn text
        public void ServerGuiTinNhan()
        {
            if (!string.IsNullOrEmpty(txtMessage.Text))
            {
                string serverMsg = $"[Server]: {txtMessage.Text}";
                rtbLog.AppendText(serverMsg + "\n");
                Broadcast(serverMsg);
                txtMessage.Clear();
            }
        }

        // Server phát tệp tin hình ảnh
        public void ServerGuiAnh()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    byte[] imgBytes = File.ReadAllBytes(ofd.FileName);
                    rtbLog.AppendText("[Server] đã gửi một hình ảnh.\n");
                    Broadcast(imgBytes);
                }
            }
        }

        // Ngắt kết nối một máy con được chọn trong danh sách ListBox
        public void KickClient()
        {
            if (lsvClients.SelectedItem != null)
            {
                string selectedName = lsvClients.SelectedItem.ToString();
                TcpClient targetClient = null;

                foreach (var pair in clientNames)
                {
                    if (pair.Value == selectedName)
                    {
                        targetClient = pair.Key;
                        break;
                    }
                }

                if (targetClient != null)
                {
                    rtbLog.AppendText($"[HỆ THỐNG] Server ngắt kết nối với {selectedName}.\n");
                    CloseSingleClient(targetClient);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một client trong danh sách để ngắt kết nối!");
            }
        }

        private void CloseSingleClient(TcpClient client)
        {
            if (client == null) return;
            string name = clientNames.ContainsKey(client) ? clientNames[client] : "Ẩn danh";

            lock (clientList)
            {
                if (clientList.Contains(client)) clientList.Remove(client);
            }
            if (clientNames.ContainsKey(client)) clientNames.Remove(client);

            this.Invoke((MethodInvoker)delegate
            {
                if (lsvClients.Items.Contains(name)) lsvClients.Items.Remove(name);
            });

            client.Close();
        }

        // Đóng toàn bộ Server và dọn dẹp các Client
        public void DungServer()
        {
            isRunning = false;
            lock (clientList)
            {
                foreach (TcpClient client in clientList) client.Close();
                clientList.Clear();
            }
            clientNames.Clear();
            lsvClients.Items.Clear();

            if (server != null) server.Stop();

            rtbLog.AppendText("[HỆ THỐNG] Server đã đóng hoàn toàn.\n");
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        // ==========================================
        // 3. MÃ HÓA VÀ GIẢI MÃ DỮ LIỆU BINARY (ĐÃ TẮT LỖI)
        // ==========================================

#pragma warning disable SYSLIB0011 // Loại bỏ hoàn toàn lỗi gạch đỏ bảo mật .NET mới

        private byte[] Serialize(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        private object Deserialize(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return bf.Deserialize(ms);
            }
        }

        private void lsvClients_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

#pragma warning restore SYSLIB0011
    }
}