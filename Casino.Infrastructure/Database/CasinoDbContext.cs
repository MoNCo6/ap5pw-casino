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
    // Database context class for the Casino application
    public class CasinoDbContext : IdentityDbContext<User, Role, int>
    {
        // DbSet properties to map entity classes to database tables
        public DbSet<Game> Games { get; set; }
        public DbSet<Carousel> Carousels { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Deposit> Deposits { get; set; }

        // Constructor to configure the context using options provided by dependency injection
        public CasinoDbContext(DbContextOptions options) : base(options)
        {
        }

        // Overriding the OnModelCreating method to configure entity relationships and seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call base method to configure identity models
            base.OnModelCreating(modelBuilder);

            // Create an instance of DatabaseInit to seed initial data
            DatabaseInit dbInit = new DatabaseInit();

            // Seed initial data for the Game and Carousel tables
            modelBuilder.Entity<Game>().HasData(dbInit.GetGames());
            modelBuilder.Entity<Carousel>().HasData(dbInit.GetCarousels());

            // Seed initial data for the Role table
            modelBuilder.Entity<Role>().HasData(dbInit.CreateRoles());

            // Create admin and manager users with roles
            (User admin, List<IdentityUserRole<int>> adminUserRoles) = dbInit.CreateAdminWithRoles();
            (User manager, List<IdentityUserRole<int>> managerUserRoles) = dbInit.CreateManagerWithRoles();

            // Seed admin and manager users into the User table
            modelBuilder.Entity<User>().HasData(admin, manager);

            // Seed user roles into the IdentityUserRole table
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(adminUserRoles);
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(managerUserRoles);

            // Configuring primary keys for User and Deposit entities
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Deposit>().HasKey(d => d.Id);

            // Configuring the one-to-many relationship between User and Deposit entities
            modelBuilder.Entity<Deposit>()
                .HasOne(d => d.User)
                .WithMany(u => u.Deposits)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Set cascade delete behavior
        }
    }
}