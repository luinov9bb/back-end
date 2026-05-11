using bookStore.Api.Authorization;
using bookStore.BusinessLogic;
using bookStore.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserActions _users;

        public UsersController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _users = bl.GetUserActions();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_users.GetAllUsersAction());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            if (!User.CanAccessUser(id))
            {
                return Forbid();
            }

            var user = _users.GetUserByIdAction(id);
            return user == null ? NotFound() : Ok(user);
        }
    }
}