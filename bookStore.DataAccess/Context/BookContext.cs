using Microsoft.EntityFrameworkCore;
using bookStore.Domain.Entities;

namespace bookStore.DataAccess.Context
{
    public class BookContext : DbContext
    {

        public virtual DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DbSession.ConnectionString);
        }
    }
}