using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models
{
    public class User(string userName, string email, DateOnly dateOfBirth, Role role) : IdentityUser
    {
        public override string? UserName { get; set; } = userName;
        public override string? Email { get; set; } = email;
        public DateOnly DateOfBirth { get; set; } = dateOfBirth;
        public Role Role { get; set; } = role;
        public int Age => GetAge(DateOfBirth);

        private static int GetAge(DateOnly dateOfBirth)
        {
            var Now = DateTime.UtcNow;
            return Now.Year
                - dateOfBirth.Year
                - (
                    Now.Month > dateOfBirth.Month
                    || Now.Month == dateOfBirth.Month && Now.Day >= dateOfBirth.Day
                        ? 0
                        : 1
                );
        }
    }

    public enum Role
    {
        User = 0,
        Admin,
        Guest,
    }
}
