using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Models.Api.Request.Auth
{
    public class AuthRequest
    {
        /// <summary>
        /// Email
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Password
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }
}
