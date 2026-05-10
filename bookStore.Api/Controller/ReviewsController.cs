using bookStore.BusinessLogic;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Review;
using Microsoft.AspNetCore.Mvc;

namespace bookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewActions _reviews;

        public ReviewsController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _reviews = bl.GetReviewActions();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_reviews.GetAllReviewsAction());
        }

        [HttpGet("book/{bookId:int}")]
        public IActionResult GetByBook(int bookId)
        {
            return Ok(_reviews.GetReviewsByBookAction(bookId));
        }

        [HttpGet("user/{userId:int}")]
        public IActionResult GetByUser(int userId)
        {
            return Ok(_reviews.GetReviewsByUserAction(userId));
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _reviews.GetReviewByIdAction(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateReviewDto dto)
        {
            return Ok(_reviews.ResponseReviewCreateAction(dto));
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateReviewDto dto)
        {
            return Ok(_reviews.ResponseReviewUpdateAction(dto));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            return Ok(_reviews.ResponseReviewDeleteAction(id));
        }
    }
}
