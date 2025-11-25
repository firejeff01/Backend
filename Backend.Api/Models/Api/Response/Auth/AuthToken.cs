using System.Text.Json.Serialization;

namespace Backend.Api.Models.Api.Response.Auth
{
    public class AuthToken
    {
        /// <summary>
        /// Token
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        [JsonPropertyName("expiry_time")]
        public DateTime ExpiryTime { get; set; }
    }
}
