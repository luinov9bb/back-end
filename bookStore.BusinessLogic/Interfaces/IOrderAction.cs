using bookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bookStore.Domain.Models.Order;
using bookStore.Domain.Models.Base;

namespace bookStore.BusinessLogic.Interfaces
{
    public interface IOrderAction
    {
        List<OrderItemDto> GetAllOrders();
    }
}
