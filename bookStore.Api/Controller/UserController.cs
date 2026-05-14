using bookStore.Api.Authorization;
using bookStore.BusinessLogic;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.User;
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

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public IActionResult AdminUpdate(int id, [FromBody] AdminUserUpdateDto dto)
        {
            if (dto == null || id != dto.Id)
            {
                return BadRequest(new ResponceMsg { IsSuccess = false, Message = "Идентификатор в URL и в теле запроса должны совпадать." });
            }

            var me = User.GetCurrentUserId();
            if (me == null)
            {
                return Unauthorized();
            }

            var result = _users.AdminUpdateUserAction(dto, me.Value);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public IActionResult AdminSoftDelete(int id)
        {
            var me = User.GetCurrentUserId();
            if (me == null)
            {
                return Unauthorized();
            }

            var result = _users.AdminSoftDeleteUserAction(id, me.Value);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}