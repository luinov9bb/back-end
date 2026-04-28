using bookStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace bookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // Временное хранилище в памяти
        private static List<UserData> _users = new List<UserData>();
        private static int _nextId = 1;

        // 1. Получить всех пользователей
        [HttpGet("all")]
        public IActionResult GetAllUsers()
        {
            return Ok(_users);
        }

        // 2. Получить одного по ID
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {id} not found" });
            }

            return Ok(user);
        }

        // 3. Создать нового пользователя
        [HttpPost]
        public IActionResult CreateUser([FromBody] UserData user)
        {
            user.Id = _nextId++;
            user.RegisteredOn = DateTime.UtcNow;

            _users.Add(user);

            return Created($"/api/users/{user.Id}", user);
        }

        // 4. Обновить данные
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserData updatedUser)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == id);

            if (existingUser == null)
            {
                return NotFound(new { Message = $"User with ID {id} not found" });
            }

            existingUser.Username = updatedUser.Username;
            existingUser.Email = updatedUser.Email;

            return Ok(existingUser);
        }

        // 5. Удалить пользователя
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {id} not found" });
            }

            _users.Remove(user);

            return NoContent();
        }
    }
}