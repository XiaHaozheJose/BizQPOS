using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BizQ;
using BizQPOS.Models;
using BizQPOS.Services;
using BizQPOS.Utilities;
using BizQPOS.Views;

namespace BizQPOS.ViewModels
{
    public class ShopSelectionViewModel
    {
        private readonly AuthenticationService _authService;
        public ObservableCollection<UserEntity> Shops { get; }
        public ICommand SelectShopCommand { get; }

        public ShopSelectionViewModel(List<UserEntity> shops, string token)
        {
            _authService = new AuthenticationService();
            Shops = new ObservableCollection<UserEntity>(shops);

            // 定义命令逻辑
            SelectShopCommand = new RelayCommand(async shopObj =>
            {
                if (shopObj is UserEntity selectedShop)
                {
                    await _authService.SwitchOperatorAsync(selectedShop.OutId);
                    MessageBox.Show($"切换到商铺：{selectedShop.Name}");

                    // 打开主页面
                    WindowManager.OpenMainWindow();

                }
            });
        }
    }
}
