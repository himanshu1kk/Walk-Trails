using System.Text.Json.Serialization;
using NzWalks.Models;

namespace NzWalks.Models;

public class PasswordResetRequest
{
    [JsonPropertyName("type")]
    public VerificationCommunicationType Type { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    
}
public enum VerificationCommunicationType
{
    EMAIL = 0,
  
}

