namespace bookStore.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }

        // Название книги
        public string Title { get; set; } = string.Empty;

        // Автор
        public string Author { get; set; } = string.Empty;

        // Жанр (Категория) 
        public string Category { get; set; } = string.Empty;

        // Описание книги
        public string Description { get; set; } = string.Empty;

        // Цена
        public decimal Price { get; set; }

        // Ссылка на обложку (типо может быть пустым)
        public string? CoverImageUrl { get; set; }
    }
}