using System.Linq.Expressions;
using ArticlesAPI.DTO;
using ArticlesAPI.Models;
using ArticlesAPI.Repositories;

namespace ArticlesAPI.Services
{
    public class ArticleService(IArticleRepository articleRepository) : IArticleService
    {
        private readonly IArticleRepository _articleRepository = articleRepository;

        public async Task<Article> Add(ArticlePostDTO articleDTO, User articleAuthor)
        {
            Article article = new(
                title: articleDTO.Title,
                author: articleAuthor.UserName,
                articleAbstract: articleDTO.Abstract,
                publishedOn: DateOnly.FromDateTime(DateTime.Now),
                id: null,
                likes: 0 //todo : refactor
            );
            if (await _articleRepository.Add(article) > 0)
                return article;
            else
                throw new Exception("Unable to add article");
        }

        public async Task<bool> Delete(int id)
        {
            return await _articleRepository.Delete(id);
        }

        public async Task<List<Article>> GetAll(Expression<Func<Article, bool>> predicate)
        {
            return await _articleRepository.GetAll(predicate);
        }

        public async Task<Article?> Get(Expression<Func<Article, bool>> predicate)
        {
            return await _articleRepository.Get(predicate);
        }

        public async Task<bool> Update(Article article)
        {
            return await _articleRepository.Update(article);
        }
    }
}
