using bookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace bookStore.DataAccess.Context
{
    public class BookContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<BookImgData> BookImgs { get; set; }
        public DbSet<CategoryData> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DbSession.ConnectionStrings);
        }
    }
}