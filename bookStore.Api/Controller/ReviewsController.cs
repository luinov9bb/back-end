using bookStore.Api.Authorization;
using bookStore.BusinessLogic;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Review;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_reviews.GetAllReviewsAction());
        }

        [AllowAnonymous]
        [HttpGet("book/{bookId:int}")]
        public IActionResult GetByBook(int bookId)
        {
            return Ok(_reviews.GetReviewsByBookAction(bookId));
        }

        [AllowAnonymous]
        [HttpGet("user/{userId:int}")]
        public IActionResult GetByUser(int userId)
        {
            return Ok(_reviews.GetReviewsByUserAction(userId));
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _reviews.GetReviewByIdAction(id);
            return item == null ? NotFound() : Ok(item);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create([FromBody] CreateReviewDto dto)
        {
            if (!User.CanAccessUser(dto.UserId))
            {
                return Forbid();
            }

            return Ok(_reviews.ResponseReviewCreateAction(dto));
        }

        [Authorize]
        [HttpPut]
        public IActionResult Update([FromBody] UpdateReviewDto dto)
        {
            if (!User.CanAccessUser(dto.UserId))
            {
                return Forbid();
            }

            return Ok(_reviews.ResponseReviewUpdateAction(dto));
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var item = _reviews.GetReviewByIdAction(id);
            if (item == null)
            {
                return NotFound();
            }

            if (!User.CanAccessUser(item.UserId))
            {
                return Forbid();
            }

            return Ok(_reviews.ResponseReviewDeleteAction(id));
        }
    }
}
