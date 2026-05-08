using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookStore.Domain.Entities.Order
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserData User { get; set; } = null!;

        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; }

        [InverseProperty("Order")]
        public List<OrderItem> Items { get; set; } = new();
        
        public bool IsDeleted  { get; set; }
    }
}