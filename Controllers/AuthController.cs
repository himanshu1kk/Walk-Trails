using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NzWalks.Data;
using NzWalks.Models;
using NzWalks.Models.DTO;
using NzWalks.Services.Authentication;
using NzWalks.Utils;

namespace NzWalks.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly NzWalksDbContext dbContext;

        public AuthController(IAuthenticationService authenticationService, NzWalksDbContext dbContext)
        {
            this.authenticationService = authenticationService;
            this.dbContext = dbContext;

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ManualLogin loginDetails)
        {

            try
            {
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

                // if (!authResult.Success)
                //     return Unauthorized(new { error = "Invalid email or password." });

                // Optionally: Add scheme if you want AuthScheme in response
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
    }
}