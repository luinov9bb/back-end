using bookStore.Domain.Entities;
using bookStore.Domain.Entities.Cart;
using bookStore.Domain.Entities.Favorite;
using bookStore.Domain.Entities.Order;
using bookStore.Domain.Entities.Review;
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
            modelBuilder.Ignore<Book>();
            modelBuilder.Ignore<BookImgData>();
            modelBuilder.Ignore<BookCategory>();
            modelBuilder.Ignore<CategoryData>();
            modelBuilder.Ignore<Cart>();
            modelBuilder.Ignore<CartItem>();
            modelBuilder.Ignore<Favorite>();
            modelBuilder.Ignore<Review>();
            modelBuilder.Ignore<Order>();
            modelBuilder.Ignore<OrderItem>();

            modelBuilder.Entity<UserData>(e =>
            {
                e.ToTable("Users");
                e.HasIndex(u => u.Username).IsUnique();
                e.HasIndex(u => u.Email).IsUnique();
                e.Ignore(u => u.Orders);
                e.Ignore(u => u.Carts);
                e.Ignore(u => u.Favorites);
                e.Ignore(u => u.Reviews);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
