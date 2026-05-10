using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.Cart;

namespace bookStore.BusinessLogic.Interfaces
{
    public interface ICartActions
    {
        CartDto GetCartByUserAction(int userId);
        ResponceMsg ResponseAddToCartAction(AddToCartDto dto);
        ResponceMsg ResponseUpdateCartItemAction(UpdateCartItemDto dto);
        ResponceMsg ResponseRemoveCartItemAction(int userId, int cartItemId);
        ResponceMsg ResponseClearCartAction(int userId);
    }
}
