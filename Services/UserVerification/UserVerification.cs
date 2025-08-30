using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NzWalks.Data;
using NzWalks.Models.DTO;
using NzWalks.Models.Domain;

namespace NzWalks.Services.Verification
{
    public class VerificationService : IVerificationService
    {
        private readonly NzWalksDbContext _dbContext;
        private readonly int VERIFICATION_EXPIRY_MINS = 15;
        private readonly int VERIFICATION_ATTEMPTS = 3;


        public VerificationService(NzWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task GenerateAndSendVerificationCodeAsync(
            VerificationCommunicationType communicationType,
            string email,
            VerificationType verificationType,
            bool isTestData = false)
        {
            Console.WriteLine("1for1");

            if (communicationType != VerificationCommunicationType.EMAIL)
                throw new ArgumentException("Verificaiton Type not supported");

            int code = 9999;
            Console.WriteLine("code for the verification is 9999");
            Console.WriteLine("1for1");

            var verification = new UserVerification
            {
                Email = email,
                VerificationCode = code,
                VerificationType = verificationType,
                VerificationAttemptsLeft = VERIFICATION_ATTEMPTS,
                CreatedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddMinutes(VERIFICATION_EXPIRY_MINS),
                IsTestData = isTestData
            };

            await _dbContext.UserVerificationDb.AddAsync(verification);
            await _dbContext.SaveChangesAsync();

            
            // await SendEmailAsync(email, code, verificationType); //to be integrated in next day

        }


    public async Task VerifyIncomingVerificationCodeAsync(VerificationInfo verificationInfo)
    {
        var now = DateTime.UtcNow; 

        var verificationData = await _dbContext.UserVerificationDb
            .Where(v => v.VerificationType == verificationInfo.VerificationType
                     && v.Email == verificationInfo.Email
                     && v.ExpireAt > now)
            .OrderByDescending(v => v.CreatedAt)
            .FirstOrDefaultAsync();

        if (verificationData == null)
            throw new Exception("No valid verification code found for this email.");

        verificationData.VerificationAttemptsLeft--;
        await _dbContext.SaveChangesAsync();

        if (!ValidateVerificationData(verificationInfo.VerificationCode, verificationData))
            throw new Exception("Invalid or expired verification code, or attempts exhausted.");
    }

    private bool ValidateVerificationData(int verificationCode, UserVerification verificationData)
    {
        if (verificationCode != verificationData.VerificationCode)
            return false;
        if (DateTime.UtcNow > verificationData.ExpireAt)
            return false;
        if (verificationData.VerificationAttemptsLeft < 0)
            return false;
        return true;
    }
}

    }
