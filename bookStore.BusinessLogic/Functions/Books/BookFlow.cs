using bookStore.BusinessLogic.Core.Books;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.Book;

namespace bookStore.BusinessLogic.Functions.Books
{
    public class BookFlow : BookAction, IBookActions
    {
        public List<BookDto> GetAllBooksAction() => ExecuteGetAllBooksAction();

        public BookDto? GetBookByIdAction(int id) => ExecuteGetBookByIdAction(id);

        public ResponceMsg ResponseBookCreateAction(BookDto dto) => ExecuteBookCreateAction(dto);

        public ResponceMsg ResponseBookUpdateAction(BookDto dto) => ExecuteBookUpdateAction(dto);

        public ResponceMsg ResponseBookDeleteAction(int id) => ExecuteBookDeleteAction(id);
    }
}
