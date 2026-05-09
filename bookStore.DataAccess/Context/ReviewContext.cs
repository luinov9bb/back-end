using bookStore.Domain.Entities;
using bookStore.Domain.Entities.Cart;
using bookStore.Domain.Entities.Favorite;
using bookStore.Domain.Entities.Order;
using bookStore.Domain.Entities.Review;
using Microsoft.EntityFrameworkCore;

namespace bookStore.DataAccess.Context
{
    public class ReviewContext : DbContext
    {
        public DbSet<Review> Reviews { get; set; }

        public ReviewContext() { }

        public ReviewContext(DbContextOptions<ReviewContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DbSession.ConnectionStrings);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<BookCategory>();
            modelBuilder.Ignore<CategoryData>();
            modelBuilder.Ignore<BookImgData>();
            modelBuilder.Ignore<Cart>();
            modelBuilder.Ignore<CartItem>();
            modelBuilder.Ignore<Favorite>();
            modelBuilder.Ignore<Order>();
            modelBuilder.Ignore<OrderItem>();

            modelBuilder.Entity<UserData>(e =>
            {
                e.ToTable("Users", t => t.ExcludeFromMigrations());
                e.Ignore(u => u.Orders);
                e.Ignore(u => u.Carts);
                e.Ignore(u => u.Favorites);
            });

            modelBuilder.Entity<Review>(e =>
            {
                e.HasOne(r => r.User)
                    .WithMany(u => u.Reviews)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(r => r.Book)
                    .WithMany(b => b.Reviews)
                    .HasForeignKey(r => r.BookId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasIndex(r => new { r.UserId, r.BookId })
                    .IsUnique();
            });

            modelBuilder.Entity<Book>(e =>
            {
                e.ToTable("Books", t => t.ExcludeFromMigrations());
                e.Ignore(b => b.Images);
                e.Ignore(b => b.BookCategories);
                e.Ignore(b => b.CartItems);
                e.Ignore(b => b.Favorites);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
