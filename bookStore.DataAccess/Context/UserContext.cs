using bookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace bookStore.DataAccess.Context
{
    public class UserContext : DbContext
    {
        public DbSet<UserData> Users { get; set; }

        public UserContext() { }

        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DbSession.ConnectionStrings);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserData>(e =>
            {
                e.ToTable("Users");
                e.Ignore(u => u.Orders);
                e.Ignore(u => u.Carts);
                e.Ignore(u => u.Favorites);
                e.Ignore(u => u.Reviews);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
