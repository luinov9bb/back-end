using bookStore.Api.Authorization;
using bookStore.BusinessLogic;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartActions _cart;

        public CartController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _cart = bl.GetCartActions();
        }

        [HttpGet("user/{userId:int}")]
        public IActionResult GetByUser(int userId)
        {
            if (!User.CanAccessUser(userId))
            {
                return Forbid();
            }

            return Ok(_cart.GetCartByUserAction(userId));
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] AddToCartDto dto)
        {
            if (!User.CanAccessUser(dto.UserId))
            {
                return Forbid();
            }

            return Ok(_cart.ResponseAddToCartAction(dto));
        }

        [HttpPut("item")]
        public IActionResult UpdateItem([FromBody] UpdateCartItemDto dto)
        {
            if (!User.CanAccessUser(dto.UserId))
            {
                return Forbid();
            }

            return Ok(_cart.ResponseUpdateCartItemAction(dto));
        }

        [HttpDelete("user/{userId:int}/item/{cartItemId:int}")]
        public IActionResult RemoveItem(int userId, int cartItemId)
        {
            if (!User.CanAccessUser(userId))
            {
                return Forbid();
            }

            return Ok(_cart.ResponseRemoveCartItemAction(userId, cartItemId));
        }

        [HttpDelete("user/{userId:int}/clear")]
        public IActionResult Clear(int userId)
        {
            if (!User.CanAccessUser(userId))
            {
                return Forbid();
            }

            return Ok(_cart.ResponseClearCartAction(userId));
        }
    }
}
