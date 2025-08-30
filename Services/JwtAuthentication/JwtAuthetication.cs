using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NzWalks.Data;
using NzWalks.Models.Domain;
using NzWalks.Models.DTO;
using NzWalks.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NzWalks.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly NzWalksDbContext dbContext;
        private readonly IConfiguration configuration;

        public AuthenticationService(NzWalksDbContext dbContext , IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        public async Task<AuthResponse> AuthenticateAsync(string userId , string password)

        
        {

            try
            {
                var user = await dbContext.Users
                    .FirstOrDefaultAsync(u => u.UserId == userId && u.Password == password && u.RegistrationStatus == RegistrationStatus.VERIFIED);

                if (user == null)
                    throw new Exception("User not found . Invalid Email or Password");

                var token = GenerateJwtToken(user);

                return new AuthResponse
                {
                    JwtToken = token,
                    Success = true

                };
            }
            catch (Exception ex)
            {
                 return new AuthResponse
        {
            Success = false,
            JwtToken = ""
            
        };
    }
            }
            

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim("UserId", user.UserId),
                new Claim(ClaimTypes.GivenName, user.FirstName ?? ""),
                new Claim(ClaimTypes.Surname, user.LastName ?? ""),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(48),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}