using ArticlesAPI.Models;
using ArticlesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArticlesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public PublicController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            var articles = await _articleService.GetAll(a => a.IsPublished == true);
            return Ok(articles);
        }
    }
}
