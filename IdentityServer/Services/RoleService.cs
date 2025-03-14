using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services
{
    public class RoleService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        : IRoleService
    {
        private readonly UserManager<User> _userManager = userManager;

        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task<IdentityResult> AssignRole(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || !Enum.TryParse(role, out Role r))
                return IdentityResult.Failed();

            return await _userManager.AddToRoleAsync(user, r.ToString());
        }
    }
}
