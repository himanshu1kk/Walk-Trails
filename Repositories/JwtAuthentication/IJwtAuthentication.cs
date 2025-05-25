using System.Threading.Tasks;
using NzWalks.Models.DTO;

namespace NzWalks.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthResponse> AuthenticateAsync(string userId, string password);
    }
}