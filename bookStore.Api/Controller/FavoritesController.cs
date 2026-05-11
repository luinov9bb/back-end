using bookStore.Api.Authorization;
using bookStore.BusinessLogic;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Favorite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteActions _favorites;

        public FavoritesController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _favorites = bl.GetFavoriteActions();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_favorites.GetAllFavoritesAction());
        }

        [HttpGet("user/{userId:int}")]
        public IActionResult GetByUser(int userId)
        {
            if (!User.CanAccessUser(userId))
            {
                return Forbid();
            }

            return Ok(_favorites.GetFavoritesByUserAction(userId));
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _favorites.GetFavoriteByIdAction(id);
            if (item == null)
            {
                return NotFound();
            }

            if (!User.CanAccessUser(item.UserId))
            {
                return Forbid();
            }

            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] AddFavoriteDto dto)
        {
            if (!User.CanAccessUser(dto.UserId))
            {
                return Forbid();
            }

            return Ok(_favorites.ResponseFavoriteCreateAction(dto));
        }

        [HttpPut]
        public IActionResult Update([FromBody] FavoriteDto dto)
        {
            if (!User.CanAccessUser(dto.UserId))
            {
                return Forbid();
            }

            return Ok(_favorites.ResponseFavoriteUpdateAction(dto));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var item = _favorites.GetFavoriteByIdAction(id);
            if (item == null)
            {
                return NotFound();
            }

            if (!User.CanAccessUser(item.UserId))
            {
                return Forbid();
            }

            return Ok(_favorites.ResponseFavoriteDeleteAction(id));
        }
    }
}
