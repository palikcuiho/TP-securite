using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IdentityServer.Services
{
    public class IdentityProfileService(
        UserManager<User> userManager,
        IOptions<IdentityServerConfiguration> identityServerConfiguration
    ) : IProfileService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IdentityServerConfiguration _identityServerConfiguration =
            identityServerConfiguration.Value;

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            if (user == null)
                return;

            var claims = new List<Claim>
            {
                new("age", user.Age.ToString()),
                new(ClaimTypes.Name, user.UserName ?? ""),
                new("scope", String.Join(",", _identityServerConfiguration.Scopes)),
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            context.IsActive = user != null;
        }
    }
}
