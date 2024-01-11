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
    public class DepositService : IDepositService
    {
        private readonly CasinoDbContext _context;
        private readonly UserManager<User> _userManager;

        public DepositService(CasinoDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Deposit> Select()
        {
            return _context.Deposits.ToList();
        }

        public async Task<bool> AddDepositAsync(int userId, int amount)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null && amount > 0)
            {
                user.Balance += amount; // Update the balance
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            return false;
        }
    }

}
