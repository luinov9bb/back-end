using bookStore.Domain.Models.Book;

namespace bookStore.BusinessLogic.Interfaces
{
    public interface IBookService
    {
        // Получить все книги
        List<BookDto> GetAllBooks();

        // Получить одну книгу по ID
        BookDto GetBookById(int id);

        // Создать новую книгу
        void CreateBook(BookRequest request);

        // Обновить существующую
        void UpdateBook(int id, BookRequest request);

        // Удалить
        void DeleteBook(int id);
    }
}