using IdentityServer.DTO;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services
{
    public interface ITokenGenerationService
    {
        Task<TokenResponse> GenerateToken(IdentityUser user, string password);
    }
}
