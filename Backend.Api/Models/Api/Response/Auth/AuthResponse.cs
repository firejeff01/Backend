using System.Text.Json.Serialization;

namespace Backend.Api.Models.Api.Response.Auth
{
    public class AuthResponse : BaseFormResponse
    {
        /// <summary>
        /// Token
        /// </summary>
        [JsonPropertyName("token")]
        public AuthToken AuthToken { get; set; }
    }
}
