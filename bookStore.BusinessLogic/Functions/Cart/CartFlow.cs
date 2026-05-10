using bookStore.BusinessLogic.Core.ShoppingCart;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.Cart;

namespace bookStore.BusinessLogic.Functions.Cart
{
    public class CartFlow : CartAction, ICartActions
    {
        public CartDto GetCartByUserAction(int userId) => ExecuteGetCartByUserAction(userId);

        public ResponceMsg ResponseAddToCartAction(AddToCartDto dto) => ExecuteAddToCartAction(dto);

        public ResponceMsg ResponseUpdateCartItemAction(UpdateCartItemDto dto) => ExecuteUpdateCartItemAction(dto);

        public ResponceMsg ResponseRemoveCartItemAction(int userId, int cartItemId) =>
            ExecuteRemoveCartItemAction(userId, cartItemId);

        public ResponceMsg ResponseClearCartAction(int userId) => ExecuteClearCartAction(userId);
    }
}
