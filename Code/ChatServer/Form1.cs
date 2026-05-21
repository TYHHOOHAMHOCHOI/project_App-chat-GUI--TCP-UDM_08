
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets; 
using System.Text;
using System.Threading;   
using System.Windows.Forms;
namespace ChatServer;

public partial class Form1 : Form
{

    private Socket serverSocket;
    private List<Socket> listClientOnline = new List<Socket>();
    private bool isRunning = false;
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {

    }

    private void button1_Click(object sender, EventArgs e)
    {

    }

    private void label1_Click(object sender, EventArgs e)
    {

    }

    private void btnClear_Click(object sender, EventArgs e)
    {
        rtbLog.Clear();
        rtbLog.AppendText("[Hệ thống] Đã xóa toàn bộ tin nhắn.\r\n");
    }

    private void lblSoClient_Click(object sender, EventArgs e)
    {

    }

    private void btnSend_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtMessage.Text.Trim())) return;

        if (serverSocket == null || !isRunning)
        {
            MessageBox.Show("Server chưa mở kết nối, không thể gửi tin nhắn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Đọc tên người gửi từ ô Username, nếu trống mặc định là Server
        string senderName = string.IsNullOrEmpty(txtUsername.Text.Trim()) ? "Server" : txtUsername.Text.Trim();
        string msg = $"{senderName}: {txtMessage.Text}";

        rtbLog.AppendText(msg + "\r\n");

        // Gọi hàm phát tin nhắn chung đi
        BroadcastMessage(msg);

        txtMessage.Clear();
        txtMessage.Focus();
    }

    private void BroadcastMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        lock (listClientOnline)
        {
            foreach (Socket client in listClientOnline)
            {
                try
                {
                    if (client != null && client.Connected)
                    {
                        client.Send(data);
                    }
                }
                catch { }
            }
        }
    }

    private void btnOpenServer_Click(object sender, EventArgs e)
    {
        try
        {
            int port = string.IsNullOrEmpty(txtPort.Text) ? 9050 : int.Parse(txtPort.Text);

            txtPort.Text = port.ToString(); // Đưa số port hiển thị lên ô nhập liệu trên giao diện
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipep);
            serverSocket.Listen(10);
            isRunning = true;

            rtbLog.AppendText($"[Hệ thống] Server đã mở thành công tại Port: {port}\r\n");
            txtMessage.Enabled = true;

            btnOpenServer.Enabled = false;
            btnDisconectAll.Enabled = true;

            Thread threadListen = new Thread(ListenForClients);
            // thuộc tính chạy ngầm( khi mà bấm X tắt đi thì sẽ tắt luôn ngầm này nếu ko có thì cái thread chính nó tắt nhưng cái ngầm này nó vẫn chạy -> nặng máy )
            threadListen.IsBackground = true;
            threadListen.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi không thể mở Socket Server: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void ListenForClients()
    {
        while (isRunning)
        {
            try
            {

                Socket clientSocket = serverSocket.Accept();

                // Khi có máy con vào, lưu Socket của nó vào danh sách quản lý
                lock (listClientOnline)
                {
                    listClientOnline.Add(clientSocket);
                }

                // Đồng bộ hiển thị lên giao diện RichTextBox và DataGridView một cách an toàn
                this.Invoke((MethodInvoker)delegate
                {
                    rtbLog.AppendText($"[Hệ thống] Máy con kết nối từ: {clientSocket.RemoteEndPoint}\r\n");
                    lblSoClient.Text = $"Số client: {listClientOnline.Count}";

                    // Thêm một dòng mới vào bảng DataGridView
                    int index = dgvClients.Rows.Add();
                    dgvClients.Rows[index].Cells["colID"].Value = listClientOnline.Count;
                    dgvClients.Rows[index].Cells["colName"].Value = $"Client {listClientOnline.Count}";
                });
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show("Máy chủ đã dừng hoạt động. Toàn bộ các kết nối đã được giải phóng thành công!", "Thông Báo : ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });

                //  lệnh break để thoát hẳn vòng lặp while, không bị treo máy!
                break;
            }
        }
    }

    private void btnDisconectAll_Click(object sender, EventArgs e)
    {
        try
        {
            //ngắt vòng lặp ở luồng ngầm (ListenForClients) tự thoát
            isRunning = false;

            //Đóng Socket chính của Server
            if (serverSocket != null)
            {

                if (serverSocket.Connected)
                {
                    //lệnh này báo là nó sẽ dừng việc nhận và gửi dữ liêu ngay bây h tránh việc đang truyền mà bị lỗi
                    serverSocket.Shutdown(SocketShutdown.Both);
                }
                serverSocket.Close();
            }

            // duyệt danh sách xem để đóng kết nối của từng client đang online
            //lock này nó để khóa cái lệnh này lại tránh cho việc cái luồng ngầm nó ko được nhét thêm client mới vào
            lock (listClientOnline)
            {
                foreach (Socket clientSocket in listClientOnline)
                {
                    if (clientSocket != null)
                    {
                        try
                        {
                            if (clientSocket.Connected)
                            {
                                clientSocket.Shutdown(SocketShutdown.Both);
                            }
                            clientSocket.Close();
                        }
                        catch { }
                    }
                }
                listClientOnline.Clear();
            }


            rtbLog.AppendText("[Hệ thống] Server đã ngắt kết nối hoàn toàn.\r\n");
            lblSoClient.Text = "Số client: 0";
            txtMessage.Enabled = false;


            btnOpenServer.Enabled = true;
            btnDisconectAll.Enabled = false;

        }
        catch (Exception ex)
        {

            MessageBox.Show($"Server đã ngắt kết nối: {ex.Message}", "Thông báo : ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }
    }

    private void btnIcon_Paint(object sender, PaintEventArgs e)
    {
        //ép về dạng button
        Button btn = (Button)sender;

        // tạo hinh  tròn phù hợp với icon
        System.Drawing.Drawing2D.GraphicsPath nútTròn = new System.Drawing.Drawing2D.GraphicsPath();
        nútTròn.AddEllipse(0, 0, btn.Width, btn.Height);


        // những phần thừa hình vuông bên ngoài tự động gọt bỏ -> hình tròn
        btn.Region = new Region(nútTròn);
    }

    private void chkHideKey_CheckedChanged(object sender, EventArgs e)
    {
        //ép về kiểu checkBox
        CheckBox cb = (CheckBox)sender;


        if (cb.Checked)
        {
            // ẩn biến thành dấu chấm
            txtKey.UseSystemPasswordChar = true;
        }
        else
        {
            //hiện chữ bình thường
            txtKey.UseSystemPasswordChar = false;
        }
    }

    private void txtKey_TextChanged(object sender, EventArgs e)
    {

    }

    private void labelPort_Click(object sender, EventArgs e)
    {

    }

    private void txtPort_TextChanged(object sender, EventArgs e)
    {

    }

    private void button1_Click_1(object sender, EventArgs e)
    {

    }

    private void rtbLog_TextChanged(object sender, EventArgs e)
    {

    }

    private void dgvClients_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        // Kiểm tra nếu click vào hàng tiêu đề (Header) thì bỏ qua
        if (e.RowIndex < 0) return;

        try
        {
            // Lấy ra Index dòng vừa click (Dòng 0 tương ứng phần tử số 0 trong List)
            int targetIndex = e.RowIndex;

            // Lấy ID hiển thị để làm thông báo trực quan
            var cellIdValue = dgvClients.Rows[e.RowIndex].Cells["colID"].Value;
            string selectedId = cellIdValue != null ? cellIdValue.ToString() : "Ẩn danh";

            Socket targetClient = null;
            lock (listClientOnline)
            {
                // Kiểm tra an toàn xem vị trí click có khớp với danh sách Online không
                if (targetIndex >= 0 && targetIndex < listClientOnline.Count)
                {
                    targetClient = listClientOnline[targetIndex];
                }
            }

            if (targetClient == null) return;

            // Trường hợp 1: Click cột nút "Disconnect" hoặc "colKick"
            if (dgvClients.Columns[e.ColumnIndex].Name == "colKick" || dgvClients.Columns[e.ColumnIndex].Name == "Disconnect")
            {
                DialogResult res = MessageBox.Show($"Bạn có muốn ngắt kết nối máy con Client {selectedId} không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    targetClient.Close(); // Đóng socket kết nối

                    // Thêm đoạn code dưới đây để dọn dẹp giao diện ngay lập tức:
                    lock (listClientOnline)
                    {
                        listClientOnline.RemoveAt(targetIndex); // Xóa khỏi danh sách quản lý
                    }
                    dgvClients.Rows.RemoveAt(targetIndex); // Xóa dòng đó khỏi bảng hiển thị
                    lblSoClient.Text = $"Số client: {listClientOnline.Count}"; // Cập nhật lại tổng số client
                }
            }

            // Trường hợp 2: Click cột nút "Gửi tin nhắn" hoặc "colSend"
            if (dgvClients.Columns[e.ColumnIndex].Name == "colSend" || dgvClients.Columns[e.ColumnIndex].Name == "Gửi tin nhắn")
            {
                if (string.IsNullOrEmpty(txtMessage.Text))
                {
                    MessageBox.Show("Hãy gõ nội dung vào ô 'Nhập tin nhắn' trước khi click gửi riêng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string privateMsg = $"[Gửi riêng] Server -> Bạn: {txtMessage.Text}";
                byte[] data = Encoding.UTF8.GetBytes(privateMsg);
                targetClient.Send(data);

                rtbLog.AppendText($"[Gửi riêng] Tới Client {selectedId}: {txtMessage.Text}\r\n");
                txtMessage.Clear();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi tương tác ô dữ liệu bảng: " + ex.Message);
        }
    }

    private void txtUsername_TextChanged(object sender, EventArgs e)
    {

    }

    private void txtMessage_TextChanged(object sender, EventArgs e)
    {

    }

    private void txtMessage_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true; // Chặn tiếng "beep" khó chịu của Windows
            btnSend.PerformClick();    // Kích hoạt lệnh click của nút Gửi
        }
    }

    private void label1_Click_1(object sender, EventArgs e)
    {

    }
}