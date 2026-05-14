namespace bookStore.Domain.Models.User
{
    public class AdminUserUpdateDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public bool IsActive { get; set; }
        public string? NewPassword { get; set; }
    }
}
