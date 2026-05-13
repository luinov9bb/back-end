using System.Security.Claims;
using bookStore.BusinessLogic;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Entities;
using bookStore.Domain.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthActions _auth;
        private readonly IUserActions _users;

        public AuthController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _auth = bl.GetAuthActions();
            _users = bl.GetUserActions();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserCreateDto dto)
        {
            var result = _auth.RegisterAction(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            var dto = new ULoginData
            {
                Credential = request.Credential?.Trim() ?? string.Empty,
                Password = request.Password ?? string.Empty,
                LoginIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                LoginDateTime = DateTime.UtcNow,
            };

            var result = _auth.LoginAction(dto);
            return result.IsSuccess ? Ok(result) : Unauthorized(result);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idValue, out var userId))
            {
                return Unauthorized();
            }

            var user = _users.GetUserByIdAction(userId);
            return user == null ? NotFound() : Ok(user);
        }
    }
}
