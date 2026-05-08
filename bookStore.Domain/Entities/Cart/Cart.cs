using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookStore.Domain.Entities.Cart
{
    [Table("Carts")]
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "User")]
        [ForeignKey(nameof(UserId))]    
        public int UserId { get; set; }

        public UserData User { get; set; } = null!;

        [Required]
        [Display(Name = "CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
