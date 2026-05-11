using bookStore.BusinessLogic;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Category;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_categories.GetAllCategoriesAction());
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var category = _categories.GetCategoryByIdAction(id);
            return category == null ? NotFound() : Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create([FromBody] CategoryDto dto)
        {
            return Ok(_categories.ResponseCategoryCreateAction(dto));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public IActionResult Update([FromBody] CategoryDto dto)
        {
            return Ok(_categories.ResponseCategoryUpdateAction(dto));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            return Ok(_categories.ResponseCategoryDeleteAction(id));
        }
    }
}
