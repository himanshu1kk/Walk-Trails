using System.Text.Json.Serialization;
using NzWalks.Models;

namespace NzWalks.Models.DTO
{
    public class AuthResponse
    {

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("jwtToken")]
        public string JwtToken { get; set; } = "";
    }
}