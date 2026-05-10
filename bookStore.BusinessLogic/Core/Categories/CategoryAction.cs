using bookStore.DataAccess.Context;
using bookStore.Domain.Entities;
using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.Category;
using Microsoft.EntityFrameworkCore;

namespace bookStore.BusinessLogic.Core.Categories
{
    public class CategoryAction
    {
        private static CategoryDto ToDto(CategoryData c) =>
            new()
            {
                Id = c.Id,
                Name = c.Name ?? string.Empty,
                IsActive = c.isActive
            };

        protected List<CategoryDto> ExecuteGetAllCategoriesAction()
        {
            using var db = new BookContext();
            return db.Categories.AsNoTracking().OrderBy(c => c.Name).Select(c => ToDto(c)).ToList();
        }

        protected CategoryDto? ExecuteGetCategoryByIdAction(int id)
        {
            using var db = new BookContext();
            var entity = db.Categories.AsNoTracking().FirstOrDefault(c => c.Id == id);
            return entity == null ? null : ToDto(entity);
        }

        protected ResponceMsg ExecuteCategoryCreateAction(CategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Название категории обязательно." };
            }

            var name = dto.Name.Trim();
            using var db = new BookContext();
            if (db.Categories.Any(c => c.Name == name))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Категория с таким именем уже есть." };
            }

            db.Categories.Add(new CategoryData
            {
                Name = name,
                isActive = dto.IsActive
            });
            db.SaveChanges();

            return new ResponceMsg { IsSuccess = true, Message = "Категория добавлена." };
        }

        protected ResponceMsg ExecuteCategoryUpdateAction(CategoryDto dto)
        {
            if (dto.Id <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректный идентификатор." };
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Название категории обязательно." };
            }

            var name = dto.Name.Trim();
            using var db = new BookContext();
            var entity = db.Categories.FirstOrDefault(c => c.Id == dto.Id);
            if (entity == null)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Категория не найдена." };
            }

            if (db.Categories.Any(c => c.Name == name && c.Id != dto.Id))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Категория с таким именем уже есть." };
            }

            entity.Name = name;
            entity.isActive = dto.IsActive;
            db.SaveChanges();

            return new ResponceMsg { IsSuccess = true, Message = "Категория обновлена." };
        }

        protected ResponceMsg ExecuteCategoryDeleteAction(int id)
        {
            using var db = new BookContext();
            var entity = db.Categories.FirstOrDefault(c => c.Id == id);
            if (entity == null)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Категория не найдена." };
            }

            db.Categories.Remove(entity);
            db.SaveChanges();

            return new ResponceMsg { IsSuccess = true, Message = "Категория удалена." };
        }
    }
}
