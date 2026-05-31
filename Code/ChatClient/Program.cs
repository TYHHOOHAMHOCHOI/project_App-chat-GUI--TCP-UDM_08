namespace ChatClient;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Mở màn hình đăng nhập — chỉ xác thực tài khoản
        using var login = new Frmlogin();
        if (login.ShowDialog() != DialogResult.OK)
            return;

        // Vào Form1, truyền username. IP/Port người dùng tự nhập rồi nhấn "Mở kết nối"
        Application.Run(new Form1(login.LoggedInUser!));
    }
}