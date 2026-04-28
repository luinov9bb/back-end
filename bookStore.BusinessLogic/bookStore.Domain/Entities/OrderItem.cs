using System.ComponentModel.DataAnnotations;

namespace bookStore.Domain.Entities
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}