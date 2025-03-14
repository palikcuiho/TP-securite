using IdentityServer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data
{
    public class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : IdentityDbContext<User>(options) { }
}
