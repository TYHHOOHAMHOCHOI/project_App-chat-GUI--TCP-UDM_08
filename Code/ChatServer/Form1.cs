
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
        rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] [Hệ thống] Đã xóa toàn bộ tin nhắn.\r\n");
    }

    private void lblSoClient_Click(object sender, EventArgs e)
    {

    }

    private void btnSend_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtMessage.Text.Trim()))

        {

            MessageBox.Show("Vui lòng nhập tin nhắn trước khi gửi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtMessage.Focus(); // Đưa con trỏ chuột quay lại ô nhập để người dùng gõ luôn
            return;
        } 
            
            

        if (serverSocket == null || !isRunning)
        {
            MessageBox.Show("Server chưa mở kết nối, không thể gửi tin nhắn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Đọc tên người gửi từ ô Username, nếu trống mặc định là Server
        string senderName = string.IsNullOrEmpty(txtUsername.Text.Trim()) ? "Server" : txtUsername.Text.Trim();
        
        string timeStamp = DateTime.Now.ToString("HH:mm:ss");
        string msg = $"[{timeStamp}] {senderName}: {txtMessage.Text}";

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
        // TRƯỜNG HỢP 1: dừng
        if (isRunning)
        {
            try
            {
                // 1. Ngắt vòng lặp ở luồng ngầm
                isRunning = false;

                // 2. Đóng Socket chính của Server
                if (serverSocket != null)
                {
                    if (serverSocket.Connected)
                    {
                        // Dừng việc nhận và gửi dữ liệu ngay lập tức
                        serverSocket.Shutdown(SocketShutdown.Both);
                    }
                    serverSocket.Close();
                }


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


                rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] [Hệ thống] Server đã đóng hoàn toàn và ngừng lắng nghe kết nối.\r\n");
                lblSoClient.Text = "Số client: 0";

                txtMessage.Enabled = false;
                btnDisconectAll.Enabled = false;

                //mở ổ port khi bấm Dừng server
                txtPort.Enabled = true;

                btnOpenServer.Text = "Mở kết nối";
                btnOpenServer.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Server gặp lỗi khi ngắt kết nối: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        // TRƯỜNG HỢP 2: mở server
        else
        {
            
            if (string.IsNullOrEmpty(txtPort.Text.Trim()))
            {
                MessageBox.Show("Cảnh báo: Ô Port không được để trống! Hệ thống tự động gán Port mặc định là 9050.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPort.Text = "9050";
            }

           
            //Thay thế dòng int.Parse bằng int.TryParse an toàn
            // Chặn crash nếu người dùng nhập chữ
            if (!int.TryParse(txtPort.Text.Trim(), out int port) || port < 1 || port > 65535)
            {
                MessageBox.Show("Cổng Port nhập vào không hợp lệ!", "Lỗi Nhập Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPort.Focus();
                return;
            }

            try
            {
                
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(ipep);
                serverSocket.Listen(10);

                
                isRunning = true;

                rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] [Hệ thống] Server đã mở thành công tại Port: {port}\r\n");
                txtMessage.Enabled = true;
                btnDisconectAll.Enabled = true;

                
                // Khóa ô nhập port lại khi server đang chạy       
                txtPort.Enabled = false;

                btnOpenServer.Text = "Dừng";
                btnOpenServer.Enabled = true;

                // Khởi chạy luồng ngầm canh cửa đón Client
                Thread threadListen = new Thread(ListenForClients);
                threadListen.IsBackground = true;
                threadListen.Start();
            }
            
            
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không thể mở Socket Server: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] [Hệ thống] Máy con kết nối từ: {clientSocket.RemoteEndPoint}\r\n");
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


            rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] [Hệ thống] Server đã ngắt kết nối hoàn toàn.\r\n");
            lblSoClient.Text = "Số client: 0";
            txtMessage.Enabled = false;

            txtPort.Enabled = true;


            btnOpenServer.Text = "Mở kết nối";// đổi Dừng -> Mở kết nối
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
        // 1. Tạo một Menu ngữ cảnh thả xuống
        ContextMenuStrip emojiMenu = new ContextMenuStrip();

        // 2. Danh sách các icon/emoji bạn muốn cho người dùng chọn
        string[] emojis = { "😀", "😁", "😂", "🤣", "😃", "😄", "😅", "😆", "😉", "😊", "😋", "😎", "😍", "😘", "👍", "👎", "❤️" };

        // 3. Vòng lặp tự động thêm từng Emoji thành một dòng trong Menu
        foreach (string emoji in emojis)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(emoji);

            // Cài đặt cỡ chữ hiển thị icon cho to và rõ ràng hơn (Cỡ 14)
            item.Font = new Font("Segoe UI Emoji", 14);

            // Khi người dùng click chọn vào một icon cụ thể
            item.Click += (s, args) =>
            {
                // Tự động chèn icon được chọn vào vị trí con trỏ đang đứng trong ô Nhập tin nhắn
                txtMessage.AppendText(emoji);
                txtMessage.Focus(); // Giữ con trỏ chuột ở ô nhập để gõ tiếp
            };

            emojiMenu.Items.Add(item);
        }

        // 4. Hiển thị menu xổ xuống ngay sát góc dưới bên trái của nút bấm mặt cười
        Button btn = (Button)sender;
        emojiMenu.Show(btn, new Point(0, btn.Height));
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

                string timeStampPriv = DateTime.Now.ToString("HH:mm:ss");
                string privateMsg = $"[{timeStampPriv}] [Gửi riêng] Server -> Bạn: {txtMessage.Text}";
                byte[] data = Encoding.UTF8.GetBytes(privateMsg);
                targetClient.Send(data);

                rtbLog.AppendText($"[{timeStampPriv}] [Gửi riêng] Tới Client {selectedId}: {txtMessage.Text}\r\n");
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