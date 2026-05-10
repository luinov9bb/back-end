using bookStore.BusinessLogic;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Favorite;
using Microsoft.AspNetCore.Mvc;

namespace bookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteActions _favorites;

        public FavoritesController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _favorites = bl.GetFavoriteActions();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_favorites.GetAllFavoritesAction());
        }

        [HttpGet("user/{userId:int}")]
        public IActionResult GetByUser(int userId)
        {
            return Ok(_favorites.GetFavoritesByUserAction(userId));
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _favorites.GetFavoriteByIdAction(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] AddFavoriteDto dto)
        {
            return Ok(_favorites.ResponseFavoriteCreateAction(dto));
        }

        [HttpPut]
        public IActionResult Update([FromBody] FavoriteDto dto)
        {
            return Ok(_favorites.ResponseFavoriteUpdateAction(dto));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            return Ok(_favorites.ResponseFavoriteDeleteAction(id));
        }
    }
}
