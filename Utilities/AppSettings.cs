namespace BizQPOS.Utilities
{
    public static class AppSettings
    {
        public static string BaseUrl => IsProduction ? "https://prod.bizq.com/backend/api/v1/" : "http://dev.bizq.com/backend/api/v1/";
        public static bool IsProduction { get; set; } = false;

        // 根据需要添加更多配置
    }
}
