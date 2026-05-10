using bookStore.BusinessLogic.Core.Categories;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.Category;

namespace bookStore.BusinessLogic.Functions.Categories
{
    public class CategoryFlow : CategoryAction, ICategoryActions
    {
        public List<CategoryDto> GetAllCategoriesAction() => ExecuteGetAllCategoriesAction();

        public CategoryDto? GetCategoryByIdAction(int id) => ExecuteGetCategoryByIdAction(id);

        public ResponceMsg ResponseCategoryCreateAction(CategoryDto dto) => ExecuteCategoryCreateAction(dto);

        public ResponceMsg ResponseCategoryUpdateAction(CategoryDto dto) => ExecuteCategoryUpdateAction(dto);

        public ResponceMsg ResponseCategoryDeleteAction(int id) => ExecuteCategoryDeleteAction(id);
    }
}
