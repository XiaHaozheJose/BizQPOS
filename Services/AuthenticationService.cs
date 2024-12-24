using System.Text.Json.Serialization;
using BizQPOS.Utilities;
using Models;
using System.Text.Json;
using BizQPOS.Models;

namespace BizQPOS.Services
{
    public class AuthenticationService
    {
        private readonly ApiService _apiService;

        public AuthenticationService()
        {
            _apiService = new ApiService(AppSettings.BaseUrl);
        }

        public class LoginResponse
        {
            [JsonPropertyName("token")]
            public required string Token { get; set; }
        }

        public void SetToken(string token)
        {
            _apiService.SetToken(token);
        }

        public async Task<string> LoginAsync(string areaCode, string phone, string password)
        {
            var loginRequest = new { areaCode, phone, password, type = "user", platform = "web" };
            var response = await _apiService.PostAsync<LoginResponse>("operators-login", loginRequest);
            var token = response.Token;

            _apiService.SetToken(token);
            return token;
        }

        public async Task<Operator?> GetMeAsync()
        {
            var response = await _apiService.GetAsync<OperatorResponse>("me");

            if (response?.Operator == null)
            {
                throw new InvalidOperationException("Failed to retrieve operator data.");
            }

            PersistCurrentUser(response.Operator, response.Operator.type == Operator.UserType.Shop);
            GlobalState.SetCurrentOperator(response);
            return response.Operator;
        }

        public async Task<ShopListResponse> GetShopsAsync(string ownerId)
        {
            return await _apiService.GetAsync<ShopListResponse>($"shops-list-basic?ownerId={ownerId}");
        }

        public async Task SwitchOperatorAsync(string outId)
        {
            var switchRequest = new { type = "shop", outId };
            var response = await _apiService.PostAsync<LoginResponse>("switch-operator", switchRequest);
            var token = response.Token;
            _apiService.SetToken(token);
        }

        private void PersistCurrentUser(Operator operatorData, bool isShop)
        {
            var userDataJson = JsonSerializer.Serialize(operatorData, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            Properties.Settings.Default.CurrentUser = userDataJson;
            Properties.Settings.Default.IsShop = isShop;
            Properties.Settings.Default.Save();
        }

        public Operator? LoadCurrentUser()
        {
            var userDataJson = Properties.Settings.Default.CurrentUser;
            if (string.IsNullOrEmpty(userDataJson))
            {
                return null;
            }

            return JsonSerializer.Deserialize<Operator>(userDataJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
