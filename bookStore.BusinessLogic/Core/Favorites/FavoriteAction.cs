using System.Linq;
using bookStore.DataAccess.Context;
using bookStore.Domain.Entities;
using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.Book;
using bookStore.Domain.Models.Favorite;
using Microsoft.EntityFrameworkCore;
using FavoriteEntity = bookStore.Domain.Entities.Favorite.Favorite;

namespace bookStore.BusinessLogic.Core.Favorites
{
    public class FavoriteAction
    {
        private static string? ResolveCoverImageUrl(Book book)
        {
            var images = book.Images;
            if (images == null || images.Count == 0)
            {
                return null;
            }

            return images.Where(i => i.IsActive).Select(i => i.Url).FirstOrDefault()
                ?? images.Select(i => i.Url).FirstOrDefault();
        }

        private static BookDto? ToBookDto(Book? book)
        {
            if (book == null)
            {
                return null;
            }

            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Category = string.Empty,
                Description = book.Description ?? string.Empty,
                Price = book.Price,
                Stock = book.Stock,
                IsDeleted = book.IsDeleted,
                CoverImageUrl = ResolveCoverImageUrl(book)
            };
        }

        private static FavoriteDto ToDto(FavoriteEntity f) =>
            new()
            {
                Id = f.Id,
                UserId = f.UserId,
                BookId = f.BookId,
                Book = ToBookDto(f.Book)
            };

        protected List<FavoriteDto> ExecuteGetAllFavoritesAction()
        {
            using var db = new FavoriteContext();
            var list = db.Favorites
                .AsNoTracking()
                .Include(f => f.Book)
                .Where(f => !f.Book.IsDeleted)
                .OrderBy(f => f.UserId)
                .ThenBy(f => f.Id)
                .ToList();
            return list.Select(ToDto).ToList();
        }

        protected List<FavoriteDto> ExecuteGetFavoritesByUserAction(int userId)
        {
            if (userId <= 0)
            {
                return new List<FavoriteDto>();
            }

            using var db = new FavoriteContext();
            var list = db.Favorites
                .AsNoTracking()
                .Include(f => f.Book)
                    .ThenInclude(b => b.Images)
                .Where(f => f.UserId == userId && !f.Book.IsDeleted)
                .OrderBy(f => f.Id)
                .ToList();
            return list.Select(ToDto).ToList();
        }

        protected FavoriteDto? ExecuteGetFavoriteByIdAction(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            using var db = new FavoriteContext();
            var entity = db.Favorites.AsNoTracking()
                .Include(f => f.Book)
                    .ThenInclude(b => b.Images)
                .FirstOrDefault(f => f.Id == id && !f.Book.IsDeleted);
            return entity == null ? null : ToDto(entity);
        }

        protected ResponceMsg ExecuteFavoriteCreateAction(AddFavoriteDto dto)
        {
            if (dto.UserId <= 0 || dto.BookId <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректные UserId или BookId." };
            }

            using var db = new FavoriteContext();
            if (!db.Set<UserData>().AsNoTracking().Any(u => u.Id == dto.UserId))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Пользователь не найден." };
            }

            if (!db.Set<Book>().AsNoTracking().Any(b => b.Id == dto.BookId && !b.IsDeleted))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Книга не найдена." };
            }

            if (db.Favorites.Any(f => f.UserId == dto.UserId && f.BookId == dto.BookId))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Книга уже в избранном." };
            }

            db.Favorites.Add(new FavoriteEntity
            {
                UserId = dto.UserId,
                BookId = dto.BookId
            });
            db.SaveChanges();

            return new ResponceMsg { IsSuccess = true, Message = "Книга добавлена в избранное." };
        }

        protected ResponceMsg ExecuteFavoriteUpdateAction(FavoriteDto dto)
        {
            if (dto.Id <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректный идентификатор." };
            }

            if (dto.UserId <= 0 || dto.BookId <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректные UserId или BookId." };
            }

            using var db = new FavoriteContext();
            var entity = db.Favorites.FirstOrDefault(f => f.Id == dto.Id);
            if (entity == null)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Запись избранного не найдена." };
            }

            if (entity.UserId != dto.UserId)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Нельзя перенести запись другому пользователю." };
            }

            if (!db.Set<Book>().AsNoTracking().Any(b => b.Id == dto.BookId && !b.IsDeleted))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Книга не найдена." };
            }

            if (db.Favorites.Any(f => f.UserId == dto.UserId && f.BookId == dto.BookId && f.Id != dto.Id))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Такая книга уже есть в избранном." };
            }

            entity.BookId = dto.BookId;
            db.SaveChanges();

            return new ResponceMsg { IsSuccess = true, Message = "Избранное обновлено." };
        }

        protected ResponceMsg ExecuteFavoriteDeleteAction(int id)
        {
            if (id <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректный идентификатор." };
            }

            using var db = new FavoriteContext();
            var entity = db.Favorites.FirstOrDefault(f => f.Id == id);
            if (entity == null)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Запись избранного не найдена." };
            }

            db.Favorites.Remove(entity);
            db.SaveChanges();

            return new ResponceMsg { IsSuccess = true, Message = "Книга удалена из избранного." };
        }
    }
}
