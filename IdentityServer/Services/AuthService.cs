using System.ComponentModel;
using IdentityServer.DTO;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IdentityServer.Services
{
    public class AuthService(
        UserManager<User> userManager,
        ITokenGenerationService tokenGenerationService,
        SignInManager<User> signInManager,
        IOptions<IdentityServerConfiguration> identityServerConfiguration
    ) : IAuthService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly ITokenGenerationService _tokenService = tokenGenerationService;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly IdentityServerConfiguration _identityServerConfiguration =
            identityServerConfiguration.Value;

        public async Task<IdentityResult> RegisterUser(RegisterRequest request)
        {
            if (
                DateOnly.TryParse(request.DateOfBirth, out DateOnly dateOfBirth)
                && Enum.TryParse(request.Role, out Role role)
            )
                return await _userManager.CreateAsync(
                    new User(request.UserName, request.Email, dateOfBirth, role),
                    request.Password
                );
            else
                return IdentityResult.Failed();
        }

        public async Task<TokenResponse> AuthenticateUser(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new TokenResponse("Invalid credentials");
            }
            if (await _userManager.IsLockedOutAsync(user))
            {
                return new TokenResponse(
                    "Account locked due to multiple failed login attempts. Try again later."
                );
            }

            var result = await _signInManager.PasswordSignInAsync(
                request.Email,
                request.Password,
                false,
                true
            );
            if (!result.Succeeded)
            {
                await _userManager.AccessFailedAsync(user);
                var attempts = await _userManager.GetAccessFailedCountAsync(user);
                if (attempts >= _identityServerConfiguration.LockoutThreshold)
                {
                    await _userManager.SetLockoutEndDateAsync(
                        user,
                        DateTimeOffset.UtcNow.AddMinutes(
                            _identityServerConfiguration.LockoutDuration
                        )
                    );
                    return new TokenResponse(
                        "Account locked due to multiple failed login attempts. Try again later."
                    );
                }
                return new TokenResponse(
                    $"Invalid credentials ({_identityServerConfiguration.LockoutThreshold - attempts} attempts remaining)"
                );
            }

            await _userManager.ResetAccessFailedCountAsync(user);

            return await _tokenService.GenerateToken(user, request.Password);
        }
    }
}
