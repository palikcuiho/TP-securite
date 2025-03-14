using System.ComponentModel.DataAnnotations;
using IdentityServer.Models;

namespace IdentityServer.DTO
{
    public class RegisterRequest(
        string userName,
        string email,
        string dateOfBirth,
        string password,
        string role
    )
    {
        public string UserName { get; set; } = userName;
        public string Email { get; set; } = email;

        [Date]
        public string DateOfBirth { get; set; } = dateOfBirth;
        public string Password { get; set; } = password;

        [EnumDataType(typeof(Role))]
        public string Role { get; set; } = role;
    }
}
