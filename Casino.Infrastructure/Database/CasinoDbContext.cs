using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Casino.Domain.Identity;

namespace Casino.Infrastructure.Database
{
    public class CasinoDbContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Carousel> Carousels { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Deposit> Deposits { get; set; }


        public CasinoDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            DatabaseInit dbInit = new DatabaseInit();
            modelBuilder.Entity<Game>().HasData(dbInit.GetGames());
            modelBuilder.Entity<Carousel>().HasData(dbInit.GetCarousels());

            modelBuilder.Entity<Role>().HasData(dbInit.CreateRoles());

            (User admin, List<IdentityUserRole<int>> adminUserRoles) = dbInit.CreateAdminWithRoles();
            (User manager, List<IdentityUserRole<int>> managerUserRoles) = dbInit.CreateManagerWithRoles();

            modelBuilder.Entity<User>().HasData(admin, manager);

            modelBuilder.Entity<IdentityUserRole<int>>().HasData(adminUserRoles);
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(managerUserRoles);


            modelBuilder.Entity<User>().HasKey(u => u.Id); // Configure the primary key for User

            modelBuilder.Entity<Deposit>().HasKey(d => d.Id); // Configure the primary key for Deposit

            // Configure the one-to-many relationship between User and Deposit
           modelBuilder.Entity<Deposit>()
            .HasOne(d => d.User)
            .WithMany(u => u.Deposits)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}