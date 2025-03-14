using System.Linq.Expressions;
using ArticlesAPI.Models;

namespace ArticlesAPI.Services
{
    public interface IArticleService
    {
        Task<Article> Add(Article article);
        Task<bool> Delete(int id);

        Task<List<Article>> GetAll(Expression<Func<Article, bool>> predicate);
        Task<bool> Update(Article article);
    }
}
