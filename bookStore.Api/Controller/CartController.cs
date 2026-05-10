using bookStore.BusinessLogic;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Cart;
using Microsoft.AspNetCore.Mvc;

namespace bookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            return Ok(_cart.GetCartByUserAction(userId));
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] AddToCartDto dto)
        {
            return Ok(_cart.ResponseAddToCartAction(dto));
        }

        [HttpPut("item")]
        public IActionResult UpdateItem([FromBody] UpdateCartItemDto dto)
        {
            return Ok(_cart.ResponseUpdateCartItemAction(dto));
        }

        [HttpDelete("user/{userId:int}/item/{cartItemId:int}")]
        public IActionResult RemoveItem(int userId, int cartItemId)
        {
            return Ok(_cart.ResponseRemoveCartItemAction(userId, cartItemId));
        }

        [HttpDelete("user/{userId:int}/clear")]
        public IActionResult Clear(int userId)
        {
            return Ok(_cart.ResponseClearCartAction(userId));
        }
    }
}
