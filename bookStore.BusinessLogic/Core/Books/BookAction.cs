using AutoMapper;
using bookStore.BusinessLogic.Mapping;
using bookStore.DataAccess.Context;
using bookStore.Domain.Entities;
using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.Book;
using Microsoft.EntityFrameworkCore;

namespace bookStore.BusinessLogic.Core.Books
{
    public class BookAction
    {
        protected static readonly IMapper Mapper = MapperConfig.Mapper;

        protected static IQueryable<Book> BooksQuery(BookContext db) =>
            db.Books
                .Where(b => !b.IsDeleted)
                .Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
                .Include(b => b.Images);

        protected List<BookDto> ExecuteGetAllBooksAction()
        {
            using var db = new BookContext();
            var list = BooksQuery(db).AsNoTracking().ToList();
            return Mapper.Map<List<BookDto>>(list);
        }

        protected BookDto? ExecuteGetBookByIdAction(int id)
        {
            using var db = new BookContext();
            var entity = BooksQuery(db).AsNoTracking().FirstOrDefault(b => b.Id == id);
            return entity == null ? null : Mapper.Map<BookDto>(entity);
        }

        protected ResponceMsg ExecuteBookCreateAction(BookDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Название обязательно." };
            }

            using var db = new BookContext();
            var book = new Book
            {
                Title = dto.Title.Trim(),
                Author = dto.Author?.Trim() ?? string.Empty,
                Description = dto.Description?.Trim() ?? string.Empty,
                Price = dto.Price,
                Stock = dto.Stock,
                IsDeleted = false
            };
            db.Books.Add(book);
            db.SaveChanges();

            ApplyCoverImageFromDto(db, book.Id, dto.CoverImageUrl);
            ApplyCategoriesFromDto(db, book.Id, dto.Category);
            db.SaveChanges();

            return new ResponceMsg { IsSuccess = true, Message = "Книга добавлена." };
        }

        protected ResponceMsg ExecuteBookUpdateAction(BookDto dto)
        {
            if (dto.Id <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректный идентификатор." };
            }

            using var db = new BookContext();
            var book = db.Books
                .Include(b => b.BookCategories)
                .FirstOrDefault(b => b.Id == dto.Id && !b.IsDeleted);
            if (book == null)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Книга не найдена." };
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Название обязательно." };
            }

            book.Title = dto.Title.Trim();
            book.Author = dto.Author?.Trim() ?? string.Empty;
            book.Description = dto.Description?.Trim() ?? string.Empty;
            book.Price = dto.Price;
            book.Stock = dto.Stock;

            db.BookCategories.RemoveRange(book.BookCategories);
            ApplyCoverImageFromDto(db, book.Id, dto.CoverImageUrl);
            ApplyCategoriesFromDto(db, book.Id, dto.Category);

            db.SaveChanges();
            return new ResponceMsg { IsSuccess = true, Message = "Книга обновлена." };
        }

        protected ResponceMsg ExecuteBookDeleteAction(int id)
        {
            using var db = new BookContext();
            var book = db.Books.FirstOrDefault(b => b.Id == id && !b.IsDeleted);
            if (book == null)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Книга не найдена." };
            }

            book.IsDeleted = true;
            db.SaveChanges();
            return new ResponceMsg { IsSuccess = true, Message = "Книга скрыта из каталога." };
        }

        private static void ApplyCoverImageFromDto(BookContext db, int bookId, string? coverUrl)
        {
            var existing = db.BookImgs.Where(i => i.BookId == bookId).ToList();
            if (existing.Count > 0)
            {
                db.BookImgs.RemoveRange(existing);
            }

            var trimmed = coverUrl?.Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                return;
            }

            db.BookImgs.Add(new BookImgData
            {
                BookId = bookId,
                Url = trimmed,
                IsActive = true,
            });
        }

        private static void ApplyCategoriesFromDto(BookContext db, int bookId, string? categoryNames)
        {
            if (string.IsNullOrWhiteSpace(categoryNames))
            {
                return;
            }

            var names = categoryNames.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            foreach (var name in names)
            {
                var category = db.Categories.FirstOrDefault(c => c.Name == name);
                if (category == null)
                {
                    continue;
                }

                var exists = db.BookCategories.Any(bc => bc.BookId == bookId && bc.CategoryId == category.Id);
                if (exists)
                {
                    continue;
                }

                db.BookCategories.Add(new BookCategory
                {
                    BookId = bookId,
                    CategoryId = category.Id
                });
            }
        }
    }
}
