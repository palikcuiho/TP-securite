using ArticlesAPI.Models;

namespace ArticlesAPI.Services
{
    public interface IUserService
    {
        public User GetCurrentUser(HttpRequest request);
    }
}
