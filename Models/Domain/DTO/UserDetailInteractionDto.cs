namespace NzWalks.Models.DTO
{
    /// <summary>
    /// DTO used for registering or interacting with user data through API.
    /// Designed for use with SQL database-backed entities.
    /// </summary>
    public class UserDetailInteractionDto
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Email { get; set; } = null!;  // Required

        public string Password { get; set; } = null!;  // Required

        public string? Phone { get; set; }

        public string? Gender { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public Role Role { get; set; }  // Optional: Set internally for roles like Admin/User
    }
    public enum Role
    {
        ADMIN = 100,
        USER = 200
    }
}
