using Microsoft.EntityFrameworkCore;
using bookStore.Domain.Entities;

namespace bookStore.DataAccess.Context
{
    public class UserContext : DbContext
    {

        public virtual DbSet<UserData> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DbSession.ConnectionString);
        }
    }
}