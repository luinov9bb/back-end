using bookStore.DataAccess.Context;
using bookStore.Domain.Entities.Order;
using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.Order;
using Microsoft.EntityFrameworkCore;
using OrderEntity = bookStore.Domain.Entities.Order.Order;

namespace bookStore.BusinessLogic.Core.Orders
{
    public class OrderAction
    {
        private static OrderDto ToDto(OrderEntity o) =>
            new()
            {
                Id = o.Id,
                UserId = o.UserId,
                OrderDate = o.OrderDate,
                Total = o.TotalPrice,
                Status = o.Status,
                IsDeleted = o.IsDeleted,
                Items = o.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    OrderId = i.OrderId,
                    BookInfo = i.BookInfo ?? string.Empty,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

        private static decimal ComputeTotal(IEnumerable<OrderItemDto> items) =>
            items.Sum(i => i.Price * i.Quantity);

        protected List<OrderDto> ExecuteGetAllOrdersAction()
        {
            using var db = new OrderContext();
            var list = db.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .Where(o => !o.IsDeleted)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
            return list.Select(ToDto).ToList();
        }

        protected List<OrderDto> ExecuteGetOrdersByUserAction(int userId)
        {
            if (userId <= 0)
            {
                return new List<OrderDto>();
            }

            using var db = new OrderContext();
            var list = db.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .Where(o => o.UserId == userId && !o.IsDeleted)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
            return list.Select(ToDto).ToList();
        }

        protected OrderDto? ExecuteGetOrderByIdAction(int id)
        {
            using var db = new OrderContext();
            var entity = db.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id && !o.IsDeleted);
            return entity == null ? null : ToDto(entity);
        }

        protected ResponceMsg ExecuteOrderCreateAction(OrderDto dto)
        {
            if (dto.UserId <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректный пользователь." };
            }

            if (dto.Items == null || dto.Items.Count == 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Заказ должен содержать позиции." };
            }

            foreach (var line in dto.Items)
            {
                if (string.IsNullOrWhiteSpace(line.BookInfo))
                {
                    return new ResponceMsg { IsSuccess = false, Message = "Укажите описание книги в каждой позиции." };
                }

                if (line.Quantity < 1)
                {
                    return new ResponceMsg { IsSuccess = false, Message = "Количество в позиции должно быть не меньше 1." };
                }

                if (line.Price < 0)
                {
                    return new ResponceMsg { IsSuccess = false, Message = "Цена не может быть отрицательной." };
                }
            }

            var total = ComputeTotal(dto.Items);
            if (dto.Total > 0 && dto.Total != total)
            {
                total = dto.Total;
            }

            using var db = new OrderContext();
            var order = new OrderEntity
            {
                UserId = dto.UserId,
                OrderDate = DateTime.UtcNow,
                TotalPrice = total,
                Status = OrderStatus.Validating,
                IsDeleted = false
            };

            foreach (var line in dto.Items)
            {
                order.Items.Add(new OrderItem
                {
                    BookInfo = line.BookInfo.Trim(),
                    Quantity = line.Quantity,
                    Price = line.Price
                });
            }

            db.Orders.Add(order);
            db.SaveChanges();

            return new ResponceMsg { IsSuccess = true, Message = $"Заказ создан, номер {order.Id}." };
        }

        private static void SoftDeleteOrderById(int orderId)
        {
            if (orderId <= 0)
            {
                return;
            }

            using var db = new OrderContext();
            var order = db.Orders.FirstOrDefault(o => o.Id == orderId && !o.IsDeleted);
            if (order == null)
            {
                return;
            }

            order.IsDeleted = true;
            db.SaveChanges();
        }

        protected ResponceMsg ExecuteCheckoutFromCartAction(int userId)
        {
            if (userId <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректный пользователь." };
            }

            List<(int BookId, int Quantity, string BookInfo, decimal Price)> snapshot;
            using (var cartDb = new CartContext())
            {
                var cart = cartDb.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Book)
                    .FirstOrDefault(c => c.UserId == userId);
                if (cart == null || cart.Items.Count == 0)
                {
                    return new ResponceMsg { IsSuccess = false, Message = "Корзина пуста." };
                }

                if (cart.Items.Any(i => i.Book == null || i.Book.IsDeleted))
                {
                    return new ResponceMsg
                    {
                        IsSuccess = false,
                        Message = "В корзине есть недоступные книги. Удалите такие позиции и повторите оформление."
                    };
                }

                foreach (var line in cart.Items)
                {
                    if (line.Book!.Stock < line.Quantity)
                    {
                        return new ResponceMsg
                        {
                            IsSuccess = false,
                            Message = $"Недостаточно экземпляров «{line.Book.Title}» на складе."
                        };
                    }
                }

                snapshot = cart.Items
                    .Select(i => (i.BookId, i.Quantity,
                        $"{i.Book!.Title} — {i.Book.Author}".Trim(),
                        i.Book.Price))
                    .ToList();
            }

            var total = snapshot.Sum(x => x.Price * x.Quantity);
            int orderId;
            using (var orderDb = new OrderContext())
            {
                var order = new OrderEntity
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    TotalPrice = total,
                    Status = OrderStatus.Validating,
                    IsDeleted = false
                };

                foreach (var line in snapshot)
                {
                    order.Items.Add(new OrderItem
                    {
                        BookInfo = line.BookInfo,
                        Quantity = line.Quantity,
                        Price = line.Price
                    });
                }

                orderDb.Orders.Add(order);
                orderDb.SaveChanges();
                orderId = order.Id;
            }

            try
            {
                using var bookDb = new BookContext();
                foreach (var line in snapshot)
                {
                    var book = bookDb.Books.FirstOrDefault(b => b.Id == line.BookId && !b.IsDeleted);
                    if (book == null || book.Stock < line.Quantity)
                    {
                        SoftDeleteOrderById(orderId);
                        return new ResponceMsg
                        {
                            IsSuccess = false,
                            Message = "Не удалось зарезервировать товар на складе. Повторите попытку."
                        };
                    }

                    book.Stock -= line.Quantity;
                }

                bookDb.SaveChanges();
            }
            catch
            {
                SoftDeleteOrderById(orderId);
                return new ResponceMsg
                {
                    IsSuccess = false,
                    Message = "Ошибка при списании со склада. Повторите попытку."
                };
            }

            using (var cartDb = new CartContext())
            {
                var cart = cartDb.Carts.Include(c => c.Items).First(c => c.UserId == userId);
                if (cart.Items.Count > 0)
                {
                    cartDb.CartItems.RemoveRange(cart.Items);
                    cart.UpdatedAt = DateTime.UtcNow;
                    cartDb.SaveChanges();
                }
            }

            return new ResponceMsg { IsSuccess = true, Message = $"Заказ создан, номер {orderId}." };
        }

        protected ResponceMsg ExecuteOrderUpdateAction(OrderDto dto)
        {
            if (dto.Id <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректный идентификатор заказа." };
            }

            using var db = new OrderContext();
            var order = db.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == dto.Id && !o.IsDeleted);
            if (order == null)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Заказ не найден." };
            }

            if (dto.UserId > 0 && dto.UserId != order.UserId)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Нельзя сменить пользователя заказа." };
            }

            order.Status = dto.Status;
            order.IsDeleted = dto.IsDeleted;

            if (dto.Items != null && dto.Items.Count > 0)
            {
                foreach (var line in dto.Items)
                {
                    if (string.IsNullOrWhiteSpace(line.BookInfo) || line.Quantity < 1 || line.Price < 0)
                    {
                        return new ResponceMsg { IsSuccess = false, Message = "Некорректные данные позиций." };
                    }
                }

                db.OrderItems.RemoveRange(order.Items);
                order.Items.Clear();

                foreach (var line in dto.Items)
                {
                    order.Items.Add(new OrderItem
                    {
                        BookInfo = line.BookInfo.Trim(),
                        Quantity = line.Quantity,
                        Price = line.Price
                    });
                }

                order.TotalPrice = dto.Total > 0 ? dto.Total : ComputeTotal(dto.Items);
            }
            else if (dto.Total > 0)
            {
                order.TotalPrice = dto.Total;
            }

            db.SaveChanges();
            return new ResponceMsg { IsSuccess = true, Message = "Заказ обновлён." };
        }

        protected ResponceMsg ExecuteOrderDeleteAction(int id)
        {
            if (id <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректный идентификатор." };
            }

            using var db = new OrderContext();
            var order = db.Orders.FirstOrDefault(o => o.Id == id && !o.IsDeleted);
            if (order == null)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Заказ не найден." };
            }

            order.IsDeleted = true;
            db.SaveChanges();

            return new ResponceMsg { IsSuccess = true, Message = "Заказ помечен как удалённый." };
        }
    }
}
