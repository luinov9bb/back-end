namespace bookStore.Domain.Models.Cart
{
    public class UpdateCartItemDto
    {
        public int UserId { get; set; }
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
