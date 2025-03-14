using IdentityServer.DTO;
using IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace IdentityServer.Controllers
{
    [Route("/api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authservice;
        private readonly IRoleService _roleService;

        public AuthController(IAuthService userManager, IRoleService roleService)
        {
            _authservice = userManager;
            _roleService = roleService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            request.Role = request.Role ?? "User";

            var result = await _authservice.RegisterUser(request);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _roleService.AssignRole(request.Email, request.Role);

            return Ok(request);
        }

        [EnableRateLimiting("fixedWindow")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var tokenResponse = await _authservice.AuthenticateUser(request);

            if (tokenResponse == null || tokenResponse.Error != null)
            {
                return Unauthorized(new { Error = "Invalid credentials" });
            }

            return Ok(tokenResponse);
        }
    }
}
