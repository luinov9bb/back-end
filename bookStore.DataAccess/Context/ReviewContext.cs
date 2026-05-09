using bookStore.Domain.Entities;
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
            modelBuilder.Entity<UserData>(e =>
            {
                e.ToTable("Users");
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

            modelBuilder.Entity<Book>()
                .Ignore(b => b.Images)
                .Ignore(b => b.BookCategories)
                .Ignore(b => b.CartItems)
                .Ignore(b => b.Favorites);

            base.OnModelCreating(modelBuilder);
        }
    }
}
