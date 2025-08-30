using System.Threading.Tasks;
using NzWalks.Models.DTO;
using NzWalks.Models.Domain;

namespace NzWalks.Services.Verification
{
    public interface IVerificationService
    {
        Task GenerateAndSendVerificationCodeAsync(
            VerificationCommunicationType communicationType,
            string email,
            VerificationType verificationType,
            bool isTestData = false
        );

        Task VerifyIncomingVerificationCodeAsync(VerificationInfo verificationInfo);
    }
}