using Microsoft.AspNetCore.Identity;
using NzWalks.Models.Domain;
using NzWalks.Models.Dto;

namespace NzWalks{
    public interface ITokenRepository{
    string CreateJwtToken(IdentityUser user,List<string> Roles);
    //when we call this method a token is created
    }
}
