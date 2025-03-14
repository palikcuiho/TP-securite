using System.Linq.Expressions;
using ArticlesAPI.DTO;
using ArticlesAPI.Models;

namespace ArticlesAPI.Services
{
    public interface IArticleService
    {
        Task<Article> Add(ArticlePostDTO articleDTO, User author);
        Task<bool> Delete(int id);

        Task<List<Article>> GetAll(Expression<Func<Article, bool>> predicate);
        Task<Article?> Get(Expression<Func<Article, bool>> predicate);
        Task<bool> Update(Article article);
    }
}
