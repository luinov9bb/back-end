using bookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace bookStore.DataAccess.Context
{
    public class BookStoreContext : DbContext
    {
    
        public DbSet<Book> Books { get; set; }
        // Когда добавишь остальные сущности, раскомментируй их здесь:
        // public DbSet<Order> Orders { get; set; }
        // public DbSet<UserData> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DbSession.ConnectionStrings);
        }
    }
}