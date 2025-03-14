using ArticlesAPI.DTO;
using ArticlesAPI.Models;
using ArticlesAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArticlesAPI.Controllers
{
    [Route("api/user/articles")]
    [Authorize(Roles = "Admin,User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IUserService _userService;

        public UserController(IArticleService articleService, IUserService userService)
        {
            _articleService = articleService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            var currentUser = _userService.GetCurrentUser(Request);
            var articles = await _articleService.GetAll(a =>
                a.IsPublished == true || a.Author == currentUser.UserName
            );
            return Ok(articles);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetById(int id)
        {
            try
            {
                var currentUser = _userService.GetCurrentUser(Request);
                var articles = await _articleService.Get(a =>
                    a.Id == id && (a.IsPublished == true || a.Author == currentUser.UserName)
                );
                return Ok(articles);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.GetBaseException().Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArticlePostDTO body)
        {
            try
            {
                var currentUser = _userService.GetCurrentUser(Request);
                var article = await _articleService.Add(body, currentUser);

                return CreatedAtAction(nameof(GetById), new { article.Id }, article);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.GetBaseException().Message);
            }
        }
    }
}
