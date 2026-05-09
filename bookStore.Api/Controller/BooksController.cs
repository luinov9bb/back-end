using bookStore.BusinessLogic;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Book;
using Microsoft.AspNetCore.Mvc;

namespace bookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookActions _books;

        public BooksController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _books = bl.GetBookActions();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_books.GetAllBooksAction());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var book = _books.GetBookByIdAction(id);
            return book == null ? NotFound() : Ok(book);
        }

        [HttpPost]
        public IActionResult Create([FromBody] BookDto dto)
        {
            return Ok(_books.ResponseBookCreateAction(dto));
        }

        [HttpPut]
        public IActionResult Update([FromBody] BookDto dto)
        {
            return Ok(_books.ResponseBookUpdateAction(dto));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            return Ok(_books.ResponseBookDeleteAction(id));
        }
    }
}
