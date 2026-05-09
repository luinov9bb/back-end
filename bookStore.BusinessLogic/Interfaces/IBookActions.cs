using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.Book;

namespace bookStore.BusinessLogic.Interfaces
{
    public interface IBookActions
    {
        List<BookDto> GetAllBooksAction();
        BookDto? GetBookByIdAction(int id);
        ResponceMsg ResponseBookCreateAction(BookDto dto);
        ResponceMsg ResponseBookUpdateAction(BookDto dto);
        ResponceMsg ResponseBookDeleteAction(int id);
    }
}
