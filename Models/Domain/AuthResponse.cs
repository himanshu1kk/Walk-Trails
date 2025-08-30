using System.Text.Json.Serialization;
namespace NzWalks.Models.Domain;


    public class AuthResponse
    {

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("jwtToken")]
        public string JwtToken { get; set; } = "";
    }

