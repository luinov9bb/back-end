using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookStore.Domain.Entities.Review
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(UserId))]
        public int UserId { get; set; }

        [Required]
        [ForeignKey(nameof(BookId))]
        public int BookId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Review text must be between 10 and 500 characters")]
        public string Text { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public bool IsApproved { get; set; } = true;

        [ForeignKey(nameof(UserId))]
        public UserData User { get; set; } = null!;

        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; } = null!;
    }
}