using bookStore.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookStore.Domain.Entities
{
    public class UserData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Username")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters long.")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Email Address")]
        [StringLength(30)]
        public string Email { get; set; } 

        [Required]
        [Display(Name = "Password")]    
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string PasswordHash { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime RegisteredOn { get; set; } = DateTime.Now;

        [Required]
        public UserRole Role { get; set; } = UserRole.User;

        public bool IsActive { get; set; } = true;

        public List<Order.Order> Orders { get; set; } = new();
        public List<Cart.Cart> Carts { get; set; } = new();
        public List<Favorite.Favorite> Favorites { get; set; } = new();
        public List<Review.Review> Reviews { get; set; } = new();
    }
}