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
    public class AccountIdentityService : IAccountService
    {
        UserManager<User> userManager;
        SignInManager<User> sigInManager;
        private readonly CasinoDbContext _context;

        public AccountIdentityService(UserManager<User> userManager, SignInManager<User> sigInManager, CasinoDbContext context)
        {
            this.userManager = userManager;
            this.sigInManager = sigInManager;
            _context = context;
        }

        public async Task<bool> Login(LoginViewModel vm)
        {
            var result = await sigInManager.PasswordSignInAsync(vm.Username, vm.Password, true, true);
            return result.Succeeded;
        }

        public Task Logout()
        {
            return sigInManager.SignOutAsync();
        }

        public async Task<string[]> Register(RegisterViewModel vm, Roles role)
        {
            User user = new User()
            {
                UserName = vm.Username,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                PhoneNumber = vm.Phone,
                CreatedAt = DateTime.Now,
            };

            string[] errors = null;

            var result = await userManager.CreateAsync(user, vm.Password);
            if (result.Succeeded)
            {
                var resultRole = await userManager.AddToRoleAsync(user, role.ToString());

                if (resultRole.Succeeded == false)
                {
                    for (int i = 0; i < result.Errors.Count(); ++i)
                        result.Errors.Append(result.Errors.ElementAt(i));
                }
            }

            if (result.Errors != null && result.Errors.Count() > 0)
            {
                errors = new string[result.Errors.Count()];
                for (int i = 0; i < result.Errors.Count(); ++i)
                {
                    errors[i] = result.Errors.ElementAt(i).Description;
                }
            }

            return errors;
        }


        public async Task<bool> AddDepositAsync(Deposit deposit)
        {
            if (deposit == null)
            {
                return false;
            }

            _context.Deposits.Add(deposit);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
