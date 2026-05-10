using bookStore.BusinessLogic;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Category;
using Microsoft.AspNetCore.Mvc;

namespace bookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryActions _categories;

        public CategoriesController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _categories = bl.GetCategoryActions();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_categories.GetAllCategoriesAction());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var category = _categories.GetCategoryByIdAction(id);
            return category == null ? NotFound() : Ok(category);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CategoryDto dto)
        {
            return Ok(_categories.ResponseCategoryCreateAction(dto));
        }

        [HttpPut]
        public IActionResult Update([FromBody] CategoryDto dto)
        {
            return Ok(_categories.ResponseCategoryUpdateAction(dto));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            return Ok(_categories.ResponseCategoryDeleteAction(id));
        }
    }
}
