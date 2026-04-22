
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookStore.Domain.Entities
{
    internal class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; } // Цена может меняться, фиксируем ту, что была при покупке
    }
}
