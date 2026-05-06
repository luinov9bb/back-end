using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookStore.Domain.Models.Order
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }

        public string BookInfo { get; set; }
        public int Quantity { get; set; }
    }
}
