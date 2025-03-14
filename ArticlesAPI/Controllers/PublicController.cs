using ArticlesAPI.Models;
using ArticlesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArticlesAPI.Controllers
{
    [Route("api/public/articles")]
    [ApiController]
    public class PublicController(IArticleService articleService) : ControllerBase
    {
        private readonly IArticleService _articleService = articleService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            var articles = await _articleService.GetAll(a => a.IsPublished == true);
            return Ok(articles);
        }
    }
}
