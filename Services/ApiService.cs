using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using BizQPOS.Utilities;

namespace BizQPOS.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private string? _baseUrl;

        public ApiService(string baseUrl)
        {
            _httpClient = new HttpClient();
            SetBaseUrl(baseUrl);

            // 自动加载持久化的 token
            var token = Properties.Settings.Default.Token;
            if (!string.IsNullOrEmpty(token))
            {
                SetToken(token);
            }
        }

        public void SetBaseUrl(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public void SetToken(string token, string headerName = "authorization")
        {
            if (_httpClient.DefaultRequestHeaders.Contains(headerName))
            {
                _httpClient.DefaultRequestHeaders.Remove(headerName);
            }

            _httpClient.DefaultRequestHeaders.Add(headerName, token);

            // 持久化 token
            Properties.Settings.Default.Token = token;
            Properties.Settings.Default.Save();
        }

        public void ClearToken()
        {
            // 清除 token
            Properties.Settings.Default.Token = string.Empty;
            Properties.Settings.Default.Save();
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var requestUrl = CombineUrl(endpoint);
            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                return await HandleResponse<T>(response);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {

            var requestUrl = CombineUrl(endpoint);
            var content = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(requestUrl, content);
                return await HandleResponse<T>(response);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<T>(responseBody);
                if (result == null)
                {
                    throw new InvalidOperationException("Failed to deserialize response: Response body was empty or invalid.");
                }
                return result;
            }
            else
            {
                var errorDetails = ParseErrorDetails(responseBody);
                throw new ApiException(errorDetails?.Message ?? response.ReasonPhrase, response.StatusCode, errorDetails);
            }
        }

        private ApiErrorDetails? ParseErrorDetails(string responseBody)
        {
            try
            {
                var errors = JsonSerializer.Deserialize<List<ApiErrorDetails>>(responseBody);
                return errors?[0];
            }
            catch
            {
                return null;
            }
        }

        private void HandleException(Exception ex)
        {
            if (ex is ApiException apiEx)
            {
                ErrorHandler.HandleApiException(apiEx);
            }
            else
            {
                ErrorHandler.HandleApiException(ex);
            }
        }

        private void EnsureLoggedIn()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.Token))
            {
                // 如果 token 不存在或无效，跳转到登录页面
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var loginWindow = new Views.LoginWindow();
                    loginWindow.Show();

                    foreach (var window in Application.Current.Windows)
                    {
                        if (window is not Views.LoginWindow)
                        {
                            (window as Window)?.Close();
                        }
                    }
                });

                throw new UnauthorizedAccessException("User is not logged in.");
            }
        }

        private string CombineUrl(string endpoint)
        {
            if (string.IsNullOrEmpty(_baseUrl))
                throw new InvalidOperationException("Base URL is not set");

            return new Uri(new Uri(_baseUrl), endpoint).ToString();
        }
    }

    public class ApiException : Exception
    {
        public System.Net.HttpStatusCode? StatusCode { get; }
        public ApiErrorDetails? ErrorDetails { get; }

        public ApiException(string message, System.Net.HttpStatusCode? statusCode, ApiErrorDetails? errorDetails = null)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorDetails = errorDetails;
        }
    }

    public class ApiErrorDetails
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("subcode")]
        public string? SubCode { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
