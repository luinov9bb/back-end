using bookStore.Api.Authorization;
using bookStore.BusinessLogic;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderAction _orders;

        public OrdersController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _orders = bl.GetOrderActions();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_orders.GetAllOrdersAction());
        }

        [HttpGet("user/{userId:int}")]
        public IActionResult GetByUser(int userId)
        {
            if (!User.CanAccessUser(userId))
            {
                return Forbid();
            }

            return Ok(_orders.GetOrdersByUserAction(userId));
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var order = _orders.GetOrderByIdAction(id);
            if (order == null)
            {
                return NotFound();
            }

            if (!User.CanAccessUser(order.UserId))
            {
                return Forbid();
            }

            return Ok(order);
        }

        [HttpPost]
        public IActionResult Create([FromBody] OrderDto dto)
        {
            if (!User.CanAccessUser(dto.UserId))
            {
                return Forbid();
            }

            return Ok(_orders.ResponseOrderCreateAction(dto));
        }

        [HttpPost("checkout-from-cart")]
        public IActionResult CheckoutFromCart([FromBody] CheckoutFromCartDto dto)
        {
            if (!User.CanAccessUser(dto.UserId))
            {
                return Forbid();
            }

            return Ok(_orders.ResponseCheckoutFromCartAction(dto.UserId));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public IActionResult Update([FromBody] OrderDto dto)
        {
            return Ok(_orders.ResponseOrderUpdateAction(dto));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            return Ok(_orders.ResponseOrderDeleteAction(id));
        }
    }
}
