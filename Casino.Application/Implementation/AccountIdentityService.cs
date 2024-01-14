using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Casino.Application.Abstraction;
using Casino.Application.ViewModels;
using Casino.Domain.Identity;
using Casino.Domain.Identity.Enums;
using Casino.Infrastructure.Database;
using Casino.Domain.Entities;

namespace Casino.Application.Implementation
{
    // Service class for account-related operations
    public class AccountIdentityService : IAccountService
    {
        // UserManager and SignInManager from ASP.NET Core Identity
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        // Database context for entity framework operations
        private readonly CasinoDbContext _context;

        // Constructor to inject UserManager, SignInManager, and DbContext
        public AccountIdentityService(UserManager<User> userManager, SignInManager<User> signInManager,
            CasinoDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _context = context;
        }

        // Method for user login using username and password
        public async Task<bool> Login(LoginViewModel vm)
        {
            var result = await signInManager.PasswordSignInAsync(vm.Username, vm.Password, true, true);
            return result.Succeeded;
        }

        // Method for user logout
        public Task Logout()
        {
            return signInManager.SignOutAsync();
        }

        // Method for registering a new user with a specified role
        public async Task<string[]> Register(RegisterViewModel vm, Roles role)
        {
            // Create a new user object from the registration form data
            User user = new User()
            {
                UserName = vm.Username,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                PhoneNumber = vm.Phone,
                CreatedAt = DateTime.Now,
            };

            // Array to hold error messages if registration fails
            string[] errors = null;

            // Attempt to create the user in the database
            var result = await userManager.CreateAsync(user, vm.Password);
            if (result.Succeeded)
            {
                // If user creation succeeded, add the user to the specified role
                var resultRole = await userManager.AddToRoleAsync(user, role.ToString());

                // Collect any errors encountered while adding the user to the role
                if (!resultRole.Succeeded)
                {
                    errors = resultRole.Errors.Select(e => e.Description).ToArray();
                }
            }
            else if (result.Errors.Any())
            {
                // If there were errors during user creation, collect them
                errors = result.Errors.Select(e => e.Description).ToArray();
            }

            // Return the array of error messages (null if no errors)
            return errors;
        }

        // Method for adding a deposit to the user's account
        public async Task<bool> AddDepositAsync(Deposit deposit)
        {
            // Validate that the deposit object is not null
            if (deposit == null)
            {
                return false;
            }

            // Add the deposit to the database and save the changes
            _context.Deposits.Add(deposit);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}