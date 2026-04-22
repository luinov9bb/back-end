using bookStore.Domain.Entities;
using bookStore.Domain.Models.Book;

namespace bookStore.BusinessLogic.Mappers
{
    public static class BookMapper
    {
        // Из базы (Entity) во фронтенд (Dto)
        public static BookDto ToDto(Book book) => new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Category = book.Category,
            Description = book.Description,
            Price = book.Price,
            CoverImageUrl = book.CoverImageUrl
        };

        // Из запроса (Request) в базу (Entity)
        public static Book ToEntity(BookRequest request) => new Book
        {
            Title = request.Title,
            Author = request.Author,
            Category = request.Category,
            Description = request.Description,
            Price = request.Price,
            CoverImageUrl = request.CoverImageUrl
        };
    }
}