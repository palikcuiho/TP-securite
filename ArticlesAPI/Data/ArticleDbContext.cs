using ArticlesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticlesAPI.Data
{
    public class ArticleDbContext(DbContextOptions<ArticleDbContext> options) : DbContext(options)
    {
        public DbSet<Article> Articles { get; set; }
    }
}
