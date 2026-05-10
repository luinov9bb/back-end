using bookStore.BusinessLogic.Functions.Auth;
using bookStore.BusinessLogic.Functions.Books;
using bookStore.BusinessLogic.Functions.Cart;
using bookStore.BusinessLogic.Functions.Categories;
using bookStore.BusinessLogic.Functions.Favorite;
using bookStore.BusinessLogic.Functions.Order;
using bookStore.BusinessLogic.Functions.Review;
using bookStore.BusinessLogic.Interfaces;

namespace bookStore.BusinessLogic
{
    public class BusinessLogic
    {
        public BusinessLogic() { }

        public ISession GetSessionBL()
        {
            return new SessionBL();
        }

        public IBookActions GetBookActions()
        {
            return new BookFlow();
        }

        public ICategoryActions GetCategoryActions()
        {
            return new CategoryFlow();
        }

        public ICartActions GetCartActions()
        {
            return new CartFlow();
        }

        public IOrderAction GetOrderActions()
        {
            return new OrderFlow();
        }

        public IFavoriteActions GetFavoriteActions()
        {
            return new FavoriteFlow();
        }

        public IReviewActions GetReviewActions()
        {
            return new ReviewFlow();
        }

        public IAuthActions GetAuthActions()
        {
            return new AuthFlow();
        }
    }
}
