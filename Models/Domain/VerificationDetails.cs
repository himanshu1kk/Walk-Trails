using System.Text.Json.Serialization;

namespace NzWalks.Models.DTO;

public class VerificationDetails
{
    public string Email { get; set; }
    public int VerificationCodeEmail { get; set; }
    public VerificationMode VerificationMode { get; set; }
}


public enum VerificationMode
{
    EMAIL = 100
}