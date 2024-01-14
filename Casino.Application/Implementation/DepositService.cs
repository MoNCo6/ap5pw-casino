using Casino.Application.Abstraction;
using Casino.Domain.Entities;
using Casino.Infrastructure.Database;
using Casino.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Application.Implementation
{
    // Service class for deposit-related operations
    public class DepositService : IDepositService
    {
        // Database context for accessing the database
        private readonly CasinoDbContext _context;

        // UserManager for handling user-related operations
        private readonly UserManager<User> _userManager;

        // Constructor to inject the database context and user manager
        public DepositService(CasinoDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Method to retrieve all deposits from the database
        public IList<Deposit> Select()
        {
            // Retrieve and return the list of all deposits
            return _context.Deposits.ToList();
        }

        // Method to add a deposit for a specific user
        public async Task<bool> AddDepositAsync(int userId, int amount)
        {
            // Find the user by their ID
            var user = await _userManager.FindByIdAsync(userId.ToString());

            // Check if the user exists and the amount is positive
            if (user != null && amount > 0)
            {
                // Update the user's balance
                user.Balance += amount;

                // Save the changes to the user
                var result = await _userManager.UpdateAsync(user);

                // Return the result of the update operation
                return result.Succeeded;
            }

            // If user doesn't exist or amount is not positive, return false
            return false;
        }
    }
}