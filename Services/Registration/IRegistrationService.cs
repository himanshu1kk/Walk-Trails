using NzWalks.Models.Domain;
using NzWalks.Models.DTO;

namespace NzWalks.Services.Registration
{
    public interface IRegistrationService
    {
        Task RegisterUserAsync(User user);
        Task VerifyUserForRegistration(VerificationDetails verificationDetails); 
    }
}
