using System.Text.Json.Serialization;
namespace NzWalks.Models.Domain;

public class UserVerification
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("userEmail")]
    public string Email { get; set; }


    [JsonPropertyName("verificationCode")]
    public int VerificationCode { get; set; }

    [JsonPropertyName("verificationType")]
    public VerificationType VerificationType { get; set; }

    [JsonPropertyName("verificationAttemptsLeft")]
    public int VerificationAttemptsLeft { get; set; } = 3;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("expireAt")]
    public DateTime ExpireAt { get; set; }

    [JsonPropertyName("isTestData")]
    public bool IsTestData { get; set; } = false;

    public UserVerification()
    {
        this.Id = Guid.NewGuid().ToString();
    }
}

public enum VerificationCommunicationType
{
    EMAIL = 0,

}