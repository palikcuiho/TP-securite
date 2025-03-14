using ArticlesAPI.Models;
using ArticlesAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArticlesAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private User _currentUser;

        public UserController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            var articles = await _articleService.GetAll(a =>
                a.IsPublished == true || a.Author == _currentUser
            );
            return Ok(articles);
        }
    }
}
