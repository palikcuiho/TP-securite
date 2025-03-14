namespace ArticlesAPI.Models
{
    public class User(string userName, string email, int age, Role role)
    {
        public string UserName { get; set; } = userName;
        public string Email { get; set; } = email;
        public Role Role { get; set; } = role;
        public int Age { get; set; } = age;
        public List<Article> Articles { get; set; } = [];
        public Article? LikedArticle { get; set; }
    }

    public enum Role
    {
        User = 0,
        Admin,
        Guest,
    }
}
