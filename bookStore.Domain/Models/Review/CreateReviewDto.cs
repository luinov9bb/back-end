using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookStore.Domain.Models.Review
{
    public class CreateReviewDto
    {
        [Required(ErrorMessage = "User ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "User ID must be positive")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Book ID is required")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Review text is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Review text must be between 10 and 500 characters")]
        public string Text { get; set; } = string.Empty;
    }
}