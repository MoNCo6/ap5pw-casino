using Casino.Application.Abstraction;
using Casino.Domain.Entities;
using Casino.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Infrastructure.Identity;
using Casino.Application.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace Casino.Application.Implementation
{
    public class UserAdminService : IUserAdminService
    {
        CasinoDbContext _casinoDbContext;
        UserManager<User> userManager;

        public UserAdminService(CasinoDbContext casinoDbContext, UserManager<User> userManager)
        {
            _casinoDbContext = casinoDbContext;
            this.userManager = userManager;
        }
        public IList<User> Select()
        {
            return _casinoDbContext.Users.ToList();

        }

        public bool Delete(int id)
        {
            bool deleted = false;

            User? user =
                _casinoDbContext.Users.FirstOrDefault(user => user.Id == id);

            if (user != null)
            {
                _casinoDbContext.Users.Remove(user);
                _casinoDbContext.SaveChanges();

                deleted = true;
            }

            return deleted;
        }

        public async Task<UserProfileViewModel> GetUserProfileAsync(int userId)
        {
            var user = await _casinoDbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }

            return new UserProfileViewModel
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email           
            };
        }

        public async Task<bool> UpdateUserAsync(string userId, UserProfileViewModel model)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                await _casinoDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }


    }
}
