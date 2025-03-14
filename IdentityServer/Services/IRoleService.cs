using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services
{
    public interface IRoleService
    {
        Task<IdentityResult> AssignRole(string email, string role);
    }
}
