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
            user.Email = user.Email.Trim();
            user.FirstName = user.FirstName?.Trim();
            user.LastName = user.LastName?.Trim();
            user.Password = Hashing_md5.ComputeHash(user.Email, user.Password);

            user.UserImageUrl = "user profile image ";//blob integration needs to be done here so that we can store a blob url
            user.RegistrationDate = DateTime.UtcNow;

            var existingUser = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser != null && existingUser.RegistrationStatus == RegistrationStatus.VERIFIED)
            {
                throw new Exception("User with this email already exists and is verified.");
            }

            await verificationService.GenerateAndSendVerificationCodeAsync(
                VerificationCommunicationType.EMAIL,
                user.Email,
                VerificationType.REGISTRATION_EMAIL
            );

            if (existingUser != null)
            {
                user.UserId = existingUser.UserId;
                dbContext.Entry(existingUser).CurrentValues.SetValues(user);
            }
            else
            {
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

            var verificationInfo = new VerificationInfo
            {
                Email = verificationDetails.Email,
                VerificationCode = verificationDetails.VerificationCodeEmail,
                VerificationType = VerificationType.REGISTRATION_EMAIL,
            };

            await verificationService.VerifyIncomingVerificationCodeAsync(verificationInfo);

            user.RegistrationStatus = RegistrationStatus.VERIFIED;
            await dbContext.SaveChangesAsync();
        }
    }
}
