namespace bookStore.Domain.Models.Cart
{
    public class AddToCartDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
