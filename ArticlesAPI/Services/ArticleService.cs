using System.Linq.Expressions;
using ArticlesAPI.Models;
using ArticlesAPI.Repositories;

namespace ArticlesAPI.Services
{
    public class ArticleService(IArticleRepository articleRepository) : IArticleService
    {
        private readonly IArticleRepository _articleRepository = articleRepository;

        public async Task<Article> Add(Article article)
        {
            await _articleRepository.Add(article);
            return article;
        }

        public async Task<bool> Delete(int id)
        {
            return await _articleRepository.Delete(id);
        }

        public async Task<List<Article>> GetAll(Expression<Func<Article, bool>> predicate)
        {
            return await _articleRepository.GetAll(predicate);
        }

        public async Task<bool> Update(Article article)
        {
            return await _articleRepository.Update(article);
        }
    }
}
