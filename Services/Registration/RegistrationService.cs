using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NzWalks.Data;
using NzWalks.Models.Domain;
using NzWalks.Models.DTO;
using NzWalks.Services.Verification;
using NzWalks.Utils;

namespace NzWalks.Services.Registration
{
    public class RegistrationService : IRegistrationService
    {
        private readonly NzWalksDbContext dbContext;
        private readonly IVerificationService verificationService;

        public RegistrationService(NzWalksDbContext dbContext, IVerificationService verificationService)
        {
            this.dbContext = dbContext;
            this.verificationService = verificationService;
        }

        public async Task RegisterUserAsync(User user)
        {
            Console.WriteLine("here1");
            user.Email = user.Email.Trim();
            user.FirstName = user.FirstName?.Trim();
            user.LastName = user.LastName?.Trim();
            user.Password = Hashing_md5.ComputeHash(user.Email, user.Password);

            user.UserImageUrl = "some default image added by ui this image can be updated by update profile";

            user.RegistrationDate = DateTime.UtcNow;
            var existingUser = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser != null && existingUser.RegistrationStatus == RegistrationStatus.VERIFIED)
            {
            throw new Exception("User with this email already exists and is verified.");
            }
            Console.WriteLine("here2");
            
              await verificationService.GenerateAndSendVerificationCodeAsync(
              VerificationCommunicationType.EMAIL,
              user.Email,
              VerificationType.REGISTRATION_EMAIL

          );
          Console.WriteLine("here3");

            if (existingUser != null)
            {
                // Update existing user (re-registration case)
                user.UserId = existingUser.UserId;
                dbContext.Entry(existingUser).CurrentValues.SetValues(user);
            }

            else
            {
                Console.WriteLine("here3");
                user.UserId = Guid.NewGuid().ToString();
                await dbContext.Users.AddAsync(user);
            }

            await dbContext.SaveChangesAsync();

        
        }


        public async Task VerifyUserForRegistration(VerificationDetails verificationDetails)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == verificationDetails.Email);

        if (user == null)
            throw new Exception($"User with Email {verificationDetails.Email} doesn't exist.");
        if (user.RegistrationStatus == RegistrationStatus.VERIFIED)
            return;

        // Email verification (mandatory)
        var verificationInfo = new VerificationInfo
        {
            Email = verificationDetails.Email,
            VerificationCode = verificationDetails.VerificationCodeEmail,
            VerificationType = VerificationType.REGISTRATION_EMAIL,
        };

            Console.WriteLine("the veriifcation info is" + JsonSerializer.Serialize(verificationInfo));
        await verificationService.VerifyIncomingVerificationCodeAsync(verificationInfo);

        // Set verified status
        user.RegistrationStatus = RegistrationStatus.VERIFIED;
        await dbContext.SaveChangesAsync();
    }
}
    }

