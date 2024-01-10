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
        IFileUploadService _fileUploadService;

        public UserAdminService(CasinoDbContext casinoDbContext, UserManager<User> userManager, IFileUploadService fileUploadService)
        {
            _casinoDbContext = casinoDbContext;
            this.userManager = userManager;
            _fileUploadService = fileUploadService;
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
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ImagePath = user.ImagePath,
                Balance = user.Balance
            };
        }

        public User? Find(int id)
        {
            return _casinoDbContext.Users.FirstOrDefault(user => user.Id == id);
        }

        public async Task Update(AdminEditUserProfileViewModel userViewModel)
        {
            var user = await _casinoDbContext.Users.FindAsync(userViewModel.Id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            user.UserName = userViewModel.UserName;
            user.FirstName = userViewModel.FirstName;
            user.LastName = userViewModel.LastName;
            user.Email = userViewModel.Email;
            user.PhoneNumber = userViewModel.PhoneNumber;
            user.Balance = userViewModel.Balance;

            if (userViewModel.Image != null)
            {
                string imageSource = await _fileUploadService.FileUploadAsync(userViewModel.Image, Path.Combine("img", "profile_images"));
                user.ImagePath = imageSource;
            }

            await _casinoDbContext.SaveChangesAsync();
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
