using bookStore.BusinessLogic;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Order;
using Microsoft.AspNetCore.Mvc;

namespace bookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderAction _orders;

        public OrdersController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _orders = bl.GetOrderActions();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_orders.GetAllOrdersAction());
        }

        [HttpGet("user/{userId:int}")]
        public IActionResult GetByUser(int userId)
        {
            return Ok(_orders.GetOrdersByUserAction(userId));
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var order = _orders.GetOrderByIdAction(id);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public IActionResult Create([FromBody] OrderDto dto)
        {
            return Ok(_orders.ResponseOrderCreateAction(dto));
        }

        [HttpPut]
        public IActionResult Update([FromBody] OrderDto dto)
        {
            return Ok(_orders.ResponseOrderUpdateAction(dto));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            return Ok(_orders.ResponseOrderDeleteAction(id));
        }
    }
}
