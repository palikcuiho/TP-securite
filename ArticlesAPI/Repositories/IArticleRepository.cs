using System.Linq.Expressions;
using ArticlesAPI.Models;

namespace ArticlesAPI.Repositories
{
    public interface IArticleRepository
    {
        Task<int> Add(Article article);

        Task<Article?> Get(int id);
        Task<Article?> Get(Expression<Func<Article, bool>> predicate);
        Task<List<Article>> GetAll(Expression<Func<Article, bool>> predicate);
        Task<bool> Update(Article article);
        Task<bool> Delete(int id);
    }
}
