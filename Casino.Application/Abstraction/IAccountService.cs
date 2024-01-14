using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Application.ViewModels;
using Casino.Domain.Entities;
using Casino.Domain.Identity.Enums;

namespace Casino.Application.Abstraction
{
    public interface IAccountService
    {
        Task<string[]> Register(RegisterViewModel vm, Roles role);
        Task<bool> Login(LoginViewModel vm);
        Task Logout();
        Task<bool> AddDepositAsync(Deposit deposit);
    }
}