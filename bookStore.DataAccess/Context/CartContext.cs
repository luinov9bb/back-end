using bookStore.Domain.Entities;
using bookStore.Domain.Entities.Cart;
using bookStore.Domain.Entities.Favorite;
using bookStore.Domain.Entities.Order;
using bookStore.Domain.Entities.Review;
using Microsoft.EntityFrameworkCore;

namespace bookStore.DataAccess.Context
{
    public class CartContext : DbContext
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public CartContext() { }

        public CartContext(DbContextOptions<CartContext> options) : base(options) { }

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
            modelBuilder.Ignore<Favorite>();
            modelBuilder.Ignore<Review>();
            modelBuilder.Ignore<Order>();
            modelBuilder.Ignore<OrderItem>();

            modelBuilder.Entity<UserData>(e =>
            {
                e.ToTable("Users", t => t.ExcludeFromMigrations());
                e.Ignore(u => u.Orders);
                e.Ignore(u => u.Favorites);
                e.Ignore(u => u.Reviews);
            });

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Items)
                .WithOne(i => i.Cart)
                .HasForeignKey(i => i.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Book)
                .WithMany(b => b.CartItems)
                .HasForeignKey(ci => ci.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Book>(e =>
            {
                e.ToTable("Books", t => t.ExcludeFromMigrations());
                e.Ignore(b => b.Images);
                e.Ignore(b => b.BookCategories);
                e.Ignore(b => b.Favorites);
                e.Ignore(b => b.Reviews);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
