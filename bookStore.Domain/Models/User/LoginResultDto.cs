namespace bookStore.Domain.Models.User
{
    public class LoginResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserDto? User { get; set; }
    }
}
