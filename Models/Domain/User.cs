using System;
using NzWalks.Models.DTO;

namespace NzWalks.Models.Domain
{
    public class User
    {

        public string UserId { get; set; }

        public string ?Username { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public DateOnly? DateOfBirth { get; set; }

        public string ?UserImageUrl { get; set; }

        public int? PointsAdded { get; set; }

        public DateTime? RegistrationDate { get; set; } = DateTime.UtcNow;

        public RegistrationStatus? RegistrationStatus { get; set; }

        public Role Role { get; set; }



        public User()
        {
            this.UserId = Guid.NewGuid().ToString();
            //means whenver we initiliaze a user object a userid will always be assigned to it

        }

    }
        public enum RegistrationStatus
        {
            UNVERIFIED = 1, // Verification not done yet, may in some cases have non-UNKNOWN Role.
            VERIFIED = 2, // proper registered user, generally has a non-UNKNOWN Role.
        }

        

    
}
