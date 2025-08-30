namespace NzWalks.Models.DTO
{
    public class UserDetailInteractionDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public Role Role { get; set; }
    }

    public enum Role
    {
        ADMIN = 100,
        USER = 200
    }
}
