using NzWalks.Models.DTO;
using NzWalks.Models.Domain;

namespace NzWalks.Extensions
{
    public static class UserDtoExtensions
    {
        public static User ToDomainUser(this UserDetailInteractionDto userDetailInteractionDto)
        {
            var user = new User
            {
                FirstName = userDetailInteractionDto.FirstName,
                LastName = userDetailInteractionDto.LastName,
                Email = userDetailInteractionDto.Email,
                Password = userDetailInteractionDto.Password,
                DateOfBirth = userDetailInteractionDto.DateOfBirth,
                PointsAdded = 0,
                RegistrationStatus = RegistrationStatus.UNVERIFIED,
                Role = Role.USER

            };
            return user;
        }

        public static bool IsValid(this UserDetailInteractionDto userDetailInteractionDto)
        {
            if (string.IsNullOrWhiteSpace(userDetailInteractionDto.Email))
            {
                return false;
            }

            return true;
        }
    }
}
