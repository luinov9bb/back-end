using bookStore.BusinessLogic.Core.Reviews;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.Review;

namespace bookStore.BusinessLogic.Functions.Review
{
    public class ReviewFlow : ReviewAction, IReviewActions
    {
        public List<ReviewDto> GetAllReviewsAction() => ExecuteGetAllReviewsAction();

        public List<ReviewDto> GetReviewsByBookAction(int bookId) => ExecuteGetReviewsByBookAction(bookId);

        public List<ReviewDto> GetReviewsByUserAction(int userId) => ExecuteGetReviewsByUserAction(userId);

        public ReviewDto? GetReviewByIdAction(int id) => ExecuteGetReviewByIdAction(id);

        public ResponceMsg ResponseReviewCreateAction(CreateReviewDto dto) => ExecuteReviewCreateAction(dto);

        public ResponceMsg ResponseReviewUpdateAction(UpdateReviewDto dto) => ExecuteReviewUpdateAction(dto);

        public ResponceMsg ResponseReviewDeleteAction(int id) => ExecuteReviewDeleteAction(id);
    }
}
