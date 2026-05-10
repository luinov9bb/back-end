using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.Order;

namespace bookStore.BusinessLogic.Interfaces
{
    public interface IOrderAction
    {
        List<OrderDto> GetAllOrdersAction();
        List<OrderDto> GetOrdersByUserAction(int userId);
        OrderDto? GetOrderByIdAction(int id);
        ResponceMsg ResponseOrderCreateAction(OrderDto dto);
        ResponceMsg ResponseOrderUpdateAction(OrderDto dto);
        ResponceMsg ResponseOrderDeleteAction(int id);
    }
}
