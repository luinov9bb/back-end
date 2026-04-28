using Microsoft.AspNetCore.Mvc;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Entities;
using ISession = bookStore.BusinessLogic.Interfaces.ISession;

namespace bookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ISession _session;

        // DI
        public LoginController(ISession session)
        {
            _session = session;
        }

        [HttpPost("auth")]
        public IActionResult Login([FromBody] ULoginData loginRequest)
        {
            if (!ModelState.IsValid) return BadRequest("Неверные данные");

            // Наполняем ULoginData данными о запросе
            loginRequest.LoginIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            loginRequest.LoginDateTime = DateTime.Now;

            var result = _session.UserLogin(loginRequest);

            if (result.Status)
            {
                //в API вместо Redirect возвращаем данные юзера
                return Ok(new { Message = "Успешный вход", User = result.Username });
            }

            return Unauthorized(new { Message = result.StatusMsg });
        }
    }
}