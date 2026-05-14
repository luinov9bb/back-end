using bookStore.Domain.Entities;
using bookStore.Domain.Entities.Cart;
using bookStore.Domain.Entities.Favorite;
using bookStore.Domain.Entities.Order;
using bookStore.Domain.Entities.Review;
using Microsoft.EntityFrameworkCore;

namespace bookStore.DataAccess.Context
{
    public class FavoriteContext : DbContext
    {
        public DbSet<Favorite> Favorites { get; set; }

        public FavoriteContext() { }

        public FavoriteContext(DbContextOptions<FavoriteContext> options) : base(options) { }

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
            modelBuilder.Ignore<Cart>();
            modelBuilder.Ignore<CartItem>();
            modelBuilder.Ignore<Order>();
            modelBuilder.Ignore<OrderItem>();
            modelBuilder.Ignore<Review>();

            modelBuilder.Entity<UserData>(e =>
            {
                e.ToTable("Users", t => t.ExcludeFromMigrations());
                e.Ignore(u => u.Orders);
                e.Ignore(u => u.Carts);
                e.Ignore(u => u.Reviews);
            });

            modelBuilder.Entity<Favorite>(e =>
            {
                e.HasOne(f => f.User)
                    .WithMany(u => u.Favorites)
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(f => f.Book)
                    .WithMany(b => b.Favorites)
                    .HasForeignKey(f => f.BookId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasIndex(f => new { f.UserId, f.BookId })
                    .IsUnique();
            });

            modelBuilder.Entity<BookImgData>(e =>
            {
                e.ToTable("BookImgs", t => t.ExcludeFromMigrations());
            });

            modelBuilder.Entity<Book>(e =>
            {
                e.ToTable("Books", t => t.ExcludeFromMigrations());
                e.HasMany(b => b.Images)
                    .WithOne(img => img.Book)
                    .HasForeignKey(img => img.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.Ignore(b => b.BookCategories);
                e.Ignore(b => b.CartItems);
                e.Ignore(b => b.Reviews);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
