using Casino.Application.Abstraction;
using Casino.Domain.Entities;
using Casino.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Domain.Identity;
using Casino.Application.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace Casino.Application.Implementation
{
    // Service class responsible for user-related operations
    public class UserService : IUserService
    {
        // Database context for accessing and manipulating user data
        private readonly CasinoDbContext _casinoDbContext;

        // UserManager for handling user-related operations
        private readonly UserManager<User> userManager;

        // Service for handling file uploads
        private readonly IFileUploadService _fileUploadService;

        // Constructor to inject database context, UserManager, and file upload service
        public UserService(CasinoDbContext casinoDbContext, UserManager<User> userManager,
            IFileUploadService fileUploadService)
        {
            _casinoDbContext = casinoDbContext;
            this.userManager = userManager;
            _fileUploadService = fileUploadService;
        }

        // Retrieves a list of all users
        public IList<User> Select()
        {
            return _casinoDbContext.Users.ToList();
        }

        // Deletes a user by their ID
        public bool Delete(int id)
        {
            bool deleted = false;

            // Find the user by ID
            User? user = _casinoDbContext.Users.FirstOrDefault(u => u.Id == id);

            // If user is found, remove and save changes
            if (user != null)
            {
                _casinoDbContext.Users.Remove(user);
                _casinoDbContext.SaveChanges();
                deleted = true;
            }

            return deleted;
        }

        // Retrieves user profile information asynchronously
        public async Task<UserProfileViewModel> GetUserProfileAsync(int userId)
        {
            var user = await _casinoDbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }

            // Map user data to UserProfileViewModel
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

        // Finds a user by their ID
        public User? Find(int id)
        {
            return _casinoDbContext.Users.FirstOrDefault(u => u.Id == id);
        }

        // Updates user profile information for admin
        public async Task Update(AdminEditUserProfileViewModel userViewModel)
        {
            var user = await _casinoDbContext.Users.FindAsync(userViewModel.Id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            // Update user information
            user.UserName = userViewModel.UserName;
            user.FirstName = userViewModel.FirstName;
            user.LastName = userViewModel.LastName;
            user.Email = userViewModel.Email;
            user.PhoneNumber = userViewModel.PhoneNumber;
            user.Balance = userViewModel.Balance;

            // If a new image is provided, upload it and update the image path
            if (userViewModel.Image != null)
            {
                string imageSource =
                    await _fileUploadService.FileUploadAsync(userViewModel.Image,
                        Path.Combine("img", "profile_images"));
                user.ImagePath = imageSource;
            }

            // Save the changes to the database
            await _casinoDbContext.SaveChangesAsync();
        }

        // Updates a user's profile based on the given model
        public async Task<bool> UpdateUserAsync(string userId, UserProfileViewModel model)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false; // Return false if user not found
            }

            // Update user properties
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            // Attempt to save the updated user info
            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // If update succeeds, save changes to database and return true
                await _casinoDbContext.SaveChangesAsync();
                return true;
            }

            return false; // Return false if update fails
        }
    }
}