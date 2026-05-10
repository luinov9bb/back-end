namespace bookStore.Domain.Models.Favorite
{
    public class AddFavoriteDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}
