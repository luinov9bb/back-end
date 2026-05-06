using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookStore.Domain.Entities.Order
{
    public enum OrderStatus
    {
        None,
        Declined,
        Accepted,
        Validating,
        Paid,
        Delivered,
        Refund
    }
}
