namespace ArticlesAPI.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public User Author { get; set; }
        public int Likes { get; set; }
        public DateOnly? PublishedOn { get; set; }
        public bool IsPublished => PublishedOn != null;
    }
}
