using System.Linq;
using System.Windows;
using BizQPOS.Views;

namespace BizQPOS.Utilities
{
    public static class WindowManager
    {
        /// <summary>
        /// 打开主窗口，并关闭当前窗口。
        /// </summary>
        public static void OpenMainWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            // 创建主窗口
            var mainWindow = new MainWindow();

            // 设置为主窗口
            Application.Current.MainWindow = mainWindow;

            // 显示主窗口
            mainWindow.Show();

            // 关闭当前活动窗口
            currentWindow?.Close();
        }

        /// <summary>
        /// 关闭当前活动窗口
        /// </summary>
        private static void CloseCurrentWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            currentWindow?.Close();
        }
    }
}
