using bookStore.BusinessLogic.Functions.Books;
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
    }
}
