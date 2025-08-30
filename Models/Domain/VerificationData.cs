using System.Text.Json.Serialization;

namespace NzWalks.Models.Domain;

public class VerificationInfo
{
    public string? Email { get; set; }

    public int VerificationCode { get; set; }

    [JsonPropertyName("verificationType")]
    public VerificationType VerificationType { get; set; }
}

public enum VerificationType
{
    PASSWORD_RECOVERY_EMAIL = 0,
    REGISTRATION_EMAIL = 100,
}