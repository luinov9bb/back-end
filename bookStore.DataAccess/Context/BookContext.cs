using bookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace bookStore.DataAccess.Context
{
    public class BookContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<BookImgData> BookImgs { get; set; }
        public DbSet<CategoryData> Categories { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }

        public BookContext() { }
        public BookContext(DbContextOptions<BookContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DbSession.ConnectionStrings);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Images)
                .WithOne(img => img.Book)
                .HasForeignKey(img => img.BookId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookCategories)
                .HasForeignKey(bc => bc.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.BookCategories)
                .HasForeignKey(bc => bc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookCategory>()
                .HasIndex(bc => new { bc.BookId, bc.CategoryId })
                .IsUnique();

            modelBuilder.Entity<Book>()
                .Ignore(b => b.CartItems)
                .Ignore(b => b.Favorites)
                .Ignore(b => b.Reviews);

            base.OnModelCreating(modelBuilder);
        }
    }
}