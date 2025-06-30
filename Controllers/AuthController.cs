using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NzWalks.Data;
using NzWalks.Models;
using NzWalks.Models.DTO;
using NzWalks.Services.Authentication;
using NzWalks.Services.Verification;
using NzWalks.Utils;

namespace NzWalks.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly NzWalksDbContext dbContext;
        private readonly IVerificationService verificationService;

        public AuthController(IAuthenticationService authenticationService, NzWalksDbContext dbContext)
        {
            this.authenticationService = authenticationService;
            this.verificationService  = verificationService;
            this.dbContext = dbContext;

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ManualLogin loginDetails)
        {

            try
            {
                Console.WriteLine("here1");
                if (loginDetails.ManualLoginType != ManualLoginType.EMAIL_WITH_PASSOWRD)
                    return BadRequest(new { error = "Unknown login type." });

                // Lookup user by email
                var existingUser = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == loginDetails.Email);
                if (existingUser == null)
                    return Unauthorized(new { error = "Invalid email or password." });
                Console.WriteLine("here1");
                Console.WriteLine(existingUser.UserId);

                // Hash password with UserId (to match your registration logic)
                var hashedPassword = Hashing_md5.ComputeHash(loginDetails.Email, loginDetails.Password);

                // Authenticate and generate JWT if valid
                var authResult = await authenticationService.AuthenticateAsync(existingUser.UserId, hashedPassword);

                return Ok(new AuthResponse
                {
                    Success = true,
                    JwtToken = authResult.JwtToken
                });
            }
            catch (Exception ex)
            {
                // Optional: Log exception here
                return Problem("Something went wrong :(");
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN, USER")]
        [HttpGet("user")]
        public async Task<IActionResult> GetUserFromToken()
        {
            try
            {
                var userId = User.FindFirstValue("UserId");

                var user = await dbContext.Users
                    .Where(u => u.UserId == userId)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound(new { error = "User not found" });
                }

                // You can redact sensitive information like password
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Problem("Something went wrong :(");
            }
        }



        // [AllowAnonymous]
        // [HttpPost("resetpwdinit")]
    //     public async Task<IActionResult> ResetPasswordInitiationAsync(
    //        [FromBody] PasswordResetRequest passwordReset
    //    )
    //     {
    //         try
    //         {

    //             var existingUser = await dbContext.Users
    //      .FirstOrDefaultAsync(u => u.Email == passwordReset.Email);
    //         ;

    //         await verificationService.GenerateAndSendVerificationCodeAsync(
    //           Models.VerificationCommunicationType.EMAIL,
    //          existingUser.Email,
    //           VerificationType.REGISTRATION_EMAIL);

    //         catch (Exception ex)
    //         {

    //         }
    //         }
    }
    }
    
