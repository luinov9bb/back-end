using bookStore.DataAccess.Context;
using bookStore.Domain.Entities;
using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.Review;
using Microsoft.EntityFrameworkCore;
using ReviewEntity = bookStore.Domain.Entities.Review.Review;

namespace bookStore.BusinessLogic.Core.Reviews
{
    public class ReviewAction
    {
        private const int TextMinLen = 10;
        private const int TextMaxLen = 500;

        private static ReviewDto ToDto(ReviewEntity r) =>
            new()
            {
                Id = r.Id,
                UserId = r.UserId,
                Username = r.User?.Username ?? string.Empty,
                BookId = r.BookId,
                BookTitle = r.Book?.Title ?? string.Empty,
                Rating = r.Rating,
                Text = r.Text,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                IsApproved = r.IsApproved
            };

        private static IQueryable<ReviewEntity> ReviewsWithNav(ReviewContext db) =>
            db.Reviews.AsNoTracking().Include(r => r.User).Include(r => r.Book);

        private static string? ValidateText(string? text)
        {
            var t = text?.Trim() ?? string.Empty;
            if (t.Length < TextMinLen)
            {
                return $"Текст отзыва должен быть не короче {TextMinLen} символов.";
            }

            if (t.Length > TextMaxLen)
            {
                return $"Текст отзыва не длиннее {TextMaxLen} символов.";
            }

            return null;
        }

        protected List<ReviewDto> ExecuteGetAllReviewsAction()
        {
            using var db = new ReviewContext();
            var list = ReviewsWithNav(db).OrderByDescending(r => r.CreatedAt).ToList();
            return list.Select(ToDto).ToList();
        }

        protected List<ReviewDto> ExecuteGetReviewsByBookAction(int bookId)
        {
            if (bookId <= 0)
            {
                return new List<ReviewDto>();
            }

            using var db = new ReviewContext();
            var list = ReviewsWithNav(db)
                .Where(r => r.BookId == bookId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
            return list.Select(ToDto).ToList();
        }

        protected List<ReviewDto> ExecuteGetReviewsByUserAction(int userId)
        {
            if (userId <= 0)
            {
                return new List<ReviewDto>();
            }

            using var db = new ReviewContext();
            var list = ReviewsWithNav(db)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
            return list.Select(ToDto).ToList();
        }

        protected ReviewDto? ExecuteGetReviewByIdAction(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            using var db = new ReviewContext();
            var entity = ReviewsWithNav(db).FirstOrDefault(r => r.Id == id);
            return entity == null ? null : ToDto(entity);
        }

        protected ResponceMsg ExecuteReviewCreateAction(CreateReviewDto dto)
        {
            if (dto.UserId <= 0 || dto.BookId <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректные UserId или BookId." };
            }

            if (dto.Rating is < 1 or > 5)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Оценка должна быть от 1 до 5." };
            }

            var textErr = ValidateText(dto.Text);
            if (textErr != null)
            {
                return new ResponceMsg { IsSuccess = false, Message = textErr };
            }

            using var db = new ReviewContext();
            if (!db.Set<UserData>().AsNoTracking().Any(u => u.Id == dto.UserId))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Пользователь не найден." };
            }

            if (!db.Set<Book>().AsNoTracking().Any(b => b.Id == dto.BookId))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Книга не найдена." };
            }

            if (db.Reviews.Any(r => r.UserId == dto.UserId && r.BookId == dto.BookId))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Отзыв к этой книге от этого пользователя уже существует." };
            }

            var text = dto.Text.Trim();
            db.Reviews.Add(new ReviewEntity
            {
                UserId = dto.UserId,
                BookId = dto.BookId,
                Rating = dto.Rating,
                Text = text,
                CreatedAt = DateTime.UtcNow,
                IsApproved = true
            });
            db.SaveChanges();

            return new ResponceMsg { IsSuccess = true, Message = "Отзыв добавлен." };
        }

        protected ResponceMsg ExecuteReviewUpdateAction(UpdateReviewDto dto)
        {
            if (dto.Id <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректный идентификатор отзыва." };
            }

            if (dto.UserId <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректный UserId." };
            }

            if (dto.Rating is < 1 or > 5)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Оценка должна быть от 1 до 5." };
            }

            var textErr = ValidateText(dto.Text);
            if (textErr != null)
            {
                return new ResponceMsg { IsSuccess = false, Message = textErr };
            }

            using var db = new ReviewContext();
            var entity = db.Reviews.FirstOrDefault(r => r.Id == dto.Id);
            if (entity == null)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Отзыв не найден." };
            }

            if (entity.UserId != dto.UserId)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Можно редактировать только свой отзыв." };
            }

            entity.Rating = dto.Rating;
            entity.Text = dto.Text.Trim();
            entity.UpdatedAt = DateTime.UtcNow;
            db.SaveChanges();

            return new ResponceMsg { IsSuccess = true, Message = "Отзыв обновлён." };
        }

        protected ResponceMsg ExecuteReviewDeleteAction(int id)
        {
            if (id <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректный идентификатор." };
            }

            using var db = new ReviewContext();
            var entity = db.Reviews.FirstOrDefault(r => r.Id == id);
            if (entity == null)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Отзыв не найден." };
            }

            db.Reviews.Remove(entity);
            db.SaveChanges();

            return new ResponceMsg { IsSuccess = true, Message = "Отзыв удалён." };
        }
    }
}
