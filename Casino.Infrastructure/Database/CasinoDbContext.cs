using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Casino.Infrastructure.Identity;



namespace Casino.Infrastructure.Database
{
    public class CasinoDbContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Carousel> Carousels { get; set; }
        public DbSet<User> Users { get; set; }


        public CasinoDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            DatabaseInit dbInit = new DatabaseInit();
            modelBuilder.Entity<Game>().HasData(dbInit.GetGames());
            modelBuilder.Entity<Carousel>().HasData(dbInit.GetCarousels());

            //Identity - User and Role initialization
            //roles must be added first
            modelBuilder.Entity<Role>().HasData(dbInit.CreateRoles());

            //then, create users ..
            (User admin, List<IdentityUserRole<int>> adminUserRoles) = dbInit.CreateAdminWithRoles();
            (User manager, List<IdentityUserRole<int>> managerUserRoles) = dbInit.CreateManagerWithRoles();

            //.. and add them to the table
            modelBuilder.Entity<User>().HasData(admin, manager);

            //and finally, connect the users with the roles
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(adminUserRoles);
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(managerUserRoles);
        }
    }
}