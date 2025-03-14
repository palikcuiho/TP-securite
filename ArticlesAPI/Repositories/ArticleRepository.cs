using System.Linq.Expressions;
using ArticlesAPI.Data;
using ArticlesAPI.Models;

namespace ArticlesAPI.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        protected ArticleDbContext _dbContext { get; }

        public ArticleRepository(ArticleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> Add(Article article)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Article?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Article>> GetAll(Expression<Func<Article, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Article article)
        {
            throw new NotImplementedException();
        }

        public Task<Article?> Get(Expression<Func<Article, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
