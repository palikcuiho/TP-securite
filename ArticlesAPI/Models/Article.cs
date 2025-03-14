namespace ArticlesAPI.Models
{
    public class Article(
        int? id,
        string title,
        string articleAbstract,
        string author,
        int likes,
        DateOnly? publishedOn
    )
    {
        public int? Id { get; set; } = id;
        public string Title { get; set; } = title;
        public string Abstract { get; set; } = articleAbstract;
        public string Author { get; set; } = author;
        public int Likes { get; set; } = likes;
        public DateOnly? PublishedOn { get; set; } = publishedOn;
        public bool IsPublished => PublishedOn != null;
    }
}
