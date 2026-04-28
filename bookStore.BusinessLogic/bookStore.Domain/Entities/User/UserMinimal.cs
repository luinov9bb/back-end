namespace bookStore.Domain.Entities
{
    public class UserMinimal
    {
        public bool Status { get; set; }
        public string StatusMsg { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        
    }
}