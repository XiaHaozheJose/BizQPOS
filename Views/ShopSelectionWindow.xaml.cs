using System.Windows;
using System.Windows.Input;
using BizQPOS.Models;
using BizQPOS.ViewModels;
using Models;

namespace BizQPOS.Views
{
    public partial class ShopSelectionWindow : Window
    {
        private readonly ShopSelectionViewModel _viewModel;

        public ShopSelectionWindow(List<UserEntity> shops, string token)
        {
            InitializeComponent();
            _viewModel = new ShopSelectionViewModel(shops, token);
            DataContext = _viewModel;
        }

        private void ShopSelected(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is UserEntity selectedShop)
            {
                _viewModel.SelectShopCommand.Execute(selectedShop);
            }
        }

    }
}
