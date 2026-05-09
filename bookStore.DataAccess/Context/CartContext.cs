using bookStore.Domain.Entities;
using bookStore.Domain.Entities.Cart;
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
            modelBuilder.Entity<UserData>(e =>
            {
                e.ToTable("Users");
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

            modelBuilder.Entity<Book>()
                .Ignore(b => b.Images)
                .Ignore(b => b.BookCategories)
                .Ignore(b => b.Favorites)
                .Ignore(b => b.Reviews);

            base.OnModelCreating(modelBuilder);
        }
    }
}
