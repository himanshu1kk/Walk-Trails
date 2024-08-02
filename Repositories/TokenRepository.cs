using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using NzWalks.Data;
using NzWalks.Models.Domain;
using NzWalks.Models.Dto;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace NzWalks{
    public class TokenRepository : ITokenRepository
    {
        public TokenRepository(IConfiguration configuration) 
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {
            var claims = new List<Claim>();
            
            claims.Add(new Claim(ClaimTypes.Email,user.Email));

            foreach(var role in roles){
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                Configuration["Jwt:Issuer"],
                Configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);//this return the string token 
            
        }
    }
}
