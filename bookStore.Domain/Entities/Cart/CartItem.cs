using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookStore.Domain.Entities.Cart
{
    [Table("CartItems")]
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Cart")]
        public int CartId { get; set; }

        [Required]
        [Display(Name = "Book")]
        public int BookId { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; } = null!;
    }
}
