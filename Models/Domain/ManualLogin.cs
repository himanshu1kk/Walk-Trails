using System.ComponentModel.DataAnnotations.Schema;

namespace NzWalks.Models.Domain{
    public class ManualLogin{
    public ManualLoginType ManualLoginType { get; set; }
    public string? Email { get; set; }
    public string Password { get; set; }
}

    public enum ManualLoginType
    {
    EMAIL_WITH_PASSOWRD = 100
}

    }
    