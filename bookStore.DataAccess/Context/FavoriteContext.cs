using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bookStore.Domain.Entities.Favorite; 
using bookStore.Domain.Entities;

namespace bookStore.DataAccess.Context
{
    public class FavoriteContext : DbContext
    {
        public DbSet<Favorite> Favorites { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DbSession.ConnectionStrings);
        }
    }
}