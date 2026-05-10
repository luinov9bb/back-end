using bookStore.BusinessLogic.Core.Orders;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.Order;

namespace bookStore.BusinessLogic.Functions.Order
{
    public class OrderFlow : OrderAction, IOrderAction
    {
        public List<OrderDto> GetAllOrdersAction() => ExecuteGetAllOrdersAction();

        public List<OrderDto> GetOrdersByUserAction(int userId) => ExecuteGetOrdersByUserAction(userId);

        public OrderDto? GetOrderByIdAction(int id) => ExecuteGetOrderByIdAction(id);

        public ResponceMsg ResponseOrderCreateAction(OrderDto dto) => ExecuteOrderCreateAction(dto);

        public ResponceMsg ResponseOrderUpdateAction(OrderDto dto) => ExecuteOrderUpdateAction(dto);

        public ResponceMsg ResponseOrderDeleteAction(int id) => ExecuteOrderDeleteAction(id);
    }
}
