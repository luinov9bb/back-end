namespace bookStore.Domain.Models.User
{
    public class LoginResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
        public UserDto? User { get; set; }
    }
}
