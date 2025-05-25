using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NzWalks.Extensions;
using NzWalks.Models.DTO;
using NzWalks.Services.Registration;

namespace NzWalks.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class UserRegistrationController : ControllerBase
    {
        private readonly IRegistrationService registrationService;

        public UserRegistrationController(IRegistrationService registrationService)
        {
            this.registrationService = registrationService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDetailInteractionDto userDetailInteractiondto)
        {
            try
            {
                if (!userDetailInteractiondto.IsValid())
                    return BadRequest(new { error = "Invalid registration details." });

                var user = userDetailInteractiondto.ToDomainUser();
                await registrationService.RegisterUserAsync(user);

                return Ok(new { message = "Registration successful." });
            }
            catch (Exception ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyUser([FromBody] VerificationDetails verificationDetails)
        {
        try
        {
        await registrationService.VerifyUserForRegistration(verificationDetails);
        return Ok(new { code = 2000, message = "Registration successful." });
        }
        catch (Exception ex)
        {
        // Add your logger here if needed
        return Problem($"Verification failed: {ex.Message}");
        }
    }

    }
}

        

