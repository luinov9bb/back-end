using bookStore.BusinessLogic;
using bookStore.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserActions _users;

        public UsersController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _users = bl.GetUserActions();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_users.GetAllUsersAction());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var user = _users.GetUserByIdAction(id);
            return user == null ? NotFound() : Ok(user);
        }
    }
}