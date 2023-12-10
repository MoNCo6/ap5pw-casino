using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Domain.Entities;
using Casino.Infrastructure.Database;

namespace Casino.Infrastructure.Database
{
    public class CasinoDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Carousel> Carousels { get; set; }

        public DbSet<Member> Members { get; set; }

        public CasinoDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            DatabaseInit dbInit = new DatabaseInit();
            modelBuilder.Entity<Game>().HasData(dbInit.GetGames());
            modelBuilder.Entity<Carousel>().HasData(dbInit.GetCarousels());
            modelBuilder.Entity<Member>().HasData(dbInit.GetMembers());
        }
    }
}