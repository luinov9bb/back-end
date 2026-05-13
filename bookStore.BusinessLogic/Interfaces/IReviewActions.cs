using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.Review;

namespace bookStore.BusinessLogic.Interfaces
{
    public interface IReviewActions
    {
        List<ReviewDto> GetAllReviewsAction();
        List<ReviewDto> GetReviewsByBookAction(int bookId);
        List<ReviewDto> GetReviewsByUserAction(int userId);
        ReviewDto? GetReviewByIdAction(int id);
        ResponceMsg ResponseReviewCreateAction(CreateReviewDto dto);
        ResponceMsg ResponseReviewUpdateAction(UpdateReviewDto dto);
        ResponceMsg ResponseReviewDeleteAction(int id);
        ResponceMsg ResponseReviewSetApprovalAction(int id, bool isApproved);
    }
}
