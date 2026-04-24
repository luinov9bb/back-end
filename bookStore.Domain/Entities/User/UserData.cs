using System.ComponentModel.DataAnnotations;

namespace bookStore.Domain.Entities.User
{
    public class UserData
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // Admin или User

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}