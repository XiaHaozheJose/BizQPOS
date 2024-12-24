using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BizQPOS.Services;
using BizQPOS.Views;
using BizQPOS.Utilities;
using Models;
using BizQPOS.Models;

namespace BizQPOS.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly AuthenticationService _authService;

        private KeyValuePair<string, string> _selectedCountry;
        private string _phone;
        private string _password;

        public ObservableCollection<KeyValuePair<string, string>> CountryList { get; }

        public KeyValuePair<string, string> SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                _selectedCountry = value;
                OnPropertyChanged();
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _authService = new AuthenticationService();
            CountryList = new ObservableCollection<KeyValuePair<string, string>>(CountryCodes.Countries);
            SelectedCountry = CountryList[0]; // 默认选择第一个国家

            LoginCommand = new RelayCommand(async _ => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            try
            {
                string areaCode = SelectedCountry.Value;

                var token = await _authService.LoginAsync(areaCode, Phone, Password);
                if (!string.IsNullOrEmpty(token))
                {
                    var me = await _authService.GetMeAsync();
                    var shopResponse = await _authService.GetShopsAsync(me?.id ?? "");
                    if (shopResponse?.shops?.Count == 1)
                    {
                        await _authService.SwitchOperatorAsync(shopResponse.shops[0].OutId);
                        MessageBox.Show($"已切换到商铺：{shopResponse.shops[0].Name}");
                        // 打开主页面
                        WindowManager.OpenMainWindow();
                    }
                    else
                    {
                        ShowShopSelection(shopResponse.shops ?? [], token);
                    }
                }
                else
                {
                    MessageBox.Show("登录失败，请检查输入信息！");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleApiException(ex);
            }
        }

        private void ShowShopSelection(List<UserEntity> shops, string token)
        {
            var shopSelectionWindow = new ShopSelectionWindow(shops, token);
            shopSelectionWindow.ShowDialog();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
