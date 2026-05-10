using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.Category;

namespace bookStore.BusinessLogic.Interfaces
{
    public interface ICategoryActions
    {
        List<CategoryDto> GetAllCategoriesAction();
        CategoryDto? GetCategoryByIdAction(int id);
        ResponceMsg ResponseCategoryCreateAction(CategoryDto dto);
        ResponceMsg ResponseCategoryUpdateAction(CategoryDto dto);
        ResponceMsg ResponseCategoryDeleteAction(int id);
    }
}
