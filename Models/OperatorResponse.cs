using System.Text.Json.Serialization;
using BizQPOS.Models;

namespace Models
{
    public class OperatorResponse
    {
        [JsonPropertyName("operator")]
        public Operator? Operator { get; set; }
    }

    public class Operator
    {
        public UserType? type { get; set; }
        public string? id { get; set; }
        public string? platform { get; set; }

        public Payload? payload { get; set; }
        public long iat { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum UserType
        {
            [JsonPropertyName("shop")]
            Shop,

            [JsonPropertyName("user")]
            User
        }
    }

    // 父类定义
    public class Payload
    {

        [JsonPropertyName("lastLoginTimeStamp")]
        public long LastLoginTimeStamp { get; set; }

        [JsonPropertyName("areaCode")]
        public string? AreaCode { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        public App? app { get; set; }

        public Web? web { get; set; }

        [JsonPropertyName("data")]
        public UserEntity? Data { get; set; }

        public class App
        {
            public long lastLoginTimeStamp { get; set; }
        }

        public class Web
        {
            public long lastLoginTimeStamp { get; set; }
        }


    }
}