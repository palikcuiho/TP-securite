using ArticlesAPI.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace ArticlesAPI.Services
{
    public class UserService : IUserService
    {
        public User GetCurrentUser(HttpRequest request)
        {
            var claims = request.HttpContext.User.Claims;
            Console.WriteLine(claims);
            if (
                claims
                    .FirstOrDefault(c =>
                        c.Type
                        == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
                    )
                    ?.Value
                    is string email
                && claims
                    .FirstOrDefault(c =>
                        c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                    )
                    ?.Value
                    is string role
                && claims
                    .FirstOrDefault(c =>
                        c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
                    )
                    ?.Value
                    is string name
                && Int32.TryParse(claims.FirstOrDefault(c => c.Type == "age")?.Value, out int age)
            )
                return new User(name, email, age, Role.User);
            else
                throw new Exception("Unable to retrieve current user.");
        }
    }
}
