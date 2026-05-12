using bookStore.DataAccess.Context;
using bookStore.Domain.Entities;
using bookStore.Domain.Entities.Cart;
using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.Cart;
using Microsoft.EntityFrameworkCore;

namespace bookStore.BusinessLogic.Core.ShoppingCart
{
    public class CartAction
    {
        private static CartDto ToCartDto(Cart cart) =>
            new()
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                Items = cart.Items
                    .Where(i => i.Book == null || !i.Book.IsDeleted)
                    .Select(i => new CartItemDto
                    {
                        Id = i.Id,
                        CartId = i.CartId,
                        BookId = i.BookId,
                        Quantity = i.Quantity,
                        BookTitle = i.Book?.Title
                    }).ToList()
            };

        private static Cart GetOrCreateCart(CartContext db, int userId)
        {
            var cart = db.Carts.Include(c => c.Items).FirstOrDefault(c => c.UserId == userId);
            if (cart != null)
            {
                return cart;
            }

            cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            db.Carts.Add(cart);
            db.SaveChanges();
            return cart;
        }

        protected CartDto ExecuteGetCartByUserAction(int userId)
        {
            if (userId <= 0)
            {
                return new CartDto { UserId = userId, Items = new List<CartItemDto>() };
            }

            using var db = new CartContext();
            var cart = db.Carts
                .AsNoTracking()
                .Include(c => c.Items).ThenInclude(i => i.Book)
                .FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                return new CartDto { UserId = userId, Items = new List<CartItemDto>() };
            }

            return ToCartDto(cart);
        }

        protected ResponceMsg ExecuteAddToCartAction(AddToCartDto dto)
        {
            if (dto.UserId <= 0 || dto.BookId <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректные UserId или BookId." };
            }

            if (dto.Quantity < 1)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Количество должно быть не меньше 1." };
            }

            using var db = new CartContext();
            var book = db.Set<Book>().AsNoTracking().FirstOrDefault(b => b.Id == dto.BookId && !b.IsDeleted);
            if (book == null)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Книга не найдена." };
            }

            if (book.Stock < dto.Quantity)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Недостаточно книг на складе." };
            }

            var cart = GetOrCreateCart(db, dto.UserId);
            db.Entry(cart).Collection(c => c.Items).Load();

            var line = cart.Items.FirstOrDefault(i => i.BookId == dto.BookId);
            var newQty = line == null ? dto.Quantity : line.Quantity + dto.Quantity;
            if (newQty > book.Stock)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Недостаточно книг на складе для такого количества." };
            }

            if (line == null)
            {
                db.CartItems.Add(new CartItem
                {
                    CartId = cart.Id,
                    BookId = dto.BookId,
                    Quantity = dto.Quantity
                });
            }
            else
            {
                line.Quantity = newQty;
            }

            cart.UpdatedAt = DateTime.UtcNow;
            db.SaveChanges();

            return new ResponceMsg { IsSuccess = true, Message = "Товар добавлен в корзину." };
        }

        protected ResponceMsg ExecuteUpdateCartItemAction(UpdateCartItemDto dto)
        {
            if (dto.UserId <= 0 || dto.CartItemId <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректные данные." };
            }

            if (dto.Quantity < 1)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Количество должно быть не меньше 1." };
            }

            using var db = new CartContext();
            var item = db.CartItems
                .Include(i => i.Cart)
                .FirstOrDefault(i => i.Id == dto.CartItemId && i.Cart.UserId == dto.UserId);
            if (item == null)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Позиция не найдена." };
            }

            var book = db.Set<Book>().AsNoTracking().FirstOrDefault(b => b.Id == item.BookId && !b.IsDeleted);
            if (book == null)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Книга не найдена." };
            }

            if (dto.Quantity > book.Stock)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Недостаточно книг на складе." };
            }

            item.Quantity = dto.Quantity;
            item.Cart.UpdatedAt = DateTime.UtcNow;
            db.SaveChanges();

            return new ResponceMsg { IsSuccess = true, Message = "Количество обновлено." };
        }

        protected ResponceMsg ExecuteRemoveCartItemAction(int userId, int cartItemId)
        {
            if (userId <= 0 || cartItemId <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректные данные." };
            }

            using var db = new CartContext();
            var item = db.CartItems
                .Include(i => i.Cart)
                .FirstOrDefault(i => i.Id == cartItemId && i.Cart.UserId == userId);
            if (item == null)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Позиция не найдена." };
            }

            var cart = item.Cart;
            db.CartItems.Remove(item);
            cart.UpdatedAt = DateTime.UtcNow;
            db.SaveChanges();

            return new ResponceMsg { IsSuccess = true, Message = "Позиция удалена." };
        }

        protected ResponceMsg ExecuteClearCartAction(int userId)
        {
            if (userId <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректный пользователь." };
            }

            using var db = new CartContext();
            var cart = db.Carts.Include(c => c.Items).FirstOrDefault(c => c.UserId == userId);
            if (cart == null || cart.Items.Count == 0)
            {
                return new ResponceMsg { IsSuccess = true, Message = "Корзина уже пуста." };
            }

            db.CartItems.RemoveRange(cart.Items);
            cart.UpdatedAt = DateTime.UtcNow;
            db.SaveChanges();

            return new ResponceMsg { IsSuccess = true, Message = "Корзина очищена." };
        }
    }
}
