using System.Windows;
using BizQPOS.Views;

namespace BizQ
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 启动登录窗口
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}
