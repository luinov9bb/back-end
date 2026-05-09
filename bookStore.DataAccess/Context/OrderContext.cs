using bookStore.Domain.Entities;
using bookStore.Domain.Entities.Cart;
using bookStore.Domain.Entities.Favorite;
using bookStore.Domain.Entities.Order;
using bookStore.Domain.Entities.Review;
using Microsoft.EntityFrameworkCore;

namespace bookStore.DataAccess.Context
{
    public class OrderContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public OrderContext() { }

        public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }

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

            modelBuilder.Entity<UserData>(e =>
            {
                e.ToTable("Users", t => t.ExcludeFromMigrations());
                e.Ignore(u => u.Carts);
                e.Ignore(u => u.Favorites);
                e.Ignore(u => u.Reviews);
            });

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
