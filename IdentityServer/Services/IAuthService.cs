using IdentityServer.DTO;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUser(RegisterRequest request);
        Task<TokenResponse> AuthenticateUser(LoginRequest request);
    }
}
