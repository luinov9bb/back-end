using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookStore.Domain.Entities
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public List<BookImgData> Images { get; set; } = new();

        [Required]
        public int Stock { get; set; }

        public List< BookCategory> BookCategories { get; set; } = new();
        public List<Cart.CartItem> CartItems { get; set; } = new();
        public List<Favorite.Favorite> Favorites { get; set; } = new();
        public List<Review.Review> Reviews { get; set; } = new();
    }
}