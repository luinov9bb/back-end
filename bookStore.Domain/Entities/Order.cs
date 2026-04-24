using bookStore.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bookStore.Domain.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public UserData? User { get; set; } // Связь с пользователем
        public decimal TotalPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}